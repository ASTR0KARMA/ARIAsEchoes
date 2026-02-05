using System;

namespace NoaDebugger
{
    abstract class InformationNumericParameterBase<T> : InformationParameterBase<T>, IMutableNumericParameter<T>
    {
        protected abstract T InputRangeMin { get; }
        protected abstract T InputRangeMax { get; }
        protected abstract T Increment { get; }

        protected InformationNumericParameterBase(string groupName, string parameterName,
                                                  T defaultValue, Action<T> onValueChanged)
            : base(groupName, parameterName, defaultValue, onValueChanged) { }

        public abstract T FromString(string textValue);

        public abstract bool IsEqualOrUnderMin(T value);

        public abstract bool IsEqualOrOverMax(T value);

        public abstract T ValidateValueForFluctuation(T value, int magnification);

        protected abstract T _ValidateValue(T value);

        public override void ChangeValue(T value)
        {
            base.ChangeValue(_ValidateValue(value));
        }

        protected bool IsNotNumeric(string text)
        {
            return string.IsNullOrEmpty(text) || text is "-" or ".";
        }
    }
}
