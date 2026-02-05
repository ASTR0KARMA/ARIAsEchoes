using UnityEngine;

namespace NoaDebugger
{
    static class UnityInputUtils
    {
        readonly static UnityInputUtilsInternal Internal = new UnityInputUtilsInternal();

        public static readonly bool IsEnableInputSystem
#if !ENABLE_LEGACY_INPUT_MANAGER && ENABLE_INPUT_SYSTEM
            = true;
#else
            = false;
#endif

        public static readonly float ScrollSensitivity
#if !ENABLE_LEGACY_INPUT_MANAGER && ENABLE_INPUT_SYSTEM
#if !UNITY_EDITOR && UNITY_WEBGL
            = 0.02f;
#elif UNITY_EDITOR_OSX
            = 0.2f;
#else
            = 0.04f;
#endif
#else
            = 25f;
#endif

        public static void AddInputModule(GameObject target) => UnityInputUtils.Internal.AddInputModule(target);

        public static void OnShortcutHandlerInitialize() => UnityInputUtils.Internal.OnShortcutHandlerInitialize();

        public static bool IsShortcutKeyDown(ShortcutAction action) =>
            UnityInputUtils.Internal.IsShortcutKeyDown(action.combination.keyboard);

        public static bool IsShortcutKeyHeld(ShortcutAction action) =>
            UnityInputUtils.Internal.IsShortcutKeyHeld(action.combination.keyboard);

        public static bool IsShortcutKeyUp(ShortcutAction action) =>
            UnityInputUtils.Internal.IsShortcutKeyUp(action.combination.keyboard);

        public static string GetKeyTextFromInt(int keyNum) => UnityInputUtils.Internal.GetKeyTextFromInt(keyNum);

        public static int GetCurrentDefaultKey(ShortcutActionType actionType) =>
            UnityInputUtils.Internal.GetDefaultKeyFromActionType(actionType);

        public static int GetDefaultShortcutKey(ShortcutActionType actionType, bool forInputSystem)
        {
            if (UnityInputUtils.IsEnableInputSystem)
            {
                return forInputSystem
                    ? UnityInputUtils.Internal.GetDefaultKeyFromActionType(actionType)
                    : NoaDebuggerDefine.ShortcutInvalidKey;
            }
            else
            {
                return forInputSystem
                    ? NoaDebuggerDefine.ShortcutInvalidKey
                    : UnityInputUtils.Internal.GetDefaultKeyFromActionType(actionType);
            }
        }

        public static int GetCurrentKey(Event current) => UnityInputUtils.Internal.GetCurrentKey(current);

        public static void CheckInputSystemAvailable()
        {
            if (UnityInputUtils.Internal.IsValidInput)
            {
                return;
            }

            LogModel.LogWarning("The Input System package is not installed, so the tool will not function properly.\nPlease either change the Active Input Handling setting or install the package from the Package Manager.");
        }
    }
}
