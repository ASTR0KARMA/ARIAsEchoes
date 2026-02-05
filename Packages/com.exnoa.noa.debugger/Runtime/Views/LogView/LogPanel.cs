using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class LogPanel : MonoBehaviour
    {
        [SerializeField]
        GameObject _select;

        [SerializeField]
        Image _logType;

        [SerializeField]
        TextMeshProUGUI _logString;

        [SerializeField]
        LongTapButton _selfButton;

        [SerializeField]
        GameObject _logCounterRoot;

        [SerializeField]
        TextMeshProUGUI _logCounter;

        [SerializeField]
        Image _backGround;

        LogPanelCommonDisplayProcessing _displayProcessing;

        int _serialNumber;

        UnityAction<int> _onPointerDown;
        UnityAction _onPointerUp;
        UnityAction _onPointerClick;
        UnityAction _onLongTap;

        void _OnValidateUI()
        {
            Assert.IsNotNull(_select);
            Assert.IsNotNull(_logType);
            Assert.IsNotNull(_logString);
            Assert.IsNotNull(_selfButton);
            Assert.IsNotNull(_backGround);
        }

        void Awake()
        {
            _OnValidateUI();

            _selfButton._onPointerDown.RemoveAllListeners();
            _selfButton._onPointerDown.AddListener(_OnPointerDown);
            _selfButton._onPointerUp.RemoveAllListeners();
            _selfButton._onPointerUp.AddListener(_OnPointerUp);
            _selfButton._onPointerClick.RemoveAllListeners();
            _selfButton._onPointerClick.AddListener(_OnPointerClick);
            _selfButton._onLongTap.RemoveAllListeners();
            _selfButton._onLongTap.AddListener(_OnLongTap);
        }

        public void Draw(LogViewLinker.LogPanelInfo logInfo, bool isSelect)
        {
            _displayProcessing ??= new LogPanelCommonDisplayProcessing(_logType, _logString, _logCounterRoot, _logCounter);

            _serialNumber = logInfo._serialNumber;

            _select.SetActive(isSelect);

            if (logInfo._viewIndex % 2 == 0)
            {
                _backGround.color = NoaDebuggerDefine.BackgroundColors.LogBright;
            }
            else
            {
                _backGround.color = NoaDebuggerDefine.BackgroundColors.LogDark;
            }

            _displayProcessing.RefreshLogType(logInfo);
            _displayProcessing.RefreshLogString(logInfo);
            _displayProcessing.RefreshLogCounter(logInfo);

            _onPointerDown = logInfo._onPointerDown;
            _onPointerUp = logInfo._onPointerUp;
            _onPointerClick = logInfo._onPointerClick;
            _onLongTap = logInfo._onLongTap;
        }

        void _OnPointerDown()
        {
            _onPointerDown?.Invoke(_serialNumber);
        }

        void _OnPointerUp()
        {
            _onPointerUp?.Invoke();
        }

        void _OnPointerClick()
        {
            _onPointerClick?.Invoke();
        }

        void _OnLongTap()
        {
            _onLongTap?.Invoke();
        }

        void OnDestroy()
        {
            _select = default;
            _logType = default;
            _logString = default;
            _selfButton = default;
            _backGround = default;
            _onPointerDown = default;
            _onPointerUp = default;
            _onPointerClick = default;
            _onLongTap = default;
            _logCounterRoot = default;
            _logCounter = default;
            _displayProcessing = default;
        }
    }
}
