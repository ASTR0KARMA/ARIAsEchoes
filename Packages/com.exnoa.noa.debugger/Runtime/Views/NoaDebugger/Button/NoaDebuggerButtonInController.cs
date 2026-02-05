using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class NoaDebuggerButtonInController : MonoBehaviour
    {
        [SerializeField]
        NoaDebuggerButtonVisualController _buttonVisualController;

        bool _isInitialized = false;

        public bool IsPlayingAnimation => _buttonVisualController.IsPlayingAnimation;

        public void Init()
        {
            if (_isInitialized)
            {
                return;
            }

            _OnValidateUI();
            _isInitialized = true;
            _buttonVisualController.Init();
        }

        void _OnValidateUI()
        {
            Assert.IsNotNull(_buttonVisualController);
        }

        public void PlayOnErrorAnimation()
        {
            _buttonVisualController.PlayOnErrorAnimation(
                () => !NoaDebuggerVisibilityManager.IsControllerActive);
        }

        public void ResetButtonColor()
        {
            _buttonVisualController.ResetButtonColor();
        }

        public void Dispose()
        {
            _buttonVisualController.Dispose();
            _buttonVisualController = default;
        }
    }
}
