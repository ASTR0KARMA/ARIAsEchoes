using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NoaDebugger
{
    class LogViewBase : NoaDebuggerToolViewBase<LogViewLinker>
    {
        [SerializeField, Header("Log status")]
        GameObject _logStatusRoot;
        [SerializeField]
        TextMeshProUGUI _logStatus;

        [SerializeField, Header("Log details display route")]
        GameObject _logDetailScroll;

        [SerializeField, Header("Button")]
        ToggleButtonBase _record;
        [SerializeField]
        DisableButtonBase _clear;
        [SerializeField]
        ToggleColorButton _selection;
        [SerializeField]
        TextMeshProUGUI _selectionCount;
        [SerializeField]
        ToggleColorButton _search;

        [SerializeField]
        public LogScrollDrawer _logScrollDrawer;

        [SerializeField]
        public LogSwitchToggleDrawer _logSwitchToggleDrawer;

        [SerializeField, Header("Footer")]
        GameObject _footer;
        [SerializeField]
        Button _selectAll;
        [SerializeField]
        Button _deselectAll;
        [SerializeField]
        DisableButtonBase _download;
        [SerializeField]
        DisableButtonBase _send;


        public event UnityAction OnRecord;

        public event UnityAction OnClear;

        public event UnityAction<bool> OnSelectingMode;

        public event UnityAction OnSelectAll;

        public event UnityAction OnDeselectAll;

        public event UnityAction OnDownload;

        public event UnityAction OnSend;

        public UnityAction OnCopy { get; set; }


        void _OnValidateUI()
        {
            Assert.IsNotNull(_logStatusRoot);
            Assert.IsNotNull(_logStatus);
            Assert.IsNotNull(_logDetailScroll);
            Assert.IsNotNull(_record);
            Assert.IsNotNull(_clear);
            Assert.IsNotNull(_selection);
            Assert.IsNotNull(_selectionCount);
            Assert.IsNotNull(_search);
            Assert.IsNotNull(_footer);
            Assert.IsNotNull(_selectAll);
            Assert.IsNotNull(_deselectAll);
            Assert.IsNotNull(_download);
            Assert.IsNotNull(_send);
        }

        protected override void _Init()
        {
            _OnValidateUI();
            _record._onClick.RemoveAllListeners();
            _record._onClick.AddListener(_OnRecord);
            _clear._onClick.RemoveAllListeners();
            _clear._onClick.AddListener(_OnClear);
            _selection._onClick.RemoveAllListeners();
            _selection._onClick.AddListener(_OnSelection);
            _search._onClick.RemoveAllListeners();
            _search._onClick.AddListener(_OnSearch);
            _selectAll.onClick.RemoveAllListeners();
            _selectAll.onClick.AddListener(_OnSelectAll);
            _deselectAll.onClick.RemoveAllListeners();
            _deselectAll.onClick.AddListener(_OnDeselectAll);
            _download._onClick.RemoveAllListeners();
            _download._onClick.AddListener(_OnDownload);
            _send._onClick.RemoveAllListeners();
            _send._onClick.AddListener(_OnSend);

            _logScrollDrawer.SwitchFilterContent(false);
            _footer.SetActive(false);
            _OnInit();
        }

        protected virtual void _OnInit() { }

        protected virtual void _OnUpdateDetail(ILogDetail detail) { }

        protected override void _OnShow(LogViewLinker linker)
        {
            var logs = linker._logs;
            bool logExists = logs is {Count: > 0};

            if (logExists)
            {
                var selectLog = logs.FirstOrDefault(x => x._serialNumber == linker._selectLogSerialNumber);

                _RefreshLogDetailView(linker._isViewLogDetail);

                if (selectLog != null)
                {
                    _OnUpdateDetail(selectLog._logDetail);
                }
            }

            _logScrollDrawer.Draw(linker, isFloatingWindow:false);

            if (string.IsNullOrEmpty(linker._logStatus))
            {
                _logStatusRoot.SetActive(false);
            }
            else
            {
                _logStatusRoot.SetActive(true);
                _logStatus.text = linker._logStatus;
            }

            _record.Init(linker._isCollecting);

            _selectionCount.text = linker._selectionCount.ToString();

            int logSum = linker._logTypeToggles._messageNum
                         + linker._logTypeToggles._warningNum
                         + linker._logTypeToggles._errorNum;

            bool hasLog = logSum > 0;
            _clear.Interactable = hasLog;

            bool hasSelectLog = linker._selectionCount > 0;

            _download.Interactable = hasSelectLog && FileDownloader.CanDownload();
            _send.Interactable = linker._isValidSend && hasSelectLog;

            _logSwitchToggleDrawer.Draw(linker._logTypeToggles);
        }

        void _RefreshLogDetailView(bool isView)
        {
            _logDetailScroll.gameObject.SetActive(isView);
        }

        public override void AlignmentUI(bool isReverse)
        {
            base.AlignmentUI(isReverse);

            int footerIndex = isReverse ? 1 : _footer.transform.parent.childCount;
            _footer.transform.SetSiblingIndex(footerIndex);
        }


        void _OnRecord(bool isOn)
        {
            OnRecord?.Invoke();
        }

        void _OnClear()
        {
            OnClear?.Invoke();
        }

        void _OnSelection(bool isOn)
        {
            OnSelectingMode?.Invoke(isOn);
            _footer.SetActive(isOn);
        }

        void _OnSearch(bool isOn)
        {
            _logScrollDrawer.SwitchFilterContent(isOn);
        }

        void _OnSelectAll()
        {
            OnSelectAll();
        }

        void _OnDeselectAll()
        {
            OnDeselectAll();
        }

        void _OnDownload()
        {
            OnDownload?.Invoke();
        }

        void _OnSend()
        {
            OnSend?.Invoke();
        }

        void OnDestroy()
        {
            _logStatusRoot = default;
            _logStatus = default;
            _logDetailScroll = default;
            _record = default;
            _clear = default;
            _selection = default;
            _selectionCount = default;
            _search = default;
            _footer = default;
            _selectAll = default;
            _deselectAll = default;
            _download = default;
            _send = default;
            _logScrollDrawer = default;
            _logSwitchToggleDrawer = default;
            OnRecord = default;
            OnClear = default;
            OnDownload = default;
            OnCopy = default;
        }
    }

    sealed class LogViewLinker : ViewLinkerBase
    {
        public bool _forceUpdate;

        public List<LogPanelInfo> _logs;

        public string _logStatus;

        public int _selectLogSerialNumber;

        public bool _isCollecting;

        public int _selectionCount;

        public bool _isSelectingMode;

        public LogTypeToggles _logTypeToggles;

        public bool? _resetScrollPos;

        public bool _isViewLogDetail;

        public bool _isValidSend;

        public struct LogTypeToggles
        {
            public bool _messageToggle;

            public bool _warningToggle;

            public bool _errorToggle;

            public int _messageNum;

            public int _warningNum;

            public int _errorNum;
        }

        public class LogPanelInfo
        {
            public int _viewIndex { private set; get; }

            public bool _isSelect { private set; get; }

            public bool _isPicOut { private set; get; }

            public bool _hasLoggedOnce { private set; get; }

            public string _logString { private set; get; }

            public string _stackTraceString { private set; get; }

            public ILogDetail _logDetail { private set; get; }

            public LogType _logType { private set; get; }

            public DateTime _receivedTime { private set; get; }

            public UnityAction<int> _onPointerDown;

            public UnityAction _onPointerUp;

            public UnityAction _onPointerClick;

            public UnityAction _onLongTap;

            public int _numberOfMatchingLogs { private set; get; }

            public int _serialNumber { private set; get; }

            public void UpdateLogData(string logString, string stackTrace, ILogDetail logDetail, LogType logType, DateTime receivedAt, int serialNumber)
            {
                _logString = logString;
                _stackTraceString = stackTrace;
                _logDetail = logDetail;
                _logType = logType;
                _receivedTime = receivedAt;

                _serialNumber = serialNumber;
            }

            public void UpdateViewData(int viewIndex, bool isSelect, bool isPicOut, bool hasLoggedOnce, int numberOfMatchingLogs)
            {
                _viewIndex = viewIndex;
                _isSelect = isSelect;
                _isPicOut = isPicOut;
                _hasLoggedOnce = hasLoggedOnce;
                _numberOfMatchingLogs = numberOfMatchingLogs;
            }
        }
    }
}
