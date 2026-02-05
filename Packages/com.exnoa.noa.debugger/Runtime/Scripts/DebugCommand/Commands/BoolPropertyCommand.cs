namespace NoaDebugger.DebugCommand
{
    sealed class BoolPropertyCommand : MutablePropertyCommandBase<bool>
    {
        protected override string TypeName => "Bool Property";

        public BoolPropertyCommand(CommandBaseInfo info, MutablePropertyCommandBaseInfo<bool> mutableInfo)
            : base(info, mutableInfo)
        {
            if (SavesOnUpdate && NoaDebuggerPrefs.HasKey(SaveKey))
            {
                SetValue(NoaDebuggerPrefs.GetBoolean(SaveKey, GetValue()));
            }
        }

        protected override void _Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }

        public bool GetValue()
        {
            return InvokeGetter();
        }

        public void SetValue(bool value)
        {
            InvokeSetter(value);

            if (SavesOnUpdate)
            {
                NoaDebuggerPrefs.SetBoolean(SaveKey, value);
            }
        }
    }
}
