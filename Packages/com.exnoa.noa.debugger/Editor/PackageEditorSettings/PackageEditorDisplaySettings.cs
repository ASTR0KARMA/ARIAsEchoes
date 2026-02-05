#if NOA_DEBUGGER
using UnityEditor;
using UnityEngine;

namespace NoaDebugger
{
    sealed class PackageEditorDisplaySettings : PackageEditorSettingsBase
    {
        ButtonPosition _startButtonPosition;

        float _startButtonScale;

        ButtonMovementType _startButtonMovementType;

        bool _saveStartButtonPosition;

        float _toolStartButtonAlpha;

        float _backgroundAlpha;

        float _floatingWindowAlpha;

        float _controllerBackgroundAlpha;

        float _noaDebuggerCanvasScale;

        int _noaDebuggerCanvasSortOrder;

        bool _isUIReversePortrait;

        bool _isShowSideMenuCloseButton;

        public PackageEditorDisplaySettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void ResetTmpDataWithSettings()
        {
            _startButtonPosition = _settings.StartButtonPosition;
            _startButtonScale = _settings.StartButtonScale;
            _startButtonMovementType = _settings.StartButtonMovementType;
            _saveStartButtonPosition = _settings.SaveStartButtonPosition;

            _toolStartButtonAlpha = _settings.ToolStartButtonAlpha;
            _backgroundAlpha = _settings.BackgroundAlpha;
            _floatingWindowAlpha = _settings.FloatingWindowAlpha;
            _controllerBackgroundAlpha = _settings.ControllerBackgroundAlpha;

            _noaDebuggerCanvasScale = _settings.NoaDebuggerCanvasScale;
            _noaDebuggerCanvasSortOrder = _settings.NoaDebuggerCanvasSortOrder;
            _isUIReversePortrait = _settings.IsUIReversePortrait;
            _isShowSideMenuCloseButton = _settings.IsShowSideMenuCloseButton;
        }

        public override void ApplySettings()
        {
            _settings.StartButtonPosition = _startButtonPosition;
            _settings.StartButtonScale = _startButtonScale;
            _settings.StartButtonMovementType = _startButtonMovementType;
            _settings.SaveStartButtonPosition = _saveStartButtonPosition;
            _settings.ToolStartButtonAlpha = _toolStartButtonAlpha;
            _settings.BackgroundAlpha = _backgroundAlpha;
            _settings.FloatingWindowAlpha = _floatingWindowAlpha;
            _settings.ControllerBackgroundAlpha = _controllerBackgroundAlpha;
            _settings.NoaDebuggerCanvasScale = _noaDebuggerCanvasScale;
            _settings.NoaDebuggerCanvasSortOrder = _noaDebuggerCanvasSortOrder;
            _settings.IsUIReversePortrait = _isUIReversePortrait;
            _settings.IsShowSideMenuCloseButton = _isShowSideMenuCloseButton;
        }

        public override void ResetDefault()
        {
            _startButtonPosition = NoaDebuggerDefine.DEFAULT_START_BUTTON_POSITION;
            _startButtonScale = NoaDebuggerDefine.DEFAULT_START_BUTTON_SCALE;
            _startButtonMovementType = NoaDebuggerDefine.DEFAULT_START_BUTTON_MOVEMENT_TYPE;
            _saveStartButtonPosition = NoaDebuggerDefine.DEFAULT_SAVE_START_BUTTON_POSITION;
            _toolStartButtonAlpha = NoaDebuggerDefine.DEFAULT_TOOL_START_BUTTON_ALPHA;
            _backgroundAlpha = NoaDebuggerDefine.DEFAULT_CANVAS_ALPHA;
            _floatingWindowAlpha = NoaDebuggerDefine.DEFAULT_CANVAS_ALPHA;
            _controllerBackgroundAlpha = NoaDebuggerDefine.DEFAULT_CONTROLLER_BACKGROUND_ALPHA;
            _noaDebuggerCanvasScale = NoaDebuggerDefine.DEFAULT_NOA_DEBUGGER_CANVAS_SCALE;
            _noaDebuggerCanvasSortOrder = NoaDebuggerDefine.DEFAULT_NOA_DEBUGGER_CANVAS_SORT_ORDER;
            _isUIReversePortrait = NoaDebuggerDefine.DEFAULT_IS_UI_REVERSE_PORTRAIT;
            _isShowSideMenuCloseButton = NoaDebuggerDefine.DEFAULT_IS_SHOW_SIDE_MENU_CLOSE_BUTTON;
        }

        public override void DrawGUI()
        {
            _DisplaySettingsCategoryHeader("Display", ResetDefault);

            _startButtonPosition = (ButtonPosition)EditorGUILayout.EnumPopup("Start button position", _startButtonPosition);
            _startButtonScale = EditorGUILayout.Slider(
                "Start button scale",
                _startButtonScale,
                NoaDebuggerDefine.StartButtonScaleMin,
                NoaDebuggerDefine.StartButtonScaleMax);
            _startButtonMovementType = (ButtonMovementType)EditorGUILayout.EnumPopup(new GUIContent("Start button movement type*", "Specifies whether the NOA Debugger start button is draggable or fixed. Options include: Draggable, Fixed."), _startButtonMovementType);
            _saveStartButtonPosition = EditorGUILayout.Toggle("Save start button position", _saveStartButtonPosition);

            _toolStartButtonAlpha = EditorGUILayout.Slider(
                "Start button opacity",
                _toolStartButtonAlpha,
                NoaDebuggerDefine.ToolStartButtonAlphaMin,
                NoaDebuggerDefine.ToolStartButtonAlphaMax);

            _backgroundAlpha = EditorGUILayout.Slider(
                "Background opacity",
                _backgroundAlpha,
                NoaDebuggerDefine.CanvasAlphaMin,
                NoaDebuggerDefine.CanvasAlphaMax);

            _floatingWindowAlpha = EditorGUILayout.Slider(
                "Floating window opacity",
                _floatingWindowAlpha,
                NoaDebuggerDefine.CanvasAlphaMin,
                NoaDebuggerDefine.CanvasAlphaMax);

            _controllerBackgroundAlpha = EditorGUILayout.Slider(
                "Controller background opacity",
                _controllerBackgroundAlpha,
                NoaDebuggerDefine.CanvasAlphaMin,
                NoaDebuggerDefine.CanvasAlphaMax);

            _noaDebuggerCanvasScale = EditorGUILayout.Slider(
                "Canvas scale",
                _noaDebuggerCanvasScale,
                NoaDebuggerDefine.CanvasScaleMin,
                NoaDebuggerDefine.CanvasScaleMax);

            _noaDebuggerCanvasSortOrder = EditorGUILayout.IntField("Canvas sort order", _noaDebuggerCanvasSortOrder);
            _isUIReversePortrait = EditorGUILayout.Toggle(new GUIContent("Optimize UI for portrait*", "Optimizes the display order of the NOA Debugger's UI elements for mobile devices when displayed in portrait orientation. If this setting is enabled, some UI elements are arranged in reverse order from the bottom to the top of the screen, allowing for easier access from the bottom of the screen."), _isUIReversePortrait);

            _isShowSideMenuCloseButton = EditorGUILayout.Toggle("Show side menu close button", _isShowSideMenuCloseButton);
        }
    }
}
#endif
