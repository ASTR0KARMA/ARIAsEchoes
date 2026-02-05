using System;

namespace NoaDebugger
{
    sealed class BoolCustomInformationParameter : CustomInformationParameterBase<bool>
    {
        public BoolCustomInformationParameter(string groupName, string parameterName, Func<bool> getter, Action<bool> setter) : base(groupName, parameterName, getter, setter) { }

        protected override void _LoadValue()
        {
            bool initValue = _getter();
            var value = NoaDebuggerPrefs.GetBoolean(PrefsKey, initValue);
            ChangeValue(value);
        }

        protected override void _SaveValue()
        {
            NoaDebuggerPrefs.SetBoolean(PrefsKey, Value);
        }

        public override void Accept(ICustomParameterVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
