using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class ToastView : ViewBase<ToastViewLinker>
    {
        [SerializeField]
        TextMeshProUGUI _label;

        [SerializeField]
        Animator _animator;

        CanvasGroup _canvasGroup;
        bool _playAnim;

        public void SetVisibility(bool visible)
        {
            _canvasGroup.alpha = visible ? 1.0f : 0.0f;

            _animator.enabled = visible;
        }

        protected override void _Init()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_label);
            Assert.IsNotNull(_animator);
            Assert.IsNotNull(_canvasGroup);
        }

        protected override void _OnShow(ToastViewLinker linker)
        {
            _label.text = linker._label;
            gameObject.SetActive(true);
            _animator.Play("show_toast", layer: 0, normalizedTime: 0f);
            _playAnim = true;
        }

        protected override void _OnHide()
        {
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
        }

        void Update()
        {
            if (!_playAnim)
            {
                return;
            }

            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsName("show_toast"))
            {
                Hide();
            }
        }
    }

    sealed class ToastViewLinker : ViewLinkerBase
    {
        public string _label;
    }
}
