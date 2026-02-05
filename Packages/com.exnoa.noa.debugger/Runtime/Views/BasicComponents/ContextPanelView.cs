using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class ContextPanelView : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI _header;
        [SerializeField]
        TextMeshProUGUI _context;
        [SerializeField]
        Color _keyContextColor;
        [SerializeField]
        Color _valueContextColor;
        [SerializeField]
        float _valuePosition = 100;
        [SerializeField]
        float _keyValueSpace = 20;
        [SerializeField]
        bool _keyContextWrap;
        [SerializeField]
        LayoutElement _lineLayoutElement;
        [SerializeField]
        TextMeshProUGUI _wrapCheckText;

        public Color KeyContextColor
        {
            set => _keyContextColor = value;
        }

        public Color ValueContextColor
        {
            set => _valueContextColor = value;
        }

        static Dictionary<char, float> _charWidthCache = new Dictionary<char, float>();

        void Start()
        {
            if (_keyContextWrap)
            {
                _wrapCheckText.font = _context.font;
            }
        }

        public void SetText(string header, Dictionary<string, string> context, string prefix = "", string suffix = "",
                            string missingValue = NoaDebuggerDefine.MISSING_VALUE)
        {
            _header.text = header;
            _context.text = _ConvertContext(context, prefix, suffix, missingValue);
        }

        string _ConvertContext(Dictionary<string, string> contextList, string prefix, string suffix,
                               string missingValue)
        {
            var sb = new StringBuilder(prefix);
            var keyColorCode = ColorUtility.ToHtmlStringRGB(_keyContextColor);
            var valueColorCode = ColorUtility.ToHtmlStringRGB(_valueContextColor);

            if (contextList != null)
            {
                foreach (var item in contextList)
                {
                    var keyText = item.Key;

                    if (_keyContextWrap)
                    {
                        float wrapWidth = _valuePosition - _keyValueSpace;
                        string keyWrapText = _GetSubstringTextByWidth(keyText, wrapWidth, out int endIndex);
                        sb.Append($"<color=#{keyColorCode}>{keyWrapText}</color>");
                        bool isEndKeyWarpText = keyText.Length == endIndex;

                        if (isEndKeyWarpText)
                        {
                            sb.Append(":");
                        }

                        sb.Append(
                            $"<pos={_valuePosition}><color=#{valueColorCode}>{_ConvertLabel(item.Value, missingValue)}</color></pos>");

                        if (!isEndKeyWarpText)
                        {
                            sb.Append("<br>");
                            keyText = keyText.Substring(endIndex);
                            keyText = _GetWrapText(keyText, wrapWidth);
                            sb.Append($"<color=#{keyColorCode}>{keyText}:</color>");
                        }
                    }
                    else
                    {
                        sb.Append($"<color=#{keyColorCode}>{keyText}:</color>");

                        sb.Append(
                            $"<pos={_valuePosition}><color=#{valueColorCode}>{_ConvertLabel(item.Value, missingValue)}</color></pos>");
                    }

                    sb.AppendLine();
                }
            }

            sb.Append(suffix);

            return sb.ToString();
        }

        string _GetWrapText(string text, float width)
        {
            var keySb = new StringBuilder();

            int currentIndex = 0;
            string subTextAll = text;

            while (currentIndex < text.Length)
            {
                string subText = _GetSubstringTextByWidth(subTextAll, width, out int endIndex);
                keySb.Append($"{subText}");
                subTextAll = subTextAll.Substring(endIndex);
                currentIndex += endIndex;

                if (currentIndex < text.Length)
                {
                    keySb.Append($"<br>");
                }
            }

            return keySb.ToString();
        }

        string _GetSubstringTextByWidth(string text, float targetWidth, out int endIndex)
        {
            endIndex = 0;
            _context.font.HasCharacters(text, out List<char> missingCharacters);

            if (text.Length * _context.fontSize < targetWidth)
            {
                endIndex = text.Length;

                return text;
            }

            float currentWidth = 0;
            _wrapCheckText.text = "";

            foreach (char c in text)
            {
                if (missingCharacters.Contains(c))
                {
                    currentWidth += _wrapCheckText.fontSize;
                }
                else
                {
                    if (_charWidthCache.TryGetValue(c, out float charWidth))
                    {
                        currentWidth += charWidth;
                    }
                    else
                    {
                        _wrapCheckText.text = c.ToString();
                        charWidth = _wrapCheckText.GetPreferredValues().x;
                        currentWidth += charWidth;
                        _charWidthCache[c] = charWidth;
                    }
                }

                if (currentWidth > targetWidth)
                {
                    break;
                }

                endIndex++;
            }

            return text.Substring(0, endIndex);
        }

        string _ConvertLabel(string label, string missingValue)
        {
            if (!_IsAcquireLabel(label))
            {
                return missingValue;
            }

            return $"{label}";
        }

        bool _IsAcquireLabel(string label)
        {
            if (string.IsNullOrEmpty(label))
                return false;

            if (label == SystemInfo.unsupportedIdentifier)
                return false;

            return true;
        }

        public void SetMinWidthForLine(float width)
        {
            _lineLayoutElement.minWidth = width;
        }

        public void SetActiveLine(bool active)
        {
            _lineLayoutElement.gameObject.SetActive(active);
        }
    }
}
