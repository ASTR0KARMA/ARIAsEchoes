using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class LongPressToggleColorButton : LongPressButton
    {
        [Serializable]
        public sealed class ToggleEvent : UnityEvent<bool> { }

        LongPressButton _component;

        [SerializeField]
        Color _off = NoaDebuggerDefine.ImageColors.Default;
        [SerializeField]
        Color _on = NoaDebuggerDefine.ImageColors.Default;
        [SerializeField]
        Color _disable = NoaDebuggerDefine.ImageColors.Disabled;
        [SerializeField]
        bool _isUseDisable = false;
        [SerializeField]
        Graphic[] _targetGraphics;

        bool _isOn;

        bool _clickEventEnabled = true;

        protected override void Awake()
        {
            base.Awake();

            foreach (Graphic graphic in _targetGraphics)
            {
                Assert.IsNotNull(graphic);
            }

            _Refresh();
        }

        protected override void _OnReset()
        {
            base._OnReset();
            if (_isOn)
            {
                _Toggle();
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            _clickEventEnabled = true;
            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if (_isOn)
            {
                _Toggle();
            }
        }

        protected override void _OnClick()
        {
            if (!_clickEventEnabled)
            {
                return;
            }

            base._OnClick();
        }

        protected override void _InvokeLongPressAction()
        {
            base._InvokeLongPressAction();

            if (!_isOn)
            {
                _Toggle();
            }
        }

        void _Toggle()
        {
            _clickEventEnabled = false;
            _isOn = !_isOn;
            _Refresh();
        }

        void _Refresh()
        {
            Color newColor;

            if (_isUseDisable && !interactable)
            {
                newColor = _disable;
            }
            else
            {
                newColor = _isOn ? _on : _off;
            }

            foreach (Graphic graphic in _targetGraphics)
            {
                graphic.color = newColor;
            }
        }
    }
}
