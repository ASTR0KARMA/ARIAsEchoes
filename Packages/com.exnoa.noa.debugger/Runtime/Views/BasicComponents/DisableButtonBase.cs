using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NoaDebugger
{
    [RequireComponent(typeof(Button))]
    abstract class DisableButtonBase : MonoBehaviour
    {
        public UnityEvent _onClick;

        protected Button _component;

        public bool Interactable
        {
            get
            {
                if (_component == null)
                {
                    _component = GetComponent<Button>();
                }

                return _component.interactable;
            }
            set
            {
                if (_component == null)
                {
                    _component = GetComponent<Button>();
                }

                _component.interactable = value;
                _Refresh();
            }
        }

        protected virtual void Awake()
        {
            _component = GetComponent<Button>();
            _Refresh();
            _component.onClick.AddListener(_OnClick);
        }

        void _OnClick()
        {
            _onClick?.Invoke();
        }

        protected abstract void _Refresh();

        void OnDestroy()
        {
            _component = default;
            _onClick = default;
        }
    }
}
