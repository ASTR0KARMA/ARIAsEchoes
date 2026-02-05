using System;

namespace NoaDebugger
{
    interface IMutableParameter<T>
    {
        T Value { get; }

        bool IsShowOverrideMarker { get; }

        void ChangeValue(T newValue);
    }

    interface IMutableNumericParameter<T> : IMutableParameter<T>
    {
        T FromString(string textValue);

        bool IsEqualOrUnderMin(T value);

        bool IsEqualOrOverMax(T value);

        T ValidateValueForFluctuation(T value, int magnification);
    }

    interface IMutableEnumParameter : IMutableParameter<Enum>
    {
        string[] EnumNames { get; }

        Enum[] EnumValues { get; }
    }
}
