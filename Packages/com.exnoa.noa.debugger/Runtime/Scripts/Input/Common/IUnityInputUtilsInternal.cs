using UnityEngine;

namespace NoaDebugger
{
    interface IUnityInputUtilsInternal
    {
        void AddInputModule(GameObject target);

        void OnShortcutHandlerInitialize();

        bool IsShortcutKeyDown(ShortcutKeyboardBinding keyboard);

        bool IsShortcutKeyHeld(ShortcutKeyboardBinding keyboard);

        bool IsShortcutKeyUp(ShortcutKeyboardBinding keyboard);

        bool IsShortcutModifierPressed(ShortcutKeyboardBinding keyboard);

        string GetKeyTextFromInt(int keyNum);

        int GetDefaultKeyFromActionType(ShortcutActionType actionType);

        int GetCurrentKey(Event current);

        bool IsValidInput { get; }
    }
}
