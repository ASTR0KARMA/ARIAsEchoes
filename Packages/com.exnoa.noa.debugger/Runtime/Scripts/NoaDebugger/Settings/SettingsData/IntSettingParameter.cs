using System;

namespace NoaDebugger
{
    [Serializable]
    sealed class IntSettingParameter : NumericSettingParameterBase<int>
    {
        IntParameterBehaviour _behaviour;

        public IntSettingParameter(int defaultValue, int inputRangeMin, int inputRangeMax, int increment)
            : base(defaultValue, increment)
        {
            _behaviour = new IntParameterBehaviour(inputRangeMin, inputRangeMax);
        }

        public override int FromString(string textValue) => _behaviour.FromString(textValue);

        public override bool IsEqualOrUnderMin(int value) => _behaviour.IsEqualOrUnderMin(value);

        public override bool IsEqualOrOverMax(int value) => _behaviour.IsEqualOrOverMax(value);

        public override int ValidateValueForFluctuation(int value, int magnification)
        {
            int calculatedValue = value + magnification * _increment;
            return _behaviour.ValidateValue(calculatedValue);
        }

        protected override int _ValidateValue(int value) => _behaviour.ValidateValue(value);
    }
}
