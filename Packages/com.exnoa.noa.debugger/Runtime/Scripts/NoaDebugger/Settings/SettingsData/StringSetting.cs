using System;

namespace NoaDebugger
{
    [Serializable]
    sealed class StringSetting : SettingParameterBase<string>
    {
        public StringSetting(string defaultValue)
            : base(defaultValue) { }
    }
}
