using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class OverlayLogFontScaler : MonoBehaviour
    {
        [SerializeField]
        RectTransform _logType;
        Vector2 _originalLogTypeSize;
        [SerializeField]
        RectTransform _logCounterTextRect;
        float _originalLogCounterTextWidth;
        [SerializeField]
        HorizontalOrVerticalLayoutGroup _logLayout;
        float _paddingLeftFixedValue;
        [SerializeField]
        NoaDebuggerText[] _targetTexts;
        Dictionary<TextMeshProUGUI, float> _originalFontSizes = new Dictionary<TextMeshProUGUI, float>();

        void _OnValidateUI()
        {
            Assert.IsNotNull(_logType);
            Assert.IsNotNull(_logCounterTextRect);
            Assert.IsNotNull(_logLayout);
            Assert.IsNotNull(_targetTexts);
        }

        void Awake()
        {
            _OnValidateUI();

            _originalLogTypeSize = _logType.sizeDelta;
            _originalLogCounterTextWidth = _logCounterTextRect.sizeDelta.x;
            _paddingLeftFixedValue = _logLayout.padding.left - _originalLogTypeSize.x; 
            foreach (var target in _targetTexts)
            {
                if (target != null)
                {
                    _originalFontSizes[target] = target.fontSize;
                }
            }
        }

        public void ChangeFontScale(float fontScale)
        {
            if(_originalFontSizes.Count == 0)
            {
                return;
            }

            foreach (var target in _targetTexts)
            {
                if (target != null)
                {
                    target.fontSize = _originalFontSizes[target] * fontScale;
                }
            }

            _logType.sizeDelta = _originalLogTypeSize * fontScale;
            _logCounterTextRect.sizeDelta = new Vector2(_originalLogCounterTextWidth * fontScale, _logCounterTextRect.sizeDelta.y);
            _logLayout.padding.left = Mathf.CeilToInt(_logType.sizeDelta.x + _paddingLeftFixedValue);
        }

        void OnDestroy()
        {
            _logType = default;
            _originalLogTypeSize = default;
            _logCounterTextRect = default;
            _logLayout = default;
            _targetTexts = default;
            _originalFontSizes.Clear();
            _originalFontSizes = null;
        }
    }
}
