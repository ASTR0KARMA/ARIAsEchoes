namespace NoaDebugger
{
    sealed class NoaDebuggerAutoSaveSettings : NoaDebuggerSettingsBase
    {
        public NoaDebuggerAutoSaveSettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void Init()
        {
            _settings.AutoSave = NoaDebuggerDefine.DEFAULT_AUTO_SAVE;
        }
    }
}
