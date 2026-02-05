using System;

namespace NoaDebugger
{
    sealed class BoolImmutableCustomInformationParameter : ImmutableCustomInformationParameterBase<bool>
    {
        public BoolImmutableCustomInformationParameter(Func<bool> getValue) : base(getValue) { }

        public override void Accept(ICustomParameterVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

