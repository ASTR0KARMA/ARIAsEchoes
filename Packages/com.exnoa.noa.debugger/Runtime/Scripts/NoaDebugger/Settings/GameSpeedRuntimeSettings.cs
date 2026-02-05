using System;
using UnityEngine;

namespace NoaDebugger
{
    [Serializable]
    sealed class GameSpeedRuntimeSettings : FeatureToolSettingsBase
    {
        protected override string PlayerPrefsKeyPrefix => "GameSpeed";

        [SerializeField]
        BoolSettingParameter _appliesGameSpeedChange;

        [SerializeField]
        FloatSettingParameter _gameSpeedIncrement;

        [SerializeField]
        FloatSettingParameter _maxGameSpeed;

        public BoolSettingParameter AppliesGameSpeedChange => _appliesGameSpeedChange;

        public FloatSettingParameter GameSpeedIncrement => _gameSpeedIncrement;

        public FloatSettingParameter MaxGameSpeed => _maxGameSpeed;

        protected override void _InitializeSettings()
        {
            _appliesGameSpeedChange = new BoolSettingParameter(
                defaultValue: _noaDebuggerSettings.AppliesGameSpeedChange);

            _gameSpeedIncrement = new FloatSettingParameter(
                defaultValue: _noaDebuggerSettings.GameSpeedIncrement,
                inputRangeMin: NoaDebuggerDefine.MIN_GAME_SPEED_INCREMENT,
                inputRangeMax: NoaDebuggerDefine.MAX_GAME_SPEED_INCREMENT,
                increment: NoaDebuggerDefine.DEFAULT_FLOAT_SETTINGS_INCREMENT);

            _maxGameSpeed = new FloatSettingParameter(
                defaultValue: _noaDebuggerSettings.MaxGameSpeed,
                inputRangeMin: NoaDebuggerDefine.MIN_CONFIGURABLE_MAX_GAME_SPEED,
                inputRangeMax: NoaDebuggerDefine.MAX_CONFIGURABLE_MAX_GAME_SPEED,
                increment: NoaDebuggerDefine.DEFAULT_FLOAT_SETTINGS_INCREMENT);
        }

        protected override void _LoadSettings(string prefsJson)
        {
            var loadInfo = JsonUtility.FromJson<GameSpeedRuntimeSettings>(prefsJson);

            if (loadInfo == null)
            {
                LogModel.LogWarning("Failed to parse JSON for GameSpeedRuntimeSettings. Using default values.");
                _SetDefaultValue();
                return;
            }

            _appliesGameSpeedChange.ApplySavedSettings(loadInfo._appliesGameSpeedChange);
            _gameSpeedIncrement.ApplySavedSettings(loadInfo._gameSpeedIncrement);
            _maxGameSpeed.ApplySavedSettings(loadInfo._maxGameSpeed);
        }

        protected override void _SetDefaultValue()
        {
            _appliesGameSpeedChange.SetDefaultValue();
            _gameSpeedIncrement.SetDefaultValue();
            _maxGameSpeed.SetDefaultValue();
        }

        protected override void _ResetSettings()
        {
            _appliesGameSpeedChange.ResetSettings();
            _gameSpeedIncrement.ResetSettings();
            _maxGameSpeed.ResetSettings();
        }

        public override void SaveCore()
        {
            base.SaveCore();
            _appliesGameSpeedChange.OnSaved();
            _gameSpeedIncrement.OnSaved();
            _maxGameSpeed.OnSaved();
        }

        public override void ApplyCache(NoaDebuggerSettings originalSettings)
        {
            originalSettings.GameSpeedIncrement = GameSpeedIncrement.Value;
            originalSettings.MaxGameSpeed = MaxGameSpeed.Value;
            originalSettings.AppliesGameSpeedChange = AppliesGameSpeedChange.Value;
        }

        public override bool IsValueChanged()
        {
            bool result = false;
            result |= _appliesGameSpeedChange.IsChanged;
            result |= _gameSpeedIncrement.IsChanged;
            result |= _maxGameSpeed.IsChanged;
            return result;
        }
    }
}
