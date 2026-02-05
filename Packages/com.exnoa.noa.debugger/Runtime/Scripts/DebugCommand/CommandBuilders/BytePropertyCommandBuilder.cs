using System;

namespace NoaDebugger.DebugCommand
{
    sealed class BytePropertyCommandBuilder : MutableNumericPropertyCommandBuilderBase<byte>
    {
        public BytePropertyCommandBuilder(
            string categoryName, string displayName, Func<byte> getter, Action<byte> setter,
            Attribute[] attributes = null, string saveKey = null)
            : base(categoryName, displayName, getter, setter, attributes, saveKey) { }

        protected override void PeekAttribute(Attribute attribute)
        {
            base.PeekAttribute(attribute);

            if (_increment <= 0)
            {
                SendIncrementWarning();
                _increment = BytePropertyCommand.DEFAULT_INCREMENT;
            }
        }

        protected override ICommand BuildCommand()
        {
            return new BytePropertyCommand(CreateCommandInfo(), CreateMutableInfo(), CreateNumericInfo());
        }

        protected override byte? TryParse(object value)
        {
            if (byte.TryParse(value.ToString(), out byte result))
            {
                return result;
            }

            return null;
        }
    }
}
