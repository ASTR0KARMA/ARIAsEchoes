using System;
using UnityEngine;

namespace NoaDebugger
{
    sealed partial class NoaDebuggerVisibilityManager : MonoBehaviour
    {
        static NoaDebuggerVisibilityManager _instance;
        static Func<bool, bool> _onToggleNoaDebuggerUI;

        [SerializeField]
        MainViewVisibility _mainViewVisibility;

        [SerializeField]
        OverlayVisibility _overlayVisibility;
        bool _isActiveOverlay;

        [SerializeField]
        FloatingWindowVisibility _floatingWindowVisibility;
        bool _isActiveFloatingWindow;

        [SerializeField]
        TriggerButtonVisibility _triggerButtonVisibility;
        bool _isActiveTriggerButton;

        [SerializeField]
        ControllerVisibility _controllerVisibility;

        ToastVisibility _toastVisibility = new ToastVisibility();

        AllUIElementVisibility _allUIElementVisibility = new AllUIElementVisibility();

        bool _isToggleAllHide;
        bool _isMainViewVisibleBeforeHidden = true; 

        void Awake()
        {
            if(_instance != null)
            {
                Destroy(this);
                return;
            }

            _instance = this;
        }


        public static Func<bool, bool> OnToggleNoaDebuggerUI
        {
            get => _onToggleNoaDebuggerUI;
            set => _onToggleNoaDebuggerUI = value;
        }

        public static bool IsMainViewActive => _instance != null && _instance._mainViewVisibility.IsActive;

        public static bool IsOverlayRootActive => _instance != null && _instance._overlayVisibility.IsRootActive;

        public static bool IsOverlayVisible => _instance != null && _instance._overlayVisibility.IsVisibility;

        public static bool IsFloatingWindowVisible => _instance != null && _instance._floatingWindowVisibility.IsVisibility;

        public static bool IsTriggerButtonActive => _instance != null && _instance._triggerButtonVisibility.IsActive;

        public static bool IsControllerActive => _instance != null && _instance._controllerVisibility.IsActive;

        public static bool IsAllUIElementActive => _instance != null && _instance._allUIElementVisibility.IsActive;

        public static void ToggleNoaDebuggerUI() => _instance?._ToggleNoaDebuggerUI();

        public static void SetMainViewVisible(bool visible) => _instance?._mainViewVisibility.SetVisible(visible);

        public static void AddOverlaySetActiveAction(Action<bool> onSetActive)
        {
            if (_instance == null)
            {
                return;
            }

            _instance._overlayVisibility.OnSetActiveOverlay += onSetActive;
        }

        public static void RemoveOverlaySetActiveAction(Action<bool> onSetActive)
        {
            if (_instance == null)
            {
                return;
            }

            _instance._overlayVisibility.OnSetActiveOverlay -= onSetActive;
        }

        public static void SetOverlayVisible(bool visible) => _instance?._overlayVisibility.SetVisible(visible);

        public static void SetOverlayVisibleSetting(bool visible) => _instance?._overlayVisibility.SetVisibleSetting(visible);

        public static void SetFloatingWindowVisible(bool visible) => _instance?._floatingWindowVisibility.SetVisible(visible);

        public static void SetFloatingWindowVisibleSetting(bool visible) => _instance?._floatingWindowVisibility.SetVisibleSetting(visible);

        public static void AddTriggerButtonSetActiveAction(Action<bool> onSetActive)
        {
            if (_instance == null)
            {
                return;
            }

            _instance._triggerButtonVisibility.OnSetActiveTriggerButton += onSetActive;
        }

        public static void RemoveTriggerButtonSetActiveAction(Action<bool> onSetActive)
        {
            if (_instance == null)
            {
                return;
            }

            _instance._triggerButtonVisibility.OnSetActiveTriggerButton -= onSetActive;
        }

        public static void SetTriggerButtonVisible(bool visible) => _instance?._triggerButtonVisibility.SetVisible(visible);

        public static void SetTriggerButtonVisibleSetting(bool visible) => _instance?._SetTriggerButtonVisibleSetting(visible);

        public static void OnErrorTriggerButtonVisibility() => _instance?._triggerButtonVisibility.OnErrorVisible();

        public static void SetControllerVisible(bool visible) => NoaDebuggerVisibilityManager._instance?._SetControllerVisible(visible);

        public static void SetAllUIElementsVisible(bool visible) => _instance?._allUIElementVisibility.SetVisible(visible);

        public static void SetAllUIElementsVisibleSetting(bool visible) => _instance?._allUIElementVisibility.SetVisibleSetting(visible);

        public static void SetToastVisible(bool visible) => _instance?._toastVisibility.SetVisible(visible);

        public static void OnInitNoaDebuggerVisibility() => _instance?._OnInitNoaDebuggerVisibility();

        public static void OnShowMainViewVisibility() => _instance?._OnShowMainViewVisibility();

        public static void OnHideMainViewVisibility() => _instance?._OnHideMainViewVisibility();

        public static void OnDisableNoaDebugger() => _instance?._OnDisableNoaDebugger();

        public static void OnVisibleButtonVisibility() => _instance?._OnVisibleButtonVisibility();

        public static void OnEnableWorldSpaceVisibility() => _instance?._OnEnableWorldSpaceVisibility();

        public static void OnDisableWorldSpaceVisibility() => _instance?._OnDisableWorldSpaceVisibility();

        public static void Dispose()
        {
            _instance?._Dispose();
            _instance = null;
            _onToggleNoaDebuggerUI = null;
        }


