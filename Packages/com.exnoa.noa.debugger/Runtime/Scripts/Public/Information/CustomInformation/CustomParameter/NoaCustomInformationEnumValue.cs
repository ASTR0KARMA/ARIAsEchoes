using System;

namespace NoaDebugger
{
    /// <summary>
    /// Custom Enum information parameter
    /// </summary>
    public class NoaCustomInformationEnumValue : NoaCustomInformationValue<Enum>
    {
        internal NoaCustomInformationEnumValue(Func<Enum> getValue, Action<Enum> setValue)
            : base(getValue, setValue)
        {
        }
    }
}
