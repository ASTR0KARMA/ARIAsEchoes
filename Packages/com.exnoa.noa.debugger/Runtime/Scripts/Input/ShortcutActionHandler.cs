using System;
using UnityEngine;

namespace NoaDebugger
{
    sealed class ShortcutActionHandler : MonoBehaviour
    {
        public enum PressState
        {
            None,
            KeyDown,
            Pressing,
            PressingLoop,
        }

        NoaDebuggerSettings _settings;
        bool _isPressed;
        float _pressStartTime;
        float _lastHoldActionTime;
        PressState _pressState;
        bool _hasProcessedLongPress;

        public bool IsShortcutsEnabled { get; set; }

        void Awake()
        {
            Init();
        }

        void Update()
        {
            if (!IsShortcutsEnabled
                || _settings == null
                || _settings.EnabledShortcutActions == null
                || _settings.EnabledShortcutActions.Count == 0)
            {
                return;
            }

            foreach (var action in _settings.EnabledShortcutActions)
            {
                var (shortPress, longPress, holdDown) = _IsTrigger(action);

                if (shortPress || longPress || holdDown)
                {
                    _ExecuteAction(action.Type, shortPress, longPress, holdDown);
                    break;
                }
            }
        }

        (bool shortPress, bool longPress, bool holdDown) _IsTrigger(ShortcutAction action)
        {
            if (!action.combination.isEnabled)
            {
                return (false, false, false);
            }

            var currentKeyDown = UnityInputUtils.IsShortcutKeyDown(action);  
            var currentKeyHeld = UnityInputUtils.IsShortcutKeyHeld(action);  
            var currentKeyUp = UnityInputUtils.IsShortcutKeyUp(action);      

            var shortPress = false;
            var longPress = false;
            var holdDown = false;

            NoaDebuggerDefine.ShortCutTriggerType triggerType = NoaDebuggerDefine.GetTriggerType(action.Type);

            if (currentKeyDown)
            {
                _isPressed = true;
                _pressStartTime = Time.unscaledTime;
                _lastHoldActionTime = Time.unscaledTime;
                _pressState = PressState.KeyDown;
                _hasProcessedLongPress = false;
            }
            else if (_isPressed && currentKeyHeld)
            {
                if (!_hasProcessedLongPress &&
                    triggerType.HasFlag(NoaDebuggerDefine.ShortCutTriggerType.LongPress) &&
                    Time.unscaledTime - _pressStartTime >= NoaDebuggerDefine.PressTimeSeconds)
                {
                    longPress = true;
                    _hasProcessedLongPress = true;
                }

                if (triggerType.HasFlag(NoaDebuggerDefine.ShortCutTriggerType.HoldDown))
                {
                    var holdTime = Time.unscaledTime - _lastHoldActionTime;
                    switch (_pressState)
                    {
                        case PressState.KeyDown:
                            if (holdTime >= NoaDebuggerDefine.PressTimeSeconds)
                            {
                                _pressState = PressState.Pressing;
                                _lastHoldActionTime = Time.unscaledTime;
                                holdDown = true;
                            }

                            break;

                        case PressState.Pressing:
                            if (holdTime >= FrameSteppingModel.PressActionFirstInterval)
                            {
                                _pressState = PressState.PressingLoop;
                                _lastHoldActionTime = Time.unscaledTime;
                                holdDown = true;
                            }

                            break;
                        case PressState.PressingLoop:
                            if (holdTime >= FrameSteppingModel.PressActionSecondInterval)
                            {
                                _lastHoldActionTime = Time.unscaledTime;
                                holdDown = true;
                            }
                            break;
                    }
                }

            }
            else if (_isPressed && currentKeyUp)
            {
                if ((!_hasProcessedLongPress && _pressState != PressState.None) &&
                    triggerType.HasFlag(NoaDebuggerDefine.ShortCutTriggerType.ShortPress))
                {
                    shortPress = true;
                }

                _isPressed = false;
                _hasProcessedLongPress = false;
                _pressState = PressState.None;
                _pressStartTime = 0f;
            }

            return (shortPress, longPress, holdDown);
        }

