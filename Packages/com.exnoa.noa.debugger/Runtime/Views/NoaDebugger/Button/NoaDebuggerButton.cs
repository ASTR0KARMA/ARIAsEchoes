using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace NoaDebugger
{
    public enum ButtonPosition
    {
        UpperLeft, 
        UpperCenter, 
        UpperRight, 
        MiddleLeft, 
        MiddleRight, 
        LowerLeft, 
        LowerCenter, 
        LowerRight, 
    }

    public enum ButtonMovementType
    {
        Draggable,
        Fixed
    }

    sealed class NoaDebuggerButton : MonoBehaviour
    {
        static readonly string DeviceOrientationKey = "NoaDebuggerButtonDeviceOrientation";

        enum TapState
        {
            None,
            Tap,
            Drag
        }

        [SerializeField]
        DragBehaviour _dragBehaviour;

        [SerializeField]
        PointerEventComponent _bootButton;

        [SerializeField]
        NoaDebuggerButtonVisualController _buttonVisualController;

        [SerializeField]
        PointerEventComponent _visibleButton;

        [SerializeField]
        NoaControllerView _controllerView;

        public NoaControllerView ControllerView => _controllerView;

        public Action OnShowController { get; set; }

        bool _isInitialized = false;

        TapState _tapState = TapState.None;

        float _pressStartTime;

        Vector2? _pressStartPos;

        bool _isShowNoaDebugger;

        NoaDebuggerSettings _noaDebuggerSettings = null;

        RectTransform _rootRect;

        RectTransform CachedRootRect => _rootRect ??= transform as RectTransform;

        public bool IsPlayingAnimation => _buttonVisualController.IsPlayingAnimation;

        void _OnValidateUI()
        {
            Assert.IsNotNull(_dragBehaviour);
            Assert.IsNotNull(_bootButton);
            Assert.IsNotNull(_buttonVisualController);
            Assert.IsNotNull(_visibleButton);
            Assert.IsNotNull(_controllerView);
        }

        public void Init()
        {
            if (_isInitialized)
            {
                return;
            }

            _OnValidateUI();
            _isInitialized = true;
            _bootButton.OnPointerDownEvent += _OnPointerDown;
            _visibleButton.OnPointerClickEvent += _ => _SetActiveUI();
            _buttonVisualController.Init();
            _UpdateSettings();

            NoaDebuggerVisibilityManager.AddTriggerButtonSetActiveAction(_OnStartButtonSetActive);

            DeviceOrientationManager.SetAction(NoaDebuggerButton.DeviceOrientationKey, _LoadPosition);
        }

        public void PlayOnLocationAnimation()
        {
            if (!_isInitialized)
            {
                Init();
            }

            if (_noaDebuggerSettings.ToolStartButtonAlpha == 0
                && _noaDebuggerSettings.StartButtonMovementType == ButtonMovementType.Draggable)
            {
                _buttonVisualController.PlayOnLocationAnimation(_IsAnimationBreak);
            }
        }

        public void PlayOnErrorAnimation()
        {
            _buttonVisualController.PlayOnErrorAnimation(_IsAnimationBreak);
        }

        public void ResetButtonColor()
        {
            _buttonVisualController.ResetButtonColor();
        }

        public void ApplySettings(bool isPortrait)
        {
            _UpdateSettings();
            _LoadPosition(isPortrait);
        }

        bool _IsAnimationBreak()
        {
            return _tapState != TapState.None || NoaDebuggerVisibilityManager.IsControllerActive;
        }

        void _OnPointerDown(PointerEventData eventData)
        {
            _tapState = TapState.Tap;
            _dragBehaviour.CanMove = false;
            _pressStartTime = Time.realtimeSinceStartup;
            _pressStartPos = eventData.position;
        }

        void _UpdateSettings()
        {
#if NOA_DEBUGGER

            _noaDebuggerSettings = NoaDebuggerSettingsManager.GetNoaDebuggerSettings();

            var startButtonScale = _noaDebuggerSettings.StartButtonScale;
            transform.localScale = new Vector3(startButtonScale, startButtonScale, startButtonScale);

            _noaDebuggerSettings.ToolStartButtonAlpha = UnityEngine.Mathf.Clamp(
                _noaDebuggerSettings.ToolStartButtonAlpha,
                NoaDebuggerDefine.ToolStartButtonAlphaMin,
                NoaDebuggerDefine.ToolStartButtonAlphaMax);

            bool isDraggable = _noaDebuggerSettings.StartButtonMovementType == ButtonMovementType.Draggable;
            gameObject.GetComponent<DragBehaviourFitWithInScreen>().enabled = isDraggable;

            _buttonVisualController.ResetButtonColor();
#endif
        }

        void _OnStartButtonSetActive(bool active)
        {
            if (active)
            {
                _buttonVisualController.ResetButtonColor(isForceResetInAnimation: false);
            }
        }

        void Update()
        {
            if (!_isInitialized)
            {
                return;
            }

            _UpdateButton();

            if (_isShowNoaDebugger)
            {
                _Reset();

                NoaDebugger.ShowNoaDebuggerLastActiveTool();
            }
        }

        void _UpdateButton()
        {
            float pressingTime = Time.realtimeSinceStartup - _pressStartTime;

            _UpdateTapStateOnPressing(pressingTime);

            _UpdateButtonFromTapState(pressingTime);
        }

        void _UpdateTapStateOnPressing(float pressingTime)
        {
            if (_tapState == TapState.Tap && pressingTime >= NoaDebuggerDefine.PressTimeSeconds)
            {
                NoaDebuggerVisibilityManager.SetControllerVisible(true);
                _tapState = TapState.None;
                _buttonVisualController.ResetButtonColor();
                OnShowController?.Invoke();
            }

            if (_tapState is TapState.Tap && _pressStartPos != null)
            {
                float distance = Vector2.Distance(_pressStartPos.Value, Input.GetCursorPosition());

                if (distance >= NoaDebuggerDefine.DragThresholdDistanceOnScreen)
                {
                    _tapState = TapState.Drag;
                    _dragBehaviour.CanMove = true;
                }
            }
        }

        void _UpdateButtonFromTapState(float pressingTime)
        {
            switch (_tapState)
            {
                case TapState.None:

                    break;

                case TapState.Tap:
                    _UpdateButtonAlphaOnPressing(pressingTime);

                    if (Input.IsButtonReleased())
                    {
                        _isShowNoaDebugger = true;
                        _tapState = TapState.None;
                        _buttonVisualController.ResetButtonColor();
                    }

                    break;

                case TapState.Drag:
                    _UpdateButtonAlphaOnPressing(pressingTime);

                    if (Input.IsButtonReleased())
                    {
                        _SavePosition(DeviceOrientationManager.IsPortrait);
                        _tapState = TapState.None;
                        _buttonVisualController.ResetButtonColor();
                    }

                    break;
            }
        }

        void _UpdateButtonAlphaOnPressing(float pressingTime)
        {
            if (_noaDebuggerSettings == null)
            {
                return;
            }

            float t = Mathf.InverseLerp(0, NoaDebuggerDefine.PressTimeSeconds, pressingTime);
            float alpha = Mathf.Lerp(_noaDebuggerSettings.ToolStartButtonAlpha, 1, t);
            _buttonVisualController.SetCanvasGroupAlpha(alpha);
        }

        void _Reset()
        {
            _tapState = TapState.None;
            _pressStartTime = 0;
            _pressStartPos = null;
            _isShowNoaDebugger = false;
        }

        void _SetActiveUI()
        {
            NoaDebuggerVisibilityManager.OnVisibleButtonVisibility();
        }

        void _LoadPosition(bool isPortrait)
        {
            if (_noaDebuggerSettings == null)
            {
                return;
            }

            if (_noaDebuggerSettings.StartButtonMovementType == ButtonMovementType.Fixed)
            {
                _SetupButtonPosition();

                return;
            }

            transform.position = _GetPositionPrefsData(isPortrait);
        }

        void _SavePosition(bool isPortrait)
        {
            if (_noaDebuggerSettings == null || _noaDebuggerSettings.SaveStartButtonPosition == false)
            {
                return;
            }

            var key = isPortrait
                ? NoaDebuggerPrefsDefine.PrefsKeyStartButtonPortrait
                : NoaDebuggerPrefsDefine.PrefsKeyStartButtonLandscape;

            var pos = transform.position;
            string saveString = $"{pos.x},{pos.y},{pos.z}";
            NoaDebuggerPrefs.SetString(key, saveString);
        }

        Vector3 _GetPositionPrefsData(bool isPortrait)
        {
            if (!_noaDebuggerSettings.SaveStartButtonPosition)
            {
                return _SetupButtonPosition();
            }

            string key = isPortrait
                ? NoaDebuggerPrefsDefine.PrefsKeyStartButtonPortrait
                : NoaDebuggerPrefsDefine.PrefsKeyStartButtonLandscape;

            string positionString = NoaDebuggerPrefs.GetString(key, "");

            if (String.IsNullOrEmpty(positionString))
            {
                return _SetupButtonPosition();
            }

            Vector3 position = _DeserializeVector3(positionString);

            if (_IsOutOfScreen(position))
            {
                position = _SetupButtonPosition();
            }

            return position;
        }

        Vector3 _DeserializeVector3(string value)
        {
            var values = value.Split(',');
            bool canParse = true;

            if (!float.TryParse(values[0], out float x))
            {
                canParse = false;
            }

            if (!float.TryParse(values[1], out float y))
            {
                canParse = false;
            }

            if (!float.TryParse(values[2], out float z))
            {
                canParse = false;
            }

            return canParse ? new Vector3(x, y, z) : _SetupButtonPosition();
        }

        void OnRectTransformDimensionsChange()
        {
            if (_dragBehaviour.isDragging)
            {
                return;
            }

            if (_IsOutOfScreen(transform.position))
            {
                transform.position = _SetupButtonPosition();
            }
        }

        bool _IsOutOfScreen(Vector3 buttonPosition)
        {
            Vector3 leftBottom = Vector3.zero;
            Vector3 rightTop = new Vector3(Screen.width, Screen.height, 0);

            Vector3 screenMargin = new Vector3(10, 10, 0);
            leftBottom -= screenMargin;
            rightTop += screenMargin;

            CachedRootRect.position = buttonPosition;
            Vector3[] buttonCorners = new Vector3[4];
            CachedRootRect.GetWorldCorners(buttonCorners);

            if (buttonCorners[2].x > rightTop.x)
            {
                return true;
            }

            if (buttonCorners[2].y > rightTop.y)
            {
                return true;
            }

            if (buttonCorners[0].x < leftBottom.x)
            {
                return true;
            }

            if (buttonCorners[0].y < leftBottom.y)
            {
                return true;
            }

            return false;
        }

        Vector3 _SetupButtonPosition()
        {
            if (_noaDebuggerSettings == null)
            {
                return CachedRootRect.position;
            }

            switch (_noaDebuggerSettings.StartButtonPosition)
            {
                case ButtonPosition.UpperLeft:
                    CachedRootRect.anchorMin = new Vector2(0, 1);
                    CachedRootRect.anchorMax = new Vector2(0, 1);
                    CachedRootRect.pivot = new Vector2(0, 1);

                    break;

                case ButtonPosition.UpperCenter:
                    CachedRootRect.anchorMin = new Vector2(0.5f, 1);
                    CachedRootRect.anchorMax = new Vector2(0.5f, 1);
                    CachedRootRect.pivot = new Vector2(0.5f, 1);

                    break;

                case ButtonPosition.UpperRight:
                    CachedRootRect.anchorMin = new Vector2(1, 1);
                    CachedRootRect.anchorMax = new Vector2(1, 1);
                    CachedRootRect.pivot = new Vector2(1, 1);

                    break;

                case ButtonPosition.MiddleRight:
                    CachedRootRect.anchorMin = new Vector2(1, 0.5f);
                    CachedRootRect.anchorMax = new Vector2(1, 0.5f);
                    CachedRootRect.pivot = new Vector2(1, 0.5f);

                    break;

                case ButtonPosition.MiddleLeft:
                    CachedRootRect.anchorMin = new Vector2(0, 0.5f);
                    CachedRootRect.anchorMax = new Vector2(0, 0.5f);
                    CachedRootRect.pivot = new Vector2(0, 0.5f);
                    CachedRootRect.anchoredPosition = new Vector2(10, 0);

                    break;

                case ButtonPosition.LowerRight:
                    CachedRootRect.anchorMin = new Vector2(1, 0);
                    CachedRootRect.anchorMax = new Vector2(1, 0);
                    CachedRootRect.pivot = new Vector2(1, 0);

                    break;

                case ButtonPosition.LowerLeft:
                    CachedRootRect.anchorMin = new Vector2(0, 0);
                    CachedRootRect.anchorMax = new Vector2(0, 0);
                    CachedRootRect.pivot = new Vector2(0, 0);

                    break;

                case ButtonPosition.LowerCenter:
                    CachedRootRect.anchorMin = new Vector2(0.5f, 0);
                    CachedRootRect.anchorMax = new Vector2(0.5f, 0);
                    CachedRootRect.pivot = new Vector2(0.5f, 0);

                    break;
            }

            CachedRootRect.anchoredPosition = new Vector2(0, 0);

            return CachedRootRect.position;
        }

        public void Dispose()
        {
            NoaDebuggerVisibilityManager.RemoveTriggerButtonSetActiveAction(_OnStartButtonSetActive);

            _dragBehaviour = default;
            _bootButton = default;
            _buttonVisualController = default;
            _visibleButton = default;
            _controllerView = default;
            _noaDebuggerSettings = default;
            _rootRect = default;
        }
    }
}
