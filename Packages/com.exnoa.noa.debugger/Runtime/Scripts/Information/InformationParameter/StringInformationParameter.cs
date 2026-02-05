using System;

namespace NoaDebugger
{
    sealed class StringInformationParameter : InformationParameterBase<string>
    {
        public StringInformationParameter(string groupName, string parameterName, string initialValue, Action<string> onValueChanged) : base(groupName, parameterName, initialValue, onValueChanged) { }

        protected override void _LoadValue()
        {
            var value = NoaDebuggerPrefs.GetString(PrefsKey, _defaultValue);
            ChangeValue(value);
        }

        protected override void _SaveValue()
        {
            NoaDebuggerPrefs.SetString(PrefsKey, Value);
        }
    }
}
