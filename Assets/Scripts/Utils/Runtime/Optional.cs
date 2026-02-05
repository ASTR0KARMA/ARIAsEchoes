using System;

namespace Utils
{
    [Serializable]
    public readonly struct Optional<T>
    {
        private readonly T _value;
        private readonly bool _hasValue;

        public bool HasValue => _hasValue;
        public T Value
        {
            get
            {
                if (!_hasValue)
                {
                    throw new InvalidOperationException("Optional has no value");
                }
                return _value;
            }
        }

        private Optional(T value)
        {
            _value = value;
            _hasValue = true;
        }
    
        public static Optional<T> Of(T value)
        {
            return new Optional<T>(value);
        }

        public static Optional<T> OfNullable(T value)
        {
            if (value == null) return Empty();
            return Of(value);
        }
    
        public static Optional<T> Empty()
        {
            return new Optional<T>();
        }
    }
}
