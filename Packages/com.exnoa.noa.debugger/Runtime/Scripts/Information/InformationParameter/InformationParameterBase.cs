using System;

namespace NoaDebugger
{
    abstract class InformationParameterBase<T> : IMutableParameter<T>
    {
        public T Value => _value;
        T _value;

        public bool IsShowOverrideMarker => _isShowOverrideMarker;
        bool _isShowOverrideMarker;

        protected readonly T _defaultValue;

        readonly Action _onChangeValue;

        readonly string _groupName;
        readonly string _parameterName;
        protected string PrefsKey => $"{_groupName}_{_parameterName}";

        protected InformationParameterBase(string groupName, string parameterName, T defaultValue, Action<T> onValueChanged)
        {
            _groupName = groupName;
            _parameterName = parameterName;
            _defaultValue = defaultValue;
            _value = defaultValue;
            _onChangeValue = () => onValueChanged?.Invoke(_value);

            if (NoaDebuggerSettingsManager.GetNoaDebuggerSettings().SaveInformationValue)
            {
                _LoadValue();
            }
        }

        public virtual void ChangeValue(T value)
        {
            bool isEqualDefault = value.Equals(_defaultValue);
            _isShowOverrideMarker = !isEqualDefault;
            _value = value;
            _onChangeValue?.Invoke();

            if (isEqualDefault)
            {
                NoaDebuggerPrefs.DeleteAt(PrefsKey);
            }
            else
            {
                _SaveValue();
            }
        }

        protected abstract void _LoadValue();

        protected abstract void _SaveValue();
    }
}
