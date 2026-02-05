using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace NoaDebugger
{
    abstract class LogPresenterBase<TLogEntry, TLogDetail> : NoaDebuggerToolBase
        where TLogEntry : LogEntry<TLogDetail>
        where TLogDetail : ILogDetail
    {
        class LogExportData : IExportData
        {
            readonly DownloadInfo _downloadData;

            public LogExportData(string exportFilenamePrefix)
            {
                _downloadData = new DownloadInfo(exportFilenamePrefix);
            }

            public DownloadInfo GetDownloadInfo() => _downloadData;
        }

        protected LinkedList<TLogEntry> _logBuffer;

        [Header("MainView")]
        [SerializeField]
        LogViewBase _mainViewPrefab;
        protected LogViewBase _mainView;

        [Header("Overlay")]
        [SerializeField]
        protected LogOverlayView _overlayPrefab;
        [SerializeField]
        protected LogOverlaySettingsView _overlaySettingsPrefab;

        protected LogOverlayPresenter _overlayPresenter;

        [SerializeField, Header("DownloadDialog")]
        DownloadDialog _dialogPrefab;
        DownloadDialogPresenter _downloadDialogPresenter;

        protected NoaDebuggerSettings _settings;

        Dictionary<LogType, int> _logCounts;
        int _selectLogSerialNumber = -1;

        List<LogViewLinker.LogPanelInfo> _panels;
        List<LogViewLinker.LogPanelInfo> _overlayPanels;
        LogViewLinker.LogPanelInfo[] _panelInstances;

        UnityAction<int> _onPointerDown = null;
        UnityAction _onPointerUp = null;
        UnityAction _onPointerClick = null;
        UnityAction _onLongTap = null;

        int _logSerialNumberOnDown;

        Dictionary<LogType, bool> _showLogFlagDictionary;
        string _filterText;
        LogFilterRepository _filterRepository;

        LogCollectorModel _logModel;

        bool _scrollInitLock;

        bool? _resetScroll;

        bool _keepScrollPosition;

        bool _hideLogDetailView;

        bool _isShow;

        bool IsSelectingMode
        {
            get => _isSelectingModeValue && _isShow;
            set => _isSelectingModeValue = value;
        }
        bool _isSelectingModeValue;

        protected abstract INoaDownloadCallbacks DownloadCallbacks { get; }

        protected abstract LogCollectorModel CreateLogCollectorModel();

        protected abstract int GetLogCapacity();

        protected abstract void SetLogPanelInfo(ref LogViewLinker.LogPanelInfo logPanelInfo, TLogEntry log);

        protected abstract string GetStatusString();

        protected abstract void OnClearLog();

        protected abstract void OnLogCopied(TLogEntry log, string clipboardText);

        protected abstract bool? OnLogDownload(string fileName, string json);

        protected abstract string GetExportFilenamePrefix();

        protected abstract void OnLogSend();

        protected abstract bool IsRegisteredSend();

        protected abstract string GetFilterSavePrefsKey();

        protected abstract string GetOverlayPrefsKey();

        protected abstract KeyValueSerializer CreateLogKeyValueSerializer();

        protected abstract LogOverlayBaseRuntimeSettings _GetOverlaySettings();

        protected virtual void _Init()
        {
            _settings = NoaDebuggerSettingsManager.GetNoaDebuggerSettings();
            _logBuffer = new LinkedList<TLogEntry>();
            _logModel = CreateLogCollectorModel();

            _showLogFlagDictionary = new Dictionary<LogType, bool>();
            _showLogFlagDictionary.Add(LogType.Log, true);
            _showLogFlagDictionary.Add(LogType.Warning, true);
            _showLogFlagDictionary.Add(LogType.Error, true);
            _filterRepository = new LogFilterRepository(GetFilterSavePrefsKey());
            _showLogFlagDictionary = _filterRepository.LoadFilter(_showLogFlagDictionary);

            _logCounts = new Dictionary<LogType, int>();
            _logCounts.Add(LogType.Log, 0);
            _logCounts.Add(LogType.Warning, 0);
            _logCounts.Add(LogType.Error, 0);

            _overlayPresenter.OnUpdateSettings += _OnUpdateOverlaySettings;
            _overlayPresenter.OnInitAction += _OnInitOverlay;

            _hideLogDetailView = true;

            _ResetBuffer();

            _onPointerDown = _OnPointerDownLog;
            _onPointerUp = _OnPointerUpLog;
            _onPointerClick = _OnPointerClickLog;
            _onLongTap = _OnLongTapLog;

            SettingsEventModel.OnUpdateLogSettings -= _OnUpdateLogSettings;
            SettingsEventModel.OnUpdateLogSettings += _OnUpdateLogSettings;
        }

        void _ResetBuffer()
        {
            int logCapacity = GetLogCapacity() * Enum.GetValues(typeof(LogType)).Length;
            _panels = new List<LogViewLinker.LogPanelInfo>(logCapacity);
            _overlayPanels = new List<LogViewLinker.LogPanelInfo>(logCapacity);
            _panelInstances = new LogViewLinker.LogPanelInfo[logCapacity];
            for (int i = 0; i < _panelInstances.Length; ++i)
            {
                _panelInstances[i] = new LogViewLinker.LogPanelInfo();
            }

            _ResetLogBuffer();
        }

        protected abstract void _ResetLogBuffer();


        protected void _UpdateView() => _DoUpdateView();

        protected void _ShowView(Transform parent)
        {
            if (_mainView == null)
            {
                _mainView = GameObject.Instantiate(_mainViewPrefab, parent);
                _InitView(_mainView);
            }

            _isShow = true;
            _DoUpdateView();

            if (!_overlayPresenter.IsOverlaySettingsEnable)
            {
                _mainView.gameObject.SetActive(true);
            }
        }

        void _InitView(LogViewBase view)
        {
            view.OnRecord += _OnRecord;
            view.OnClear += _OnClearLogs;
            view.OnSelectingMode += OnSelectingMode;
            view.OnSelectAll += _OnSelectAll;
            view.OnDeselectAll += _OnDeselectAll;
            view.OnDownload += _OnDownload;
            view.OnSend += _OnLogSend;
            view.OnCopy = _CopySelectedLog;
            view._logSwitchToggleDrawer.OnSwitchByLogType += _OnSwitchByType;
            view._logScrollDrawer.OnChangeFilterText += _OnChangeFilterText;
        }

        void _HideView()
        {
            if (_mainView == null)
            {
                return;
            }

            _isShow = false;
            _mainView.gameObject.SetActive(false);
        }

        void _DoUpdateView()
        {
            _CreateLogViewInfos(out int selectLogSerialNumber, out int selectingCount);
            var linker = new LogViewLinker()
            {
                _isCollecting = _logModel.IsCollecting,
                _logs = _panels,
                _logStatus = GetStatusString(),
                _selectLogSerialNumber = selectLogSerialNumber,
                _selectionCount = selectingCount,
                _isSelectingMode = IsSelectingMode,
                _logTypeToggles = new LogViewLinker.LogTypeToggles()
                {
                    _messageToggle = _showLogFlagDictionary[LogType.Log],
                    _warningToggle = _showLogFlagDictionary[LogType.Warning],
                    _errorToggle = _showLogFlagDictionary[LogType.Error],
                    _messageNum = _logCounts[LogType.Log],
                    _warningNum = _logCounts[LogType.Warning],
                    _errorNum = _logCounts[LogType.Error],
                },
                _forceUpdate = !_scrollInitLock,
                _resetScrollPos = _resetScroll,
                _isViewLogDetail = !_hideLogDetailView,
                _isValidSend = IsRegisteredSend(),
            };
            if (_isShow)
            {
                _mainView.Show(linker);
                _SetNotificationStatus(ToolNotificationStatus.None);
            }

            if (_overlayPresenter.IsShowOverlay)
            {
                _overlayPresenter.UpdateOverlayView(_overlayPanels, linker._logTypeToggles);
                if (!NoaDebuggerVisibilityManager.IsMainViewActive)
                {
                    _SetNotificationStatus(ToolNotificationStatus.None);
                }
            }

            if (!_keepScrollPosition && _resetScroll != null)
            {
                _resetScroll = null;
            }

            _scrollInitLock = false;
        }

        protected abstract void _SetNotificationStatus(ToolNotificationStatus notifyStatus);

        protected abstract bool _GetOverlayShowErrorLogsFromNoaDebuggerSettings();

        protected abstract bool _GetOverlayShowMessageLogsFromNoaDebuggerSettings();

        protected abstract bool _GetOverlayShowMessageWarningLogsFromNoaDebuggerSettings();

        void _CreateLogViewInfos(out int selectLogSerialNumber, out int selectingCount)
        {
            selectLogSerialNumber = -1;
            selectingCount = 0;
            _logCounts[LogType.Log] = 0;
            _logCounts[LogType.Warning] = 0;
            _logCounts[LogType.Error] = 0;
            _panels.Clear();
            _overlayPanels.Clear();

            var showLogOverlayFlagDictionary = new Dictionary<LogType, bool>()
            {
                {LogType.Log, _GetOverlayShowMessageLogsFromNoaDebuggerSettings()},
                {LogType.Warning, _GetOverlayShowMessageWarningLogsFromNoaDebuggerSettings()},
                {LogType.Error, _GetOverlayShowErrorLogsFromNoaDebuggerSettings()},
            };

            var logIndex = 0;
            var viewIndex = 0;
            foreach (TLogEntry log in _logBuffer)
            {
                int index = logIndex++;
                selectingCount += log.IsSelecting ? 1 : 0;

                _logCounts[log.LogType]++;

                if (string.IsNullOrEmpty(_filterText) == false && log.LogString.Contains(_filterText) == false)
                {
                    continue;
                }

                bool isShowMain = _showLogFlagDictionary[log.LogType];
                bool isShowOverlay = showLogOverlayFlagDictionary[log.LogType];
                if (showLogOverlayFlagDictionary.All(kv => kv.Value == false))
                {
                    isShowOverlay = isShowMain;
                }

                if (isShowMain == false && isShowOverlay == false)
                {
                    continue;
                }

                bool isSelect = log._serialNumber == _selectLogSerialNumber;
                LogViewLinker.LogPanelInfo panel = _panelInstances[index];
                SetLogPanelInfo(ref panel, log);
                panel.UpdateViewData(viewIndex: viewIndex++,
                                     isSelect: isSelect,
                                     isPicOut: log.IsSelecting,
                                     hasLoggedOnce: log.HasLoggedOnce,
                                     numberOfMatchingLogs:
                                     log.NumberOfMatchingLogs);
                panel._onPointerDown = _onPointerDown;
                panel._onPointerUp = _onPointerUp;
                panel._onPointerClick = _onPointerClick;
                panel._onLongTap = _onLongTap;

                if (isShowMain)
                {
                    _panels.Add(panel);
                }
                if (isShowOverlay)
                {
                    _overlayPanels.Add(panel);
                }

                log.HasLoggedOnce = true;

                if (isSelect)
                {
                    selectLogSerialNumber = log._serialNumber;
                }
            }

            int maximumLogCount = _overlayPresenter._overlayToolSettings.MaximumLogCount.Value;
            _overlayPanels = _overlayPanels.TakeLast(maximumLogCount).ToList();
        }

        protected void _AlignmentUI(bool isReverse)
        {
            _mainView.AlignmentUI(isReverse);
            _overlayPresenter.AlignmentUI(isReverse);
        }

        protected void _OnReceivedLog()
        {
            if (_logBuffer.All(logEntry => logEntry._serialNumber != _selectLogSerialNumber))
            {
                _hideLogDetailView = true;
            }
        }


        void _OnRecord()
        {
            _logModel.ToggleCollect(!_logModel.IsCollecting);
            _DoUpdateView();
        }

        void _OnClearLogs()
        {
            _logBuffer.Clear();
            _selectLogSerialNumber = -1;
            _resetScroll = null;
            _keepScrollPosition = false;
            OnClearLog();
            _DoUpdateView();

            _SetNotificationStatus(ToolNotificationStatus.None);
        }

        void OnSelectingMode(bool isSelecting)
        {
            IsSelectingMode = isSelecting;
            _hideLogDetailView = true;
            _DoUpdateView();
        }

        void _OnSelectAll()
        {
            foreach (var log in _logBuffer)
            {
                log.IsSelecting = true;
            }

            _DoUpdateView();
        }

        void _OnDeselectAll()
        {
            foreach (var log in _logBuffer)
            {
                log.IsSelecting = false;
            }

            _DoUpdateView();
        }

        void _OnDownload()
        {
            if (_downloadDialogPresenter == null)
            {
                _downloadDialogPresenter = new DownloadDialogPresenter(_dialogPrefab);
                _downloadDialogPresenter.OnExecDownload += _OnExecDownload;
            }

            _downloadDialogPresenter.ShowDialog();
        }

        void _OnLogSend()
        {
            if (IsRegisteredSend())
            {
                OnLogSend();
                NoaDebugger.ShowToast(new ToastViewLinker()
                {
                    _label = NoaDebuggerDefine.LogSentText,
                });
            }
        }

        void _OnSwitchByType(LogType logType, bool isOn)
        {
            _showLogFlagDictionary[logType] = isOn;
            _filterRepository.SaveFilter(_showLogFlagDictionary);

            _DoUpdateView();
        }

        void _OnChangeFilterText(string text)
        {
            _filterText = text;
            _resetScroll = true;
            _keepScrollPosition = false;
            _DoUpdateView();
        }


        void _OnPointerDownLog(int serialNumber)
        {
            _logSerialNumberOnDown = serialNumber;

            _resetScroll = false;
            _keepScrollPosition = true;
            _DoUpdateView();
        }

        void _OnPointerUpLog()
        {
            _keepScrollPosition = false;
        }

        void _OnPointerClickLog()
        {
            if (IsSelectingMode)
            {
                var targetLog = _GetLog(_logSerialNumberOnDown);
                targetLog.IsSelecting = !targetLog.IsSelecting;
            }
            else
            {
                if (_hideLogDetailView && (_selectLogSerialNumber == _logSerialNumberOnDown))
                {
                    _selectLogSerialNumber = -1;
                }

                _hideLogDetailView = _selectLogSerialNumber == _logSerialNumberOnDown;
                _scrollInitLock = true;
                _selectLogSerialNumber = _logSerialNumberOnDown;
            }

            _DoUpdateView();
        }

        void _OnLongTapLog()
        {
            if(UserAgentModel.IsWebGLandiOSorMacSafari)
            {
                return;
            }

            _CopyLog(_logSerialNumberOnDown);
        }

        void _CopySelectedLog()
        {
            _CopyLog(_selectLogSerialNumber);
        }

        void _CopyLog(int serialNumber)
        {
            TLogEntry logEntry = _GetLog(serialNumber);
            if (logEntry == null)
            {
                return;
            }

            string clipboardText = logEntry.LogDetail.GetClipboardText();
            ClipboardModel.Copy(clipboardText);
            OnLogCopied(logEntry, clipboardText);
            NoaDebugger.ShowToast(new ToastViewLinker {_label = NoaDebuggerDefine.ClipboardCopiedText});
            _scrollInitLock = true;
            _DoUpdateView();
        }

        TLogEntry _GetLog(int serialNumber)
        {
            return _logBuffer.FirstOrDefault(x => x._serialNumber == serialNumber);
        }


        void _OnExecDownload(string label, Action<FileDownloader.DownloadExecutedInfo> completed)
        {
            DownloadInfo downloadInfo = new LogExportData(GetExportFilenamePrefix()).GetDownloadInfo();
            string fileName = downloadInfo.GetExportFileName(label, DateTime.Now);
            string json = _CreateExportJsonString(label);

            if (DownloadCallbacks != null)
            {
                FileDownloader.DownloadFileWithUserCallbacksClass(
                    fileName,
                    json,
                    completed,
                    _downloadDialogPresenter.HideDialog,
                    DownloadCallbacks);

                return;
            }

            bool? isAllowBaseDownload = OnLogDownload(fileName, json);
            if (isAllowBaseDownload is false)
            {
                _downloadDialogPresenter.HideDialog();
                return;
            }
            FileDownloader.DownloadFile(fileName, json, null, completed);
        }

        string _CreateExportJsonString(string label)
        {
            List<KeyValueSerializer> exportData = new List<KeyValueSerializer>();
            KeyValueSerializer logData = CreateLogKeyValueSerializer();
            exportData.Add(logData);

            exportData.Add(KeyValueSerializer.CreateSubData(label));
            return KeyValueSerializer.SerializeToJson(exportData.ToArray());
        }


        protected abstract LogOverlaySettingsDefaultGetter _CreateOverlaySettingsDefaultGetter();

        protected bool _GetPinStatus()
        {
            return _overlayPresenter.IsOverlayEnable;
        }

        protected void _TogglePin(Transform parent)
        {
            _overlayPresenter.ToggleActiveOverlayView(parent);
        }

        void _OnUpdateOverlaySettings()
        {
            _DoUpdateView();
        }

        void _OnInitOverlay(LogOverlayView overlay)
        {
            overlay.OnEnabledAction += _OnOverlayEnabled;
            _DoUpdateView();
        }

        void _OnOverlayEnabled()
        {
            _DoUpdateView();
        }


        protected void _OnHidden()
        {
            _HideView();

            if (_overlayPresenter != null)
            {
                _overlayPresenter.DestroyOverlaySettings();
            }

            _selectLogSerialNumber = -1;
            _hideLogDetailView = true;
            _downloadDialogPresenter?.Dispose();
        }


        protected void _OnDispose()
        {
            _OnHidden();
            _logModel?.Destroy();
        }


        public LinkedList<TLogEntry> GetLogList()
        {
            return _logBuffer;
        }

        public void ClearLog()
        {
            _OnClearLogs();
        }


        void _OnUpdateLogSettings()
        {
            _ResetBuffer();
            ClearLog();
        }

        protected virtual void OnDestroy()
        {
            _logBuffer = default;
            _mainViewPrefab = default;
            _mainView = default;
            _overlayPrefab = default;
            _overlaySettingsPrefab = default;
            _overlayPresenter = default;
            _dialogPrefab = default;
            _downloadDialogPresenter = default;
            _settings = default;
            _logCounts = default;
            _panels = default;
            _overlayPanels = default;
            _panelInstances = default;
            _onPointerDown = default;
            _onPointerUp = default;
            _onPointerClick = default;
            _onLongTap = default;
            _showLogFlagDictionary = default;
            _filterText = default;
            _logModel = default;
            SettingsEventModel.OnUpdateLogSettings -= _OnUpdateLogSettings;

        }
    }

    abstract class LogOverlayPresenter : OverlayPresenterBase
        <LogOverlayView, LogOverlaySettingsView, LogOverlayBaseRuntimeSettings,LogOverlayViewLinker>
    {
        public LogOverlayPresenter(LogOverlayView overlayPrefab,
                                   LogOverlaySettingsView overlaySettingsPrefab,
                                   string prefsKeyPrefix)
            : base(overlayPrefab, overlaySettingsPrefab, prefsKeyPrefix) { }

        List<LogViewLinker.LogPanelInfo> _overlayPanels;
        LogViewLinker.LogTypeToggles _logTypeToggles;

        protected override ViewLinkerBase _CreateOverlayViewLinker()
        {
            return new LogOverlayViewLinker()
            {
                _position = _GetPositionFromNoaDebuggerSettings(),
                _minimumOpacity = _GetMinimumOpacityFromNoaDebuggerSettings(),
                _fontScale = _GetFontScaleFromNoaDebuggerSettings(),
                _maximumLogCount = _GetMaximumLogCountFromNoaDebuggerSettings(),
                _showTimestamp = _GetShowTimestampFromNoaDebuggerSettings(),
                _activeDulation = _GetActiveDurationFromNoaDebuggerSettings(),
                _logs = _overlayPanels,
                _logTypeToggles = _logTypeToggles,
            };
        }

        NoaDebug.OverlayPosition _GetPositionFromNoaDebuggerSettings()
        {
            NoaDebug.OverlayPosition position = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_POSITION;

            if(_overlayToolSettings is ApiLogOverlayRuntimeSettings)
            {
                position = _settings.ApiLogOverlayPosition;
            }
            else if (_overlayToolSettings is ConsoleLogOverlayRuntimeSettings)
            {
                position = _settings.ConsoleLogOverlayPosition;
            }

            return position;
        }

        float _GetMinimumOpacityFromNoaDebuggerSettings()
        {
            float opacity = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_MINIMUM_OPACITY;

            if(_overlayToolSettings is ApiLogOverlayRuntimeSettings)
            {
                opacity = _settings.ApiLogOverlayMinimumOpacity;
            }
            else if (_overlayToolSettings is ConsoleLogOverlayRuntimeSettings)
            {
                opacity = _settings.ConsoleLogOverlayMinimumOpacity;
            }

            return opacity;
        }

        float _GetFontScaleFromNoaDebuggerSettings()
        {
            float scale = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_FONT_SCALE;

            if(_overlayToolSettings is ApiLogOverlayRuntimeSettings)
            {
                scale = _settings.ApiLogOverlayFontScale;
            }
            else if (_overlayToolSettings is ConsoleLogOverlayRuntimeSettings)
            {
                scale = _settings.ConsoleLogOverlayFontScale;
            }

            return scale;
        }

        int _GetMaximumLogCountFromNoaDebuggerSettings()
        {
            int count = 10;
            if(_overlayToolSettings is ApiLogOverlayRuntimeSettings)
            {
                count = _settings.ApiLogOverlayMaximumLogCount;
            }
            else if (_overlayToolSettings is ConsoleLogOverlayRuntimeSettings)
            {
                count = _settings.ConsoleLogOverlayMaximumLogCount;
            }

            return count;
        }

        bool _GetShowTimestampFromNoaDebuggerSettings()
        {
            bool isShow = false;
            if(_overlayToolSettings is ApiLogOverlayRuntimeSettings)
            {
                isShow = _settings.ApiLogOverlayShowTimestamp;
            }
            else if (_overlayToolSettings is ConsoleLogOverlayRuntimeSettings)
            {
                isShow = _settings.ConsoleLogOverlayShowTimestamp;
            }

            return isShow;
        }

        float _GetActiveDurationFromNoaDebuggerSettings()
        {
            float duration = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_ACTIVE_DURATION;
            if(_overlayToolSettings is ApiLogOverlayRuntimeSettings)
            {
                duration = _settings.ApiLogOverlayActiveDuration;
            }
            else if (_overlayToolSettings is ConsoleLogOverlayRuntimeSettings)
            {
                duration = _settings.ConsoleLogOverlayActiveDuration;
            }

            return duration;
        }

        public void UpdateOverlayView(List<LogViewLinker.LogPanelInfo> panels, LogViewLinker.LogTypeToggles toggles)
        {
            _overlayPanels = panels;
            _logTypeToggles = toggles;
            UpdateOverlayView();
        }
    }
}
