using UnityEngine;

namespace Singleton
{
    [DefaultExecutionOrder(-1)]
    public abstract class PersistentSingleton<T> : Singleton<T> where T : Component
    {

        public override void Awake()
        {
            base.Awake();

            if (Instance.HasValue && Instance.Value == this)
            {
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}