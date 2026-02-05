using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class ShortcutSettingsPanel : SettingsPanelBase<bool>
    {
        [SerializeField]
        ToggleButtonBase _toggleButton;

        [SerializeField]
        Graphic _clickTarget;

        [SerializeField]
        ToggleObjectButton _expansionToggleButton;

        [SerializeField]
        NoaDebuggerText _commandName;

        [SerializeField]
        NoaDebuggerText _triggerType;

        [SerializeField]
        NoaDebuggerText _shortcuts;

        [SerializeField]
        RectTransform _rootTransform;

        [SerializeField]
        RectTransform _commandNameContent;

        [SerializeField]
        RectTransform[] _disableTargets;

        bool _expanded = false;

        public static ShortcutSettingsPanel Instantiate(ShortcutSettingsPanel prefab, Transform parent, ShortcutActionInfo action)
        {
            var instance = GameObject.Instantiate(prefab, parent);
            instance._SetCommandText(action);
            return instance;
        }

        public override void Initialize(IMutableParameter<bool> settings)
        {
            base.Initialize(settings);

            Assert.IsNotNull(_toggleButton);
            Assert.IsNotNull(_clickTarget);
            Assert.IsNotNull(_expansionToggleButton);
            Assert.IsNotNull(_commandName);
            Assert.IsNotNull(_triggerType);
            Assert.IsNotNull(_shortcuts);

            _toggleButton._onClick.RemoveAllListeners();
            _toggleButton._onClick.AddListener(_OnToggleChange);
            _expansionToggleButton._onClick.RemoveAllListeners();
            _expansionToggleButton._onClick.AddListener(_OnExpansionToggle);

            Refresh();
        }

        void _SetCommandText(ShortcutActionInfo action)
        {
            _commandName.SetText(action.Name);
            _triggerType.SetText(action.TriggerType);
            _shortcuts.SetText(action.KeyBinding);
        }

        protected override void _Refresh()
        {
            _toggleButton.Init(_parameter.Value);

            foreach(var obj in _disableTargets)
            {
                obj.gameObject.SetActive(_expanded);
            }
            _expansionToggleButton.Init(_expanded);
        }

        public override void SetEnabled(bool isEnabled)
        {
            base.SetEnabled(isEnabled);

            _clickTarget.raycastTarget = isEnabled;
        }

        void _OnToggleChange(bool isOn)
        {
            _parameter.ChangeValue(isOn);
            Refresh();
        }

        void _OnExpansionToggle(bool isOn)
        {
            _expanded = isOn;
            Refresh();
        }
    }
}
