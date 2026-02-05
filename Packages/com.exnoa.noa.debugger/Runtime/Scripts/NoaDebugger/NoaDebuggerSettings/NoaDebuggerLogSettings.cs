namespace NoaDebugger
{
    sealed class NoaDebuggerLogSettings : NoaDebuggerSettingsBase
    {
        public NoaDebuggerLogSettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void Init()
        {
            _settings.ConsoleLogCount = NoaDebuggerDefine.DEFAULT_CONSOLE_LOG_COUNT;
            _settings.ApiLogCount = NoaDebuggerDefine.DEFAULT_API_LOG_COUNT;
        }
    }
}
