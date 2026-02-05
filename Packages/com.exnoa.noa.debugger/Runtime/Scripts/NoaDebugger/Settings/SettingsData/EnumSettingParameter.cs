using System;
using System.Linq;
using UnityEngine;

namespace NoaDebugger
{
    [Serializable]
    sealed class EnumSettingParameter : SettingParameterBase<Enum>, IMutableEnumParameter
    {
        [SerializeField]
        string _valueStr;

        readonly Type _enumType;
        public string[] EnumNames { get; }
        public Enum[] EnumValues { get; }

        public EnumSettingParameter(Enum defaultValue)
            : base(defaultValue)
        {
            _enumType = defaultValue.GetType();
            EnumNames = Enum.GetNames(_enumType);
            EnumValues = Enum.GetValues(_enumType)
                              .Cast<Enum>()
                              .ToArray();
        }

        public override void ApplySavedSettings(SettingParameterBase<Enum> savedSetting)
        {
            if (savedSetting == null || !savedSetting.IsSaved)
            {
                SetDefaultValue();
                return;
            }

            if (savedSetting is EnumSettingParameter enumSettings)
            {
                Enum.TryParse(_enumType, enumSettings._valueStr, out object value);

                if (value == null)
                {
                    SetDefaultValue();
                    return;
                }

                _value = (Enum)value;
                _savedValue = _value;
                _valueStr = enumSettings._valueStr;

                _RefreshFlags(_value);
            }
        }

        public override void SetDefaultValue()
        {
            base.SetDefaultValue();
            _valueStr = _value.ToString();
        }

        public override void ResetSettings()
        {
            base.ResetSettings();
            _valueStr = _value.ToString();
        }

        public override void ChangeValue(Enum newValue)
        {
            _valueStr = newValue.ToString();
            base.ChangeValue(newValue);
        }
    }
}
