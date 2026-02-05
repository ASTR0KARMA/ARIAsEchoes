using System;

namespace NoaDebugger
{
    /// <summary>
    /// Custom bool information parameter
    /// </summary>
    public class NoaCustomInformationBoolValue : NoaCustomInformationValue<bool>
    {
        internal NoaCustomInformationBoolValue(Func<bool> getValue, Action<bool> setValue)
            : base(getValue, setValue)
        {
        }
    }
}
