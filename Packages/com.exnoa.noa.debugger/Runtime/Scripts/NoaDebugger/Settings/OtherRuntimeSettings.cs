using UnityEngine;

namespace NoaDebugger
{
     sealed class OtherRuntimeSettings : FeatureToolSettingsBase
     {
         protected override string PlayerPrefsKeyPrefix => "Other";

        bool _autoCreateEventSystem;

        [SerializeField]
        EnumSettingParameter _errorNotificationType;

        public bool AutoCreateEventSystem => _autoCreateEventSystem;

        public EnumSettingParameter ErrorNotificationType => _errorNotificationType;

        protected override void _InitializeSettings()
        {
            _autoCreateEventSystem = _noaDebuggerSettings.AutoCreateEventSystem;
            _errorNotificationType = new EnumSettingParameter(
                defaultValue: _noaDebuggerSettings.ErrorNotificationType);
        }

        protected override void _LoadSettings(string prefsJson)
        {
            var loadInfo = JsonUtility.FromJson<OtherRuntimeSettings>(prefsJson);

            if (loadInfo == null)
            {
                LogModel.LogWarning("Failed to parse JSON for OtherRuntimeSettings. Using default values.");
                _SetDefaultValue();
                return;
            }

            _errorNotificationType.ApplySavedSettings(loadInfo._errorNotificationType);
        }

        protected override void _SetDefaultValue()
        {
            _errorNotificationType.SetDefaultValue();
        }

        protected override void _ResetSettings()
        {
            _errorNotificationType.ResetSettings();
        }

        public override void SaveCore()
        {
            base.SaveCore();
            _errorNotificationType.OnSaved();
        }

        public override void ApplyCache(NoaDebuggerSettings originalSettings)
        {
            originalSettings.ErrorNotificationType = (ErrorNotificationType) _errorNotificationType.Value;
        }

        public override bool IsValueChanged()
        {
            return _errorNotificationType.IsChanged;
        }
    }
}
