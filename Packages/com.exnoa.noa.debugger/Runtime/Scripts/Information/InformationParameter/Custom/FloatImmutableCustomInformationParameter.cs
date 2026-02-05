using System;

namespace NoaDebugger
{
    sealed class FloatImmutableCustomInformationParameter : ImmutableCustomInformationParameterBase<float>
    {
        public FloatImmutableCustomInformationParameter(Func<float> getValue) : base(getValue) { }

        public override void Accept(ICustomParameterVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

