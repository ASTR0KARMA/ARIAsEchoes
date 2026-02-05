using System;

namespace NoaDebugger
{
    [Serializable]
    sealed class BoolSettingParameter : SettingParameterBase<bool>
    {
        public BoolSettingParameter(bool defaultValue)
            : base(defaultValue) { }
    }
}
