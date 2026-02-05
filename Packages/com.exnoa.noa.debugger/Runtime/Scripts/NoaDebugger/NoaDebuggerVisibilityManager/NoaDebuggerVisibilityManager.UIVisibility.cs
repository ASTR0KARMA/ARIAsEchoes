using System;
using System.Linq;
using UnityEngine;

namespace NoaDebugger
{
    sealed partial class NoaDebuggerVisibilityManager
    {
        [Serializable]
        sealed class MainViewVisibility
        {
            [SerializeField]
            GameObject _noaDebugger;
            [SerializeField]
            GameObject _background;

            public bool IsActive => _noaDebugger.activeInHierarchy;

            public void SetVisible(bool visible)
            {
                _noaDebugger.SetActive(visible);
                _SetActiveBg(visible);
            }

            public void OnEnableWorldSpaceVisibility()
            {
                _SetActiveBg(false);
            }

            public void OnDisableWorldSpaceVisibility()
            {
                _SetActiveBg(_noaDebugger.activeSelf);
            }

            void _SetActiveBg(bool visible)
            {
                if (NoaDebuggerManager.IsWorldSpaceRenderingEnabled)
                {
                    return;
                }

                _background.SetActive(visible);
            }
        }

        [Serializable]
        sealed class OverlayVisibility
        {
            [SerializeField]
            GameObject _overlayRoot;
            bool _visibleSetting = true;

            public bool IsRootActive => _overlayRoot.activeInHierarchy;

            public bool IsVisibility
            {
                get
                {
                    if (_overlayRoot.activeInHierarchy == false)
                    {
                        return false;
                    }

                    bool isOverlayVisible = false;
                    foreach (Transform child in _overlayRoot.transform)
                    {
                        if (child.gameObject.activeInHierarchy)
                        {
                            isOverlayVisible = true;
                            break;
                        }
                    }

                    return isOverlayVisible;
                }
            }

            public event Action<bool> OnSetActiveOverlay;

            void _SetOverlayActive(bool active)
            {
                OnSetActiveOverlay?.Invoke(active);
                _overlayRoot.SetActive(active);
            }

            public void SetVisible(bool visible)
            {
                if (NoaDebuggerManager.IsWorldSpaceRenderingEnabled)
                {
                    _SetOverlayActive(false);
                    return;
                }

                if (_visibleSetting == false)
                {
                    _SetOverlayActive(false);
                    return;
                }

                _SetOverlayActive(visible);

                if (visible)
                {
                    _visibleSetting = true;
                }
            }

            public void SetVisibleSetting(bool visible)
            {
                _visibleSetting = visible;
                SetVisible(visible);
            }
        }

        [Serializable]
        sealed class FloatingWindowVisibility
        {
            [SerializeField]
            GameObject _floatingWindowRoot;
            bool _visibleSetting = true;

            public bool IsVisibility
            {
                get
                {
                    if (_floatingWindowRoot.activeInHierarchy == false)
                    {
                        return false;
                    }

                    bool isFloatingWindowVisible = false;
                    foreach (Transform child in _floatingWindowRoot.transform)
                    {
                        if (child.gameObject.activeInHierarchy)
                        {
                            isFloatingWindowVisible = true;
                            break;
                        }
                    }

                    return isFloatingWindowVisible;
                }
            }

            public void SetVisible(bool visible)
            {
                if (NoaDebuggerManager.IsWorldSpaceRenderingEnabled)
                {
                    _floatingWindowRoot.SetActive(false);
                    return;
                }

                if (_visibleSetting == false)
                {
                    _floatingWindowRoot.SetActive(false);
                    return;
                }

                _floatingWindowRoot.SetActive(visible);

                if (visible)
                {
                    foreach (Transform child in _floatingWindowRoot.transform)
                    {
                        child.gameObject.SetActive(true);
                    }

                    _visibleSetting = true;
                }
            }

            public void SetVisibleSetting(bool visible)
            {
                _visibleSetting = visible;
                SetVisible(visible);
            }
        }

        [Serializable]
        sealed class TriggerButtonVisibility
        {
            [SerializeField]
            GameObject _triggerButton;
            [SerializeField]
            GameObject _visibleButton;

            bool _visibleSetting = true;

            public bool IsActive => _triggerButton.activeInHierarchy;

            public event Action<bool> OnSetActiveTriggerButton;

            void _SetTriggerButtonActive(bool active)
            {
                OnSetActiveTriggerButton?.Invoke(active);
                _triggerButton.SetActive(active);
            }

            public void SetVisible(bool visible)
            {
                if (NoaDebuggerManager.IsWorldSpaceRenderingEnabled)
                {
                    return;
                }

                if (_visibleSetting == false)
                {
                    _SetTriggerButtonActive(false);
                    _visibleButton.SetActive(false);
                    return;
                }

                if (IsControllerActive)
                {
                    _SetTriggerButtonActive(false);
                    _visibleButton.SetActive(true);
                    return;
                }

                _SetTriggerButtonActive(visible);
                _visibleButton.SetActive(!visible);

                if (visible)
                {
                    _visibleSetting = true;
                }
            }

            public void InvalidateTriggerButton()
            {
                _SetTriggerButtonActive(false);
                _visibleButton.SetActive(false);
            }

            public void SetVisibleSetting(bool visible)
            {
                _visibleSetting = visible;

                if (visible)
                {
                    SetVisible(visible);
                }
                else
                {
                    InvalidateTriggerButton();
                }
            }

            public void OnErrorVisible()
            {
                if (IsActive == false
                    && NoaDebuggerManager.IsError() == false
                    && NoaDebuggerSettingsManager.GetNoaDebuggerSettings().ErrorNotificationType == ErrorNotificationType.None)
                {
                    return;
                }

                _SetTriggerButtonActive(true);
            }
        }

        [Serializable]
        sealed class ControllerVisibility
        {
            [SerializeField]
            GameObject _controller;

            public bool IsActive => _controller.activeInHierarchy;

            public void SetVisible(bool visible)
            {
                _controller.SetActive(visible);
            }
        }

        sealed class AllUIElementVisibility
        {
            public bool IsActive => NoaDebuggerManager.GetAllUIElements().Any(elementView => elementView.GameObject.activeSelf);

            bool _visibleSetting = true;

            public void SetVisible(bool visible)
            {
                if (visible && _visibleSetting == false)
                {
                    return;
                }

                foreach (var elementView in NoaDebuggerManager.GetAllUIElements())
                {
                    elementView.GameObject.SetActive(visible);
                }

                if (visible)
                {
                    _visibleSetting = true;
                }
            }

            public void SetVisibleSetting(bool visible)
            {
                _visibleSetting = visible;
                SetVisible(visible);
            }
        }

        sealed class ToastVisibility
        {
            public void SetVisible(bool visible)
            {
                if (NoaDebugger.ToastInstance != null)
                {
                    NoaDebugger.ToastInstance.SetVisibility(visible);
                }
            }
        }
    }
}
