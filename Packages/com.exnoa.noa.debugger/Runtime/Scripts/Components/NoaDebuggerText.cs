using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace NoaDebugger
{
    sealed class NoaDebuggerText : TextMeshProUGUI
    {

        static TMP_FontAsset _runtimeDefaultFontAsset;
        static Material _runtimeDefaultFontMaterial;
        static float _runtimeDefaultFontSizeRate;

        static TMP_FontAsset _fontAsset;
        static Material _fontMaterial;
        static float _fontSizeRate = NoaDebuggerDefine.DEFAULT_FONT_SIZE_RATE;
        static bool _isInit;

        const int BASE_FONT_POINT_SIZE = 73;
        const float BASE_FONT_CAP_LINE = 52f;

        public static void Init(NoaDebuggerSettings settings)
        {
            if (settings.IsCustomFontSpecified)
            {
                NoaDebuggerText._runtimeDefaultFontAsset = settings.FontAsset;
                NoaDebuggerText._runtimeDefaultFontMaterial = settings.FontMaterial;
                NoaDebuggerText._runtimeDefaultFontSizeRate = settings.FontSizeRate;

                NoaDebuggerText.ChangeFont(settings.FontAsset, settings.FontMaterial, settings.FontSizeRate);
            }
            _isInit = true;
        }

        public static void ChangeFont(TMP_FontAsset fontAsset, Material fontMaterial, float fontSizeRate)
        {
            _fontAsset = fontAsset;
            _fontMaterial = fontMaterial;
            _fontSizeRate = fontSizeRate;
        }

        public static void ResetFont()
        {
            ChangeFont(NoaDebuggerText._runtimeDefaultFontAsset,
                       NoaDebuggerText._runtimeDefaultFontMaterial,
                       NoaDebuggerText._runtimeDefaultFontSizeRate);
        }

        public static bool HasFontAsset => NoaDebuggerText._fontAsset != null;

        public static float CalculateFontSizeRate(TMP_FontAsset targetFont)
        {
            var targetFaceInfo = targetFont.faceInfo;

            float basePointSize = BASE_FONT_POINT_SIZE;
            float targetPointSize = targetFaceInfo.pointSize;

            if(targetFaceInfo.pointSize == 0f)
            {
                LogModel.LogWarning($"{targetFont.name} point size is 0.");
                return 1.0f;
            }

            float pointSizeScale = basePointSize / targetPointSize;

            float targetHeight = targetFaceInfo.capLine;
            targetHeight *= pointSizeScale; 
            float baseHeight = BASE_FONT_CAP_LINE;

            if(targetHeight == 0f)
            {
                LogModel.LogWarning($"{targetFont.name} capLine is 0.");
                return 1.0f;
            }

            float fontSizeRate = baseHeight / targetHeight;

            string tmpText = fontSizeRate.ToString("f2");
            return float.Parse(tmpText);
        }

        public static string GetHasFontAssetString(TMP_FontAsset targetFont, string text)
        {
            if (targetFont == null || string.IsNullOrEmpty(text))
            {
                return text;
            }

            text = _DecodeUnicodeEscapedString(text);

            targetFont.HasCharacters(text, out List<char> missingCharactersInTargetFont);

            if (missingCharactersInTargetFont.Count == 0)
            {
                return text;
            }

            List<char> missingCharacters;

            if (targetFont.fallbackFontAssetTable.Count == 0)
            {
                missingCharacters = missingCharactersInTargetFont;
            }
            else
            {
                List<List<char>> missingCharsPerFallbackFont = new();

                foreach (TMP_FontAsset fallbackFont in targetFont.fallbackFontAssetTable)
                {
                    fallbackFont.HasCharacters(text, out List<char> list);

                    if (list == null)
                    {
                        fallbackFont.ReadFontAssetDefinition();
                        fallbackFont.HasCharacters(text, out list);
                    }

                    if (list != null)
                    {
                        missingCharsPerFallbackFont.Add(list);
                    }
                }

                missingCharacters = missingCharactersInTargetFont
                                    .Where(ch => missingCharsPerFallbackFont.All(list => list.Contains(ch)))
                                    .ToList();
            }

            if (targetFont.atlasPopulationMode == AtlasPopulationMode.Dynamic && missingCharacters.Count > 0)
            {
                targetFont.TryAddCharacters(text);
            }

            string result = text;

            foreach (char character in missingCharacters)
            {
                var unicodeEscape = _GetUnicodeEscapeSequence(character);
                result = result.Replace(character.ToString(), unicodeEscape);
            }

            return result;
        }

        static string _DecodeUnicodeEscapedString(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, @"\\u([0-9A-Fa-f]{4})", match =>
            {
                var unicodeHex = match.Groups[1].Value;
                var unicodeDecimal = int.Parse(unicodeHex, System.Globalization.NumberStyles.HexNumber);

                if (unicodeDecimal == 0x0020)
                {
                    return "u0020";
                }

                return ((char)unicodeDecimal).ToString();
            });
        }

        static string _GetUnicodeEscapeSequence(char character)
        {
            return $"u{((int)character):X4}";
        }


        public override string text
        {
            get => base.text;
            set
            {
                ApplyFont();
                base.text = _TruncateToMaxLength(value);
            }
        }

        new public float fontSize
        {
            get { return m_fontSize; }
            set
            {
                _defaultFontSize = value;
                var newFontSize = value * _fontSizeRate;
                base.fontSize = newFontSize;
            }
        }

        bool _isApplied;

        [SerializeField, HideInInspector]
        float _defaultFontSize = -1f;
        [SerializeField, HideInInspector]
        float _defaultFontSizeMin = -1f;
        [SerializeField, HideInInspector]
        float _defaultFontSizeMax = -1f;

        protected override void Awake()
        {
            base.Awake();

            ApplyFont();
        }

        public NoaDebuggerApiStatus.SetFont ApplyFont(bool isForce = false)
        {
            if (!isForce)
            {
                if (!Application.isPlaying)
                {
                    return NoaDebuggerApiStatus.SetFont.FailedIsNotPlayMode;
                }

                if (_isApplied)
                {
                    return NoaDebuggerApiStatus.SetFont.Succeed;
                }

                _isApplied = true;
            }

            if (!_isInit)
            {
                LogModel.DebugLogWarning("NoaDebuggerText is not initialized.");
            }

            if (NoaDebuggerText._runtimeDefaultFontAsset == null)
            {
                NoaDebuggerText._runtimeDefaultFontAsset = font;
                NoaDebuggerText._runtimeDefaultFontMaterial = fontMaterial;
                NoaDebuggerText._runtimeDefaultFontSizeRate = NoaDebuggerDefine.DEFAULT_FONT_SIZE_RATE;
            }

            if (_defaultFontSize <= -1f)
            {
                _defaultFontSize = fontSize;
            }
            if (_defaultFontSizeMin <= -1f)
            {
                _defaultFontSizeMin = fontSizeMin;
            }
            if (_defaultFontSizeMax <= -1f)
            {
                _defaultFontSizeMax = fontSizeMax;
            }

            if (_fontAsset == null)
            {
                return NoaDebuggerApiStatus.SetFont.FailedFontAssetIsNull;
            }

            if (_fontSizeRate <= -1f)
            {
                _fontSizeRate = CalculateFontSizeRate(_fontAsset);
            }

            font = _fontAsset;
            fontMaterial = _fontMaterial;

            base.fontSize = _defaultFontSize * _fontSizeRate;

            if (enableAutoSizing)
            {
                fontSizeMin = _defaultFontSizeMin * _fontSizeRate;
                fontSizeMax = _defaultFontSizeMax * _fontSizeRate;
            }

            return NoaDebuggerApiStatus.SetFont.Succeed;
        }

        string _TruncateToMaxLength(string text)
        {
            const int maxLength = 99999;
            if (!string.IsNullOrEmpty(text) && text.Length > maxLength)
            {
                string result = text.Substring(0, maxLength);
                return result;
            }

            return text;
        }
    }
}
