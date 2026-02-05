using System;
namespace NoaDebugger
{
    sealed class InformationCustomKeyValue
    {
        public string Key { get; }
        public string DisplayName { get; }
        public ICustomInformationParameter Parameter { get; private set; }
        public int Order { get; }
        public bool IsSelect { get; set; }

        public InformationCustomKeyValue(string keyName, string displayName, int order, bool isSelect = false)
        {
            Key = keyName;
            DisplayName = string.IsNullOrEmpty(displayName) ? keyName : displayName;
            Order = order;
            IsSelect = isSelect;
        }

        public void SetParameter(string groupName, Func<int> getValue, Action<int> setValue)
        {
            Parameter = new IntCustomInformationParameter(groupName, Key, getValue, setValue);
        }

        public void SetParameter(string groupName, Func<bool> getValue, Action<bool> setValue)
        {
            Parameter = new BoolCustomInformationParameter(groupName, Key, getValue, setValue);
        }

        public void SetParameter(string groupName, Func<string> getValue, Action<string> setValue)
        {
            Parameter = new StringCustomInformationParameter(groupName, Key, getValue, setValue);
        }

        public void SetParameter(string groupName, Func<float> getValue, Action<float> setValue)
        {
            Parameter = new FloatCustomInformationParameter(groupName, Key, getValue, setValue);
        }

        public void SetParameter(string groupName, Func<Enum> getValue, Action<Enum> setValue)
        {
            Parameter = new EnumCustomInformationParameter(groupName, Key, getValue, setValue);
        }

        public void SetImmutableParameter(Func<int> getValue)
        {
            Parameter = new IntImmutableCustomInformationParameter(getValue);
        }

        public void SetImmutableParameter(Func<string> getValue)
        {
            Parameter = new StringImmutableCustomInformationParameter(getValue);
        }

        public void SetImmutableParameter(Func<bool> getValue)
        {
            Parameter = new BoolImmutableCustomInformationParameter(getValue);
        }

        public void SetImmutableParameter(Func<float> getValue)
        {
            Parameter = new FloatImmutableCustomInformationParameter(getValue);
        }

        public void SetImmutableParameter(Func<Enum> getValue)
        {
            Parameter = new EnumImmutableCustomInformationParameter(getValue);
        }
    }
}
