using System;
using System.Linq;

namespace NoaDebugger.DebugCommand
{
    sealed class UShortPropertyCommand : MutableNumericPropertyCommandBase<ushort>
    {
        static readonly ushort DEFAULT_INPUT_RANGE_MIN = ushort.MinValue;
        static readonly ushort DEFAULT_INPUT_RANGE_MAX = ushort.MaxValue;
        public static readonly ushort DEFAULT_INCREMENT = 1;

        protected override string TypeName => "UShort Property";

        public UShortPropertyCommand(
            CommandBaseInfo info,
            MutablePropertyCommandBaseInfo<ushort> mutableInfo,
            MutableNumericPropertyCommandBaseInfo<ushort> numericInfo)
            : base(info, mutableInfo, numericInfo)
        {
            _inputRangeMin = numericInfo._inputRangeMin ?? UShortPropertyCommand.DEFAULT_INPUT_RANGE_MIN;
            _inputRangeMax = numericInfo._inputRangeMax ?? UShortPropertyCommand.DEFAULT_INPUT_RANGE_MAX;
            Increment = numericInfo._increment ?? UShortPropertyCommand.DEFAULT_INCREMENT;

            if (SavesOnUpdate && NoaDebuggerPrefs.HasKey(SaveKey))
            {
                SetValue(NoaDebuggerPrefs.GetUShort(SaveKey, GetValue()));
            }
        }

        protected override void _Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override ushort GetValue()
        {
            return _ValidateValue(InvokeGetter());
        }

        public override void SetValue(ushort value)
        {
            value = _ValidateValue(value);
            InvokeSetter(value);

            if (SavesOnUpdate)
            {
                NoaDebuggerPrefs.SetUShort(SaveKey, value);
            }
        }

        public override ushort FromString(string textValue)
        {
            if (IsNotNumeric(textValue))
            {
                return 0;
            }

            if (ushort.TryParse(textValue, out ushort result))
            {
                return result;
            }

            return textValue.FirstOrDefault() == '-' ? ushort.MinValue : ushort.MaxValue;
        }

        public override bool IsEqualOrUnderMin(ushort value)
        {
            return value <= _inputRangeMin;
        }

        public override bool IsEqualOrOverMax(ushort value)
        {
            return value >= _inputRangeMax;
        }

        public override ushort ValidateValueForFluctuation(ushort value, int magnification)
        {
            if (magnification < 0)
            {
                for (int i = 0; i > magnification; i--)
                {
                    if(Increment > value - _inputRangeMin)
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
                    if(Increment > _inputRangeMax - value)
                    {
                        return _inputRangeMax;
                    }

                    value += Increment;
                }

                return value;
            }

            return _ValidateValue(value);
        }

        ushort _ValidateValue(ushort value)
        {
            return Math.Clamp(value, _inputRangeMin, _inputRangeMax);
        }
    }
}
