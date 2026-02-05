using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace NoaDebugger
{
    abstract class NumericSettingsPanelBase<T> : SettingsPanelBase<T> where T : struct
    {
        [SerializeField]
        DraggableNumericInputSlider _swipe;
        [SerializeField]
        NoaDebuggerScrollableInputComponent _input;
        [SerializeField]
        LongPressButton _leftArrowButton;
        [SerializeField]
        LongPressButton _rightArrowButton;

        [SerializeField]
        BlockableScrollRect _parentScroll;

        T _tmpValue;
        T _beginDragValue;

        IMutableNumericParameter<T> _numericParameter;

        public override void Initialize(IMutableParameter<T> settings)
        {
            base.Initialize(settings);

            Assert.IsNotNull(_swipe);
            Assert.IsNotNull(_input);
            Assert.IsNotNull(_leftArrowButton);
            Assert.IsNotNull(_rightArrowButton);
            Assert.IsNotNull(_parentScroll);

            _numericParameter = settings as IMutableNumericParameter<T>;

            _InitializeUI();

            Refresh();
        }

        void _InitializeUI()
        {
            if (UserAgentModel.IsMobileBrowser)
            {
                _swipe.gameObject.SetActive(false);
            }
            else
            {
                _swipe.gameObject.SetActive(true);
                _swipe.Initialize(_parentScroll, _input);
                _swipe.OnBeginSlider.AddListener(_OnBeginDrag);
                _swipe.OnSliding.AddListener(_OnSliding);
                _swipe.OnEndSlider.AddListener(_SetValue);
            }

            _input.gameObject.SetActive(true);
            _input._onEndEdit.RemoveAllListeners();
            _input._onEndEdit.AddListener(_OnEndInputEdit);

            _leftArrowButton._onPointerDown.RemoveAllListeners();
            _leftArrowButton._onPointerDown.AddListener(_OnDownArrowButton);
            _leftArrowButton._onPointerExit.RemoveAllListeners();
            _leftArrowButton._onPointerExit.AddListener(_OnExitArrowButton);
            _leftArrowButton.onClick.RemoveAllListeners();
            _leftArrowButton.onClick.AddListener(_OnClickLeftArrowButton);
            _leftArrowButton._onLongPress.RemoveAllListeners();
            _leftArrowButton._onLongPress.AddListener(_DecrementValue);
            _rightArrowButton._onPointerDown.RemoveAllListeners();
            _rightArrowButton._onPointerDown.AddListener(_OnDownArrowButton);
            _rightArrowButton._onPointerExit.RemoveAllListeners();
            _rightArrowButton._onPointerExit.AddListener(_OnExitArrowButton);
            _rightArrowButton.onClick.RemoveAllListeners();
            _rightArrowButton.onClick.AddListener(_OnClickRightArrowButton);
            _rightArrowButton._onLongPress.RemoveAllListeners();
            _rightArrowButton._onLongPress.AddListener(_IncrementValue);
        }

        protected override void _Refresh()
        {
            T value = _parameter.Value;
            _input.Text = value.ToString();

            _leftArrowButton.SetInteractable(!_numericParameter.IsEqualOrUnderMin(value));
            _rightArrowButton.SetInteractable(!_numericParameter.IsEqualOrOverMax(value));
        }

        void _OnEndInputEdit(string text)
        {
            T value = _numericParameter.FromString(text);
            _numericParameter.ChangeValue(value);
            Refresh();
        }

        void _OnBeginDrag()
        {
            _beginDragValue = _parameter.Value;
        }

        void _OnSliding(float distance)
        {
            _FluctuateValue(_beginDragValue, _DelimitDistance(distance));
        }

        void _OnDownArrowButton()
        {
            _parentScroll._canMoveScroll = false;
        }

        void _OnExitArrowButton(PointerEventData eventData)
        {
            _parentScroll._canMoveScroll = true;
        }

        void _OnClickLeftArrowButton()
        {
            _parentScroll._canMoveScroll = true;

            _DecrementValue();
        }

        void _DecrementValue()
        {
            _FluctuateValue(_parameter.Value, -1);
            _SetValue();
        }

        void _OnClickRightArrowButton()
        {
            _parentScroll._canMoveScroll = true;

            _IncrementValue();
        }

        void _IncrementValue()
        {
            _FluctuateValue(_parameter.Value, 1);
            _SetValue();
        }

        void _FluctuateValue(T beginValue, int count)
        {
            _tmpValue = _numericParameter.ValidateValueForFluctuation(beginValue, count);
            _input.Text = _tmpValue.ToString();
        }

        void _SetValue()
        {
            _parameter.ChangeValue(_tmpValue);
            Refresh();
        }

        int _DelimitDistance(float distance)
        {
            return Mathf.FloorToInt(distance / NoaDebuggerDefine.NUMERIC_INPUT_DRAG_SENSITIVITY);
        }
    }
}
