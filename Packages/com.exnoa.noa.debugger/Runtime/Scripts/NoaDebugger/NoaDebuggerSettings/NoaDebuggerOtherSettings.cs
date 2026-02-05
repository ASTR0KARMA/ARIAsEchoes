namespace NoaDebugger
{
    sealed class NoaDebuggerOtherSettings : NoaDebuggerSettingsBase
    {
        public NoaDebuggerOtherSettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void Init()
        {
            _settings.AutoCreateEventSystem = NoaDebuggerDefine.DEFAULT_AUTO_CREATE_EVENT_SYSTEM;
            _settings.ErrorNotificationType = NoaDebuggerDefine.DEFAULT_ERROR_NOTIFICATION_TYPE;
        }
    }
}
