using System;
using System.Linq;

namespace NoaDebugger
{
    sealed class EnumCustomInformationParameter : CustomInformationParameterBase<Enum>, IMutableEnumParameter
    {
        readonly Type _enumType;

        public EnumCustomInformationParameter(string groupName, string parameterName, Func<Enum> getter, Action<Enum> setter) : base(groupName, parameterName, getter, setter)
        {
            _enumType = getter().GetType();
            EnumNames = Enum.GetNames(_enumType);
            EnumValues = Enum.GetValues(_enumType)
                             .Cast<Enum>()
                             .ToArray();
        }

        public string[] EnumNames { get; }
        public Enum[] EnumValues { get; }

        protected override void _LoadValue()
        {
            Enum initValue = _getter();
            Type enumType = initValue.GetType();
            var value = NoaDebuggerPrefs.GetString(PrefsKey, initValue.ToString());
            Enum.TryParse(enumType, value, out object result);
            if (result != null)
            {
                ChangeValue((Enum)result);
            }
        }

        protected override void _SaveValue()
        {
            NoaDebuggerPrefs.SetString(PrefsKey, Value.ToString());
        }

        public override void Accept(ICustomParameterVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