        void _ToggleNoaDebuggerUI()
        {
            bool nextIsVisible = !_controllerVisibility.IsActive
                          && !_mainViewVisibility.IsActive
                          && !_triggerButtonVisibility.IsActive
                          && !_overlayVisibility.IsVisibility
                          && !_floatingWindowVisibility.IsVisibility
                          && !_allUIElementVisibility.IsActive;

            INoaToggleUICallbacks userCallbacks = NoaController.ToggleUICallbacks;
            userCallbacks?.OnBeforeToggleUI(nextIsVisible); 
            bool isAllow = _IsAllowToggleUI(userCallbacks, nextIsVisible);

            if (isAllow)
            {
                if (nextIsVisible)
                {
                    _isToggleAllHide = false;

                    if (_isMainViewVisibleBeforeHidden)
                    {
                        NoaDebuggerManager.ShowDebugger(); 
                    }
                    else
                    {
                        _triggerButtonVisibility.SetVisible(true);
                        _overlayVisibility.SetVisible(true);
                        _floatingWindowVisibility.SetVisible(true);
                        _allUIElementVisibility.SetVisible(true);
                    }

                    _allUIElementVisibility.SetVisible(true);
                }
                else
                {
                    _isToggleAllHide = true;
                    _isMainViewVisibleBeforeHidden = _mainViewVisibility.IsActive;

                    NoaDebuggerManager.HideDebugger(); 
                    _controllerVisibility.SetVisible(false);
                    _triggerButtonVisibility.SetVisible(false);
                    _overlayVisibility.SetVisible(false);
                    _floatingWindowVisibility.SetVisible(false);
                    _allUIElementVisibility.SetVisible(false);

                    NoaDebugger.ShowToast(new ToastViewLinker {_label = NoaDebuggerDefine.HideNoaDebuggerUIText});
                }
            }

            bool currentVisibility = _controllerVisibility.IsActive
                                     || _mainViewVisibility.IsActive
                                     || _triggerButtonVisibility.IsActive
                                     || _overlayVisibility.IsVisibility
                                     || _floatingWindowVisibility.IsVisibility
                                     || _allUIElementVisibility.IsActive;
            userCallbacks?.OnAfterToggleUI(currentVisibility);
        }

        bool _IsAllowToggleUI(INoaToggleUICallbacks userCallbacks, bool nextIsVisible)
        {
            if (userCallbacks != null)
            {
                return userCallbacks.IsAllowBaseToggleUI;
            }

            if (_onToggleNoaDebuggerUI != null)
            {
                return _onToggleNoaDebuggerUI.Invoke(nextIsVisible);
            }

            return true;
        }

        void _SetTriggerButtonVisibleSetting(bool visible)
        {
            _triggerButtonVisibility.SetVisibleSetting(visible);
        }

        void _SetControllerVisible(bool visible)
        {
            if (visible)
            {
                _controllerVisibility.SetVisible(true);
                _triggerButtonVisibility.InvalidateTriggerButton();
            }
            else
            {
                _controllerVisibility.SetVisible(false);
                _triggerButtonVisibility.SetVisible(true);
            }
        }

        void _OnInitNoaDebuggerVisibility()
        {
            _mainViewVisibility.SetVisible(false);
        }

        void _OnShowMainViewVisibility()
        {
            _mainViewVisibility.SetVisible(true);
            _overlayVisibility.SetVisible(false);
            _floatingWindowVisibility.SetVisible(false);
            _triggerButtonVisibility.SetVisible(false);
        }

        void _OnHideMainViewVisibility()
        {
            _mainViewVisibility.SetVisible(false);
            _overlayVisibility.SetVisible(true);
            _floatingWindowVisibility.SetVisible(true);
            _triggerButtonVisibility.SetVisible(true);
        }

        void _OnDisableNoaDebugger()
        {
            _mainViewVisibility.SetVisible(false);
            _overlayVisibility.SetVisible(false);
            _floatingWindowVisibility.SetVisible(false);
        }

        void _OnVisibleButtonVisibility()
        {
            if (_isToggleAllHide)
            {
                _overlayVisibility.SetVisible(true);
                _floatingWindowVisibility.SetVisible(true);
                _allUIElementVisibility.SetVisible(true);
                _isToggleAllHide = false;
            }

            _triggerButtonVisibility.SetVisible(true);
        }

        void _OnEnableWorldSpaceVisibility()
        {
            _mainViewVisibility.OnEnableWorldSpaceVisibility();

            _isActiveOverlay = _overlayVisibility.IsVisibility;
            _overlayVisibility.SetVisible(false);
            _isActiveFloatingWindow = _floatingWindowVisibility.IsVisibility;
            _floatingWindowVisibility.SetVisible(false);

            _isActiveTriggerButton = _triggerButtonVisibility.IsActive;
            _triggerButtonVisibility.SetVisible(false);
        }

        void _OnDisableWorldSpaceVisibility()
        {
            _mainViewVisibility.OnDisableWorldSpaceVisibility();

            if (_mainViewVisibility.IsActive == false)
            {
                _overlayVisibility.SetVisible(_isActiveOverlay);
                _floatingWindowVisibility.SetVisible(_isActiveFloatingWindow);
                _triggerButtonVisibility.SetVisible(_isActiveTriggerButton);
            }
        }

        void _Dispose()
        {
            _mainViewVisibility = default;
            _overlayVisibility = default;
            _floatingWindowVisibility = default;
            _triggerButtonVisibility = default;
            _controllerVisibility = default;
            _toastVisibility = default;
            _allUIElementVisibility = default;
        }
    }
}
