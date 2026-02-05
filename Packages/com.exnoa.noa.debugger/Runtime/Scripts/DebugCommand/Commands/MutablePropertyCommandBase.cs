using System;
using System.Collections.Generic;

namespace NoaDebugger.DebugCommand
{
    abstract class MutablePropertyCommandBase<T> : CommandBase, ICommand
    {
        public bool IsInteractable { get; set; } = true;
        public bool IsVisible { get; set; } = true;

        readonly Func<T> _getter;
        readonly Action<T> _setter;

        protected string SaveKey { get; }

        protected bool SavesOnUpdate => !string.IsNullOrEmpty(SaveKey);

        protected MutablePropertyCommandBase(CommandBaseInfo info, MutablePropertyCommandBaseInfo<T> mutableInfo)
            : base(info)
        {
            _getter = mutableInfo._getter;
            _setter = mutableInfo._setter;
            SaveKey = mutableInfo._saveKey;
        }

        protected abstract void _Accept(ICommandVisitor visitor);
        public void Accept(ICommandVisitor visitor)
        {
            _Accept(visitor);
        }

        public override Dictionary<string, string> CreateDetailContext()
        {
            Dictionary<string, string> context = base.CreateDetailContext();
            context.Add("Value", $"{_getter()}");
            context.Add("SaveOnUpdate", $"{SavesOnUpdate}");

            return context;
        }

        protected T InvokeGetter()
        {
            try
            {
                return _getter();
            }
            catch
            {
                throw;
            }
        }

        protected void InvokeSetter(T value)
        {
            try
            {
                _setter(value);
            }
            catch
            {
                throw;
            }
        }
    }

    struct MutablePropertyCommandBaseInfo<T>
    {
        public Func<T> _getter;
        public Action<T> _setter;
        public string _saveKey;

        public MutablePropertyCommandBaseInfo(Func<T> getter, Action<T> setter, string saveKey = null)
        {
            _getter = getter;
            _setter = setter;
            _saveKey = saveKey;
        }
    }
}
