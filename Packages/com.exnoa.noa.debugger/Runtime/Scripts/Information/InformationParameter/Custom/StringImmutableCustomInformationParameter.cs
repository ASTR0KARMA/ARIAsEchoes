using System;

namespace NoaDebugger
{
    sealed class StringImmutableCustomInformationParameter : ImmutableCustomInformationParameterBase<string>
    {
        public StringImmutableCustomInformationParameter(Func<string> getValue) : base(getValue) { }

        public override void Accept(ICustomParameterVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

