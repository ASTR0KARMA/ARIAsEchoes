using System;

namespace NoaDebugger
{
    [Serializable]
    sealed class ConsoleLogOverlayRuntimeSettings : LogOverlayBaseRuntimeSettings
    {
        protected override string PlayerPrefsKeyPrefix => NoaDebuggerPrefsDefine.PrefsKeyPrefixConsoleLogOverlaySettings;

        protected override void _InitializeSettings()
        {
            var getter = new ConsoleLogOverlaySettingsDefaultGetter();
            ReceiveDefaultGetter(getter);
            base._InitializeSettings();
        }

        protected override void _LoadSettings(string prefsJson)
        {
            LoadSettingsFromJson<ConsoleLogOverlayRuntimeSettings>(prefsJson);
        }
    }
}
