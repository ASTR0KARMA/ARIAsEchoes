using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class SettingsGroupPanel : MonoBehaviour
    {
        [SerializeField]
        Button _resetButton;

        public event UnityAction OnReset;

        void Awake()
        {
            _resetButton.onClick.AddListener(_OnClickReset);
        }

        void _OnClickReset()
        {
            OnReset?.Invoke();
        }
    }
}
