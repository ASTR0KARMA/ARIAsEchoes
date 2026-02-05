namespace NoaDebugger
{
    sealed class NoaDebuggerGameSpeedSettings : NoaDebuggerSettingsBase
    {
        public NoaDebuggerGameSpeedSettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void Init()
        {
            _settings.AppliesGameSpeedChange = NoaDebuggerDefine.DEFAULT_GAME_SPEED_CHANGE_APPLICABLE;
            _settings.GameSpeedIncrement = NoaDebuggerDefine.DEFAULT_GAME_SPEED_INCREMENT;
            _settings.MaxGameSpeed = NoaDebuggerDefine.DEFAULT_MAX_GAME_SPEED;
        }
    }
}
