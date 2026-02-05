using UnityEngine;
using UnityEngine.Events;

namespace NoaDebugger
{
    abstract class MutableSettingsViewBase : SettingsViewBase
    {
        [SerializeField]
        protected SettingsGroupPanel _groupPanel;

        public event UnityAction<SettingsResetButtonType> OnReset;

        protected abstract SettingsResetButtonType ResetButtonType { get; }

        protected virtual void Awake()
        {
            if (_groupPanel != null)
            {
                _groupPanel.OnReset += () =>
                {
                    OnReset?.Invoke(ResetButtonType);
                };
            }
        }
    }
}
