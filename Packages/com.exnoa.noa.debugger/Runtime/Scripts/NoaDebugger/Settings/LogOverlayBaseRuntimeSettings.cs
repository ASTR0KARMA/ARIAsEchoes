using System;
using UnityEngine;

namespace NoaDebugger
{
    [Serializable]
    class LogOverlayBaseRuntimeSettings : OverlayToolSettingsBase
    {
        protected override string PlayerPrefsKeyPrefix
        {
            get
            {
                LogModel.DebugLogWarning("PlayerPrefsKeyPrefixを実装してください。");
                return "";
            }
        }

        protected override NoaDebug.OverlayPosition DefaultPosition
        {
            get
            {
                if (_defaultGetter == null)
                {
                    LogModel.Log("DefaultGetter is not set.");
                    return NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_POSITION;
                }
                return _defaultGetter.DefaultPosition;
            }
        }

        public FloatSettingParameter FontScale => _fontScale;
        [SerializeField]
        FloatSettingParameter _fontScale;

        public IntSettingParameter MaximumLogCount => _maximumLogCount;
        [SerializeField]
        IntSettingParameter _maximumLogCount;

        public FloatSettingParameter MinimumOpacity => _minimumOpacity;
        [SerializeField]
        FloatSettingParameter _minimumOpacity;

        public FloatSettingParameter ActiveDuration => _activeDuration;
        [SerializeField]
        FloatSettingParameter _activeDuration;

        public BoolSettingParameter ShowTimestamp => _showTimestamp;
        [SerializeField]
        BoolSettingParameter _showTimestamp;

        public BoolSettingParameter ShowMessageLogs => _showMessageLogs;
        [SerializeField]
        BoolSettingParameter _showMessageLogs;

        public BoolSettingParameter ShowWarningLogs => _showWarningLogs;
        [SerializeField]
        BoolSettingParameter _showWarningLogs;

        public BoolSettingParameter ShowErrorLogs => _showErrorLogs;
        [SerializeField]
        BoolSettingParameter _showErrorLogs;

        LogOverlaySettingsDefaultGetter _defaultGetter;

        public void ReceiveDefaultGetter(LogOverlaySettingsDefaultGetter defaultGetter)
        {
            _defaultGetter = defaultGetter;
        }

        protected override void _InitializeSettings()
        {
            base._InitializeSettings();

            if (_defaultGetter == null)
            {
                LogModel.LogWarning("DefaultGetter is not set.");
                return;
            }

            _fontScale = new FloatSettingParameter(
                defaultValue: _defaultGetter.DefaultFontScale,
                inputRangeMin: NoaDebuggerDefine.LogOverlayFontScaleMin,
                inputRangeMax: NoaDebuggerDefine.LogOverlayFontScaleMax,
                increment: NoaDebuggerDefine.LogOverlayFontScaleIncrement);

            _maximumLogCount = new IntSettingParameter(
                defaultValue: _defaultGetter.DefaultMaximumLogCount,
                inputRangeMin: NoaDebuggerDefine.LogOverlayMaximumLogCountMin,
                inputRangeMax: NoaDebuggerDefine.LogOverlayMaximumLogCountMax,
                increment: NoaDebuggerDefine.LogOverlayMaximumLogCountIncrement);

            _minimumOpacity = new FloatSettingParameter(
                defaultValue: _defaultGetter.DefaultMinimumOpacity,
                inputRangeMin: NoaDebuggerDefine.LogOverlayMinimumOpacityMin,
                inputRangeMax: NoaDebuggerDefine.LogOverlayMinimumOpacityMax,
                increment: NoaDebuggerDefine.LogOverlayMinimumOpacityIncrement);

            _activeDuration = new FloatSettingParameter(
                defaultValue: _defaultGetter.DefaultActiveDuration,
                inputRangeMin: NoaDebuggerDefine.LogOverlayActiveDurationMin,
                inputRangeMax: NoaDebuggerDefine.LogOverlayActiveDurationMax,
                increment: NoaDebuggerDefine.LogOverlayActiveDurationIncrement);

            _showTimestamp = new BoolSettingParameter(
                defaultValue: _defaultGetter.DefaultShowTimestamp);

            _showMessageLogs = new BoolSettingParameter(
                defaultValue: _defaultGetter.DefaultShowMessageLogs);

            _showWarningLogs = new BoolSettingParameter(
                defaultValue: _defaultGetter.DefaultShowWarningLogs);

            _showErrorLogs = new BoolSettingParameter(
                defaultValue: _defaultGetter.DefaultShowErrorLogs);
        }

        protected override void _LoadSettings(string prefsJson)
        {
            LogModel.DebugLogWarning("_LoadSettingsInternalを実装してください。");
        }

