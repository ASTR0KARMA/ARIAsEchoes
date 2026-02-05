using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class StringSettingsPanel : SettingsPanelBase<string>
    {
        [SerializeField]
        TMP_InputField _input;

        public override void Initialize(IMutableParameter<string> settings)
        {
            base.Initialize(settings);

            Assert.IsNotNull(_input);
            _input.onValueChanged.RemoveAllListeners();
            _input.onValueChanged.AddListener(_OnInputValueChanged);

            Refresh();
        }

        protected override void _Refresh()
        {
            _input.text = _parameter.Value;
        }

        void _OnInputValueChanged(string value)
        {
            _parameter.ChangeValue(value);
            Refresh();
        }
    }
}
