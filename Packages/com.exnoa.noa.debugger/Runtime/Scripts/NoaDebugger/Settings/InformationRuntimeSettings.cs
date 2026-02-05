using UnityEngine;

namespace NoaDebugger
{
    sealed class InformationRuntimeSettings : FeatureToolSettingsBase
    {
        [SerializeField]
        BoolSettingParameter _saveChanges;

        public BoolSettingParameter SaveChanges => _saveChanges;

        protected override string PlayerPrefsKeyPrefix => "Information";

        protected override void _InitializeSettings()
        {
            _saveChanges = new BoolSettingParameter(
                defaultValue: _noaDebuggerSettings.SaveInformationValue);
        }

        protected override void _LoadSettings(string prefsJson)
        {
            var loadInfo = JsonUtility.FromJson<InformationRuntimeSettings>(prefsJson);

            if (loadInfo == null)
            {
                LogModel.LogWarning("Failed to parse JSON for InformationRuntimeSettings. Using default values.");
                _SetDefaultValue();
                return;
            }

            _saveChanges.ApplySavedSettings(loadInfo._saveChanges);
        }

        protected override void _SetDefaultValue()
        {
            _saveChanges.SetDefaultValue();
        }

        protected override void _ResetSettings()
        {
            _saveChanges.ResetSettings();
        }

        public override void SaveCore()
        {
            base.SaveCore();
            _saveChanges.OnSaved();
        }

        public override void ApplyCache(NoaDebuggerSettings originalSettings)
        {
            originalSettings.SaveInformationValue = _saveChanges.Value;
        }

        public override bool IsValueChanged()
        {
            return _saveChanges.IsChanged;
        }
    }
}
