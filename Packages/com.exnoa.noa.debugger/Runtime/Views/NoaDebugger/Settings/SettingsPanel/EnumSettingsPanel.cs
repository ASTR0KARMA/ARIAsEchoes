using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class EnumSettingsPanel : SettingsPanelBase<Enum>
    {
        [SerializeField]
        TMP_Dropdown _dropdown;

        IMutableEnumParameter _enumParameter;
        List<TMP_Dropdown.OptionData> _optionDataList;

        public override void Initialize(IMutableParameter<Enum> settings)
        {
            base.Initialize(settings);

            Assert.IsNotNull(_dropdown);

            _enumParameter = _parameter as IMutableEnumParameter;

            _InitializeUI();

            Refresh();
        }

        void _InitializeUI()
        {
            _optionDataList = new List<TMP_Dropdown.OptionData>(_enumParameter.EnumNames.Length);
            foreach(string enumName in _enumParameter.EnumNames)
            {
                TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(enumName);
                _optionDataList.Add(option);
            }

            _dropdown.options = _optionDataList;
            _dropdown.onValueChanged.RemoveAllListeners();
            _dropdown.onValueChanged.AddListener(_OnChangeDropdown);
        }

        protected override void _Refresh()
        {
            _dropdown.SetValueWithoutNotify(Array.IndexOf(_enumParameter.EnumValues, _enumParameter.Value));
        }

        void _OnChangeDropdown(int index)
        {
            Enum value = _enumParameter.EnumValues[index];
            _parameter.ChangeValue(value);
            Refresh();
        }
    }
}
