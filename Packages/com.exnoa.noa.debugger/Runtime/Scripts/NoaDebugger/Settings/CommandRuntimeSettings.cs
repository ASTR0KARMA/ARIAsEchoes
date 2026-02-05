using UnityEngine;
using System;
using NoaDebugger.DebugCommand;

namespace NoaDebugger
{
    [Serializable]
    sealed class CommandRuntimeSettings : FeatureToolSettingsBase
    {
        protected override string PlayerPrefsKeyPrefix => "Command";

        [SerializeField]
        EnumSettingParameter _commandFormatLandscape;

        [SerializeField]
        EnumSettingParameter _commandFormatPortrait;

        public EnumSettingParameter CommandFormatLandscape => _commandFormatLandscape;

        public EnumSettingParameter CommandFormatPortrait => _commandFormatPortrait;

        protected override void _InitializeSettings()
        {
            _commandFormatLandscape = new EnumSettingParameter(
                defaultValue: _noaDebuggerSettings.CommandFormatLandscape);

            _commandFormatPortrait = new EnumSettingParameter(
                defaultValue: _noaDebuggerSettings.CommandFormatPortrait);
        }

        protected override void _LoadSettings(string prefsJson)
        {
            var loadInfo = JsonUtility.FromJson<CommandRuntimeSettings>(prefsJson);

            if (loadInfo == null)
            {
                LogModel.LogWarning("Failed to parse JSON for CommandRuntimeSettings. Using default values.");
                _SetDefaultValue();
                return;
            }

            _commandFormatLandscape.ApplySavedSettings(loadInfo._commandFormatLandscape);
            _commandFormatPortrait.ApplySavedSettings(loadInfo._commandFormatPortrait);
        }

        protected override void _SetDefaultValue()
        {
            _commandFormatLandscape.SetDefaultValue();
            _commandFormatPortrait.SetDefaultValue();
        }

        protected override void _ResetSettings()
        {
            _commandFormatLandscape.ResetSettings();
            _commandFormatPortrait.ResetSettings();
        }

        public override void SaveCore()
        {
            base.SaveCore();
            _commandFormatLandscape.OnSaved();
            _commandFormatPortrait.OnSaved();
        }

        public override void ApplyCache(NoaDebuggerSettings originalSettings)
        {
            originalSettings.CommandFormatLandscape = (CommandDisplayFormat) _commandFormatLandscape.Value;
            originalSettings.CommandFormatPortrait = (CommandDisplayFormat) _commandFormatPortrait.Value;
        }

        public override bool IsValueChanged()
        {
            bool result = false;
            result |= _commandFormatLandscape.IsChanged;
            result |= _commandFormatPortrait.IsChanged;
            return result;
        }
    }
}
