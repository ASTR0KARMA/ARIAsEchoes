using UnityEngine;

namespace NoaDebugger
{
    sealed class DisplaySettingsGroupView : MutableSettingsViewBase
    {
        [SerializeField] EnumSettingsPanel _startButtonPosition;
        [SerializeField] FloatSettingsPanel _startButtonScale;
        [SerializeField] EnumSettingsPanel _startButtonMovementType;
        [SerializeField] BoolSettingsPanel _saveStartButtonPosition;
        [SerializeField] FloatSettingsPanel _toolStartButtonAlpha;
        [SerializeField] FloatSettingsPanel _backgroundAlpha;
        [SerializeField] FloatSettingsPanel _floatingWindowAlpha;
        [SerializeField] FloatSettingsPanel _controllerBackgroundAlpha;
        [SerializeField] FloatSettingsPanel _noaDebuggerCanvasScale;
        [SerializeField] ReadOnlySettingsPanel _noaDebuggerCanvasSortOrder;
        [SerializeField] BoolSettingsPanel _isUIReversePortrait;
        [SerializeField] BoolSettingsPanel _isShowSideMenuCloseButton;

        protected override SettingsResetButtonType ResetButtonType
        {
            get { return SettingsResetButtonType.Display; }
        }

        public override void Initialize(SettingsViewLinker linker)
        {
            var settings = linker._displaySettings;
            _startButtonPosition.Initialize(settings.StartButtonPosition);
            _startButtonScale.Initialize(settings.StartButtonScale);
            _startButtonMovementType.Initialize(settings.StartButtonMovementType);
            _saveStartButtonPosition.Initialize(settings.SaveStartButtonPosition);
            _toolStartButtonAlpha.Initialize(settings.ToolStartButtonAlpha);
            _backgroundAlpha.Initialize(settings.BackgroundAlpha);
            _floatingWindowAlpha.Initialize(settings.FloatingWindowAlpha);
            _controllerBackgroundAlpha.Initialize(settings.ControllerBackgroundAlpha);
            _noaDebuggerCanvasScale.Initialize(settings.NoaDebuggerCanvasScale);
            _noaDebuggerCanvasSortOrder.Initialize(settings.NoaDebuggerCanvasSortOrder.ToString());
            _isUIReversePortrait.Initialize(settings.IsUIReversePortrait);
            _isShowSideMenuCloseButton.Initialize(settings.IsShowSideMenuCloseButton);
        }
    }
}
