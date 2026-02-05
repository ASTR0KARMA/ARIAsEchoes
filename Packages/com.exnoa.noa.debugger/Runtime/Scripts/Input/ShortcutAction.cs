using System;
using System.Collections.Generic;
using UnityEngine;

namespace NoaDebugger
{
    [Serializable]
    sealed class ShortcutInputCombination
    {
        public bool isEnabled = true;

        public ShortcutKeyboardBinding keyboard = new ShortcutKeyboardBinding();
    }

    [Serializable]
    sealed class ShortcutAction
    {
        [SerializeField]
        ShortcutActionType actionType;

        [SerializeField]
        public ShortcutInputCombination combination = new();

        public ShortcutActionType Type => actionType;

        public ShortcutAction(ShortcutActionType type)
        {
            actionType = type;
        }

        public string GetDisplayTextByActionType()
        {
            string commandNameStr = Type.ToString();
            NoaDebuggerDefine.ShortCutTriggerType triggerType = NoaDebuggerDefine.GetTriggerType(Type);

            if (triggerType == NoaDebuggerDefine.ShortCutTriggerType.LongPress)
            {
                commandNameStr += " (Hold)";
            }

            return commandNameStr;
        }

        public string GetDisplayTextByCombination(bool isRuntime = false)
        {
            return GetDisplayTextByCombination(isRuntime, DeviceModel.IsWindows, DeviceModel.IsMac);
        }

        public string GetDisplayTextByCombination(bool isRuntime, bool isWindows, bool isMac)
        {
            bool hasModifierKeys = combination.keyboard.modifiers.Count > 0;
            bool isMatchModifierKey = false;
            var modifierTexts = new List<string>();
            foreach (var modifier in combination.keyboard.modifiers)
            {
                if (modifier == ShortcutModifierKey.None)
                {
                    isMatchModifierKey = true;
                    continue;
                }

                isMatchModifierKey = _TryGetModifierKey(modifier, isRuntime, isWindows, isMac, out string modifierKey);
                if (isMatchModifierKey)
                {
                    modifierTexts.Add(modifierKey);
                }
                else
                {
                    break;
                }
            }

            if (hasModifierKeys && !isMatchModifierKey)
            {
                return "";
            }

            string keyText = UnityInputUtils.GetKeyTextFromInt(combination.keyboard.key);
            return modifierTexts.Count > 0 ?
                string.Join(" + ", modifierTexts) + " + " + keyText :
                keyText;
        }

        bool _TryGetModifierKey(ShortcutModifierKey modifier, bool isRuntime, bool isWindows, bool isMac, out string modifierKey)
        {
            modifierKey = "";

            switch (modifier)
            {
                case ShortcutModifierKey.Ctrl:
                    if (isWindows)
                    {
                        modifierKey = "Ctrl";
                    }
                    else if (isMac)
                    {
                        modifierKey = "Cmd";

                        if (!isRuntime)
                        {
#if UNITY_EDITOR_OSX
                            modifierKey = "⌘";  
#endif
                        }
                    }

                    break;

                case ShortcutModifierKey.Alt:
                    if (isWindows)
                    {
                        modifierKey = "Alt";
                    }
                    else if (isMac)
                    {
                        modifierKey = "Opt";

                        if (!isRuntime)
                        {
#if UNITY_EDITOR_OSX
                            modifierKey = "⌥";  
#endif
                        }
                    }

                    break;

                case ShortcutModifierKey.Shift:
                    if (isWindows || isMac)
                    {
                        modifierKey = "Shift";

                        if (!isRuntime)
                        {
                            modifierKey = "⇧";
                        }
                    }

                    break;

                case ShortcutModifierKey.None:
                    return true;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return !string.IsNullOrEmpty(modifierKey);
        }

        public string GetDisplayTextByTriggerType()
        {
            NoaDebuggerDefine.ShortCutTriggerType triggerType = NoaDebuggerDefine.GetTriggerType(Type);
            return _GetDisplayTextByTriggerType(triggerType);
        }
        string _GetDisplayTextByTriggerType(NoaDebuggerDefine.ShortCutTriggerType triggerType)
        {
            List<NoaDebuggerDefine.ShortCutTriggerType> triggerTypes = new List<NoaDebuggerDefine.ShortCutTriggerType>
            {
                NoaDebuggerDefine.ShortCutTriggerType.ShortPress,
                NoaDebuggerDefine.ShortCutTriggerType.LongPress,
                NoaDebuggerDefine.ShortCutTriggerType.HoldDown
            };

            List<string> triggerNames = new List<string>();

            foreach (var type in triggerTypes)
            {
                if (triggerType.HasFlag(type))
                {
                    triggerNames.Add(type.ToString());
                }
            }

            return string.Join(" | ", triggerNames);
        }
    }
}
