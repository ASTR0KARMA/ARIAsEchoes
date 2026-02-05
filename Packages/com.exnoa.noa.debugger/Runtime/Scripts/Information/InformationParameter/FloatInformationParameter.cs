using System;
using System.Linq;

namespace NoaDebugger
{
    sealed class FloatInformationParameter : InformationNumericParameterBase<float>, IMutableNumericParameter<float>
    {
        readonly int _needDigits;

        protected override float InputRangeMin => float.MinValue;
        protected override float InputRangeMax => float.MaxValue;
        protected override float Increment => 0.1f;

        public FloatInformationParameter(string groupName, string parameterName, float initialValue, Action<float> onValueChanged): base(groupName, parameterName, initialValue, onValueChanged)
        {
            string incrementStr = Increment.ToString();
            if (incrementStr.Contains(NoaDebuggerDefine.DecimalPoint))
            {
                _needDigits = incrementStr.Length - incrementStr.IndexOf(NoaDebuggerDefine.DecimalPoint) - 1;
            }
        }

        public override float FromString(string textValue)
        {
            float value;

            if (IsNotNumeric(textValue))
            {
                value =  0;
            }
            else if (float.TryParse(textValue, out float result))
            {
                value = result;
            }
            else
            {
                value = textValue.FirstOrDefault() == '-' ? float.MinValue : float.MaxValue;
            }

            return _ValidateValue(value);
        }

        public override bool IsEqualOrUnderMin(float value)
        {
            return value <= InputRangeMin;
        }

        public override bool IsEqualOrOverMax(float value)
        {
            return value >= InputRangeMax;
        }

        public override float ValidateValueForFluctuation(float value, int magnification)
        {
            float calculatedValue = value + magnification * Increment;
            float clampedValue = _ValidateValue(calculatedValue);

            return MathF.Round(clampedValue, _needDigits, MidpointRounding.AwayFromZero);
        }

        protected override float _ValidateValue(float value)
        {
            return Math.Clamp(value , InputRangeMin, InputRangeMax);
        }

        protected override void _LoadValue()
        {
            var value = NoaDebuggerPrefs.GetFloat(PrefsKey, _defaultValue);
            ChangeValue(value);
        }

        protected override void _SaveValue()
        {
            NoaDebuggerPrefs.SetFloat(PrefsKey, Value);
        }
    }
}
