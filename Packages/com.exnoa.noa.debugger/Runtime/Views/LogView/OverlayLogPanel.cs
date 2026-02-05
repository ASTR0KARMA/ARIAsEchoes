using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class OverlayLogPanel : MonoBehaviour
    {
        const float HIGHLIGHT_TIME = 1f;

        [SerializeField]
        Image _logBackground;
        [SerializeField]
        Image _logType;
        [SerializeField]
        TextMeshProUGUI _logString;
        [SerializeField]
        GameObject _logCounterRoot;
        [SerializeField]
        TextMeshProUGUI _logCounter;

        [SerializeField]
        OverlayLogFontScaler _fontScaler;

        [SerializeField, Header("HighlightColor")]
        Color _highlightColor;

        LogPanelCommonDisplayProcessing _displayProcessing;

        LogViewLinker.LogPanelInfo _logInfo;
        int _logHashCode;
        static float _opacityRate = -1;
        static Color _defaultColor;

        IEnumerator _highlightCoroutine = null;

        public RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = transform as RectTransform;
                }

                return _rectTransform;
            }
        }
        RectTransform _rectTransform;

        public int LogSerialNumber => _logInfo._serialNumber;

        void _OnValidateUI()
        {
            Assert.IsNotNull(_logBackground);
            Assert.IsNotNull(_logType);
            Assert.IsNotNull(_logString);
            Assert.IsNotNull(_logCounterRoot);
            Assert.IsNotNull(_logCounter);
            Assert.IsNotNull(_fontScaler);
        }

        void OnDestroy()
        {
            if(_highlightCoroutine != null)
            {
                if (GlobalCoroutine.IsExecutable)
                {
                    GlobalCoroutine.Stop(_highlightCoroutine);
                }
                _highlightCoroutine = null;
            }

            _logType = default;
            _logString = default;
            _logString = default;
            _logCounterRoot = default;
            _logCounter = default;
            _fontScaler = default;
            _displayProcessing = default;
            _logBackground = default;
            _logInfo = default;
        }

        void Start()
        {
            _OnValidateUI();
        }

        public void Draw(LogViewLinker.LogPanelInfo logInfo, bool isShowTimestamp)
        {
            if (_opacityRate < 0)
            {
                var settings = NoaDebuggerSettingsManager.GetNoaDebuggerSettings();
                _opacityRate = settings.OverlayBackgroundOpacity;
                _defaultColor = _logBackground.color;
            }

            _displayProcessing ??= new LogPanelCommonDisplayProcessing(_logType, _logString, _logCounterRoot, _logCounter);

            bool isFirstTime = logInfo._hasLoggedOnce == false;
            _logHashCode = logInfo._serialNumber;
            _logInfo = logInfo;

            _RefreshLogString(logInfo, isShowTimestamp);
            _displayProcessing.RefreshLogType(logInfo);
            _displayProcessing.RefreshLogCounter(logInfo);

            if (_highlightCoroutine == null)
            {
                _ChangeLogBackgroundColor(_defaultColor);
            }
            if (isFirstTime)
            {
                if (_highlightCoroutine != null)
                {
                    GlobalCoroutine.Stop(_highlightCoroutine);
                    _highlightCoroutine = null;
                }

                _highlightCoroutine = _HighlightTemporarily();
                GlobalCoroutine.Run(_highlightCoroutine);
            }
        }

        void _RefreshLogString(LogViewLinker.LogPanelInfo logInfo, bool isShowTimestamp)
        {
            string logString = logInfo._logString;
            if (isShowTimestamp == false)
            {
                string[] logSplit = logInfo._logString.Split(' ', 2);
                logString = logSplit[1];
            }

            string[] splitLogs = logString.Split(NoaDebuggerDefine.NewLine, StringSplitOptions.RemoveEmptyEntries);
            if(splitLogs.Length > 1)
            {
                logString = splitLogs[0];
                logString += NoaDebuggerDefine.Ellipsis; 
            }

            _displayProcessing.RefreshLogString(logString);
        }

        public void ChangeFontScale(float fontScale) => _fontScaler.ChangeFontScale(fontScale);

        public void ResetHashCode()
        {
            _logHashCode = -1;
        }

        public bool Equal(LogViewLinker.LogPanelInfo logPanelInfo)
        {
            return _logHashCode == logPanelInfo._serialNumber;
        }

        IEnumerator _HighlightTemporarily()
        {
            _ChangeLogBackgroundColor(_highlightColor);

            float startTime = Time.realtimeSinceStartup;
            float elapsedTime = 0;

            while(elapsedTime < HIGHLIGHT_TIME)
            {
                elapsedTime = Time.realtimeSinceStartup - startTime;
                yield return null;
            }

            _ChangeLogBackgroundColor(_defaultColor);
        }

        void _ChangeLogBackgroundColor(Color changeColor)
        {
            var newColor = changeColor;
            newColor.a = changeColor.a * _opacityRate;
            _logBackground.color = newColor;
        }
    }
}
