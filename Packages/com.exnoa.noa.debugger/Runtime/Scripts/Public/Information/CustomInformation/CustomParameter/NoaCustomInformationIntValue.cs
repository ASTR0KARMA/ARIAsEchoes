using System;

namespace NoaDebugger
{
    /// <summary>
    /// Custom int information parameter
    /// </summary>
    public class NoaCustomInformationIntValue : NoaCustomInformationValue<int>
    {
        internal NoaCustomInformationIntValue(Func<int> getValue, Action<int> setValue)
            : base(getValue, setValue)
        {
        }
    }
}
