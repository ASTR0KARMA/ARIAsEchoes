using System;
using System.Linq;

namespace NoaDebugger
{
    sealed class FloatParameterBehaviour : NumericBehaviourBase<float>
    {
        public FloatParameterBehaviour(float inputRangeMin, float inputRangeMax) : base(inputRangeMin, inputRangeMax) { }

        public float FromString(string textValue)
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

            return ValidateValue(value);
        }

        public bool IsEqualOrUnderMin(float value)
        {
            return value <= _inputRangeMin;
        }

        public bool IsEqualOrOverMax(float value)
        {
            return value >= _inputRangeMax;
        }

        public float ValidateValue(float value)
        {
            return Math.Clamp(value, _inputRangeMin, _inputRangeMax);
        }
    }
}

