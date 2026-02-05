#if !INPUT_SYSTEM_PACKAGE_AVAILABLE
using UnityEngine;

namespace NoaDebugger
{
    sealed class UnityInputUtilsInternal : IUnityInputUtilsInternal
    {
        public void AddInputModule(GameObject target) { }

        public void OnShortcutHandlerInitialize() { }

        public bool IsShortcutKeyDown(ShortcutKeyboardBinding keyboard) => false;

        public bool IsShortcutKeyHeld(ShortcutKeyboardBinding keyboard) => false;

        public bool IsShortcutKeyUp(ShortcutKeyboardBinding keyboard) => false;

        public bool IsShortcutModifierPressed(ShortcutKeyboardBinding keyboard) => false;

        public string GetKeyTextFromInt(int keyNum) => null;

        public int GetDefaultKeyFromActionType(ShortcutActionType actionType) => -1;

        public int GetCurrentKey(Event current) => -1;

        public bool IsValidInput => false;
    }
}
#endif
