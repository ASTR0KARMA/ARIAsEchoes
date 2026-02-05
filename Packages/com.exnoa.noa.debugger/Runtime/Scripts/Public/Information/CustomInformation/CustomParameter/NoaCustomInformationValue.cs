using System;

namespace NoaDebugger
{
    /// <summary>
    /// Custom information parameter base class
    /// </summary>
    public class NoaCustomInformationValue<T>
    {
        public T Value
        {
            get
            {
                return GetValue();
            }
            set
            {
                if (SetValue == null)
                {
                    return;
                }

                SetValue(value);
            }
        }

        Func<T> GetValue { get; }
        Action<T> SetValue { get; }

        protected NoaCustomInformationValue(Func<T> getValue, Action<T> setValue = null)
        {
            GetValue = getValue;
            SetValue = setValue;
        }
    }
}
