namespace NoaDebugger.DebugCommand
{
    sealed class CharPropertyCommand : MutablePropertyCommandBase<char>
    {
        protected override string TypeName => "Char Property";

        public CharPropertyCommand(CommandBaseInfo info, MutablePropertyCommandBaseInfo<char> mutableInfo)
            : base(info, mutableInfo)
        {
            if (SavesOnUpdate && NoaDebuggerPrefs.HasKey(SaveKey))
            {
                SetValue(NoaDebuggerPrefs.GetChar(SaveKey, GetValue()));
            }
        }

        protected override void _Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }

        public char GetValue()
        {
            return InvokeGetter();
        }

        public void SetValue(char value)
        {
            InvokeSetter(value);

            if (SavesOnUpdate)
            {
                NoaDebuggerPrefs.SetChar(SaveKey, value);
            }
        }
    }
}
