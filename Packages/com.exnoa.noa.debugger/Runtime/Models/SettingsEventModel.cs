using UnityEngine.Events;

namespace NoaDebugger
{
    static class SettingsEventModel
    {
        public static UnityAction OnUpdateDisplaySettings;

        public static UnityAction OnUpdateGameSpeedSettings;

        public static UnityAction OnUpdateLogSettings;

        public static UnityAction OnUpdateCommonOverlaySettings;

        public static UnityAction OnUpdateHierarchySettings;

        public static UnityAction OnUpdateCommandSettings;

        public static UnityAction OnUpdateUIElementSettings;

        public static UnityAction OnUpdateShortcutSettings;

        public static UnityAction OnSettingsValueChanged;
    }
}
