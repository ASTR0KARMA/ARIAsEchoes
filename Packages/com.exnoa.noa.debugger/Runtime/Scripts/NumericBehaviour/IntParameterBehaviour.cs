using System;
using System.Linq;

namespace NoaDebugger
{
    sealed class IntParameterBehaviour : NumericBehaviourBase<int>
    {
        public IntParameterBehaviour(int inputRangeMin, int inputRangeMax) : base(inputRangeMin, inputRangeMax) { }

        public int FromString(string textValue)
        {
            int value;

            if (IsNotNumeric(textValue))
            {
                value =  0;
            }
            else if (int.TryParse(textValue, out int result))
            {
                value = result;
            }
            else
            {
                value = textValue.FirstOrDefault() == '-' ? int.MinValue : int.MaxValue;
            }

            return ValidateValue(value);
        }

        public bool IsEqualOrUnderMin(int value)
        {
            return value <= _inputRangeMin;
        }

        public bool IsEqualOrOverMax(int value)
        {
            return value >= _inputRangeMax;
        }

        public int ValidateValue(int value)
        {
            return Math.Clamp(value, _inputRangeMin, _inputRangeMax);
        }
    }
}
