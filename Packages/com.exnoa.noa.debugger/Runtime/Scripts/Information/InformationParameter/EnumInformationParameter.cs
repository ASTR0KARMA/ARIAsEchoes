using System;
using System.Linq;

namespace NoaDebugger
{
    sealed class EnumInformationParameter : InformationParameterBase<Enum>, IMutableEnumParameter
    {
        public EnumInformationParameter(string groupName, string parameterName, Enum initialValue, Action<Enum> onValueChanged) : base(groupName, parameterName, initialValue, onValueChanged)
        {
            var enumType = initialValue.GetType();
            EnumNames = Enum.GetNames(enumType);
            EnumValues = Enum.GetValues(enumType)
                              .Cast<Enum>()
                              .ToArray();
        }

        public string[] EnumNames { get; }
        public Enum[] EnumValues { get; }

        protected override void _LoadValue()
        {
            var value = NoaDebuggerPrefs.GetString(PrefsKey, _defaultValue.ToString());
            Enum.TryParse(_defaultValue.GetType(), value, out object result);
            ChangeValue((Enum)result);
        }

        protected override void _SaveValue()
        {
            NoaDebuggerPrefs.SetString(PrefsKey, Value.ToString());
        }
    }
}
