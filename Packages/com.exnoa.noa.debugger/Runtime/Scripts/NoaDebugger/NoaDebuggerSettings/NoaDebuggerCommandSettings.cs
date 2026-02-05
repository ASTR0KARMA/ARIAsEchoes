namespace NoaDebugger
{
    sealed class NoaDebuggerCommandSettings : NoaDebuggerSettingsBase
    {
        public NoaDebuggerCommandSettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void Init()
        {
            _settings.CommandFormatLandscape = NoaDebuggerDefine.DEFAULT_COMMAND_FORMAT_LANDSCAPE;
            _settings.CommandFormatPortrait = NoaDebuggerDefine.DEFAULT_COMMAND_FORMAT_PORTRAIT;
        }
    }
}
