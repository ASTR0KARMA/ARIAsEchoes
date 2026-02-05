using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityInput = UnityEngine.Input;

namespace NoaDebugger
{
    sealed class UnityInputUtilsInternal : IUnityInputUtilsInternal
    {
        public void AddInputModule(GameObject target)
        {
            target.AddComponent<StandaloneInputModule>();
        }

        public void OnShortcutHandlerInitialize()
        {
        }

        public bool IsShortcutKeyDown(ShortcutKeyboardBinding keyboard)
        {
            var isModifierPressed = IsShortcutModifierPressed(keyboard);
            return isModifierPressed && UnityInput.GetKeyDown((KeyCode)keyboard.key);
        }

        public bool IsShortcutKeyHeld(ShortcutKeyboardBinding keyboard)
        {
            var isModifierPressed = IsShortcutModifierPressed(keyboard);
            return isModifierPressed && UnityInput.GetKey((KeyCode)keyboard.key);
        }

        public bool IsShortcutKeyUp(ShortcutKeyboardBinding keyboard)
        {
            var wasModifierPressed = IsShortcutModifierPressed(keyboard);
            return wasModifierPressed && UnityInput.GetKeyUp((KeyCode)keyboard.key);
        }

        public bool IsShortcutModifierPressed(ShortcutKeyboardBinding keyboard)
        {
            var isModifierPressed = true;
            foreach (var modifier in keyboard.modifiers)
            {
                switch (modifier)
                {
                    case ShortcutModifierKey.Alt:
                        isModifierPressed &= UnityInput.GetKey(KeyCode.LeftAlt) || UnityInput.GetKey(KeyCode.RightAlt);
                        break;
                    case ShortcutModifierKey.Ctrl:
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
                        isModifierPressed &= UnityInput.GetKey(KeyCode.LeftCommand) || UnityInput.GetKey(KeyCode.RightCommand);
#else
                        isModifierPressed &= UnityInput.GetKey(KeyCode.LeftControl) || UnityInput.GetKey(KeyCode.RightControl);
#endif
                        break;
                    case ShortcutModifierKey.Shift:
                        isModifierPressed &= UnityInput.GetKey(KeyCode.LeftShift) || UnityInput.GetKey(KeyCode.RightShift);
                        break;
                    case ShortcutModifierKey.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return isModifierPressed;
        }

        public string GetKeyTextFromInt(int keyNum)
        {
            KeyCode key = (KeyCode)keyNum;
            return key switch
            {
                KeyCode.Space => "Space",
                KeyCode.Return => "Enter",
                KeyCode.Tab => "Tab",
                KeyCode.BackQuote => "`",
                KeyCode.Quote => "'",
                KeyCode.Semicolon => ";",
                KeyCode.Comma => ",",
                KeyCode.Period => ".",
                KeyCode.Slash => "/",
                KeyCode.Backslash => "\\",
                KeyCode.LeftBracket => "[",
                KeyCode.RightBracket => "]",
                KeyCode.Minus => "-",
                KeyCode.Equals => "=",
                KeyCode.Alpha0 => "0",
                KeyCode.Alpha1 => "1",
                KeyCode.Alpha2 => "2",
                KeyCode.Alpha3 => "3",
                KeyCode.Alpha4 => "4",
                KeyCode.Alpha5 => "5",
                KeyCode.Alpha6 => "6",
                KeyCode.Alpha7 => "7",
                KeyCode.Alpha8 => "8",
                KeyCode.Alpha9 => "9",
                _ => key.ToString()
            };
        }

        public int GetDefaultKeyFromActionType(ShortcutActionType actionType)
        {
            KeyCode key = actionType switch
            {
                ShortcutActionType.ToggleDebugger => KeyCode.D,
                ShortcutActionType.ToggleOverlay => KeyCode.O,
                ShortcutActionType.ToggleFloatingWindow => KeyCode.F,
                ShortcutActionType.ToggleTriggerButton => KeyCode.S,
                ShortcutActionType.ToggleUIElements => KeyCode.E,
                ShortcutActionType.ResetApplication => KeyCode.R,
                ShortcutActionType.ToggleAllUI => KeyCode.X,
                ShortcutActionType.CaptureScreenshot => KeyCode.C,
                ShortcutActionType.TogglePauseResume => KeyCode.Space,
                ShortcutActionType.DecreaseGameSpeed => KeyCode.J,
                ShortcutActionType.IncreaseGameSpeed => KeyCode.K,
                ShortcutActionType.FrameStepping => KeyCode.M,
                ShortcutActionType.ResetGameSpeed => KeyCode.L,
                ShortcutActionType.CustomAction1 => KeyCode.Alpha1,
                ShortcutActionType.CustomAction2 => KeyCode.Alpha2,
                ShortcutActionType.CustomAction3 => KeyCode.Alpha3,
                ShortcutActionType.CustomAction4 => KeyCode.Alpha4,
                ShortcutActionType.CustomAction5 => KeyCode.Alpha5,
#if NOA_DEBUGGER_DEBUG
                _ => throw new Exception("デフォルトキーが設定されていないShortcutActionTypeです。定義を追加してください。")
#else
                _ => KeyCode.None
#endif
            };

            return (int)key;
        }

        public int GetCurrentKey(Event current)
        {
            return (int)current.keyCode;
        }

        public bool IsValidInput => true;
    }
}
