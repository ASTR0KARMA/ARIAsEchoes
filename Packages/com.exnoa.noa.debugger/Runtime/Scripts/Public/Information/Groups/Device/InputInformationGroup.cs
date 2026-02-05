using UnityEngine.XR;

namespace NoaDebugger
{
    /// <summary>
    /// Represents a collection of the input information.
    /// </summary>
    public sealed class InputInformationGroup
    {
        /// <summary>
        /// True if a touchscreen device is detected; otherwise false.
        /// </summary>
        public bool HasTouchscreen
        {
#if !ENABLE_LEGACY_INPUT_MANAGER && ENABLE_INPUT_SYSTEM
            get => InputSystemInformation.HasTouchscreen;
#else
            get => InputManagerInformation.HasTouchscreen;
#endif
        }

        /// <summary>
        /// True if a mouse device is detected; otherwise false.
        /// </summary>
        public bool HasMouse
        {
#if !ENABLE_LEGACY_INPUT_MANAGER && ENABLE_INPUT_SYSTEM
            get => InputSystemInformation.HasMouse;
#else
            get => InputManagerInformation.HasMouse;
#endif
        }

        /// <summary>
        /// True if a keyboard device is detected; otherwise false.
        /// </summary>
        public bool HasKeyboard
        {
#if !ENABLE_LEGACY_INPUT_MANAGER && ENABLE_INPUT_SYSTEM
            get => InputSystemInformation.HasKeyboard;
#else
            get => InputManagerInformation.HasKeyboard;
#endif
        }

        /// <summary>
        /// True if a joystick or gamepad device is detected; otherwise false.
        /// </summary>
        public bool HasGamepads
        {
#if !ENABLE_LEGACY_INPUT_MANAGER && ENABLE_INPUT_SYSTEM
            get => InputSystemInformation.HasGamepad;
#else
            get => InputManagerInformation.HasGamepad;
#endif
        }

        /// <summary>
        /// True if XR for the application is enabled; otherwise false.
        /// </summary>
        public bool EnabledXR
        {
            get
            {
#if USE_XR
                return XRSettings.enabled;
#else
                return false;
#endif
            }
        }
    }
}
