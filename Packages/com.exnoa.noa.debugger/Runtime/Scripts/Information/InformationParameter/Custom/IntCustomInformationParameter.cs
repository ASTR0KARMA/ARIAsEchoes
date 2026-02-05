using System;

namespace NoaDebugger
{
    sealed class IntCustomInformationParameter : CustomInformationParameterBase<int>, IMutableNumericParameter<int>
    {
        const int INPUT_RANGE_MIN = int.MinValue;
        const int INPUT_RANGE_MAX = int.MaxValue;
        const int INCREMENT = 1;

        IntParameterBehaviour _behaviour;

        public IntCustomInformationParameter(string groupName, string parameterName, Func<int> getter, Action<int> setter) : base(groupName, parameterName, getter, setter)
        {
            _behaviour = new IntParameterBehaviour(INPUT_RANGE_MIN, INPUT_RANGE_MAX);
        }

        public int FromString(string textValue) => _behaviour.FromString(textValue);

        public bool IsEqualOrUnderMin(int value) => _behaviour.IsEqualOrUnderMin(value);

        public bool IsEqualOrOverMax(int value) => _behaviour.IsEqualOrOverMax(value);

        public int ValidateValueForFluctuation(int value, int magnification)
        {
            int calculatedValue = value + magnification * IntCustomInformationParameter.INCREMENT;
            return _behaviour.ValidateValue(calculatedValue);
        }

        protected override void _LoadValue()
        {
            var initValue = _getter();
            var value = NoaDebuggerPrefs.GetInt(PrefsKey, initValue);
            ChangeValue(value);
        }

        protected override void _SaveValue()
        {
            NoaDebuggerPrefs.SetInt(PrefsKey, Value);
        }

        public override void Accept(ICustomParameterVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
