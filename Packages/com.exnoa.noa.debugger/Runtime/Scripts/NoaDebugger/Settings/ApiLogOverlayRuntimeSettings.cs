using System;

namespace NoaDebugger
{
    [Serializable]
    sealed class ApiLogOverlayRuntimeSettings : LogOverlayBaseRuntimeSettings
    {
        protected override string PlayerPrefsKeyPrefix => NoaDebuggerPrefsDefine.PrefsKeyPrefixApiLogOverlaySettings;

        protected override void _InitializeSettings()
        {
            var getter = new ApiLogOverlaySettingsDefaultGetter();
            ReceiveDefaultGetter(getter);
            base._InitializeSettings();
        }

        protected override void _LoadSettings(string prefsJson)
        {
            LoadSettingsFromJson<ApiLogOverlayRuntimeSettings>(prefsJson);
        }
    }
}
