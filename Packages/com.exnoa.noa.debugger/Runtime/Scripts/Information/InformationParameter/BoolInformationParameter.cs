using System;

namespace NoaDebugger
{
    sealed class BoolInformationParameter : InformationParameterBase<bool>
    {
        public BoolInformationParameter(string groupName, string parameterName, bool initialValue, Action<bool> onValueChanged) : base(groupName, parameterName, initialValue, onValueChanged) { }

        protected override void _LoadValue()
        {
            var value = NoaDebuggerPrefs.GetBoolean(PrefsKey, _defaultValue);
            ChangeValue(value);
        }

        protected override void _SaveValue()
        {
            NoaDebuggerPrefs.SetBoolean(PrefsKey, Value);
        }
    }
}
