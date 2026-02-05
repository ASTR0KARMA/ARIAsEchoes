namespace NoaDebugger
{
    abstract class NumericBehaviourBase<T>
    {
        readonly protected T _inputRangeMin;
        readonly protected T _inputRangeMax;

        protected NumericBehaviourBase(T inputRangeMin, T inputRangeMax)
        {
            _inputRangeMin = inputRangeMin;
            _inputRangeMax = inputRangeMax;
        }

        protected bool IsNotNumeric(string text)
        {
            return string.IsNullOrEmpty(text) || text is "-" or ".";
        }
    }
}
