using System;
using System.Collections;
using UnityEngine.Events;

namespace NoaDebugger.DebugCommand
{
    sealed class CoroutineCommand : CommandBase, ICommand
    {
        public bool IsInteractable
        {
            get => _isInteractable && !_isCoroutineWaiting;
            set => _isInteractable = value;
        }

        public bool IsVisible { get; set; } = true;
        protected override string TypeName => "Coroutine";

        readonly Func<IEnumerator> _coroutine;
        bool _isInteractable = true;
        bool _isCoroutineWaiting;
        UnityAction _completeCallback;

        public CoroutineCommand(CommandBaseInfo info, Func<IEnumerator> coroutine) : base(info)
        {
            _coroutine = coroutine;
        }

        public void Accept(ICommandVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Invoke(UnityAction onComplete)
        {
            try
            {
                _completeCallback = onComplete;
                _isCoroutineWaiting = true;
                IEnumerator iterator = _coroutine();
                GlobalCoroutine.Run(iterator, _OnComplete);
            }
            catch
            {
                throw;
            }
        }

        void _OnComplete()
        {
            _isCoroutineWaiting = false;
            _completeCallback?.Invoke();
        }
    }
}
