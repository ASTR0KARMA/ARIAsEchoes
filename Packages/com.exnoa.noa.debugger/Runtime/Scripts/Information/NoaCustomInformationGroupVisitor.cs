using System.Collections.Generic;

namespace NoaDebugger
{
    sealed class NoaCustomInformationGroupVisitor : ICustomParameterVisitor
    {
        readonly Dictionary<string, NoaCustomInformationIntValue> _intKeyValues;
        readonly Dictionary<string, NoaCustomInformationFloatValue> _floatKeyValues;
        readonly Dictionary<string, NoaCustomInformationStringValue> _stringKeyValues;
        readonly Dictionary<string, NoaCustomInformationBoolValue> _boolKeyValues;
        readonly Dictionary<string, NoaCustomInformationEnumValue> _enumKeyValues;
        readonly string _keyName;

        public NoaCustomInformationGroupVisitor(
            Dictionary<string, NoaCustomInformationIntValue> intKeyValues,
            Dictionary<string, NoaCustomInformationFloatValue> floatKeyValues,
            Dictionary<string, NoaCustomInformationStringValue> stringKeyValues,
            Dictionary<string, NoaCustomInformationBoolValue> boolKeyValues,
            Dictionary<string, NoaCustomInformationEnumValue> enumKeyValues,
            string keyName)
        {
            _intKeyValues = intKeyValues;
            _floatKeyValues = floatKeyValues;
            _stringKeyValues = stringKeyValues;
            _boolKeyValues = boolKeyValues;
            _enumKeyValues = enumKeyValues;
            _keyName = keyName;
        }

        public void Visit(IntImmutableCustomInformationParameter parameter)
        {
            var informationValue = new NoaCustomInformationIntValue(
                () => parameter.Value,
                null
            );
            _intKeyValues.Add(_keyName, informationValue);
        }

        public void Visit(FloatImmutableCustomInformationParameter parameter)
        {
            var informationValue = new NoaCustomInformationFloatValue(
                () => parameter.Value,
                null
            );
            _floatKeyValues.Add(_keyName, informationValue);
        }

        public void Visit(BoolImmutableCustomInformationParameter parameter)
        {
            var informationValue = new NoaCustomInformationBoolValue(
                () => parameter.Value,
                null
            );
            _boolKeyValues.Add(_keyName, informationValue);
        }

        public void Visit(StringImmutableCustomInformationParameter parameter)
        {
            var informationValue = new NoaCustomInformationStringValue(
                () => parameter.Value,
                null
            );
            _stringKeyValues.Add(_keyName, informationValue);
        }

        public void Visit(EnumImmutableCustomInformationParameter parameter)
        {
            var informationValue = new NoaCustomInformationEnumValue(
                () => parameter.Value,
                null
            );
            _enumKeyValues.Add(_keyName, informationValue);
        }

        public void Visit(IntCustomInformationParameter parameter)
        {
            var informationValue = new NoaCustomInformationIntValue(
                () => parameter.Value,
                (value) => parameter.ChangeValue(value)
            );
            _intKeyValues.Add(_keyName, informationValue);
        }

        public void Visit(FloatCustomInformationParameter parameter)
        {
            var informationValue = new NoaCustomInformationFloatValue(
                () => parameter.Value,
                (value) => parameter.ChangeValue(value)
            );
            _floatKeyValues.Add(_keyName, informationValue);
        }

        public void Visit(BoolCustomInformationParameter parameter)
        {
            var informationValue = new NoaCustomInformationBoolValue(
                () => parameter.Value,
                (value) => parameter.ChangeValue(value)
            );
            _boolKeyValues.Add(_keyName, informationValue);
        }

        public void Visit(StringCustomInformationParameter parameter)
        {
            var informationValue = new NoaCustomInformationStringValue(
                () => parameter.Value,
                (value) => parameter.ChangeValue(value)
            );
            _stringKeyValues.Add(_keyName, informationValue);
        }

        public void Visit(EnumCustomInformationParameter parameter)
        {
            var informationValue = new NoaCustomInformationEnumValue(
                () => parameter.Value,
                (value) => parameter.ChangeValue(value)
            );
            _enumKeyValues.Add(_keyName, informationValue);
        }
    }
}
