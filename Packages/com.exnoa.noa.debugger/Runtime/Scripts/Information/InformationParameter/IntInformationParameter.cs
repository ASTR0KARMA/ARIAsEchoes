using System;
using System.Linq;

namespace NoaDebugger
{
    sealed class IntInformationParameter : InformationNumericParameterBase<int>, IMutableNumericParameter<int>
    {
        protected override int InputRangeMin => int.MinValue;
        protected override int InputRangeMax => int.MaxValue;
        protected override int Increment => 1;

        public IntInformationParameter(string groupName, string parameterName, int initialValue, Action<int> onValueChanged) : base(groupName, parameterName, initialValue, onValueChanged) { }

        public override int FromString(string textValue)
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

            return _ValidateValue(value);
        }

        public override bool IsEqualOrUnderMin(int value)
        {
            return value <= InputRangeMin;
        }

        public override bool IsEqualOrOverMax(int value)
        {
            return value >= InputRangeMax;
        }

        public override int ValidateValueForFluctuation(int value, int magnification)
        {
            int calculatedValue = value + magnification * Increment;
            return _ValidateValue(calculatedValue);
        }

        protected override int _ValidateValue(int value)
        {
            return Math.Clamp(value , InputRangeMin, InputRangeMax);
        }

        protected override void _LoadValue()
        {
            var value = NoaDebuggerPrefs.GetInt(PrefsKey, _defaultValue);
            ChangeValue(value);
        }

        protected override void _SaveValue()
        {
            NoaDebuggerPrefs.SetInt(PrefsKey, Value);
        }
    }
}
