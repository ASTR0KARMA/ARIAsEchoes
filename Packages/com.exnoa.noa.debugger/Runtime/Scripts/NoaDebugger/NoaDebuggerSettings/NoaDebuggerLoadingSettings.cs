namespace NoaDebugger
{
    sealed class NoaDebuggerLoadingSettings : NoaDebuggerSettingsBase
    {
        public NoaDebuggerLoadingSettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void Init()
        {
            _settings.AutoInitialize = NoaDebuggerDefine.DEFAULT_AUTO_INITIALIZE;
        }
    }
}
