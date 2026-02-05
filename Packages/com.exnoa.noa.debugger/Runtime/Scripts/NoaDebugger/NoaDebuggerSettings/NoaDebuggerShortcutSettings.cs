using System.Collections.Generic;
using System.Linq;

namespace NoaDebugger
{
    sealed class NoaDebuggerShortcutSettings : NoaDebuggerSettingsBase
    {
        public NoaDebuggerShortcutSettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void Init()
        {
            _settings.EnableAllShortcuts = NoaDebuggerDefine.DEFAULT_ENABLE_ALL_SHORTCUTS;
            _settings.ShortcutActionsOnInputManager = GetDefaultShortcutActions(forInputSystem: false);
            _settings.ShortcutActions = GetDefaultShortcutActions(forInputSystem: true);
        }

        public static List<ShortcutAction> GetDefaultShortcutActions(bool? forInputSystem = null)
        {
            var defaultActions = NoaDebuggerDefine.SortedShortcutActionType
                                                  .Select(type => new ShortcutAction(type)
                                                  {
                                                      combination = GetDefaultCombination(type, forInputSystem)
                                                  });

            return defaultActions.ToList();
        }

        public static ShortcutInputCombination GetDefaultCombination(ShortcutActionType type, bool? forInputSystem = null)
        {
            var combination = new ShortcutInputCombination();

            switch (type)
            {
                case ShortcutActionType.TogglePauseResume:
                    combination.keyboard.modifiers = new List<ShortcutModifierKey>
                    {
                        ShortcutModifierKey.Shift
                    };
                    break;

                case ShortcutActionType.ToggleDebugger:
                case ShortcutActionType.ToggleOverlay:
                case ShortcutActionType.ToggleFloatingWindow:
                case ShortcutActionType.ToggleTriggerButton:
                case ShortcutActionType.ToggleUIElements:
                case ShortcutActionType.ResetApplication:
                case ShortcutActionType.ToggleAllUI:
                case ShortcutActionType.CaptureScreenshot:
                case ShortcutActionType.DecreaseGameSpeed:
                case ShortcutActionType.IncreaseGameSpeed:
                case ShortcutActionType.FrameStepping:
                case ShortcutActionType.ResetGameSpeed:
                case ShortcutActionType.CustomAction1:
                case ShortcutActionType.CustomAction2:
                case ShortcutActionType.CustomAction3:
                case ShortcutActionType.CustomAction4:
                case ShortcutActionType.CustomAction5:
                    combination.keyboard.modifiers = new List<ShortcutModifierKey>
                    {
                        ShortcutModifierKey.Alt,
                        ShortcutModifierKey.Shift
                    };
                    break;
            }

            combination.keyboard.key = forInputSystem == null
                ? UnityInputUtils.GetCurrentDefaultKey(type)
                : UnityInputUtils.GetDefaultShortcutKey(type, forInputSystem.Value);

            return combination;
        }

        public static List<ShortcutAction> GetUpdatedShortcutSettings(List<ShortcutAction> baseList, bool forInputSystem)
        {
            List<ShortcutAction> updatedSettings = new List<ShortcutAction>();

            ShortcutActionType[] actionTypeArray = NoaDebuggerDefine.SortedShortcutActionType;

            foreach (ShortcutActionType actionType in actionTypeArray)
            {
                ShortcutAction customShortcutInfo = baseList.FirstOrDefault(
                    customShortcutInfo => customShortcutInfo.Type.Equals(actionType));

                if (customShortcutInfo == null)
                {
                    updatedSettings.Add(new ShortcutAction(actionType)
                    {
                        combination = GetDefaultCombination(actionType, forInputSystem)
                    });
                }
                else
                {
                    updatedSettings.Add(customShortcutInfo);
                }
            }

            return updatedSettings;
        }

        public static bool CheckUpdateDirtyShortcutSettings(NoaDebuggerSettings settings)
        {
            bool isUpdateFromDirty = false;
            List<ShortcutAction> currentShortcutSettings = settings.EnabledShortcutActions;

            foreach (ShortcutAction shortcutSettings in currentShortcutSettings)
            {
                int currentKey = shortcutSettings.combination.keyboard.key;

                if (currentKey == NoaDebuggerDefine.ShortcutInvalidKey &&
                    currentKey != GetDefaultCombination(shortcutSettings.Type).keyboard.key)
                {
                    shortcutSettings.combination = GetDefaultCombination(shortcutSettings.Type);
                    isUpdateFromDirty = true;
                }
            }

            return isUpdateFromDirty;
        }
    }
}
