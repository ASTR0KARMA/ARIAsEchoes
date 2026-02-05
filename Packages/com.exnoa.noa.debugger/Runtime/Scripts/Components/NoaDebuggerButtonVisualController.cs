using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class NoaDebuggerButtonVisualController : MonoBehaviour
    {
        enum ButtonType
        {
            InScene,
            InController
        }

        bool _isInitialized = false;

        NoaDebuggerSettings _noaDebuggerSettings = null;

        bool _isPlayingAnimation = false;

        (float, float)[] _alertAnimationParameters = null;

        (float, float)[] CachedAlertAnimationParameters =>
            _alertAnimationParameters ??= new[]
            {
                (0.75f, 1f),
                (0.75f, 0f),
                (0.75f, 1f),
                (0.75f, 0f),
                (0.75f, 1f)
            };

        [SerializeField]
        ButtonType _buttonType;

        [SerializeField]
        Image _bootButtonBackgroundImage;

        [SerializeField]
        CanvasGroup _bootButtonCanvasGroup;

        public bool IsPlayingAnimation => _isPlayingAnimation;

        public void Init()
        {
            if (_isInitialized)
            {
                return;
            }

            _isInitialized = true;

            _noaDebuggerSettings = NoaDebuggerSettingsManager.GetNoaDebuggerSettings();

            if (_buttonType == ButtonType.InController)
            {
                _SetCanvasGroupEnabled(false);
            }
        }

        public void ResetButtonColor(bool isForceResetInAnimation = true)
        {
            if (_noaDebuggerSettings == null)
            {
                return;
            }

            if (!isForceResetInAnimation && _isPlayingAnimation)
            {
                return;
            }

            if (_buttonType == ButtonType.InController)
            {
                _SetCanvasGroupEnabled(false);
            }

            _isPlayingAnimation = false;

            bool isErrorNotice = NoaDebugger.IsErrorNotice &&
                                 _noaDebuggerSettings.ErrorNotificationType == ErrorNotificationType.Full;

            if (_buttonType == ButtonType.InScene)
            {
                _bootButtonCanvasGroup.alpha = _noaDebuggerSettings.ToolStartButtonAlpha;
            }

            Color defaultColor = _buttonType == ButtonType.InScene
                ? NoaDebuggerDefine.BackgroundColors.NoaDebuggerButtonDefault
                : NoaDebuggerDefine.BackgroundColors.NoaDebuggerButtonInControllerDefault;

            Color newColor = isErrorNotice
                ? NoaDebuggerDefine.BackgroundColors.NoaDebuggerButtonAlert
                : defaultColor;

            if (_buttonType == ButtonType.InController)
            {
                newColor.a = _noaDebuggerSettings.ControllerBackgroundAlpha;
            }

            _bootButtonBackgroundImage.color = newColor;
        }

        public void PlayOnErrorAnimation(Func<bool> breakCondition)
        {
            if (_noaDebuggerSettings.ErrorNotificationType == ErrorNotificationType.None ||
                _bootButtonCanvasGroup == null)
            {
                return;
            }

            if (_buttonType == ButtonType.InController)
            {
                _SetCanvasGroupEnabled(true);
            }

            Action onFinish = null;

            if (!NoaDebuggerVisibilityManager.IsTriggerButtonActive && !NoaDebuggerVisibilityManager.IsMainViewActive)
            {
                if (_buttonType == ButtonType.InScene && _IsActive())
                {
                    _SetActive(true, true);
                    onFinish = () => _SetActive(false);
                }
            }

            _bootButtonBackgroundImage.color = NoaDebuggerDefine.BackgroundColors.NoaDebuggerButtonAlert;

            StartCoroutine(_CanvasGroupAlphaAnimation(CachedAlertAnimationParameters, breakCondition, onFinish));
        }

        public void PlayOnLocationAnimation(Func<bool> breakCondition)
        {
            Action onFinish = null;

            if (!NoaDebuggerVisibilityManager.IsTriggerButtonActive && !NoaDebuggerVisibilityManager.IsMainViewActive)
            {
                _SetActive(true);
                onFinish = () => _SetActive(false);
            }

            var animationParameters = new[]
            {
                (0.25f, 1f),
                (0.58f, 1f),
                (0.17f, 0f),
                (0.17f, 1f),
                (0.58f, 1f),
                (0.25f, 0f)
            };

            StartCoroutine(_CanvasGroupAlphaAnimation(animationParameters, breakCondition, onFinish));
        }

        public void SetCanvasGroupAlpha(float alpha)
        {
            _bootButtonCanvasGroup.alpha = alpha;
        }

        void _SetCanvasGroupEnabled(bool isEnabled)
        {
            _bootButtonCanvasGroup.enabled = isEnabled;
        }

        bool _IsActive()
        {
            switch (_buttonType)
            {
                case ButtonType.InScene:
                    return !NoaDebuggerVisibilityManager.IsControllerActive;

                case ButtonType.InController:
                    return NoaDebuggerVisibilityManager.IsControllerActive;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        IEnumerator _CanvasGroupAlphaAnimation((float, float)[] parameters, Func<bool> breakCondition,
                                               Action onFinish = null)
        {
            if (_isPlayingAnimation || !_IsActive())
            {
                yield break;
            }

            _isPlayingAnimation = true;
            float animationStartTime = Time.realtimeSinceStartup;
            float totalElapsedTime = 0f;
            float totalPhaseTime = 0f;

            foreach ((float, float) parameter in parameters)
            {
                float phaseTime = parameter.Item1;
                float phaseTargetAlpha = parameter.Item2;

                float phaseStartAlpha = _bootButtonCanvasGroup.alpha;

                float totalPhaseTimeBefore = totalPhaseTime;
                totalPhaseTime += phaseTime;

                while (totalElapsedTime <= totalPhaseTime)
                {
                    if (breakCondition())
                    {
                        _isPlayingAnimation = false;

                        yield break;
                    }

                    totalElapsedTime = Time.realtimeSinceStartup - animationStartTime;

                    float phaseElapsedTime = totalElapsedTime - totalPhaseTimeBefore;

                    float t = Mathf.InverseLerp(0, phaseTime, phaseElapsedTime);
                    float alpha = Mathf.Lerp(phaseStartAlpha, phaseTargetAlpha, t);
                    _bootButtonCanvasGroup.alpha = alpha;

                    yield return null;
                }
            }

            ResetButtonColor();
            onFinish?.Invoke();
            _isPlayingAnimation = false;
        }

        void _SetActive(bool isActive, bool onError = false)
        {
            ResetButtonColor();

            if (onError)
            {
                if (NoaDebuggerVisibilityManager.IsControllerActive)
                {
                    return;
                }

                NoaDebuggerVisibilityManager.OnErrorTriggerButtonVisibility();
            }
            else
            {
                NoaDebuggerVisibilityManager.SetTriggerButtonVisible(isActive);
            }
        }

        public void Dispose()
        {
            _noaDebuggerSettings = default;
            _alertAnimationParameters = default;
        }
    }
}
