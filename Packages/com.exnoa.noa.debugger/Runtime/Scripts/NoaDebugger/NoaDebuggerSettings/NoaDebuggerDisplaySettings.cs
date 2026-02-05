namespace NoaDebugger
{
    sealed class NoaDebuggerDisplaySettings : NoaDebuggerSettingsBase
    {
        public NoaDebuggerDisplaySettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void Init()
        {
            _settings.StartButtonPosition = NoaDebuggerDefine.DEFAULT_START_BUTTON_POSITION;
            _settings.StartButtonScale = NoaDebuggerDefine.DEFAULT_START_BUTTON_SCALE;
            _settings.StartButtonMovementType = NoaDebuggerDefine.DEFAULT_START_BUTTON_MOVEMENT_TYPE;
            _settings.SaveStartButtonPosition = NoaDebuggerDefine.DEFAULT_SAVE_START_BUTTON_POSITION;
            _settings.BackgroundAlpha = NoaDebuggerDefine.DEFAULT_CANVAS_ALPHA;
            _settings.ToolStartButtonAlpha = NoaDebuggerDefine.DEFAULT_TOOL_START_BUTTON_ALPHA;
            _settings.FloatingWindowAlpha = NoaDebuggerDefine.DEFAULT_CANVAS_ALPHA;
            _settings.ControllerBackgroundAlpha = NoaDebuggerDefine.DEFAULT_CONTROLLER_BACKGROUND_ALPHA;
            _settings.NoaDebuggerCanvasScale = NoaDebuggerDefine.DEFAULT_NOA_DEBUGGER_CANVAS_SCALE;
            _settings.NoaDebuggerCanvasSortOrder = NoaDebuggerDefine.DEFAULT_NOA_DEBUGGER_CANVAS_SORT_ORDER;
            _settings.IsUIReversePortrait = NoaDebuggerDefine.DEFAULT_IS_UI_REVERSE_PORTRAIT;
            _settings.IsShowSideMenuCloseButton = NoaDebuggerDefine.DEFAULT_IS_SHOW_SIDE_MENU_CLOSE_BUTTON;
        }
    }
}
