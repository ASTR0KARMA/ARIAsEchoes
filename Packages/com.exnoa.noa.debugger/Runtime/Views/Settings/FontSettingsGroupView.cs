using UnityEngine;

namespace NoaDebugger
{
    sealed class FontSettingsGroupView : SettingsViewBase
    {
        [SerializeField] ReadOnlySettingsPanel _isCustomFontSettingsEnabled;
        [SerializeField] ReadOnlySettingsPanel _fontAsset;
        [SerializeField] ReadOnlySettingsPanel _fontMaterial;
        [SerializeField] ReadOnlySettingsPanel _fontSizeRate;

        public override void Initialize(SettingsViewLinker linker)
        {
            var settings = linker._fontSettings;
            var isFontEnabled = settings.IsCustomFontSettingsEnabled ? NoaDebuggerDefine.SETTINGS_ENABLED_VALUE : NoaDebuggerDefine.SETTINGS_DISABLED_VALUE;
            _isCustomFontSettingsEnabled.Initialize(isFontEnabled);
            _fontAsset.Initialize(settings.FontAsset);
            _fontMaterial.Initialize(settings.FontMaterial);
            _fontSizeRate.Initialize(settings.FontSizeRate);
        }
    }
}
