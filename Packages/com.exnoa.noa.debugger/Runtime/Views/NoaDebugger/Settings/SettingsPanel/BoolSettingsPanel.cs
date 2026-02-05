using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class BoolSettingsPanel : SettingsPanelBase<bool>
    {
        [SerializeField]
        ToggleButtonBase _toggleButton;

        [SerializeField]
        Graphic _clickTarget;

        public override void Initialize(IMutableParameter<bool> settings)
        {
            base.Initialize(settings);

            Assert.IsNotNull(_toggleButton);
            Assert.IsNotNull(_clickTarget);

            _toggleButton._onClick.RemoveAllListeners();
            _toggleButton._onClick.AddListener(_OnToggleChange);

            Refresh();
        }

        protected override void _Refresh()
        {
            _toggleButton.Init(_parameter.Value);
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
    }
}