        protected void LoadSettingsFromJson<T>(string overlayPrefsJson) where T : LogOverlayBaseRuntimeSettings
        {
            var loadInfo = JsonUtility.FromJson<T>(overlayPrefsJson);

            if (loadInfo == null)
            {
                LogModel.LogWarning("Failed to parse JSON for LogOverlayBaseRuntimeSettings. Using default values.");
                _SetDefaultValue();
                return;
            }

            _position.ApplySavedSettings(loadInfo._position);
            _fontScale.ApplySavedSettings(loadInfo._fontScale);
            _maximumLogCount.ApplySavedSettings(loadInfo._maximumLogCount);
            _minimumOpacity.ApplySavedSettings(loadInfo._minimumOpacity);
            _activeDuration.ApplySavedSettings(loadInfo._activeDuration);
            _showTimestamp.ApplySavedSettings(loadInfo._showTimestamp);
            _showMessageLogs.ApplySavedSettings(loadInfo._showMessageLogs);
            _showWarningLogs.ApplySavedSettings(loadInfo._showWarningLogs);
            _showErrorLogs.ApplySavedSettings(loadInfo._showErrorLogs);
        }

        protected override void _SetDefaultValue()
        {
            base._SetDefaultValue();
            _fontScale.SetDefaultValue();
            _maximumLogCount.SetDefaultValue();
            _minimumOpacity.SetDefaultValue();
            _activeDuration.SetDefaultValue();
            _showTimestamp.SetDefaultValue();
            _showMessageLogs.SetDefaultValue();
            _showWarningLogs.SetDefaultValue();
            _showErrorLogs.SetDefaultValue();
        }

        protected override void _ResetSettings()
        {
            base._ResetSettings();
            _fontScale.ResetSettings();
            _maximumLogCount.ResetSettings();
            _minimumOpacity.ResetSettings();
            _activeDuration.ResetSettings();
            _showTimestamp.ResetSettings();
            _showMessageLogs.ResetSettings();
            _showWarningLogs.ResetSettings();
            _showErrorLogs.ResetSettings();
        }

        public override void SaveCore()
        {
            base.SaveCore();
            _fontScale.OnSaved();
            _maximumLogCount.OnSaved();
            _minimumOpacity.OnSaved();
            _activeDuration.OnSaved();
            _showTimestamp.OnSaved();
            _showMessageLogs.OnSaved();
            _showWarningLogs.OnSaved();
            _showErrorLogs.OnSaved();
        }

        public override void ApplyCache(NoaDebuggerSettings originalSettings)
        {
            if (GetType() == typeof(ConsoleLogOverlayRuntimeSettings))
            {
                originalSettings.ConsoleLogOverlayFontScale = _fontScale.Value;
                originalSettings.ConsoleLogOverlayMaximumLogCount = _maximumLogCount.Value;
                originalSettings.ConsoleLogOverlayMinimumOpacity = _minimumOpacity.Value;
                originalSettings.ConsoleLogOverlayActiveDuration = _activeDuration.Value;
                originalSettings.ConsoleLogOverlayShowTimestamp = _showTimestamp.Value;
                originalSettings.ConsoleLogOverlayShowMessageLogs = _showMessageLogs.Value;
                originalSettings.ConsoleLogOverlayShowWarningLogs = _showWarningLogs.Value;
                originalSettings.ConsoleLogOverlayShowErrorLogs = _showErrorLogs.Value;
                originalSettings.ConsoleLogOverlayPosition = (NoaDebug.OverlayPosition) _position.Value;
            }
            else if(GetType() == typeof(ApiLogOverlayRuntimeSettings))
            {
                originalSettings.ApiLogOverlayFontScale = _fontScale.Value;
                originalSettings.ApiLogOverlayMaximumLogCount = _maximumLogCount.Value;
                originalSettings.ApiLogOverlayMinimumOpacity = _minimumOpacity.Value;
                originalSettings.ApiLogOverlayActiveDuration = _activeDuration.Value;
                originalSettings.ApiLogOverlayShowTimestamp = _showTimestamp.Value;
                originalSettings.ApiLogOverlayShowMessageLogs = _showMessageLogs.Value;
                originalSettings.ApiLogOverlayShowErrorLogs = _showErrorLogs.Value;
                originalSettings.ApiLogOverlayPosition = (NoaDebug.OverlayPosition) _position.Value;
            }
        }

        public override bool IsValueChanged()
        {
            bool result = base.IsValueChanged();
            result |= _fontScale.IsChanged;
            result |= _maximumLogCount.IsChanged;
            result |= _minimumOpacity.IsChanged;
            result |= _activeDuration.IsChanged;
            result |= _showTimestamp.IsChanged;
            result |= _showMessageLogs.IsChanged;
            result |= _showWarningLogs.IsChanged;
            result |= _showErrorLogs.IsChanged;
            return result;
        }
    }
}
