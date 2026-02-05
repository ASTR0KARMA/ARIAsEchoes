using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace NoaDebugger
{
    sealed class NoaDebuggerScrollableInputComponent : MonoBehaviour
    {
        [SerializeField]
        NoaDebuggerInputField _targetInput;

        [SerializeField]
        PointerEventComponent _pointerEventComponent;

        bool _isInputMode;

        public bool IsInputMode => _isInputMode;

        bool _isInputModeBeforeOnEndEdit;
        int _onEndEditFrame;
        int _onPointerDownFrame;

        bool _isMobile;


        public string Text
        {
            get => _targetInput.text;
            set => _targetInput.text = value;
        }

        public TMP_Text TextComponent => _targetInput.textComponent;

        public int CharacterLimit
        {
            set => _targetInput.characterLimit = value;
        }


        public event UnityAction<bool> OnInputModeChanged;

        public TMP_InputField.OnChangeEvent _onValueChanged;

        public TMP_InputField.SelectionEvent _onSelect;

        public TMP_InputField.SubmitEvent _onEndEdit;

        void Awake()
        {
            _isInputMode = false;
            _isInputModeBeforeOnEndEdit = false;
            _targetInput.onValueChanged.AddListener(_OnValueChanged);
            _targetInput.onSelect.AddListener(_OnSelect);
            _targetInput.onEndEdit.AddListener(_OnEndEdit);

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            _isMobile = true;
#else
            _isMobile = UserAgentModel.IsMobileBrowser;
#endif
            if (_isMobile)
            {
                _pointerEventComponent.gameObject.SetActive(true);
                _pointerEventComponent.OnPointerDownEvent += _OnDownInputAreaForMobile;
                _pointerEventComponent.OnPointerClickEvent += _OnClickInputAreaForMobile;
            }
            else
            {
                _pointerEventComponent.gameObject.SetActive(false);
            }
        }

        public void UpdateValidation(TMP_InputField.OnValidateInput validate = null)
        {
            _targetInput.UseCustomValidation(validate);
        }

        public void UpdateContentType(
            TMP_InputField.ContentType contentType,
            TMP_InputField.InputType inputType = TMP_InputField.InputType.Standard,
            TouchScreenKeyboardType keyboardType = TouchScreenKeyboardType.Default,
            TMP_InputField.CharacterValidation characterValidation = TMP_InputField.CharacterValidation.None)
        {
            _targetInput.UseContentType(contentType, inputType, keyboardType, characterValidation);
        }

        public void Select()
        {
            _targetInput.Select();
        }

        void _OnValueChanged(string text)
        {
            _onValueChanged?.Invoke(text);
        }

        void _OnSelect(string text)
        {
            _ChangeInputMode(isInputMode: true);
            _onSelect?.Invoke(text);
        }

        void _OnEndEdit(string text)
        {
            if (_isMobile)
            {
                _isInputModeBeforeOnEndEdit = _isInputMode;
                _onEndEditFrame = Time.frameCount;
            }

            _ChangeInputMode(isInputMode: false);
            _onEndEdit?.Invoke(text);

            if (!EventSystem.current.alreadySelecting)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        void _ChangeInputMode(bool isInputMode)
        {
            _isInputMode = isInputMode;
            OnInputModeChanged?.Invoke(isInputMode);
        }

        void _OnDownInputAreaForMobile(PointerEventData pointerEventData)
        {
            _onPointerDownFrame = Time.frameCount;
        }

        void _OnClickInputAreaForMobile(PointerEventData pointerEventData)
        {
            bool isSameFrameEndEditAndPointerDown = _onEndEditFrame == _onPointerDownFrame;
            if(isSameFrameEndEditAndPointerDown ? _isInputModeBeforeOnEndEdit : _isInputMode)
            {
                _targetInput.DeactivateInputField();
            }
            else
            {
                _targetInput.Select();
            }
        }
    }
}
