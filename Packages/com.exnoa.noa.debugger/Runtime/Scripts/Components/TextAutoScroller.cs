using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NoaDebugger
{
    class TextAutoScroller : UIBehaviour
    {
        enum ScrollState
        {
            StartWait,

            Scroll,

            EndWait,

            Unnecessary,
        }

        static readonly float SCROLL_START_DELAY = 1f;

        static readonly float SCROLL_END_DELAY = 1.5f;

        static readonly float SCROLL_X_PER_SECOND = 15f;

        [SerializeField]
        protected TextMeshProUGUI _scrollText;

        [SerializeField]
        RectMask2D _targetMask;

        ScrollState _currentScrollState;

        bool _isScroll = false;

        bool _isInit = false;

        RectTransform ScrollTextRect => _scrollText.rectTransform;
        RectTransform TargetMaskRect => _targetMask.rectTransform;

        float _scrollStartPosX;

        float _waitTime;

        void Update()
        {
            if (!_isInit)
            {
                _Initialize();
            }

            if (!_isScroll)
            {
                return;
            }

            switch (_currentScrollState)
            {
                case ScrollState.StartWait:
                    _waitTime -= Time.unscaledDeltaTime;
                    _CheckWaitTime(nextState:ScrollState.Scroll);
                    break;

                case ScrollState.Scroll:
                    float scrollValue = TextAutoScroller.SCROLL_X_PER_SECOND * Time.unscaledDeltaTime;
                    RectTransform targetTransform = ScrollTextRect;
                    Vector2 newPos = targetTransform.anchoredPosition;
                    float movedPosX = newPos.x - scrollValue;
                    float endPosX = _GetEndPosX();
                    if (movedPosX <= endPosX)
                    {
                        movedPosX = endPosX;
                        _SetScrollState(ScrollState.EndWait);
                    }
                    newPos.x = movedPosX;
                    targetTransform.anchoredPosition = newPos;
                    break;

                case ScrollState.EndWait:
                    _waitTime -= Time.unscaledDeltaTime;
                    _CheckWaitTime(nextState:ScrollState.StartWait);
                    break;

                case ScrollState.Unnecessary:
                    return;
            }
        }

        protected override void OnRectTransformDimensionsChange()
        {
            _CalculateWidth();
        }

        void _Initialize()
        {
            _OnInitialize();
            _isInit = true;
        }

        protected virtual void _OnInitialize()
        {
            _scrollStartPosX = ScrollTextRect.anchoredPosition.x;
        }

        public void SetIsScroll(bool isScroll)
        {
            if (_isScroll == isScroll)
            {
                return;
            }

            _isScroll = isScroll;

            _ResetPosition();
        }

        void _ResetPosition()
        {
            if (!_isInit)
            {
                return;
            }

            RectTransform targetTransform = ScrollTextRect;
            Vector2 startPos = targetTransform.anchoredPosition;
            startPos.x = _scrollStartPosX;
            targetTransform.anchoredPosition = startPos;
        }

        void _CalculateWidth()
        {
            float textWidth = _scrollText.preferredWidth;
            float maskWidth = TargetMaskRect.rect.width;

            Vector2 sizeDelta = ScrollTextRect.sizeDelta;
            sizeDelta.x = textWidth;
            ScrollTextRect.sizeDelta = sizeDelta;

            if (textWidth <= maskWidth)
            {
                _SetScrollState(ScrollState.Unnecessary);
                return;
            }

            _SetScrollState(ScrollState.StartWait);
        }

        float _GetEndPosX()
        {
            float textWidth = ScrollTextRect.rect.width;
            float maskWidth = TargetMaskRect.rect.width;
            return _scrollStartPosX - (textWidth - maskWidth);
        }

        void _SetScrollState(ScrollState state)
        {
            switch (state)
            {
                case ScrollState.StartWait:
                    _waitTime = TextAutoScroller.SCROLL_START_DELAY;
                    _ResetPosition();
                    break;

                case ScrollState.Scroll:
                    break;

                case ScrollState.EndWait:
                    _waitTime = TextAutoScroller.SCROLL_END_DELAY;
                    break;

                case ScrollState.Unnecessary:
                    _ResetPosition();
                    break;
            }

            _currentScrollState = state;
        }

        void _CheckWaitTime(ScrollState nextState)
        {
            if (_waitTime <= 0)
            {
                _SetScrollState(nextState);
            }
        }

        public virtual void SetText(string text)
        {
            _SetScrollTargetText(text);
        }

        protected void _SetScrollTargetText(string text)
        {
            _scrollText.text = text;

            _CalculateWidth();
        }
    }
}
