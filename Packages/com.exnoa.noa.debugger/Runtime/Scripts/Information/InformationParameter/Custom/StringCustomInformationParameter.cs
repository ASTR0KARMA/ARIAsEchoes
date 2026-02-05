using System;

namespace NoaDebugger
{
    sealed class StringCustomInformationParameter : CustomInformationParameterBase<string>
    {
        public StringCustomInformationParameter(string groupName, string parameterName, Func<string> getter, Action<string> setter) : base(groupName, parameterName, getter, setter) { }

        protected override void _LoadValue()
        {
            string initValue = _getter();
            var value = NoaDebuggerPrefs.GetString(PrefsKey, initValue);
            ChangeValue(value);
        }

        protected override void _SaveValue()
        {
            NoaDebuggerPrefs.SetString(PrefsKey, Value);
        }

        public override void Accept(ICustomParameterVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

