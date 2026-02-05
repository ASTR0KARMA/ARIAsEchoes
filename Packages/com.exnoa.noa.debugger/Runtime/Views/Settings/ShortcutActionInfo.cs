using System;
using UnityEngine;

namespace NoaDebugger
{
    [Serializable]
    sealed class ShortcutActionInfo
    {
        string _name;
        string _triggerType;
        string _keyBinding;

        [SerializeField]
        BoolSettingParameter _enabled;

        [SerializeField]
        ShortcutActionType _type;

        ShortcutAction _action;

        public string Name => _name;

        public string TriggerType => _triggerType;

        public string KeyBinding => _keyBinding;

        public BoolSettingParameter Enabled => _enabled;

        public ShortcutActionType SavedType => _type;

        public ShortcutAction Action => _action;

        public ShortcutActionInfo(BoolSettingParameter enabled, ShortcutAction action)
        {
            _enabled = enabled;
            _type = action.Type;
            _name = action.GetDisplayTextByActionType();
            _triggerType = action.GetDisplayTextByTriggerType();


#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            bool isMobile = true;
#else
            bool isMobile = UserAgentModel.IsMobileBrowser;
#endif
            if (isMobile)
            {
                _keyBinding = action.GetDisplayTextByCombination(true, true, false);
            }
            else
            {
                _keyBinding = action.GetDisplayTextByCombination(true);
            }

            _action = action;
        }
    }
}
