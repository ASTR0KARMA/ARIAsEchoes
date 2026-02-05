using UnityEngine;

namespace NoaDebugger
{
     sealed class LoadingRuntimeSettings : FeatureToolSettingsBase
     {
         protected override string PlayerPrefsKeyPrefix => "Loading";

        bool _autoInitialize;

        public bool AutoInitialize => _autoInitialize;

        protected override void _InitializeSettings()
        {
            _autoInitialize = _noaDebuggerSettings.AutoInitialize;
        }

        protected override void _LoadSettings(string prefsJson)
        {
        }

        protected override void _SetDefaultValue()
        {
        }

        protected override void _ResetSettings()
        {
        }

        public override void ApplyCache(NoaDebuggerSettings originalSettings)
        {
        }

        public override bool IsValueChanged()
        {
            return false;
        }
    }
}
