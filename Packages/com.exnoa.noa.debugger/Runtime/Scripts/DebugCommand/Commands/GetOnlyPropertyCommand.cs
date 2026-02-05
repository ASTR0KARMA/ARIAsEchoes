using System;
using System.Collections.Generic;

namespace NoaDebugger.DebugCommand
{
    sealed class GetOnlyPropertyCommand : CommandBase, ICommand
    {
        public bool IsInteractable
        {
            get => false;
            set { }
        }

        public bool IsVisible { get; set; } = true;

        protected override string TypeName => "GetOnly Property";

        readonly Func<object> _getter;

        public GetOnlyPropertyCommand(CommandBaseInfo info, Func<object> getter) : base(info)
        {
            _getter = getter;
        }

        public void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string GetValue()
        {
            try
            {
                object value = _getter();

                return (value is not null) ? value.ToString() : string.Empty;
            }
            catch
            {
                throw;
            }
        }

        public override Dictionary<string, string> CreateDetailContext()
        {
            Dictionary<string, string> context = base.CreateDetailContext();
            context.Add("Value", $"{GetValue()}");

            return context;
        }
    }
}