        void _ExecuteAction(ShortcutActionType actionType, bool shortPress, bool longPress, bool holdDown)
        {
            switch (actionType)
            {
                case ShortcutActionType.ToggleDebugger:
                    if (NoaDebug.IsDebuggerVisible)
                    {
                        NoaDebug.Hide();
                    }
                    else
                    {
                        NoaDebug.Show();
                    }
                    break;

                case ShortcutActionType.ToggleOverlay:
                    NoaDebuggerVisibilityManager.SetOverlayVisible(!NoaDebuggerVisibilityManager.IsOverlayVisible);
                    break;

                case ShortcutActionType.ToggleFloatingWindow:
                    NoaDebuggerVisibilityManager.SetFloatingWindowVisible(!NoaDebuggerVisibilityManager.IsFloatingWindowVisible);
                    break;

                case ShortcutActionType.ToggleTriggerButton:
                    NoaDebuggerVisibilityManager.SetTriggerButtonVisible(!NoaDebuggerVisibilityManager.IsTriggerButtonActive);
                    break;

                case ShortcutActionType.ToggleUIElements:
                    NoaDebuggerVisibilityManager.SetAllUIElementsVisible(!NoaDebuggerVisibilityManager.IsAllUIElementActive);
                    break;

                case ShortcutActionType.ResetApplication:
                    NoaController.ResetApplication();
                    break;

                case ShortcutActionType.ToggleAllUI:
                    NoaDebuggerVisibilityManager.ToggleNoaDebuggerUI();
                    break;

                case ShortcutActionType.CaptureScreenshot:
                    NoaController.CaptureScreenshot();
                    break;

                case ShortcutActionType.TogglePauseResume:
                    NoaController.TogglePauseResume();
                    break;

                case ShortcutActionType.DecreaseGameSpeed:
                    if (shortPress)
                    {
                        NoaController.DecreaseGameSpeed();
                    }
                    else if (longPress)
                    {
                        NoaController.MinimizeGameSpeed();
                    }
                    break;

                case ShortcutActionType.IncreaseGameSpeed:
                    if (shortPress)
                    {
                        NoaController.IncreaseGameSpeed();
                    }
                    else if (longPress)
                    {
                        NoaController.MaximizeGameSpeed();
                    }
                    break;

                case ShortcutActionType.FrameStepping:
                    if (shortPress || holdDown)
                    {
                        NoaController.FrameStepping();
                    }
                    break;

                case ShortcutActionType.ResetGameSpeed:
                    NoaController.ResetGameSpeed();
                    break;

                case ShortcutActionType.CustomAction1:
                case ShortcutActionType.CustomAction2:
                case ShortcutActionType.CustomAction3:
                case ShortcutActionType.CustomAction4:
                case ShortcutActionType.CustomAction5:
                {
                    int startIndex = Array.IndexOf(NoaDebuggerDefine.SortedShortcutActionType, ShortcutActionType.CustomAction1);
                    int buttonIndex = Array.IndexOf(NoaDebuggerDefine.SortedShortcutActionType, actionType) - startIndex;
                    var customActionType = NoaController.GetCustomActionType(buttonIndex);

                    if (shortPress || longPress)
                    {
                        if (customActionType == NoaController.CustomActionType.Button)
                        {
                            if (shortPress)
                            {
                                NoaController.RunCustomTapAction(buttonIndex);
                            }
                            else
                            {
                                NoaController.RunCustomLongPressAction(buttonIndex);
                            }
                        }
                        else if (customActionType == NoaController.CustomActionType.ToggleButton)
                        {
                            NoaController.SetCustomToggle(buttonIndex, !NoaController.GetCustomToggle(buttonIndex));
                        }
                    }
                    break;
                }
            }
        }

        public void Init()
        {
            _settings = NoaDebuggerSettingsManager.GetNoaDebuggerSettings();
            UnityInputUtils.OnShortcutHandlerInitialize();
        }
    }
}
