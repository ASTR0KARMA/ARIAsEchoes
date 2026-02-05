using System;

namespace NoaDebugger
{
    [Serializable]
    sealed class FloatSettingParameter : NumericSettingParameterBase<float>
    {
        readonly int _needDigits;
        FloatParameterBehaviour _behaviour;

        public FloatSettingParameter(float defaultValue, float inputRangeMin, float inputRangeMax, float increment)
            : base(defaultValue, increment)
        {
            _behaviour = new FloatParameterBehaviour(inputRangeMin, inputRangeMax);

            string incrementStr = increment.ToString();
            if (incrementStr.Contains(NoaDebuggerDefine.DecimalPoint))
            {
                _needDigits = incrementStr.Length - incrementStr.IndexOf(NoaDebuggerDefine.DecimalPoint) - 1;
            }
        }

        public override float FromString(string textValue) => _behaviour.FromString(textValue);

        public override bool IsEqualOrUnderMin(float value) => _behaviour.IsEqualOrUnderMin(value);

        public override bool IsEqualOrOverMax(float value) => _behaviour.IsEqualOrOverMax(value);

        public override float ValidateValueForFluctuation(float value, int magnification)
        {
            float calculatedValue = value + magnification * _increment;
            float clampedValue = _behaviour.ValidateValue(calculatedValue);

            return MathF.Round(clampedValue, _needDigits, MidpointRounding.AwayFromZero);
        }

        protected override float _ValidateValue(float value) => _behaviour.ValidateValue(value);
    }
}
