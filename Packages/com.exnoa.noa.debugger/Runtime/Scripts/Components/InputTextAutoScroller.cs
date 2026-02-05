using UnityEngine;
using UnityEngine.Events;

namespace NoaDebugger
{
    sealed class InputTextAutoScroller : TextAutoScroller
    {
        [SerializeField]
        NoaDebuggerScrollableInputComponent _inputComponent;

        protected override void _OnInitialize()
        {
            base._OnInitialize();

            _inputComponent._onValueChanged.AddListener(_SetScrollTargetText);

            _inputComponent._onSelect.AddListener(_OnSelect);
            _inputComponent._onEndEdit.AddListener(_OnEndEdit);

            _inputComponent.OnInputModeChanged += _SetTextAlpha;
        }

        public override void SetText(string text)
        {
            base.SetText(text);
            _inputComponent.Text = text;
            _SetTextAlpha(_inputComponent.IsInputMode);
        }

        void _OnSelect(string text)
        {
            SetIsScroll(false);
        }

        void _OnEndEdit(string text)
        {
            SetIsScroll(true);
            _SetScrollTargetText(text);
        }

        void _SetTextAlpha(bool isInputMode)
        {
            _inputComponent.TextComponent.color = isInputMode
                ? NoaDebuggerDefine.TextColors.Default
                : Color.clear;
            _scrollText.color = isInputMode
                ? Color.clear
                : NoaDebuggerDefine.TextColors.Default;
        }

        public void SetOnSelect(UnityAction<string> action)
        {
            _inputComponent._onSelect.AddListener(action);
        }

        public void SetOnEndEdit(UnityAction<string> action)
        {
            _inputComponent._onEndEdit.AddListener(action);
        }

        public void SetCharacterLimit(int num)
        {
            _inputComponent.CharacterLimit = num;
        }
    }
}
