using System;

namespace NoaDebugger.DebugCommand
{
    sealed class ULongPropertyCommandBuilder : MutableNumericPropertyCommandBuilderBase<ulong>
    {
        public ULongPropertyCommandBuilder(
            string categoryName, string displayName, Func<ulong> getter, Action<ulong> setter,
            Attribute[] attributes = null, string saveKey = null)
            : base(categoryName, displayName, getter, setter, attributes, saveKey) { }

        protected override void PeekAttribute(Attribute attribute)
        {
            base.PeekAttribute(attribute);

            if (_increment <= 0)
            {
                SendIncrementWarning();
                _increment = ULongPropertyCommand.DEFAULT_INCREMENT;
            }
        }

        protected override ICommand BuildCommand()
        {
            return new ULongPropertyCommand(CreateCommandInfo(), CreateMutableInfo(), CreateNumericInfo());
        }

        protected override ulong? TryParse(object value)
        {
            if (ulong.TryParse(value.ToString(), out ulong result))
            {
                return result;
            }

            return null;
        }
    }
}
