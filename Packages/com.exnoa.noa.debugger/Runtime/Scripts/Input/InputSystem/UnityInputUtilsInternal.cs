#if INPUT_SYSTEM_PACKAGE_AVAILABLE
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace NoaDebugger
{
    sealed class UnityInputUtilsInternal : IUnityInputUtilsInternal
    {
        public void AddInputModule(GameObject target)
        {
            target.AddComponent<InputSystemUIInputModule>();
        }

        public void OnShortcutHandlerInitialize()
        {
            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
        }

        public bool IsShortcutKeyDown(ShortcutKeyboardBinding keyboard)
        {
            if (Keyboard.current != null)
            {
                var isModifierPressed = IsShortcutModifierPressed(keyboard);
                return isModifierPressed && Keyboard.current[(Key)keyboard.key].wasPressedThisFrame;
            }
            return false;
        }

        public bool IsShortcutKeyHeld(ShortcutKeyboardBinding keyboard)
        {
            if (Keyboard.current != null)
            {
                var isModifierPressed = IsShortcutModifierPressed(keyboard);
                return isModifierPressed && Keyboard.current[(Key)keyboard.key].isPressed;
            }
            return false;
        }

        public bool IsShortcutKeyUp(ShortcutKeyboardBinding keyboard)
        {
            if (Keyboard.current != null)
            {
                var wasModifierPressed = IsShortcutModifierPressed(keyboard);
                return wasModifierPressed && Keyboard.current[(Key)keyboard.key].wasReleasedThisFrame;
            }
            return false;
        }

        public bool IsShortcutModifierPressed(ShortcutKeyboardBinding keyboard)
        {
            if (Keyboard.current != null)
            {
                bool isModifierPressed = true;
                foreach (var modifier in keyboard.modifiers)
                {
                    switch (modifier)
                    {
                        case ShortcutModifierKey.Alt:
                            isModifierPressed &= Keyboard.current.altKey.isPressed;
                            break;
                        case ShortcutModifierKey.Ctrl:
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
                            isModifierPressed &= Keyboard.current.leftCommandKey.isPressed || Keyboard.current.rightCommandKey.isPressed;
#else
                            isModifierPressed &= Keyboard.current.leftCtrlKey.isPressed || Keyboard.current.rightCtrlKey.isPressed;
#endif
                            break;
                        case ShortcutModifierKey.Shift:
                            isModifierPressed &= Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;
                            break;
                    }
                }
                return isModifierPressed;
            }
            return false;
        }

        public string GetKeyTextFromInt(int keyNum)
        {
            Key key = (Key)keyNum;
            return key switch
            {
                Key.Space => "Space",
                Key.Enter => "Enter",
                Key.Tab => "Tab",
                Key.Backquote => "`",
                Key.Quote => "'",
                Key.Semicolon => ";",
                Key.Comma => ",",
                Key.Period => ".",
                Key.Slash => "/",
                Key.Backslash => "\\",
                Key.LeftBracket => "[",
                Key.RightBracket => "]",
                Key.Minus => "-",
                Key.Equals => "=",
                Key.Digit0 => "0",
                Key.Digit1 => "1",
                Key.Digit2 => "2",
                Key.Digit3 => "3",
                Key.Digit4 => "4",
                Key.Digit5 => "5",
                Key.Digit6 => "6",
                Key.Digit7 => "7",
                Key.Digit8 => "8",
                Key.Digit9 => "9",
                _ => key.ToString()
            };
        }

        public int GetDefaultKeyFromActionType(ShortcutActionType actionType)
        {
            Key key = actionType switch
            {
                ShortcutActionType.ToggleDebugger => Key.D,
                ShortcutActionType.ToggleOverlay => Key.O,
                ShortcutActionType.ToggleFloatingWindow => Key.F,
                ShortcutActionType.ToggleTriggerButton => Key.S,
                ShortcutActionType.ToggleUIElements => Key.E,
                ShortcutActionType.ResetApplication => Key.R,
                ShortcutActionType.ToggleAllUI => Key.X,
                ShortcutActionType.CaptureScreenshot => Key.C,
                ShortcutActionType.TogglePauseResume => Key.Space,
                ShortcutActionType.DecreaseGameSpeed => Key.J,
                ShortcutActionType.IncreaseGameSpeed => Key.K,
                ShortcutActionType.FrameStepping => Key.M,
                ShortcutActionType.ResetGameSpeed => Key.L,
                ShortcutActionType.CustomAction1 => Key.Digit1,
                ShortcutActionType.CustomAction2 => Key.Digit2,
                ShortcutActionType.CustomAction3 => Key.Digit3,
                ShortcutActionType.CustomAction4 => Key.Digit4,
                ShortcutActionType.CustomAction5 => Key.Digit5,
#if NOA_DEBUGGER_DEBUG
                _ => throw new Exception("デフォルトキーが設定されていないShortcutActionTypeです。定義を追加してください。")
#else
                _ => Key.None
#endif
            };

            return (int)key;
        }

        public int GetCurrentKey(Event current)
        {
            Key key = current.keyCode switch
            {
                KeyCode.A => Key.A,
                KeyCode.B => Key.B,
                KeyCode.C => Key.C,
                KeyCode.D => Key.D,
                KeyCode.E => Key.E,
                KeyCode.F => Key.F,
                KeyCode.G => Key.G,
                KeyCode.H => Key.H,
                KeyCode.I => Key.I,
                KeyCode.J => Key.J,
                KeyCode.K => Key.K,
                KeyCode.L => Key.L,
                KeyCode.M => Key.M,
                KeyCode.N => Key.N,
                KeyCode.O => Key.O,
                KeyCode.P => Key.P,
                KeyCode.Q => Key.Q,
                KeyCode.R => Key.R,
                KeyCode.S => Key.S,
                KeyCode.T => Key.T,
                KeyCode.U => Key.U,
                KeyCode.V => Key.V,
                KeyCode.W => Key.W,
                KeyCode.X => Key.X,
                KeyCode.Y => Key.Y,
                KeyCode.Z => Key.Z,

                KeyCode.Alpha0 => Key.Digit0,
                KeyCode.Alpha1 => Key.Digit1,
                KeyCode.Alpha2 => Key.Digit2,
                KeyCode.Alpha3 => Key.Digit3,
                KeyCode.Alpha4 => Key.Digit4,
                KeyCode.Alpha5 => Key.Digit5,
                KeyCode.Alpha6 => Key.Digit6,
                KeyCode.Alpha7 => Key.Digit7,
                KeyCode.Alpha8 => Key.Digit8,
                KeyCode.Alpha9 => Key.Digit9,

                KeyCode.F1 => Key.F1,
                KeyCode.F2 => Key.F2,
                KeyCode.F3 => Key.F3,
                KeyCode.F4 => Key.F4,
                KeyCode.F5 => Key.F5,
                KeyCode.F6 => Key.F6,
                KeyCode.F7 => Key.F7,
                KeyCode.F8 => Key.F8,
                KeyCode.F9 => Key.F9,
                KeyCode.F10 => Key.F10,
                KeyCode.F11 => Key.F11,
                KeyCode.F12 => Key.F12,

                KeyCode.Space => Key.Space,
                KeyCode.Return => Key.Enter,
                KeyCode.Tab => Key.Tab,
                KeyCode.BackQuote => Key.Backquote,
                KeyCode.Minus => Key.Minus,
                KeyCode.Equals => Key.Equals,
                KeyCode.Backspace => Key.Backspace,
                KeyCode.LeftBracket => Key.LeftBracket,
                KeyCode.RightBracket => Key.RightBracket,
                KeyCode.Backslash => Key.Backslash,
                KeyCode.Semicolon => Key.Semicolon,
                KeyCode.Quote => Key.Quote,
                KeyCode.Comma => Key.Comma,
                KeyCode.Period => Key.Period,
                KeyCode.Slash => Key.Slash,

                _ => Key.None
            };

            return (int)key;
        }

        public bool IsValidInput => true;
    }
}
#endif
