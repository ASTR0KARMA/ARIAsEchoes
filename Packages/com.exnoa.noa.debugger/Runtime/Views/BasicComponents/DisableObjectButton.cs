using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class DisableObjectButton : DisableButtonBase
    {
        [SerializeField]
        GameObject _enable;
        [SerializeField]
        GameObject _disable;

        protected override void Awake()
        {
            Assert.IsNotNull(_enable);
            Assert.IsNotNull(_disable);
            base.Awake();
        }

        protected override void _Refresh()
        {
            _enable.SetActive(Interactable);
            _disable.SetActive(!Interactable);
        }
    }
}
