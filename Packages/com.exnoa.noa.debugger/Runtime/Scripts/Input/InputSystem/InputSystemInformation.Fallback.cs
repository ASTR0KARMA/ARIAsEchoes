#if !INPUT_SYSTEM_PACKAGE_AVAILABLE
namespace NoaDebugger
{
    public static class InputSystemInformation
    {
        public static bool HasTouchscreen => false;

        public static bool HasMouse => false;

        public static bool HasKeyboard => false;

        public static bool HasGamepad => false;
    }
}
#endif
