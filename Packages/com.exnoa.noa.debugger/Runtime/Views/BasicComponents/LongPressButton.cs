using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NoaDebugger
{
    public class LongPressButton : Button
    {
        [Serializable]
        public class DisableTarget
        {
            [SerializeField]
            Image _image;
            [SerializeField]
            Color _normalColor;
            [SerializeField]
            Color _disabledColor;

            public void SetColor(bool isEnable)
            {
                _image.color = isEnable ? _normalColor : _disabledColor;
            }
        }

        enum PressState
        {
            None,

            PointerDown,

            Pressing,

            PressingLoop,

            PressingSpeedUp,
        }

        [SerializeField]
        DisableTarget[] _disableTargets;

        [Serializable]
        public sealed class PointerExitEvent : UnityEvent<PointerEventData> { }
        public sealed class ButtonResetEvent  : UnityEvent {}

        PressState _pressState = PressState.None;
        float _lastCheckedTime;
        float _currentInterval;
        int _pressingActionCount;

        bool _isNeedSpeedUp;
        float _pressTimeSeconds;
        float _pressActionFirstInterval;
        float _pressActionIntervalChangeCount;
        float _pressActionSecondInterval;

        public UnityAction _onEnabled;

        public ButtonClickedEvent _onPointerDown;

        public ButtonClickedEvent _onLongPress;

        public PointerExitEvent _onPointerExit;

        public ButtonClickedEvent _onPointerUp;

        public UnityAction _onClick;

        protected override void Awake()
        {
            base.Awake();
            _pressTimeSeconds = NoaDebuggerDefine.PressTimeSeconds;
            _pressActionIntervalChangeCount = NoaDebuggerDefine.PressActionIntervalChangeCount;
            SetActionIntervalSettings(new LongPressButtonActionIntervalSettings());
            onClick.AddListener(_OnClick);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _onEnabled?.Invoke();
        }

        protected virtual void _OnClick()
        {
            _onClick?.Invoke();
        }

        protected virtual void _OnReset() {}

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (_pressState != PressState.None)
            {
                return;
            }

            base.OnPointerDown(eventData);
            _onPointerDown?.Invoke();

            if (!interactable)
            {
                return;
            }

            _pressState = PressState.PointerDown;
            _lastCheckedTime = Time.realtimeSinceStartup;
            _pressingActionCount = 0;
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            _onPointerExit?.Invoke(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            _onPointerUp?.Invoke();

            if (_pressState != PressState.PointerDown)
            {
                eventData.eligibleForClick = false;
            }

            _pressState = PressState.None;

            EventSystem.current.SetSelectedGameObject(null);
        }

        public void SetActionIntervalSettings(LongPressButtonActionIntervalSettings setting)
        {
            _isNeedSpeedUp = setting._isNeedSpeedUp;
            _pressActionFirstInterval = setting._firstInterval;
            _pressActionSecondInterval = setting._secondInterval;
        }

        public void ResetButtonState()
        {
            _pressState = PressState.None;
            _OnReset();
        }

        void Update()
        {
            if (!interactable)
            {
                return;
            }

            switch (_pressState)
            {
                case PressState.None:
                    return;

                case PressState.PointerDown:
                    if (_IsOverThresholdTime(_pressTimeSeconds))
                    {
                        _pressState = PressState.Pressing;
                    }

                    break;

                case PressState.Pressing:
                    if (_IsOverThresholdTime(_pressActionFirstInterval))
                    {
                        _InvokeLongPressAction();

                        if (_isNeedSpeedUp)
                        {
                            _pressingActionCount++;

                            if (_pressingActionCount >= _pressActionIntervalChangeCount)
                            {
                                _pressState = PressState.PressingSpeedUp;
                            }
                        }
                    }

                    break;

                case PressState.PressingSpeedUp:
                    if (_IsOverThresholdTime(_pressActionSecondInterval))
                    {
                        _InvokeLongPressAction();
                    }

                    break;
            }
        }

        bool _IsOverThresholdTime(float thresholdTime)
        {
            return Time.realtimeSinceStartup - _lastCheckedTime >= thresholdTime;
        }

        protected virtual void _InvokeLongPressAction()
        {
            _onLongPress?.Invoke();

            _lastCheckedTime = Time.realtimeSinceStartup;
        }

        public void SetInteractable(bool isInteractable)
        {
            interactable = isInteractable;

            foreach (DisableTarget target in _disableTargets)
            {
                target.SetColor(isInteractable);
            }
        }

        public bool IsLongPress()
        {
            return _pressState == PressState.Pressing ||
                   _pressState == PressState.PressingLoop ||
                   _pressState == PressState.PressingSpeedUp;
        }
    }

    public class LongPressButtonActionIntervalSettings
    {
        public bool _isNeedSpeedUp = true;
        public float _firstInterval= NoaDebuggerDefine.PressActionFirstInterval;
        public float _secondInterval = NoaDebuggerDefine.PressActionSecondInterval;
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(LongPressButton))]
    public class LongPressButtonEditor : UnityEditor.UI.ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            UnityEditor.EditorGUILayout.PropertyField(
                serializedObject.FindProperty("_disableTargets"), new GUIContent("Disable Targets"));

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
