using System;

namespace NoaDebugger.DebugCommand
{
    sealed class MethodCommand : CommandBase, ICommand
    {
        public bool IsInteractable { get; set; } = true;
        public bool IsVisible { get; set; } = true;

        protected override string TypeName => "Method";

        readonly Action _method;

        public MethodCommand(CommandBaseInfo info, Action method) : base(info)
        {
            _method = method;
        }

        public void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Invoke()
        {
            try
            {
                _method();
            }
            catch
            {
                throw;
            }
        }
    }
}
