#if ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine;

namespace NoaDebugger
{
    public static class InputManagerInformation
    {
        public static bool HasTouchscreen => Input.touchSupported;

        public static bool HasMouse => Input.mousePresent;

        public static bool HasKeyboard => Input.anyKeyDown;

        public static bool HasGamepad => Input.GetJoystickNames().Length > 0;
    }
}
#endif
