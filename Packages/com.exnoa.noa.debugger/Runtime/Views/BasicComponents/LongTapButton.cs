using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class LongTapButton : Button
    {
        bool _isPressing;
        float _pressedTime;
        bool _isLongTap;

        [Serializable]
        public sealed class PointerExitEvent : UnityEvent<PointerEventData> { }

        public PointerExitEvent _onPointerExit;

        public ButtonClickedEvent _onPointerDown;

        public ButtonClickedEvent _onPointerUp;

        public ButtonClickedEvent _onPointerClick;

        public ButtonClickedEvent _onLongTap;

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            if (!interactable)
            {
                return;
            }

            _onPointerExit?.Invoke(eventData);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            _onPointerDown?.Invoke();

            if (!interactable)
            {
                return;
            }

            _isPressing = true;
            _pressedTime = Time.realtimeSinceStartup;
            _isLongTap = false;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (_isLongTap || !interactable)
            {
                return;
            }

            _onPointerClick?.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            _onPointerUp?.Invoke();

            if (_isLongTap)
            {
                eventData.eligibleForClick = false;
            }

            _isPressing = false;
            _isLongTap = false;

            EventSystem.current.SetSelectedGameObject(null);
        }

        void Update()
        {
            if (!interactable
                || !_isPressing
                || Time.realtimeSinceStartup - _pressedTime < NoaDebuggerDefine.PressTimeSeconds)
            {
                return;
            }

            _onLongTap?.Invoke();
            _isPressing = false;
            _isLongTap = true;
        }
    }
}
