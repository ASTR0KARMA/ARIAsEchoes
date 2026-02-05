namespace NoaDebugger
{
    abstract class NumericSettingParameterBase<T> : SettingParameterBase<T>, IMutableNumericParameter<T> where T : struct
    {
        protected readonly T _increment;

        public NumericSettingParameterBase(
            T defaultValue, T increment)
            : base(defaultValue)
        {
            _increment = increment;
        }

        public abstract T FromString(string textValue);

        public abstract bool IsEqualOrUnderMin(T value);

        public abstract bool IsEqualOrOverMax(T value);

        public abstract T ValidateValueForFluctuation(T value, int magnification);

        protected abstract T _ValidateValue(T value);

        public override void ChangeValue(T newValue)
        {
            base.ChangeValue(_ValidateValue(newValue));
        }
    }
}
