namespace NoaDebugger
{
    sealed class NoaDebuggerFontSettings : NoaDebuggerSettingsBase
    {
        public NoaDebuggerFontSettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void Init()
        {
            _settings.IsCustomFontSettingsEnabled = NoaDebuggerDefine.DEFAULT_IS_CUSTOM_FONT_SETTINGS_ENABLED;
            _settings.FontAsset = null;
            _settings.FontMaterial = null;
            _settings.FontSizeRate = NoaDebuggerDefine.DEFAULT_FONT_SIZE_RATE;
        }
    }
}
