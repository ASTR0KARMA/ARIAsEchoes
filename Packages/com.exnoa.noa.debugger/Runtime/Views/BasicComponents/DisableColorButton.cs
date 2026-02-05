using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class DisableColorButton : DisableButtonBase
    {
        [SerializeField]
        Color _enable = NoaDebuggerDefine.ImageColors.Default;
        [SerializeField]
        Color _disable = NoaDebuggerDefine.ImageColors.Disabled;
        [SerializeField]
        Graphic[]  _targetGraphics;

        protected override void Awake()
        {
            foreach (Graphic graphic in _targetGraphics)
            {
                Assert.IsNotNull(graphic);
            }
            base.Awake();
        }

        protected override void _Refresh()
        {
            Color newColor = Interactable ? _enable : _disable;

            foreach (Graphic graphic in _targetGraphics)
            {
                graphic.color = newColor;
            }
        }
    }
}
