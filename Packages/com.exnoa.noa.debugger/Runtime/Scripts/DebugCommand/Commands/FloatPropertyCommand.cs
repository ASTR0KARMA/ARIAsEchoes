using System;

namespace NoaDebugger.DebugCommand
{
    sealed class FloatPropertyCommand : MutableNumericPropertyCommandBase<float>
    {
        const float DEFAULT_INPUT_RANGE_MIN = float.MinValue;
        const float DEFAULT_INPUT_RANGE_MAX = float.MaxValue;
        public const float DEFAULT_INCREMENT = 1;

        protected override string TypeName => "Float Property";

        readonly int _needDigits;

        FloatParameterBehaviour _behaviour;

        public FloatPropertyCommand(
            CommandBaseInfo info,
            MutablePropertyCommandBaseInfo<float> mutableInfo,
            MutableNumericPropertyCommandBaseInfo<float> numericInfo,
            int needDigits = 0)
            : base(info, mutableInfo, numericInfo)
        {
            _inputRangeMin = numericInfo._inputRangeMin ?? FloatPropertyCommand.DEFAULT_INPUT_RANGE_MIN;
            _inputRangeMax = numericInfo._inputRangeMax ?? FloatPropertyCommand.DEFAULT_INPUT_RANGE_MAX;
            Increment = numericInfo._increment ?? FloatPropertyCommand.DEFAULT_INCREMENT;

            _behaviour = new FloatParameterBehaviour(_inputRangeMin, _inputRangeMax);

            if (SavesOnUpdate && NoaDebuggerPrefs.HasKey(SaveKey))
            {
                SetValue(NoaDebuggerPrefs.GetFloat(SaveKey, GetValue()));
            }

            _needDigits = needDigits;
        }

        protected override void _Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override float GetValue()
        {
            return _behaviour.ValidateValue(InvokeGetter());
        }

        public override void SetValue(float value)
        {
            value = _behaviour.ValidateValue(value);
            InvokeSetter(value);

            if (SavesOnUpdate)
            {
                NoaDebuggerPrefs.SetFloat(SaveKey, value);
            }
        }

        public override float FromString(string textValue) => _behaviour.FromString(textValue);

        public override bool IsEqualOrUnderMin(float value) => _behaviour.IsEqualOrUnderMin(value);

        public override bool IsEqualOrOverMax(float value) => _behaviour.IsEqualOrOverMax(value);

        public override float ValidateValueForFluctuation(float value, int magnification)
        {
            float tmpValue = value;

            if (magnification < 0)
            {
                for (int i = 0; i > magnification; i--)
                {
                    if ((tmpValue < 0 && _inputRangeMin - tmpValue > -Increment) ||
                        (tmpValue > 0 && _inputRangeMin > tmpValue - Increment))
                    {
                        return _inputRangeMin;
                    }

                    tmpValue -= Increment;
                }
            }

            else if (magnification > 0)
            {
                for (int i = 0; i < magnification; i++)
                {
                    if ((tmpValue < 0 && _inputRangeMax < tmpValue + Increment) ||
                        (tmpValue > 0 && _inputRangeMax - tmpValue < Increment))
                    {
                        return _inputRangeMax;
                    }

                    tmpValue += Increment;
                }
            }

            float clamped = _behaviour.ValidateValue(value + Increment * magnification);

            return MathF.Round(clamped, _needDigits, MidpointRounding.AwayFromZero);
        }
    }
}
