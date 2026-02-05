using System.Globalization;
using UnityEngine;

namespace NoaDebugger
{
    sealed class FontRuntimeSettings : FeatureToolSettingsBase
    {
        protected override string PlayerPrefsKeyPrefix => "Font";

        bool _isCustomFontSettingsEnabled;
        string _fontAsset;
        string _fontMaterial;
        string _fontSizeRate;

        public bool IsCustomFontSettingsEnabled => _isCustomFontSettingsEnabled;

        public string FontAsset => _fontAsset;

        public string FontMaterial => _fontMaterial;

        public string FontSizeRate => _fontSizeRate;

        protected override void _InitializeSettings()
        {
            _isCustomFontSettingsEnabled = _noaDebuggerSettings.IsCustomFontSettingsEnabled;
            _fontAsset = _noaDebuggerSettings.FontAsset != null ? _noaDebuggerSettings.FontAsset.name : "";
            _fontMaterial = _noaDebuggerSettings.FontMaterial != null ? _noaDebuggerSettings.FontMaterial.name : "";
            _fontSizeRate = _noaDebuggerSettings.FontSizeRate.ToString(CultureInfo.InvariantCulture);
        }

        protected override void _LoadSettings(string prefsJson)
        {
        }

        protected override void _SetDefaultValue()
        {
        }

        protected override void _ResetSettings()
        {
        }

        public override void ApplyCache(NoaDebuggerSettings originalSettings)
        {
        }

        public override bool IsValueChanged()
        {
            return false;
        }
    }
}
