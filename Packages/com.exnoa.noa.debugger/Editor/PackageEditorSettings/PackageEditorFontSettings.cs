#if NOA_DEBUGGER
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace NoaDebugger
{
    sealed class PackageEditorFontSettings : PackageEditorSettingsBase
    {
        bool _isCustomFontSettingsEnabled;

        FontSettings _fontSetting;

        public PackageEditorFontSettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void ResetTmpDataWithSettings()
        {
            _isCustomFontSettingsEnabled = _settings.IsCustomFontSettingsEnabled;
            _fontSetting = new FontSettings(_settings.FontAsset, _settings.FontMaterial, _settings.FontSizeRate);
        }

        public override void ApplySettings()
        {
            _settings.IsCustomFontSettingsEnabled = _isCustomFontSettingsEnabled;
            _settings.FontAsset = _fontSetting._fontAsset;
            _settings.FontMaterial = _fontSetting.FontMaterialPreset;
            _settings.FontSizeRate = _fontSetting._fontSizeRate;
        }

        public override void ResetDefault()
        {
            _isCustomFontSettingsEnabled = NoaDebuggerDefine.DEFAULT_IS_CUSTOM_FONT_SETTINGS_ENABLED;
            _fontSetting = new FontSettings(null, null, NoaDebuggerDefine.DEFAULT_FONT_SIZE_RATE);
        }

        public override void DrawGUI()
        {
            _DisplaySettingsCategoryHeader("Font", ResetDefault);

            _isCustomFontSettingsEnabled = EditorGUILayout.Toggle("Custom font settings enabled", _isCustomFontSettingsEnabled);

            if (_isCustomFontSettingsEnabled)
            {
                EditorGUI.BeginChangeCheck();
                TMP_FontAsset beforeFontAsset = _fontSetting._fontAsset;

                _fontSetting._fontAsset = EditorGUILayout.ObjectField(
                    "Font asset", _fontSetting._fontAsset, typeof(TMP_FontAsset), false) as TMP_FontAsset;

                if (EditorGUI.EndChangeCheck())
                {
                    _fontSetting.GetMaterialPresets();

                    if (_fontSetting._fontAsset != null && beforeFontAsset != _fontSetting._fontAsset)
                    {
                        _AutomaticFontSizeRateSetting();
                    }

                    if (_fontSetting._fontAsset == null)
                    {
                        _fontSetting._fontSizeRate = NoaDebuggerDefine.DEFAULT_FONT_SIZE_RATE;
                    }
                }

                _fontSetting._materialIndex = EditorGUILayout.Popup(
                    "Material preset", _fontSetting._materialIndex, _fontSetting.MaterialPresetNames);

                _fontSetting._fontSizeRate = EditorGUILayout.FloatField(
                    "Font size rate", _fontSetting._fontSizeRate);

                if (GUILayout.Button("Automatic Font Size Rate Setting",GUILayout.Width(200f)))
                {
                    _AutomaticFontSizeRateSetting();
                }

                EditorGUILayout.HelpBox("Specify the font included within the application for the Font asset.\nIf a Font asset is specified, the font asset included with NOA Debugger is excluded at build time.", MessageType.Info);
            }
        }

        void _AutomaticFontSizeRateSetting()
        {
            if (_fontSetting._fontAsset != null)
            {
                _fontSetting._fontSizeRate = NoaDebuggerText.CalculateFontSizeRate(_fontSetting._fontAsset);
            }
        }

        class FontSettings
        {
            Material[] _materialPresets;
            string[] _materialPresetNames;

            public TMP_FontAsset _fontAsset;

            public Material FontMaterialPreset
            {
                get
                {
                    if (_materialPresets.Length == 0 || _materialIndex < 0)
                    {
                        return null;
                    }

                    return _materialPresets[_materialIndex];
                }
            }

            public float _fontSizeRate;

            public string[] MaterialPresetNames => _materialPresetNames;

            public int _materialIndex;

            public FontSettings(TMP_FontAsset fontAsset, Material material, float fontSizeRate)
            {
                _fontAsset = fontAsset;
                _fontSizeRate = fontSizeRate;

                GetMaterialPresets();

                if (material != null)
                {
                    _materialIndex = Array.IndexOf(_materialPresetNames, material.name);
                    if (_materialIndex == -1)
                    {
                        _materialIndex = 0;
                    }
                }
            }

            public void GetMaterialPresets()
            {
                TMP_FontAsset fontAsset = _fontAsset;
                if (fontAsset == null)
                {
                    _materialPresets = Array.Empty<Material>();
                    _materialPresetNames = Array.Empty<string>();
                    _materialIndex = 0;
                    return;
                }

                _materialPresets = FindMaterialReferences(fontAsset);
                _materialPresetNames = new string[_materialPresets.Length];

                for(int i = 0; i < _materialPresets.Length; i++)
                {
                    Material material = _materialPresets[i];
                    _materialPresetNames[i] = material.name;
                }

                _materialIndex = 0;
            }
        }

        static Material[] FindMaterialReferences(TMP_FontAsset fontAsset)
        {
            List<Material> refs = new List<Material>();
            Material mat = fontAsset.material;
            refs.Add(mat);

            string searchPattern = "t:Material" + " " + fontAsset.name.Split(new char[] { ' ' })[0];
            string[] materialAssetGUIDs = AssetDatabase.FindAssets(searchPattern);

            for (int i = 0; i < materialAssetGUIDs.Length; i++)
            {
                string materialPath = AssetDatabase.GUIDToAssetPath(materialAssetGUIDs[i]);
                Material targetMaterial = AssetDatabase.LoadAssetAtPath<Material>(materialPath);

                if (targetMaterial.HasProperty(ShaderUtilities.ID_MainTex) && targetMaterial.GetTexture(ShaderUtilities.ID_MainTex) != null && mat.GetTexture(ShaderUtilities.ID_MainTex) != null && targetMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID() == mat.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
                {
                    if (!refs.Contains(targetMaterial))
                        refs.Add(targetMaterial);
                }
                else
                {
                }
            }

            return refs.ToArray();
        }
    }
}
#endif
