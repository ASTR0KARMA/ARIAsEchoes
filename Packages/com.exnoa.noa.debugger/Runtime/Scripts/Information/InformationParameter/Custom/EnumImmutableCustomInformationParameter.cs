using System;

namespace NoaDebugger
{
    sealed class EnumImmutableCustomInformationParameter : ImmutableCustomInformationParameterBase<Enum>
    {
        public EnumImmutableCustomInformationParameter(Func<Enum> getValue) : base(getValue) { }

        public override void Accept(ICustomParameterVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

