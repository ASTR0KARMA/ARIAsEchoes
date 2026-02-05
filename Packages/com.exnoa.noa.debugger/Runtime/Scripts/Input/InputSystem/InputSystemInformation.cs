#if INPUT_SYSTEM_PACKAGE_AVAILABLE
using System.Linq;
using UnityEngine.InputSystem;

namespace NoaDebugger
{
    public static class InputSystemInformation
    {
        public static bool HasTouchscreen => Touchscreen.current != null;

        public static bool HasMouse => Mouse.current != null;

        public static bool HasKeyboard => Keyboard.current != null;

        public static bool HasGamepad =>
            (from device in InputSystem.devices
             where device is Joystick or Gamepad
             select device.displayName).Any();
    }
}
#endif
