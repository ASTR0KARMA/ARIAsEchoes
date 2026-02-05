using UnityEngine;

namespace NoaDebugger
{
     sealed class DisplayRuntimeSettings : FeatureToolSettingsBase
     {
         protected override string PlayerPrefsKeyPrefix => "Display";

        [SerializeField]
        EnumSettingParameter _startButtonPosition;

        [SerializeField]
        FloatSettingParameter _startButtonScale;

        [SerializeField]
        EnumSettingParameter _startButtonMovementType;

        [SerializeField]
        BoolSettingParameter _saveStartButtonPosition;

        [SerializeField]
        FloatSettingParameter _toolStartButtonAlpha;

        [SerializeField]
        FloatSettingParameter _backgroundAlpha;

        [SerializeField]
        FloatSettingParameter _floatingWindowAlpha;

        [SerializeField]
        FloatSettingParameter _controllerBackgroundAlpha;

        [SerializeField]
        FloatSettingParameter _noaDebuggerCanvasScale;

        int _noaDebuggerCanvasSortOrder;

        [SerializeField]
        BoolSettingParameter _isUIReversePortrait;

        [SerializeField]
        BoolSettingParameter _isShowSideMenuCloseButton;

        public EnumSettingParameter StartButtonPosition => _startButtonPosition;

        public FloatSettingParameter StartButtonScale => _startButtonScale;

        public EnumSettingParameter StartButtonMovementType => _startButtonMovementType;

        public BoolSettingParameter SaveStartButtonPosition => _saveStartButtonPosition;

        public FloatSettingParameter ToolStartButtonAlpha => _toolStartButtonAlpha;

        public FloatSettingParameter BackgroundAlpha => _backgroundAlpha;

        public FloatSettingParameter FloatingWindowAlpha => _floatingWindowAlpha;

        public FloatSettingParameter ControllerBackgroundAlpha => _controllerBackgroundAlpha;

        public FloatSettingParameter NoaDebuggerCanvasScale => _noaDebuggerCanvasScale;

        public int NoaDebuggerCanvasSortOrder => _noaDebuggerCanvasSortOrder;

        public BoolSettingParameter IsUIReversePortrait => _isUIReversePortrait;

        public BoolSettingParameter IsShowSideMenuCloseButton => _isShowSideMenuCloseButton;

        protected override void _InitializeSettings()
        {
            _startButtonPosition = new EnumSettingParameter(
                defaultValue: _noaDebuggerSettings.StartButtonPosition);

            _startButtonScale = new FloatSettingParameter(
                defaultValue: _noaDebuggerSettings.StartButtonScale,
                inputRangeMin: NoaDebuggerDefine.StartButtonScaleMin,
                inputRangeMax: NoaDebuggerDefine.StartButtonScaleMax,
                increment: NoaDebuggerDefine.DEFAULT_FLOAT_SETTINGS_INCREMENT);

            _startButtonMovementType = new EnumSettingParameter(
                defaultValue: _noaDebuggerSettings.StartButtonMovementType);

            _saveStartButtonPosition = new BoolSettingParameter(
                defaultValue: _noaDebuggerSettings.SaveStartButtonPosition);

            _toolStartButtonAlpha = new FloatSettingParameter(
                defaultValue: _noaDebuggerSettings.ToolStartButtonAlpha,
                inputRangeMin: NoaDebuggerDefine.ToolStartButtonAlphaMin,
                inputRangeMax: NoaDebuggerDefine.ToolStartButtonAlphaMax,
                increment: NoaDebuggerDefine.DEFAULT_FLOAT_SETTINGS_INCREMENT);

            _backgroundAlpha = new FloatSettingParameter(
                defaultValue: _noaDebuggerSettings.BackgroundAlpha,
                inputRangeMin: NoaDebuggerDefine.CanvasAlphaMin,
                inputRangeMax: NoaDebuggerDefine.CanvasAlphaMax,
                increment: NoaDebuggerDefine.DEFAULT_FLOAT_SETTINGS_INCREMENT);

            _floatingWindowAlpha = new FloatSettingParameter(
                defaultValue: _noaDebuggerSettings.FloatingWindowAlpha,
                inputRangeMin: NoaDebuggerDefine.CanvasAlphaMin,
                inputRangeMax: NoaDebuggerDefine.CanvasAlphaMax,
                increment: NoaDebuggerDefine.DEFAULT_FLOAT_SETTINGS_INCREMENT);

            _controllerBackgroundAlpha = new FloatSettingParameter(
                defaultValue: _noaDebuggerSettings.ControllerBackgroundAlpha,
                inputRangeMin: NoaDebuggerDefine.CanvasAlphaMin,
                inputRangeMax: NoaDebuggerDefine.CanvasAlphaMax,
                increment: NoaDebuggerDefine.DEFAULT_FLOAT_SETTINGS_INCREMENT);

            _noaDebuggerCanvasScale = new FloatSettingParameter(
                defaultValue: _noaDebuggerSettings.NoaDebuggerCanvasScale,
                inputRangeMin: NoaDebuggerDefine.CanvasScaleMin,
                inputRangeMax: NoaDebuggerDefine.CanvasScaleMax,
                increment: NoaDebuggerDefine.DEFAULT_FLOAT_SETTINGS_INCREMENT);

            _noaDebuggerCanvasSortOrder = _noaDebuggerSettings.NoaDebuggerCanvasSortOrder;

            _isUIReversePortrait = new BoolSettingParameter(
                defaultValue: _noaDebuggerSettings.IsUIReversePortrait);

            _isShowSideMenuCloseButton = new BoolSettingParameter(
                defaultValue: _noaDebuggerSettings.IsShowSideMenuCloseButton);
        }

