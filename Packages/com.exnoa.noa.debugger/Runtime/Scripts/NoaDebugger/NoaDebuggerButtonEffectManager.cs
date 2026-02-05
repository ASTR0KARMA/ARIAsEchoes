using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class NoaDebuggerButtonEffectManager : MonoBehaviour
    {
        [SerializeField]
        NoaDebuggerButton _noaDebuggerButton;
        [SerializeField]
        NoaDebuggerButtonInController _buttonInController;

        bool _isInitialized = false;

        public NoaDebuggerButton NoaDebuggerButton => _noaDebuggerButton;

        public void Init()
        {
            if (_isInitialized)
            {
                return;
            }

            _OnValidateUI();
            _isInitialized = true;
            _noaDebuggerButton.Init();
            _buttonInController.Init();
        }

        void _OnValidateUI()
        {
            Assert.IsNotNull(_noaDebuggerButton);
            Assert.IsNotNull(_buttonInController);
        }

        public void PlayOnErrorAnimation()
        {
            if (NoaDebuggerVisibilityManager.IsControllerActive)
            {
                _buttonInController.PlayOnErrorAnimation();
            }
            else
            {
                _noaDebuggerButton.PlayOnErrorAnimation();
            }
        }

        public void PlayOnLocationAnimation()
        {
            if (!NoaDebuggerVisibilityManager.IsControllerActive)
            {
                _noaDebuggerButton.PlayOnLocationAnimation();
            }
        }

        public void ResetButtonColor()
        {
            if (NoaDebuggerVisibilityManager.IsControllerActive)
            {
                _buttonInController.ResetButtonColor();
            }
            else
            {
                _noaDebuggerButton.ResetButtonColor();
            }
        }

        public bool IsPlayingAnimation()
        {
            if (NoaDebuggerVisibilityManager.IsControllerActive)
            {
                return _buttonInController.IsPlayingAnimation;
            }
            else
            {
                return _noaDebuggerButton.IsPlayingAnimation;
            }
        }

        public void Dispose()
        {
            _noaDebuggerButton.Dispose();
            _buttonInController.Dispose();
            _noaDebuggerButton = default;
            _buttonInController = default;
        }
    }
}
