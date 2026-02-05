using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NoaDebugger
{
    public class NoaControllerView : MonoBehaviour
    {
        public event Action OnShow;

        public event Action OnHide;

        public event Action<bool> OnTogglePauseResume;

        public event Action OnGameSpeedDecrement;

        public event Action OnGameSpeedMinimize;

        public event Action OnGameSpeedIncrement;

        public event Action OnGameSpeedMaximize;

        public event Action OnGameSpeedReset;

        public event Action OnFrameSteppingButtonEnabled;

        public event Action OnFrameStepping;

        public event Action OnResetApplication;

        public event Action OnHideNoaDebuggerUI;

        public event Action OnCaptureScreenshot;

        public event Action OnShowNoaDebugger;

        public event Action OnClose;

        [Header("Transform References")]
        [SerializeField]
        RectTransform _controllerViewTransform;

        [SerializeField]
        RectTransform _noaDebuggerButtonTransform;

        [Header("Controller Elements")]
        [SerializeField]
        ControllerCustomActionButton[] _customActionButtons;

        [SerializeField]
        ToggleButtonBase _pauseResumeButton;

        [SerializeField]
        LongTapButton _gameSpeedDecrementButton;

        [SerializeField]
        DisableButtonBase _gameSpeedDecrementDisableButton;

        [SerializeField]
        LongTapButton _gameSpeedIncrementButton;

        [SerializeField]
        DisableButtonBase _gameSpeedIncrementDisableButton;

        [SerializeField]
        LongTapButton _gameSpeedResetButton;

        [SerializeField]
        TextMeshProUGUI _gameSpeedText;

        [SerializeField]
        LongPressToggleColorButton _frameSteppingButton;

        [SerializeField]
        Button _closeButton;

        [SerializeField]
        LongTapButton _resetAppButton;

        [SerializeField]
        Button _hideNoaDebuggerUIButton;

        [SerializeField]
        Button _screenshotButton;

        [SerializeField]
        Button _bootButton;

        [Header("Orders")]
        [SerializeField]
        RectTransform[] _controllerOrderForLowerLeft;

        [SerializeField]
        RectTransform[] _controllerOrderForUpperLeft;

        [SerializeField]
        RectTransform[] _controllerOrderForUpperRight;

        [SerializeField]
        RectTransform[] _controllerOrderForLowerRight;

        [Header("Opacity")]
        [SerializeField, ]
        Image[] _opacityTargetImages;

        public bool IsProcessFrameStepping => _frameSteppingButton.IsLongPress();

        void Awake()
        {
            _ValidateFields();
            _ApplyNoaDebuggerSettings();
            _InitializeCallbacks();
        }

        void OnEnable()
        {
            _ReorderButtons();
            OnShow?.Invoke();
        }

        void OnDisable()
        {
            OnHide?.Invoke();
        }

        public void SetCustomTapAction(int buttonIndex, Action action)
        {
            _customActionButtons[buttonIndex].OnTap = action;
        }

        public void SetCustomLongPressAction(int buttonIndex, Action action)
        {
            _customActionButtons[buttonIndex].OnLongTap = action;
        }

        public void SetCustomToggleAction(int buttonIndex, Action<bool> action)
        {
            _customActionButtons[buttonIndex].OnToggle = action;
        }

        public void SetCustomToggle(int buttonIndex, bool isOn)
        {
            _customActionButtons[buttonIndex].IsToggleOn = isOn;
        }

        public bool GetCustomToggle(int buttonIndex)
        {
            return _customActionButtons[buttonIndex].IsToggleOn;
        }

        public void SetCustomActionType(int buttonIndex, NoaController.CustomActionType actionType)
        {
            _customActionButtons[buttonIndex].SetCustomActionType(actionType);
        }

        public void TogglePauseResume(bool isPlaying)
        {
            _pauseResumeButton.Init(isPlaying);
        }

        public void UpdateGameSpeed(float gameSpeed, float minGameSpeed, float maxGameSpeed)
        {
            _gameSpeedText.text = $"{gameSpeed:N1}x";
            _gameSpeedDecrementDisableButton.Interactable = gameSpeed > minGameSpeed;
            _gameSpeedIncrementDisableButton.Interactable = gameSpeed < maxGameSpeed;
        }

        public void SetResetAppButtonInteractable(bool interactable)
        {
            _resetAppButton.interactable = interactable;
        }

        public void SetLongPressButtonActionIntervalSettings(LongPressButtonActionIntervalSettings setting)
        {
            _frameSteppingButton.SetActionIntervalSettings(setting);
        }

        public void ReSetFrameSteppingButtonState()
        {
            _frameSteppingButton.ResetButtonState();
        }

        public void ApplyNoaDebuggerSettings()
        {
            _ApplyNoaDebuggerSettings();
        }

        void _ValidateFields()
        {
            Assert.IsNotNull(_controllerViewTransform);
            Assert.IsNotNull(_noaDebuggerButtonTransform);
            Assert.IsNotNull(_customActionButtons);
            Assert.AreEqual(NoaController.CustomActionButtonCount, _customActionButtons.Length);
            Assert.IsNotNull(_pauseResumeButton);
            Assert.IsNotNull(_gameSpeedDecrementButton);
            Assert.IsNotNull(_gameSpeedDecrementDisableButton);
            Assert.IsNotNull(_gameSpeedIncrementButton);
            Assert.IsNotNull(_gameSpeedIncrementDisableButton);
            Assert.IsNotNull(_frameSteppingButton);
            Assert.IsNotNull(_gameSpeedResetButton);
            Assert.IsNotNull(_gameSpeedText);
            Assert.IsNotNull(_closeButton);
            Assert.IsNotNull(_resetAppButton);
            Assert.IsNotNull(_hideNoaDebuggerUIButton);
            Assert.IsNotNull(_screenshotButton);
            Assert.IsNotNull(_bootButton);
            _ValidateControllerOrder(_controllerOrderForLowerLeft);
            _ValidateControllerOrder(_controllerOrderForUpperLeft);
            _ValidateControllerOrder(_controllerOrderForUpperRight);
            _ValidateControllerOrder(_controllerOrderForLowerRight);
        }

        void _ValidateControllerOrder(IReadOnlyList<RectTransform> order)
        {
            Assert.AreEqual(order.Count, transform.childCount);

            for (var i = 0; i < order.Count; ++i)
            {
                Assert.IsNotNull(order[i]);

                for (var j = 0; j < i; ++j)
                {
                    Assert.AreNotEqual(order[i], order[j]);
                }
            }
        }

        void _ApplyNoaDebuggerSettings()
        {
            var noaDebuggerSettings = NoaDebuggerSettingsManager.GetNoaDebuggerSettings();
            noaDebuggerSettings.ControllerBackgroundAlpha = UnityEngine.Mathf.Clamp(
                noaDebuggerSettings.ControllerBackgroundAlpha,
                NoaDebuggerDefine.CanvasAlphaMin,
                NoaDebuggerDefine.CanvasAlphaMax);

            foreach (Image image in _opacityTargetImages)
            {
                if (image == null)
                {
                    continue;
                }
                _SetImageOpacity(image);
            }
        }

        void _SetImageOpacity(Image image)
        {
            Color newColor = image.color;
            newColor.a = NoaDebuggerSettingsManager.GetNoaDebuggerSettings().ControllerBackgroundAlpha;
            image.color = newColor;
        }

        void _InitializeCallbacks()
        {
            _pauseResumeButton._onClick.AddListener(isPlaying => OnTogglePauseResume?.Invoke(isPlaying));
            _gameSpeedDecrementButton.onClick.AddListener(() => OnGameSpeedDecrement?.Invoke());
            _gameSpeedDecrementButton._onLongTap.AddListener(() => OnGameSpeedMinimize?.Invoke());
            _gameSpeedIncrementButton.onClick.AddListener(() => OnGameSpeedIncrement?.Invoke());
            _gameSpeedIncrementButton._onLongTap.AddListener(() => OnGameSpeedMaximize?.Invoke());
            _gameSpeedResetButton._onLongTap.AddListener(() => OnGameSpeedReset?.Invoke());
            _frameSteppingButton._onEnabled = () => OnFrameSteppingButtonEnabled?.Invoke();
            _frameSteppingButton._onClick = () => OnFrameStepping?.Invoke();
            _frameSteppingButton._onLongPress.AddListener(() => OnFrameStepping?.Invoke());
            _closeButton.onClick.AddListener(() => OnClose?.Invoke());
            _resetAppButton._onLongTap.AddListener(() => OnResetApplication?.Invoke());
            _hideNoaDebuggerUIButton.onClick.AddListener(() => OnHideNoaDebuggerUI?.Invoke());
            _screenshotButton.onClick.AddListener(() => OnCaptureScreenshot?.Invoke());
            _bootButton.onClick.AddListener(() => OnShowNoaDebugger?.Invoke());
        }

        void _ReorderButtons()
        {
            var corners = new Vector3[4];
            _noaDebuggerButtonTransform.GetWorldCorners(corners);
            var center = new Vector2((corners[2].x + corners[0].x) / 2, (corners[1].y + corners[0].y) / 2);

            RectTransform[] controllerOrder;

            if (center.x < Screen.width / 2.0f)
            {
                if (center.y < Screen.height / 2.0f)
                {
                    controllerOrder = _controllerOrderForLowerLeft;
                    _controllerViewTransform.anchorMin = new Vector2(0, 0);
                    _controllerViewTransform.anchorMax = new Vector2(0, 0);
                    _controllerViewTransform.pivot = new Vector2(0, 0);
                }
                else
                {
                    controllerOrder = _controllerOrderForUpperLeft;
                    _controllerViewTransform.anchorMin = new Vector2(0, 1);
                    _controllerViewTransform.anchorMax = new Vector2(0, 1);
                    _controllerViewTransform.pivot = new Vector2(0, 1);
                }
            }
            else
            {
                if (center.y < Screen.height / 2.0f)
                {
                    controllerOrder = _controllerOrderForLowerRight;
                    _controllerViewTransform.anchorMin = new Vector2(1, 0);
                    _controllerViewTransform.anchorMax = new Vector2(1, 0);
                    _controllerViewTransform.pivot = new Vector2(1, 0);
                }
                else
                {
                    controllerOrder = _controllerOrderForUpperRight;
                    _controllerViewTransform.anchorMin = new Vector2(1, 1);
                    _controllerViewTransform.anchorMax = new Vector2(1, 1);
                    _controllerViewTransform.pivot = new Vector2(1, 1);
                }
            }

            foreach (RectTransform t in controllerOrder)
            {
                t.SetAsLastSibling();
            }
        }
    }
}
