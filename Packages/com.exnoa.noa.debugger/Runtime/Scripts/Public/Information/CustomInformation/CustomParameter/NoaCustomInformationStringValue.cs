using System;

namespace NoaDebugger
{
    /// <summary>
    /// Custom string information parameter
    /// </summary>
    public class NoaCustomInformationStringValue : NoaCustomInformationValue<string>
    {
        internal NoaCustomInformationStringValue(Func<string> getValue, Action<string> setValue)
            : base(getValue, setValue)
        {
        }
    }
}
