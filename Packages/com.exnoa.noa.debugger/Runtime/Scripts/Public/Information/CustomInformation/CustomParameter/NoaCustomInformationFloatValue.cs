using System;

namespace NoaDebugger
{
    /// <summary>
    /// Custom float information parameter
    /// </summary>
    public class NoaCustomInformationFloatValue : NoaCustomInformationValue<float>
    {
        internal NoaCustomInformationFloatValue(Func<float> getValue, Action<float> setValue)
            : base(getValue, setValue)
        {
        }
    }
}
