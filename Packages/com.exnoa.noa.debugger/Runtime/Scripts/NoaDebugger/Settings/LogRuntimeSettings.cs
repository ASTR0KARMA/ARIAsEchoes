using UnityEngine;

namespace NoaDebugger
{
     sealed class LogRuntimeSettings : FeatureToolSettingsBase
     {
         protected override string PlayerPrefsKeyPrefix => "Log";

        [SerializeField]
        IntSettingParameter _consoleLogCount;

        [SerializeField]
        IntSettingParameter _apiLogCount;

        public IntSettingParameter ConsoleLogCount => _consoleLogCount;

        public IntSettingParameter ApiLogCount => _apiLogCount;

        protected override void _InitializeSettings()
        {
            _consoleLogCount = new IntSettingParameter(
                defaultValue: _noaDebuggerSettings.ConsoleLogCount,
                inputRangeMin: NoaDebuggerDefine.ConsoleLogCountMin,
                inputRangeMax: NoaDebuggerDefine.ConsoleLogCountMax,
                increment: NoaDebuggerDefine.DEFAULT_INT_SETTINGS_INCREMENT);

            _apiLogCount = new IntSettingParameter(
                defaultValue: _noaDebuggerSettings.ApiLogCount,
                inputRangeMin: NoaDebuggerDefine.ApiLogCountMin,
                inputRangeMax: NoaDebuggerDefine.ApiLogCountMax,
                increment: NoaDebuggerDefine.DEFAULT_INT_SETTINGS_INCREMENT);
        }

        protected override void _LoadSettings(string prefsJson)
        {
            var loadInfo = JsonUtility.FromJson<LogRuntimeSettings>(prefsJson);

            if (loadInfo == null)
            {
                LogModel.LogWarning("Failed to parse JSON for LogRuntimeSettings. Using default values.");
                _SetDefaultValue();
                return;
            }

            _consoleLogCount.ApplySavedSettings(loadInfo.ConsoleLogCount);
            _apiLogCount.ApplySavedSettings(loadInfo.ApiLogCount);
        }

        protected override void _SetDefaultValue()
        {
            _consoleLogCount.SetDefaultValue();
            _apiLogCount.SetDefaultValue();
        }

        protected override void _ResetSettings()
        {
            _consoleLogCount.ResetSettings();
            _apiLogCount.ResetSettings();
        }

        public override void SaveCore()
        {
            base.SaveCore();
            _consoleLogCount.OnSaved();
            _apiLogCount.OnSaved();
        }

        public override void ApplyCache(NoaDebuggerSettings originalSettings)
        {
            originalSettings.ApiLogCount = _apiLogCount.Value;
            originalSettings.ConsoleLogCount = _consoleLogCount.Value;
        }

        public override bool IsValueChanged()
        {
            bool result = false;
            result |= _consoleLogCount.IsChanged;
            result |= _apiLogCount.IsChanged;
            return result;
        }
    }
}
