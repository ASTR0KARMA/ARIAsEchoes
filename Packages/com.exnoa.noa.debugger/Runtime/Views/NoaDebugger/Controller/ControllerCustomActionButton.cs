using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    public class ControllerCustomActionButton : MonoBehaviour
    {
        public Action OnTap
        {
            get => _onTap;
            set
            {
                _onTap = value;
                _SetLongTapButtonInteractable();
            }
        }

        public Action OnLongTap
        {
            get => _onLongTap;
            set
            {
                _onLongTap = value;
                _SetLongTapButtonInteractable();
            }
        }

        public Action<bool> OnToggle
        {
            get => _onToggle;
            set
            {
                _onToggle = value;
                _SetToggleButtonInteractable();
            }
        }

        public bool IsToggleOn
        {
            get => _toggleButton.IsOn;
            set => _toggleButton.Init(value);
        }

        public NoaController.CustomActionType ActionType { get; private set; } = NoaController.CustomActionType.Default;

        [SerializeField]
        LongTapButton _longTapButton;

        [SerializeField]
        ToggleButtonBase _toggleButton;

        Action _onTap;
        Action _onLongTap;
        Action<bool> _onToggle;

        void Awake()
        {
            Assert.IsNotNull(_longTapButton);
            Assert.IsNotNull(_toggleButton);

            _longTapButton.onClick.AddListener(() => _onTap?.Invoke());
            _longTapButton._onLongTap.AddListener(() => _onLongTap?.Invoke());
            _toggleButton._onClick.AddListener(isOn => _onToggle?.Invoke(isOn));

            SetCustomActionType(ActionType);
        }

        public void SetCustomActionType(NoaController.CustomActionType actionType)
        {
            ActionType = actionType;

            switch (actionType)
            {
                case NoaController.CustomActionType.Button:
                    _longTapButton.gameObject.SetActive(true);
                    _SetLongTapButtonInteractable();
                    _toggleButton.gameObject.SetActive(false);

                    break;

                case NoaController.CustomActionType.ToggleButton:
                    _longTapButton.gameObject.SetActive(false);
                    _toggleButton.gameObject.SetActive(true);
                    _SetToggleButtonInteractable();

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
            }
        }

        void _SetLongTapButtonInteractable()
        {
            _longTapButton.interactable = _onTap != null || _onLongTap != null;
        }

        void _SetToggleButtonInteractable()
        {
            _toggleButton.Interactable = _onToggle != null;
        }
    }
}
