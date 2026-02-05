using UnityEngine;
using Utils;

namespace Singleton
{
    [DefaultExecutionOrder(-1)]
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        protected static Optional<T> _instance;

        public static Optional<T> Instance
        {
            get  
            {
                if (!_instance.HasValue)
                {
                    _instance = Optional<T>.OfNullable(FindFirstObjectByType<T>());
                }

                return _instance;
            }
        }

        public static bool HasInstance => Instance.HasValue;

        public virtual void Awake()
        {
            if (!_instance.HasValue)
            {
                _instance = Optional<T>.Of(this as T);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public virtual void OnDestroy()
        {
            if (_instance.HasValue && _instance.Value == this)
            {
                _instance = Optional<T>.Empty();
            }
        }
    }
}