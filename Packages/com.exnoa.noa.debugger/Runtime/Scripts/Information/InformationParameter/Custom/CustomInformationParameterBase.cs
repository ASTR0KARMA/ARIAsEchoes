using System;

namespace NoaDebugger
{
    abstract class CustomInformationParameterBase<T> : IMutableParameter<T>, ICustomInformationParameter
    {
        public T Value => _getter();

        public bool IsShowOverrideMarker => false;

        readonly string _groupName;
        readonly string _parameterName;
        protected string PrefsKey => $"{_groupName}_{_parameterName}";

        protected readonly Func<T> _getter;
        readonly Action<T> _setter;

        protected CustomInformationParameterBase(string groupName, string parameterName, Func<T> getValue, Action<T> setValue)
        {
            if (getValue == null)
            {
                throw new ArgumentException($"function getValue is null");
            }

            if (setValue == null)
            {
                throw new ArgumentException($"function setValue is null");
            }

            _groupName = groupName;
            _parameterName = parameterName;
            _getter = getValue;
            _setter = setValue;

            if (NoaDebuggerSettingsManager.GetNoaDebuggerSettings().SaveInformationValue)
            {
                _LoadValue();
            }
        }

        public void ChangeValue(T newValue)
        {
            _setter(newValue);
            _SaveValue();
        }

        protected abstract void _LoadValue();

        protected abstract void _SaveValue();

        public string GetStringValue()
        {
            return Value.ToString();
        }

        public abstract void Accept(ICustomParameterVisitor visitor);
    }

    interface ICustomInformationParameter
    {
        string GetStringValue();

        void Accept(ICustomParameterVisitor visitor);
    }
}
