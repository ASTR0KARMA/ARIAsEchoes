using System;

namespace NoaDebugger
{
    sealed class IntImmutableCustomInformationParameter : ImmutableCustomInformationParameterBase<int>
    {
        public IntImmutableCustomInformationParameter(Func<int> getValue) : base(getValue) { }

        public override void Accept(ICustomParameterVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
