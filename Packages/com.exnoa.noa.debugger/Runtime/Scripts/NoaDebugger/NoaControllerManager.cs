using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoaDebugger
{
    sealed class NoaControllerManager
    {
        struct CustomActionSettings
        {
            public Action _onTap;
            public Func<string> _onTapMessageFunc;
            public Action _onLongPress;
            public Func<string> _onLongPressMessageFunc;
            public Action<bool> _onToggle;
            public Func<bool, string> _onToggleMessageFunc;
            public NoaController.CustomActionType _actionType;
        }

        static NoaControllerManager _instance;
        readonly NoaControllerView _controllerView;
        readonly ApplicationOperateModel _applicationOperateModel;
        Action _onShow;
        Action _onHide;
        readonly CustomActionSettings[] _customActionSettings;
        Action<bool> _onTogglePauseResume;
        Action<float> _onGameSpeedChanged;
        Func<bool> _onApplicationReset;
        Func<bool, bool> _onToggleNoaDebuggerUI;
        Func<NoaController.ScreenshotTarget> _onBeforeScreenshot;
        Func<bool> _onCaptureScreenshot;
        Action _onAfterScreenshot;
        Action _onFrameStepping;
        byte[] _capturedScreenshot;
        NoaDebuggerButtonEffectManager _buttonEffectManager;

        public static Action OnShow
        {
            get => NoaControllerManager._instance._onShow;
            set => NoaControllerManager._instance._onShow = value;
        }

        public static Action OnHide
        {
            get => NoaControllerManager._instance._onHide;
            set => NoaControllerManager._instance._onHide = value;
        }

        public static Action<bool> OnTogglePauseResume
        {
            get => NoaControllerManager._instance._onTogglePauseResume;
            set => NoaControllerManager._instance._onTogglePauseResume = value;
        }

        public static Action<float> OnGameSpeedChanged
        {
            get => NoaControllerManager._instance._onGameSpeedChanged;
            set => NoaControllerManager._instance._onGameSpeedChanged = value;
        }

        public static Func<bool> OnApplicationReset
        {
            get => NoaControllerManager._instance._onApplicationReset;
            set => NoaControllerManager._instance._onApplicationReset = value;
        }

        public static Func<NoaController.ScreenshotTarget> OnBeforeScreenshot
        {
            get => NoaControllerManager._instance._onBeforeScreenshot;
            set => NoaControllerManager._instance._onBeforeScreenshot = value;
        }

        public static Func<bool> OnCaptureScreenshot
        {
            get => NoaControllerManager._instance._onCaptureScreenshot;
            set => NoaControllerManager._instance._onCaptureScreenshot = value;
        }

        public static Action OnAfterScreenshot
        {
            get => NoaControllerManager._instance._onAfterScreenshot;
            set => NoaControllerManager._instance._onAfterScreenshot = value;
        }

        public static Action OnFrameStepping
        {
            get => NoaControllerManager._instance._onFrameStepping;
            set => NoaControllerManager._instance._onFrameStepping = value;
        }

        public static bool IsGamePlaying => GameSpeedModel.IsGamePlaying;

        public static float GameSpeed => GameSpeedModel.GameSpeed;

        public static void Initialize(NoaControllerView controllerView, NoaDebuggerButtonEffectManager buttonEffectManager)
        {
            NoaControllerManager._instance = new NoaControllerManager(controllerView, buttonEffectManager);
        }

        public static void SetCustomTapAction(int buttonIndex, Action action, Func<string> messageFunc = null)
        {
            NoaControllerManager._instance._SetCustomTapAction(buttonIndex, action, messageFunc);
        }

        public static void SetCustomLongPressAction(int buttonIndex, Action action, Func<string> messageFunc = null)
        {
            NoaControllerManager._instance._SetCustomLongPressAction(buttonIndex, action, messageFunc);
        }

        public static void SetCustomToggleAction(int buttonIndex, Action<bool> action,
                                                 Func<bool, string> messageFunc = null,
                                                 bool? initialState = null)
        {
            NoaControllerManager._instance._SetCustomToggleAction(buttonIndex, action, _ => null);
            if(initialState.HasValue)
            {
                NoaControllerManager._instance._SetCustomToggle(buttonIndex, initialState.Value);
            }
            NoaControllerManager._instance._SetCustomToggleMessage(buttonIndex, messageFunc);
        }

        public static void SetCustomActionType(int buttonIndex, NoaController.CustomActionType actionType)
        {
            NoaControllerManager._instance._SetCustomActionType(buttonIndex, actionType);
        }

        public static NoaController.CustomActionType GetCustomActionType(int buttonIndex)
        {
            return NoaControllerManager._instance._GetCustomActionType(buttonIndex);
        }

        public static void RunCustomTapAction(int buttonIndex)
        {
            NoaControllerManager._instance._RunCustomTapAction(buttonIndex);
        }

        public static void RunCustomLongPressAction(int buttonIndex)
        {
            NoaControllerManager._instance._RunCustomLongPressAction(buttonIndex);
        }

        public static void SetCustomToggle(int buttonIndex, bool isOn)
        {
            NoaControllerManager._instance._SetCustomToggle(buttonIndex, isOn);
        }

        public static bool GetCustomToggle(int buttonIndex)
        {
            return NoaControllerManager._instance._GetCustomToggle(buttonIndex);
        }

        public static void TogglePauseResume()
        {
            NoaControllerManager._instance._TogglePauseResume(!IsGamePlaying);
        }

        public static void IncreaseGameSpeed()
        {
            NoaControllerManager._instance._IncreaseGameSpeed();
        }

        public static void DecreaseGameSpeed()
        {
            NoaControllerManager._instance._DecreaseGameSpeed();
        }

        public static void MinimizeGameSpeed()
        {
            NoaControllerManager._instance._MinimizeGameSpeed();
        }

        public static void MaximizeGameSpeed()
        {
            NoaControllerManager._instance._MaximizeGameSpeed();
        }

        public static void ResetGameSpeed()
        {
            NoaControllerManager._instance._ResetGameSpeed();
        }

        public static void FrameStepping()
        {
            NoaControllerManager._instance._FrameStepping();
        }

        public static void ResetApplication()
        {
            NoaControllerManager._instance._ResetApplication();
        }

        public static void CaptureScreenshot()
        {
            NoaControllerManager._instance._CaptureScreenshot();
        }

        public static byte[] GetCapturedScreenshot()
        {
            return NoaControllerManager._instance._capturedScreenshot;
        }

        public static void ClearCapturedScreenshot()
        {
            NoaControllerManager._instance._capturedScreenshot = null;
        }

        [Obsolete("Use NoaControllerManager.CaptureScreenshot() instead.")]
        public static void TakeScreenshot(Action<byte[]> callback)
        {
            GlobalCoroutine.Run(NoaControllerManager._instance.CaptureScreenshotCoroutine(callback));
        }

        NoaControllerManager(NoaControllerView controllerView, NoaDebuggerButtonEffectManager buttonEffectManager)
        {
            _controllerView = controllerView;
            _controllerView.OnShow += _OnShow;
            _controllerView.OnHide += _OnHide;
            _controllerView.OnTogglePauseResume += _TogglePauseResume;
            _controllerView.OnGameSpeedDecrement += _DecreaseGameSpeed;
            _controllerView.OnGameSpeedMinimize += _MinimizeGameSpeed;
            _controllerView.OnGameSpeedIncrement += _IncreaseGameSpeed;
            _controllerView.OnGameSpeedMaximize += _MaximizeGameSpeed;
            _controllerView.OnFrameStepping += _FrameStepping;
            _controllerView.OnClose += _Close;
            _controllerView.OnGameSpeedReset += _ResetGameSpeed;
            _controllerView.OnResetApplication += _ResetApplication;
            _controllerView.OnHideNoaDebuggerUI += NoaDebuggerVisibilityManager.ToggleNoaDebuggerUI;
            _controllerView.OnCaptureScreenshot += _CaptureScreenshot;
            _controllerView.OnShowNoaDebugger += _ShowNoaDebugger;

            _controllerView.OnFrameSteppingButtonEnabled += () =>
            {
                var setting = new LongPressButtonActionIntervalSettings()
                {
                    _isNeedSpeedUp = false,
                    _secondInterval = FrameSteppingModel.PressActionSecondInterval,
                    _firstInterval = FrameSteppingModel.PressActionFirstInterval
                };

                _controllerView.SetLongPressButtonActionIntervalSettings(setting);
            };
            GameSpeedModel.Initialize();

            _buttonEffectManager = buttonEffectManager;

            _applicationOperateModel = new ApplicationOperateModel
            {
                OnFinishTransition = _OnFinishTransitionToInitialScene
            };

            _customActionSettings = new CustomActionSettings[NoaController.CustomActionButtonCount];

            SceneManager.sceneLoaded -= _OnLoadScene;
            SceneManager.sceneLoaded += _OnLoadScene;

            SettingsEventModel.OnUpdateGameSpeedSettings -= _OnUpdateGameSpeedSettings;
            SettingsEventModel.OnUpdateGameSpeedSettings += _OnUpdateGameSpeedSettings;
            SettingsEventModel.OnUpdateDisplaySettings -= _OnUpdateDisplaySettings;
            SettingsEventModel.OnUpdateDisplaySettings += _OnUpdateDisplaySettings;
        }

        void _OnLoadScene(Scene _, LoadSceneMode __)
        {
            bool isProcessFrameStepping = _controllerView.IsProcessFrameStepping;

            _controllerView.ReSetFrameSteppingButtonState();

            if (isProcessFrameStepping)
            {
                NoaControllerManager.TogglePauseResume();
            }
        }

        void _OnShow()
        {
            for (int i = 0; i < NoaController.CustomActionButtonCount; ++i)
            {
                _controllerView.SetCustomTapAction(i, _customActionSettings[i]._onTap);
                _controllerView.SetCustomLongPressAction(i, _customActionSettings[i]._onLongPress);
                _controllerView.SetCustomToggleAction(i, _customActionSettings[i]._onToggle);
                _controllerView.SetCustomActionType(i, _customActionSettings[i]._actionType);
            }

            _controllerView.TogglePauseResume(GameSpeedModel.IsGamePlaying);

            _controllerView.UpdateGameSpeed(
                GameSpeedModel.GameSpeed, GameSpeedModel.MinGameSpeed, GameSpeedModel.MaxGameSpeed);

            _buttonEffectManager.ResetButtonColor();

            _onShow?.Invoke();
        }

        void _OnHide()
        {
            _onHide?.Invoke();
        }

        void _SetCustomTapAction(int buttonIndex, Action action, Func<string> messageFunc = null)
        {
            if (!NoaControllerManager._IsButtonIndexInRange(buttonIndex))
            {
                return;
            }

            if (action != null)
            {
                action += () => _NotifyCustomActionTapped(buttonIndex);
            }

            _customActionSettings[buttonIndex]._onTap = action;
            _customActionSettings[buttonIndex]._onTapMessageFunc = messageFunc;
            _controllerView.SetCustomTapAction(buttonIndex, action);
        }

        void _NotifyCustomActionTapped(int buttonIndex)
        {
            string message = _customActionSettings[buttonIndex]._onTapMessageFunc != null
                ? _customActionSettings[buttonIndex]._onTapMessageFunc.Invoke()
                : string.Format(NoaDebuggerDefine.TapCustomActionText, buttonIndex + 1);

            if (!string.IsNullOrEmpty(message))
            {
                NoaDebugger.ShowToast(new ToastViewLinker {_label = message});
            }
        }

        void _SetCustomLongPressAction(int buttonIndex, Action action, Func<string> messageFunc = null)
        {
            if (!NoaControllerManager._IsButtonIndexInRange(buttonIndex))
            {
                return;
            }

            if (action != null)
            {
                action += () => _NotifyCustomActionLongPressed(buttonIndex);
            }

            _customActionSettings[buttonIndex]._onLongPress = action;
            _customActionSettings[buttonIndex]._onLongPressMessageFunc = messageFunc;
            _controllerView.SetCustomLongPressAction(buttonIndex, action);
        }

        void _NotifyCustomActionLongPressed(int buttonIndex)
        {
            string message = _customActionSettings[buttonIndex]._onLongPressMessageFunc != null
                ? _customActionSettings[buttonIndex]._onLongPressMessageFunc.Invoke()
                : string.Format(NoaDebuggerDefine.LongPressCustomActionText, buttonIndex + 1);

            if (!string.IsNullOrEmpty(message))
            {
                NoaDebugger.ShowToast(new ToastViewLinker {_label = message});
            }
        }

        void _SetCustomToggleAction(int buttonIndex, Action<bool> action, Func<bool, string> messageFunc = null)
        {
            if (!NoaControllerManager._IsButtonIndexInRange(buttonIndex))
            {
                return;
            }

            Action<bool> decoratedAction = isOn =>
            {
                action?.Invoke(isOn);
                _NotifyCustomActionToggled(buttonIndex, isOn);
            };

            _customActionSettings[buttonIndex]._onToggle = decoratedAction;
            _customActionSettings[buttonIndex]._onToggleMessageFunc = messageFunc;
            _controllerView.SetCustomToggleAction(buttonIndex, decoratedAction);
        }


        void _SetCustomToggleMessage(int buttonIndex, Func<bool, string> messageFunc = null)
        {
            if (!NoaControllerManager._IsButtonIndexInRange(buttonIndex))
            {
                return;
            }

            _customActionSettings[buttonIndex]._onToggleMessageFunc = messageFunc;
        }

        void _NotifyCustomActionToggled(int buttonIndex, bool isOn)
        {
            string message;

            if (_customActionSettings[buttonIndex]._onToggleMessageFunc != null)
            {
                message = _customActionSettings[buttonIndex]._onToggleMessageFunc.Invoke(isOn);
            }
            else
            {
                message = isOn
                    ? string.Format(NoaDebuggerDefine.ToggledOnCustomActionText, buttonIndex + 1)
                    : string.Format(NoaDebuggerDefine.ToggledOffCustomActionText, buttonIndex + 1);
            }

            if (!string.IsNullOrEmpty(message))
            {
                NoaDebugger.ShowToast(new ToastViewLinker {_label = message});
            }
        }

        void _SetCustomActionType(int buttonIndex, NoaController.CustomActionType actionType)
        {
            if (!NoaControllerManager._IsButtonIndexInRange(buttonIndex))
            {
                return;
            }

            _customActionSettings[buttonIndex]._actionType = actionType;
            _controllerView.SetCustomActionType(buttonIndex, actionType);
        }

        NoaController.CustomActionType _GetCustomActionType(int buttonIndex)
        {
            if (!NoaControllerManager._IsButtonIndexInRange(buttonIndex))
            {
                return NoaController.CustomActionType.Default;
            }

            return _customActionSettings[buttonIndex]._actionType;
        }

        void _RunCustomTapAction(int buttonIndex)
        {
            if (!NoaControllerManager._IsButtonIndexInRange(buttonIndex))
            {
                return;
            }

            _customActionSettings[buttonIndex]._onTap?.Invoke();
        }

        void _RunCustomLongPressAction(int buttonIndex)
        {
            if (!NoaControllerManager._IsButtonIndexInRange(buttonIndex))
            {
                return;
            }

            _customActionSettings[buttonIndex]._onLongPress?.Invoke();
        }

        void _SetCustomToggle(int buttonIndex, bool isOn)
        {
            if (!NoaControllerManager._IsButtonIndexInRange(buttonIndex)
                || _customActionSettings[buttonIndex]._onToggle == null)
            {
                return;
            }

            _controllerView.SetCustomToggle(buttonIndex, isOn);
            _customActionSettings[buttonIndex]._onToggle?.Invoke(isOn);
        }

        bool _GetCustomToggle(int buttonIndex)
        {
            if (!NoaControllerManager._IsButtonIndexInRange(buttonIndex)
                || _customActionSettings[buttonIndex]._onToggle == null)
            {
                return false;
            }

            return _controllerView.GetCustomToggle(buttonIndex);
        }

        static bool _IsButtonIndexInRange(int buttonIndex)
        {
            return (0 <= buttonIndex) && (buttonIndex < NoaController.CustomActionButtonCount);
        }

        void _TogglePauseResume(bool isResume)
        {
            if (isResume)
            {
                GameSpeedModel.Resume();
            }
            else
            {
                GameSpeedModel.Pause();
            }

            _controllerView.TogglePauseResume(GameSpeedModel.IsGamePlaying);
            _onTogglePauseResume?.Invoke(isResume);
        }

        void _IncreaseGameSpeed()
        {
            GameSpeedModel.Increment();
            _UpdateGameSpeed();
        }

        void _MaximizeGameSpeed()
        {
            GameSpeedModel.Maximize();
            _UpdateGameSpeed();
        }

        void _DecreaseGameSpeed()
        {
            GameSpeedModel.Decrement();
            _UpdateGameSpeed();
        }

        void _MinimizeGameSpeed()
        {
            GameSpeedModel.Minimize();
            _UpdateGameSpeed();
        }

        void _ResetGameSpeed()
        {
            GameSpeedModel.Reset();
            _UpdateGameSpeed();
        }

        void _UpdateGameSpeed()
        {
            if (NoaDebuggerVisibilityManager.IsControllerActive)
            {
                _controllerView.UpdateGameSpeed(
                    GameSpeedModel.GameSpeed, GameSpeedModel.MinGameSpeed, GameSpeedModel.MaxGameSpeed);
            }

            _onGameSpeedChanged?.Invoke(GameSpeedModel.GameSpeed);
        }

        void _FrameStepping()
        {
            GlobalCoroutine.Run(FrameSteppingModel.FrameStepping(_onFrameStepping));
        }

        void _Close()
        {
            NoaDebuggerVisibilityManager.SetControllerVisible(false);
        }

        void _ResetApplication()
        {
            INoaApplicationResetCallbacks userCallbacks = NoaController.ApplicationResetCallbacks;
            userCallbacks?.OnBeforeApplicationReset(); 
            bool isAllow = _IsAllowApplicationReset(userCallbacks);

            if (isAllow)
            {
                _controllerView.SetResetAppButtonInteractable(false);
                _applicationOperateModel.TransitionToInitialScene();
            }

            userCallbacks?.OnAfterApplicationReset();
        }

        bool _IsAllowApplicationReset(INoaApplicationResetCallbacks userCallbacks)
        {
            if (userCallbacks != null)
            {
                return userCallbacks.IsAllowBaseApplicationReset;
            }

            if (_onApplicationReset != null)
            {
                return _onApplicationReset.Invoke();
            }

            return true;
        }

        void _OnFinishTransitionToInitialScene()
        {
            GlobalCoroutine.Run(_OnFinishTransitionToInitialSceneRoutine());
        }

        IEnumerator _OnFinishTransitionToInitialSceneRoutine()
        {
            NoaDebugger.ShowToast(new ToastViewLinker {_label = NoaDebuggerDefine.TransitionToInitialSceneText});

            yield return null;

            _controllerView.SetResetAppButtonInteractable(true);
        }

        void _CaptureScreenshot()
        {
            GlobalCoroutine.Run(CaptureScreenshotCoroutine());
        }

        void _ShowNoaDebugger()
        {
            NoaDebuggerVisibilityManager.SetControllerVisible(false);
            NoaDebuggerVisibilityManager.SetTriggerButtonVisible(false);

            NoaDebugger.ShowNoaDebuggerLastActiveTool();
        }

        void _OnUpdateGameSpeedSettings()
        {
            GameSpeedModel.Initialize();
        }

        void _OnUpdateDisplaySettings()
        {
            _controllerView.ApplyNoaDebuggerSettings();
        }

        IEnumerator CaptureScreenshotCoroutine(Action<byte[]> callback = null)
        {
            INoaScreenshotCallbacks callbacks = NoaController.ScreenshotCallbacks;


            NoaController.ScreenshotTarget screenshotTarget = _GetScreenshotTarget(callbacks);
            callbacks?.OnBeforePrepareScreenshot();

            NoaDebuggerVisibilityManager.ScreenshotUIVisibilityManager screenshotUIVisibilityManager =
                new NoaDebuggerVisibilityManager.ScreenshotUIVisibilityManager(screenshotTarget);
            screenshotUIVisibilityManager.Before();

            yield return new WaitForEndOfFrame();

            _capturedScreenshot = null;
            bool isAllowCapture = _IsAllowCaptureScreenshot(callbacks);
            callbacks?.OnCaptureScreenshot();

            if (isAllowCapture)
            {
                _capturedScreenshot = NoaControllerManager.CaptureScreenshotInternal();
            }

            screenshotUIVisibilityManager.After();

            _ExecAfterScreenshotCallback(callbacks, callback);

            if (_capturedScreenshot != null)
            {
                NoaDebugger.ShowToast(new ToastViewLinker {_label = NoaDebuggerDefine.CaptureScreenshotText});
            }
        }

        NoaController.ScreenshotTarget _GetScreenshotTarget(INoaScreenshotCallbacks callbacks)
        {
            if (callbacks != null)
            {
                return callbacks.ScreenshotTarget;
            }

            if (_onBeforeScreenshot != null)
            {
                return _onBeforeScreenshot.Invoke();
            }

            return NoaController.ScreenshotTarget.None;
        }

        bool _IsAllowCaptureScreenshot(INoaScreenshotCallbacks callbacks)
        {
            if (callbacks != null)
            {
                return callbacks.IsAllowBaseScreenshot;
            }

            if (_onCaptureScreenshot != null)
            {
                return _onCaptureScreenshot.Invoke();
            }

            return true;
        }

        void _ExecAfterScreenshotCallback(INoaScreenshotCallbacks callbacks, Action<byte[]> obsoleteCallback)
        {
            if (obsoleteCallback != null)
            {
                obsoleteCallback.Invoke(_capturedScreenshot);
                NoaControllerManager.ClearCapturedScreenshot();
            }
            else
            {
                if (callbacks != null)
                {
                    callbacks.OnAfterScreenshot();
                }
                else if (_onAfterScreenshot != null)
                {
                    _onAfterScreenshot?.Invoke();
                }
            }
        }

        static byte[] CaptureScreenshotInternal()
        {
            var screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenshot.Apply();
            byte[] data = screenshot.EncodeToPNG();
            GameObject.Destroy(screenshot);

            return data;
        }
    }
}