        protected override void _LoadSettings(string prefsJson)
        {
            var loadInfo = JsonUtility.FromJson<DisplayRuntimeSettings>(prefsJson);

            if (loadInfo == null)
            {
                LogModel.LogWarning("Failed to parse JSON for DisplayRuntimeSettings. Using default values.");
                _SetDefaultValue();
                return;
            }

            _startButtonPosition.ApplySavedSettings(loadInfo.StartButtonPosition);
            _startButtonScale.ApplySavedSettings(loadInfo.StartButtonScale);
            _startButtonMovementType.ApplySavedSettings(loadInfo.StartButtonMovementType);
            _saveStartButtonPosition.ApplySavedSettings(loadInfo.SaveStartButtonPosition);
            _toolStartButtonAlpha.ApplySavedSettings(loadInfo.ToolStartButtonAlpha);
            _backgroundAlpha.ApplySavedSettings(loadInfo.BackgroundAlpha);
            _floatingWindowAlpha.ApplySavedSettings(loadInfo.FloatingWindowAlpha);
            _controllerBackgroundAlpha.ApplySavedSettings(loadInfo.ControllerBackgroundAlpha);
            _noaDebuggerCanvasScale.ApplySavedSettings(loadInfo.NoaDebuggerCanvasScale);
            _isUIReversePortrait.ApplySavedSettings(loadInfo.IsUIReversePortrait);
            _isShowSideMenuCloseButton.ApplySavedSettings(loadInfo.IsShowSideMenuCloseButton);
        }

        protected override void _SetDefaultValue()
        {
            _startButtonPosition.SetDefaultValue();
            _startButtonScale.SetDefaultValue();
            _startButtonMovementType.SetDefaultValue();
            _saveStartButtonPosition.SetDefaultValue();
            _toolStartButtonAlpha.SetDefaultValue();
            _backgroundAlpha.SetDefaultValue();
            _floatingWindowAlpha.SetDefaultValue();
            _controllerBackgroundAlpha.SetDefaultValue();
            _noaDebuggerCanvasScale.SetDefaultValue();
            _isUIReversePortrait.SetDefaultValue();
            _isShowSideMenuCloseButton.SetDefaultValue();
        }

        protected override void _ResetSettings()
        {
            _startButtonPosition.ResetSettings();
            _startButtonScale.ResetSettings();
            _startButtonMovementType.ResetSettings();
            _saveStartButtonPosition.ResetSettings();
            _toolStartButtonAlpha.ResetSettings();
            _backgroundAlpha.ResetSettings();
            _floatingWindowAlpha.ResetSettings();
            _controllerBackgroundAlpha.ResetSettings();
            _noaDebuggerCanvasScale.ResetSettings();
            _isUIReversePortrait.ResetSettings();
            _isShowSideMenuCloseButton.ResetSettings();
        }

        public override void SaveCore()
        {
            base.SaveCore();
            _startButtonPosition.OnSaved();
            _startButtonScale.OnSaved();
            _startButtonMovementType.OnSaved();
            _saveStartButtonPosition.OnSaved();
            _toolStartButtonAlpha.OnSaved();
            _backgroundAlpha.OnSaved();
            _floatingWindowAlpha.OnSaved();
            _controllerBackgroundAlpha.OnSaved();
            _noaDebuggerCanvasScale.OnSaved();
            _isUIReversePortrait.OnSaved();
            _isShowSideMenuCloseButton.OnSaved();
        }

        public override void ApplyCache(NoaDebuggerSettings originalSettings)
        {
            originalSettings.StartButtonPosition = (ButtonPosition) _startButtonPosition.Value;
            originalSettings.StartButtonScale = _startButtonScale.Value;
            originalSettings.StartButtonMovementType = (ButtonMovementType) _startButtonMovementType.Value;
            originalSettings.SaveStartButtonPosition = _saveStartButtonPosition.Value;
            originalSettings.ToolStartButtonAlpha = _toolStartButtonAlpha.Value;
            originalSettings.BackgroundAlpha = _backgroundAlpha.Value;
            originalSettings.FloatingWindowAlpha = _floatingWindowAlpha.Value;
            originalSettings.ControllerBackgroundAlpha = _controllerBackgroundAlpha.Value;
            originalSettings.NoaDebuggerCanvasScale = _noaDebuggerCanvasScale.Value;
            originalSettings.IsUIReversePortrait = _isUIReversePortrait.Value;
            originalSettings.IsShowSideMenuCloseButton = _isShowSideMenuCloseButton.Value;
        }

        public override bool IsValueChanged()
        {
            bool result = false;
            result |= _startButtonPosition.IsChanged;
            result |= _startButtonScale.IsChanged;
            result |= _startButtonMovementType.IsChanged;
            result |= _saveStartButtonPosition.IsChanged;
            result |= _toolStartButtonAlpha.IsChanged;
            result |= _backgroundAlpha.IsChanged;
            result |= _floatingWindowAlpha.IsChanged;
            result |= _controllerBackgroundAlpha.IsChanged;
            result |= _noaDebuggerCanvasScale.IsChanged;
            result |= _isUIReversePortrait.IsChanged;
            result |= _isShowSideMenuCloseButton.IsChanged;
            return result;
        }
     }
}
