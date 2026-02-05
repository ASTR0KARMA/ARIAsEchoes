namespace NoaDebugger.DebugCommand
{
    sealed class IntPropertyCommand : MutableNumericPropertyCommandBase<int>
    {
        const int DEFAULT_INPUT_RANGE_MIN = int.MinValue;
        const int DEFAULT_INPUT_RANGE_MAX = int.MaxValue;
        public const int DEFAULT_INCREMENT = 1;

        IntParameterBehaviour _behaviour;

        protected override string TypeName => "Int Property";

        public IntPropertyCommand(
            CommandBaseInfo info,
            MutablePropertyCommandBaseInfo<int> mutableInfo,
            MutableNumericPropertyCommandBaseInfo<int> numericInfo)
            : base(info, mutableInfo, numericInfo)
        {
            _inputRangeMin = numericInfo._inputRangeMin ?? IntPropertyCommand.DEFAULT_INPUT_RANGE_MIN;
            _inputRangeMax = numericInfo._inputRangeMax ?? IntPropertyCommand.DEFAULT_INPUT_RANGE_MAX;
            Increment = numericInfo._increment ?? IntPropertyCommand.DEFAULT_INCREMENT;

            _behaviour = new IntParameterBehaviour(_inputRangeMin, _inputRangeMax);

            if (SavesOnUpdate && NoaDebuggerPrefs.HasKey(SaveKey))
            {
                SetValue(NoaDebuggerPrefs.GetInt(SaveKey, GetValue()));
            }
        }

        protected override void _Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override int GetValue()
        {
            return _behaviour.ValidateValue(InvokeGetter());
        }

        public override void SetValue(int value)
        {
            value = _behaviour.ValidateValue(value);
            InvokeSetter(value);

            if (SavesOnUpdate)
            {
                NoaDebuggerPrefs.SetInt(SaveKey, value);
            }
        }

        public override int FromString(string textValue) => _behaviour.FromString(textValue);

        public override bool IsEqualOrUnderMin(int value) => _behaviour.IsEqualOrUnderMin(value);

        public override bool IsEqualOrOverMax(int value) => _behaviour.IsEqualOrOverMax(value);

        public override int ValidateValueForFluctuation(int value, int magnification)
        {
            if (magnification < 0)
            {
                for (int i = 0; i > magnification; i--)
                {
                    if ((value < 0 && _inputRangeMin - value > -Increment) ||
                        (value > 0 && _inputRangeMin > value - Increment))
                    {
                        return _inputRangeMin;
                    }

                    value -= Increment;
                }

                return value;
            }

            if (magnification > 0)
            {
                for (int i = 0; i < magnification; i++)
                {
                    if ((value < 0 && _inputRangeMax < value + Increment) ||
                        (value > 0 && _inputRangeMax - value < Increment))
                    {
                        return _inputRangeMax;
                    }

                    value += Increment;
                }

                return value;
            }

            return _behaviour.ValidateValue(value);
        }
    }
}
