using UnityEngine;
using System;

namespace NoaDebugger
{
    [Serializable]
    sealed class HierarchyRuntimeSettings : FeatureToolSettingsBase
    {
        protected override string PlayerPrefsKeyPrefix => "Hierarchy";

        [SerializeField]
        IntSettingParameter _hierarchyLevels;

        public IntSettingParameter HierarchyLevels => _hierarchyLevels;

        protected override void _InitializeSettings()
        {
            _hierarchyLevels = new IntSettingParameter(
                defaultValue: _noaDebuggerSettings.HierarchyLevels,
                inputRangeMin: NoaDebuggerDefine.HierarchyLevelsMin,
                inputRangeMax: NoaDebuggerDefine.HierarchyLevelsMax,
                increment: NoaDebuggerDefine.DEFAULT_INT_SETTINGS_INCREMENT);
        }

        protected override void _LoadSettings(string prefsJson)
        {
            var loadInfo = JsonUtility.FromJson<HierarchyRuntimeSettings>(prefsJson);

            if (loadInfo == null)
            {
                LogModel.LogWarning("Failed to parse JSON for HierarchyRuntimeSettings. Using default values.");
                _SetDefaultValue();
                return;
            }

            _hierarchyLevels.ApplySavedSettings(loadInfo._hierarchyLevels);
        }

        protected override void _SetDefaultValue()
        {
            _hierarchyLevels.SetDefaultValue();
        }

        protected override void _ResetSettings()
        {
            _hierarchyLevels.ResetSettings();
        }

        public override void SaveCore()
        {
            base.SaveCore();
            _hierarchyLevels.OnSaved();
        }

        public override void ApplyCache(NoaDebuggerSettings originalSettings)
        {
            originalSettings.HierarchyLevels = _hierarchyLevels.Value;
        }

        public override bool IsValueChanged()
        {
            return _hierarchyLevels.IsChanged;
        }
    }
}
