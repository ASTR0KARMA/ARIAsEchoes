namespace NoaDebugger
{
    interface ICustomParameterVisitor
    {
        void Visit(IntImmutableCustomInformationParameter parameter);
        void Visit(FloatImmutableCustomInformationParameter parameter);
        void Visit(BoolImmutableCustomInformationParameter parameter);
        void Visit(StringImmutableCustomInformationParameter parameter);
        void Visit(EnumImmutableCustomInformationParameter parameter);

        void Visit(IntCustomInformationParameter parameter);
        void Visit(FloatCustomInformationParameter parameter);
        void Visit(BoolCustomInformationParameter parameter);
        void Visit(StringCustomInformationParameter parameter);
        void Visit(EnumCustomInformationParameter parameter);
    }
}
