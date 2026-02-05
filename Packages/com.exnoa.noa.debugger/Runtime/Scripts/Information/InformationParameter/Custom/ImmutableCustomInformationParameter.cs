using System;

namespace NoaDebugger
{
    abstract class ImmutableCustomInformationParameterBase<T> : ICustomInformationParameter
    {
        readonly Func<T> _getter;

        protected ImmutableCustomInformationParameterBase(Func<T> getValue)
        {
            if (getValue == null)
            {
                throw new ArgumentException($"function getValue is null");
            }

            _getter = getValue;
        }

        public T Value => _getter();

        public string GetStringValue()
        {
            return _getter().ToString();
        }

        public abstract void Accept(ICustomParameterVisitor visitor);
    }
}
