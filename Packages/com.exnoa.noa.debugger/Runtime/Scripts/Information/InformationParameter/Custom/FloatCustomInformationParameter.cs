using System;

namespace NoaDebugger
{
    sealed class FloatCustomInformationParameter : CustomInformationParameterBase<float>, IMutableNumericParameter<float>
    {
        readonly int _needDigits;
        const float INPUT_RANGE_MIN = float.MinValue;
        const float INPUT_RANGE_MAX = float.MaxValue;
        const float INCREMENT = 0.1f;

        FloatParameterBehaviour _behaviour;

        public FloatCustomInformationParameter(string groupName, string parameterName, Func<float> getter, Action<float> setter) : base(groupName, parameterName, getter, setter)
        {
            _behaviour = new FloatParameterBehaviour(INPUT_RANGE_MIN, INPUT_RANGE_MAX);

            string incrementStr = INCREMENT.ToString();
            if (incrementStr.Contains(NoaDebuggerDefine.DecimalPoint))
            {
                _needDigits = incrementStr.Length - incrementStr.IndexOf(NoaDebuggerDefine.DecimalPoint) - 1;
            }
        }

        public float FromString(string textValue) => _behaviour.FromString(textValue);

        public bool IsEqualOrUnderMin(float value) => _behaviour.IsEqualOrUnderMin(value);

        public bool IsEqualOrOverMax(float value) => _behaviour.IsEqualOrOverMax(value);

        public float ValidateValueForFluctuation(float value, int magnification)
        {
            float calculatedValue = value + magnification * INCREMENT;
            float clampedValue = _behaviour.ValidateValue(calculatedValue);

            return MathF.Round(clampedValue, _needDigits, MidpointRounding.AwayFromZero);
        }

        protected override void _LoadValue()
        {
            float initValue = _getter();
            var value = NoaDebuggerPrefs.GetFloat(PrefsKey, initValue);
            ChangeValue(value);
        }

        protected override void _SaveValue()
        {
            NoaDebuggerPrefs.SetFloat(PrefsKey, Value);
        }

        public override void Accept(ICustomParameterVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

