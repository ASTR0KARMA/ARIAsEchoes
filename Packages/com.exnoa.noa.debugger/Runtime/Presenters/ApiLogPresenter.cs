using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace NoaDebugger
{
    sealed class ApiLogPresenter : LogPresenterBase<ApiLogEntry, ApiLog>,
                                   INoaDebuggerTool,
                                   INoaDebuggerOverlayTool
    {
        const string DETAIL_HEADER_PREFIX = "[";
        const string DETAIL_HEADER_SUFFIX = "]";

        public const string DETAIL_HEADER_GENERAL = "General";
        public const string DETAIL_CONTENT_URL = "URL";
        public const string DETAIL_CONTENT_REQUEST_METHOD = "Request Method";
        public const string DETAIL_CONTENT_STATUS_CODE = "Status Code";
        public const string DETAIL_HEADER_REQUEST_HEADERS = "Request Headers";
        public const string DETAIL_HEADER_REQUEST_BODY = "Request Body";
        public const string DETAIL_HEADER_RESPONSE_HEADERS = "Response Headers";
        public const string DETAIL_HEADER_RESPONSE_BODY = "Response Body";

        int _requestCount = 0;
        long _totalReceivedBytes = 0;
        long _totalResponseTimeMilliSeconds = 0;
        ToolNotificationStatus _notifyStatus;

        RingBuffer<ApiLogEntry> _errorLogBuffer;

        RingBuffer<ApiLogEntry> _normalLogBuffer;

        public ToolNotificationStatus NotifyStatus => _notifyStatus;

        public UnityAction<ApiLogEntry> _onErrorLogReceived;

        public UnityAction<ApiLogEntry, string> _onLogCopied;

        public Func<string, string, bool> _onLogDownload;

        public Func<string, List<ApiLogEntry>, bool> _onLogDownloadWithLogEntries;

        public UnityAction<List<ApiLogEntry>> _onLogSend;

        int _logSerialNumberCounter = 0;

        protected override INoaDownloadCallbacks DownloadCallbacks => NoaApiLog.CommonDownloadCallbacks;

        protected override void _Init()
        {
            _overlayPresenter = new ApiLogOverlayPresenter(
                _overlayPrefab, _overlaySettingsPrefab, prefsKeyPrefix:GetOverlayPrefsKey());
            base._Init();
        }

        protected override LogCollectorModel CreateLogCollectorModel()
        {
            var model = new ApiLogCollectorModel();
            model.OnLogReceived += _ReceiveLog;
            return model;
        }

        protected override int GetLogCapacity()
        {
            var capacity = _settings ? _settings.ApiLogCount : NoaDebuggerDefine.DEFAULT_API_LOG_COUNT;

            if (capacity < NoaDebuggerDefine.ApiLogCountMin || capacity > NoaDebuggerDefine.ApiLogCountMax)
            {
                capacity = NoaDebuggerDefine.DEFAULT_API_LOG_COUNT;
            }

            return capacity;
        }

        protected override void SetLogPanelInfo(ref LogViewLinker.LogPanelInfo logPanelInfo, ApiLogEntry log)
        {
            logPanelInfo.UpdateLogData(logString: log.LogViewString,
                                       stackTrace: "",
                                       logDetail: log.LogDetail,
                                       logType: log.LogType,
                                       receivedAt: log.ReceivedAt,
                                       log._serialNumber);
        }

        protected override string GetStatusString()
        {
            return $"Request: {_requestCount} | Size: {DataUnitConverterModel.ToHumanReadableBytes(_totalReceivedBytes)} | Time: {_totalResponseTimeMilliSeconds} ms";
        }

        protected override void OnClearLog()
        {
            _requestCount = 0;
            _totalReceivedBytes = 0;
            _totalResponseTimeMilliSeconds = 0;
            _normalLogBuffer.Clear();
            _errorLogBuffer.Clear();
        }

        protected override void OnLogCopied(ApiLogEntry log, string clipboardText)
        {
            _onLogCopied?.Invoke(log, clipboardText);
        }

        protected override bool? OnLogDownload(string fileName, string json)
        {
            if (_onLogDownload != null)
            {
                return _onLogDownload.Invoke(fileName, json);
            }

            if (_onLogDownloadWithLogEntries != null)
            {
                var targetLogs = _logBuffer.Where(log => log.IsSelecting).ToList();
                return _onLogDownloadWithLogEntries.Invoke(fileName, targetLogs);
            }

            return null;
        }

        protected override string GetExportFilenamePrefix() => "apilog";

        protected override void OnLogSend()
        {
            var targetLogs = _logBuffer.Where(log => log.IsSelecting).ToList();
            _onLogSend?.Invoke(targetLogs);
        }

        protected override bool IsRegisteredSend()
        {
            return _onLogSend != null;
        }

        protected override string GetFilterSavePrefsKey() => NoaDebuggerPrefsDefine.PrefsKeyIsApiLogFilterFlags;

        protected override LogOverlaySettingsDefaultGetter _CreateOverlaySettingsDefaultGetter() => new ApiLogOverlaySettingsDefaultGetter();

        protected override string GetOverlayPrefsKey() => NoaDebuggerPrefsDefine.PrefsKeyPrefixApiLogOverlaySettings;

        protected override KeyValueSerializer CreateLogKeyValueSerializer()
        {
            var logCount = new Dictionary<LogType, int>
            {
                {LogType.Log, 0},
                {LogType.Error, 0}
            };

            var objectParsers = new List<KeyValueArrayParser.ObjectParser>(_logBuffer.Count);
            long totalContentBytes = 0;
            long totalResponseTime = 0;

            var selectedLogs = _logBuffer.Where(log => log.IsSelecting).ToList();
            IEnumerable<ApiLogEntry> targetLogs;
            if (NoaApiLog.ApiLogDownloadCallbacks != null)
            {
                targetLogs = NoaApiLog.ApiLogDownloadCallbacks.OnBeforeConversion(selectedLogs);
            }
            else
            {
                targetLogs = selectedLogs;
            }

            foreach (ApiLogEntry log in targetLogs)
            {
                var logElementParsers = new List<IKeyValueParser>();

                string[] headerElements = log.LogString.Split(' ', 2);
                long contentSize = log.LogDetail.ContentSize;
                long responseTime = log.LogDetail.ResponseTimeMilliSeconds;
                logElementParsers.Add(new KeyValueParser("_apiPath", headerElements[0]));
                logElementParsers.Add(new KeyValueParser("_contentSize", $"{contentSize}"));
                logElementParsers.Add(new KeyValueParser("_responseTimeMilliSeconds", $"{responseTime}"));

                logElementParsers = ApiLogPresenter.AddLogDetailParsers(logElementParsers, log.LogDetail);

                logElementParsers.Add(new KeyValueParser(nameof(log.LogType), log.LogType.ToString()));

                logElementParsers.Add(new KeyValueParser(nameof(log.ReceivedAt), log.ReceivedAt.ToString("yyyyMMdd-HHmmss")));

                objectParsers.Add(new KeyValueArrayParser.ObjectParser(logElementParsers.ToArray()));
                logCount[log.LogType] += 1;
                totalContentBytes += contentSize;
                totalResponseTime += responseTime;
            }

            var logs = new KeyValueArrayParser("_logs", objectParsers.ToArray());

            var successCount = new KeyValueParser("_successCount", logCount[LogType.Log].ToString());
            var errorCount = new KeyValueParser("_errorCount", logCount[LogType.Error].ToString());
            var requestCount = new KeyValueParser("_requestCount", $"{objectParsers.Count}");
            var totalReceivedBytes = new KeyValueParser("_totalReceivedBytes", $"{totalContentBytes}");
            var totalResponseTimeMilliSeconds = new KeyValueParser("_totalResponseTimeMilliSeconds", $"{totalResponseTime}");

            return new KeyValueSerializer(
                "LogData",
                new IKeyValueParser[]
                {
                    logs,
                    successCount,
                    errorCount,
                    requestCount,
                    totalReceivedBytes,
                    totalResponseTimeMilliSeconds
                });
        }

        protected override LogOverlayBaseRuntimeSettings _GetOverlaySettings()
        {
            return NoaDebuggerSettingsManager.GetCacheSettings<ApiLogOverlayRuntimeSettings>();
        }

        public void Init()
        {
            _Init();
        }

        protected override void _ResetLogBuffer()
        {
            _normalLogBuffer = new RingBuffer<ApiLogEntry>(GetLogCapacity());
            _errorLogBuffer = new RingBuffer<ApiLogEntry>(GetLogCapacity());
        }

        class ApiLogMenuInfo : IMenuInfo
        {
            public string Name => "APILog";
            public string MenuName => "APILog";
            public int SortNo => NoaDebuggerDefine.API_LOG_MENU_SORT_NO;
        }

        ApiLogMenuInfo _apiLogMenuInfo;

        public IMenuInfo MenuInfo()
        {
            if (_apiLogMenuInfo == null)
            {
                _apiLogMenuInfo = new ApiLogMenuInfo();
            }

            return _apiLogMenuInfo;
        }

        public void ShowView(Transform parent) => _ShowView(parent);

        public bool GetPinStatus() => _GetPinStatus();

        public void TogglePin(Transform parent) => _TogglePin(parent);

        public void InitOverlay(Transform parent)
        {
            if (_overlayPresenter.IsOverlayEnable)
            {
                _overlayPresenter.InstantiateOverlay(parent);
            }
        }

        public bool GetSettingsStatus()
        {
            return _overlayPresenter.IsOverlaySettingsEnable;
        }

        public void ToggleOverlaySettings(Transform parent)
        {
            _overlayPresenter.ToggleActiveOverlaySettingsView(_mainView.gameObject, parent);
        }

        public void AlignmentUI(bool isReverse) => _AlignmentUI(isReverse);

        public void OnHidden() => _OnHidden();

        public void OnToolDispose()
        {
            _OnDispose();
            _onErrorLogReceived = null;
            _onLogCopied = null;
            _onLogDownload = null;
        }

        void _ReceiveLog(ApiLog log)
        {
            DateTime receivedAt = DateTime.Now;
            string logViewString = $"[{receivedAt:HH:mm:ss}] {log.Url.PathAndQuery} ({DataUnitConverterModel.ToHumanReadableBytes(log.ContentSize)} / {log.ResponseTimeMilliSeconds} ms)";

            var entry = new ApiLogEntry(
                _logSerialNumberCounter++,
                logString: log.Url.PathAndQuery,
                logViewString: logViewString,
                logDetail: log,
                logType: log.IsSuccess ? LogType.Log : LogType.Error,
                receivedAt: receivedAt);

            var targetBuffer = entry.LogType == LogType.Log ? _normalLogBuffer : _errorLogBuffer;
            if (targetBuffer.IsFull)
            {
                var oldLogEntry = targetBuffer.At(0);

                _totalReceivedBytes -= oldLogEntry.LogDetail.ContentSize;
                _totalResponseTimeMilliSeconds -= oldLogEntry.LogDetail.ResponseTimeMilliSeconds;

                _logBuffer.Remove(oldLogEntry);

                --_requestCount;
            }

            targetBuffer.Append(entry);
            _logBuffer.AddLast(entry);

            ++_requestCount;
            _totalReceivedBytes += log.ContentSize;
            _totalResponseTimeMilliSeconds += log.ResponseTimeMilliSeconds;

            if (entry.LogType == LogType.Error)
            {
                _SetNotificationStatus(ToolNotificationStatus.Error);
                _onErrorLogReceived?.Invoke(entry);
            }

            _OnReceivedLog();
            _UpdateView();
        }

        protected override void _SetNotificationStatus(ToolNotificationStatus notifyStatus)
        {
            _notifyStatus = notifyStatus;
            NoaDebuggerManager.OnChangeNotificationStatus<ApiLogPresenter>(_notifyStatus);
        }

        protected override bool _GetOverlayShowErrorLogsFromNoaDebuggerSettings()
        {
            return _settings.ApiLogOverlayShowErrorLogs;
        }

        protected override bool _GetOverlayShowMessageLogsFromNoaDebuggerSettings()
        {
            return _settings.ApiLogOverlayShowMessageLogs;
        }

        protected override bool _GetOverlayShowMessageWarningLogsFromNoaDebuggerSettings()
        {
            return false;
        }

        public static string CreateLogDetailString(ApiLog log)
        {
            log.ConvertBody();

            string toHeaderString(string text) => $"{ApiLogPresenter.DETAIL_HEADER_PREFIX}{text}{ApiLogPresenter.DETAIL_HEADER_SUFFIX}";

            StringBuilder logDetailsBuilder = new();
            logDetailsBuilder.AppendLine(toHeaderString(ApiLogPresenter.DETAIL_HEADER_GENERAL))
                             .AppendLine($"{ApiLogPresenter.DETAIL_CONTENT_URL}: {log.Url}")
                             .AppendLine($"{ApiLogPresenter.DETAIL_CONTENT_REQUEST_METHOD}: {log.Method}")
                             .AppendLine($"{ApiLogPresenter.DETAIL_CONTENT_STATUS_CODE}: {log.StatusCode}");
            if (log.RequestHeaders != null && log.RequestHeaders.Any())
            {
                logDetailsBuilder.AppendLine(toHeaderString(ApiLogPresenter.DETAIL_HEADER_REQUEST_HEADERS));
                foreach ((string key, string value) in log.RequestHeaders)
                {
                    logDetailsBuilder.AppendLine($"{key}: {value}");
                }
            }

            if (!string.IsNullOrEmpty(log.RequestBody))
            {
                logDetailsBuilder.AppendLine(toHeaderString(ApiLogPresenter.DETAIL_HEADER_REQUEST_BODY));
                logDetailsBuilder.AppendLine(log.PrettyPrintedRequestBody);
            }

            if (log.ResponseHeaders != null && log.ResponseHeaders.Any())
            {
                logDetailsBuilder.AppendLine(toHeaderString(ApiLogPresenter.DETAIL_HEADER_RESPONSE_HEADERS));
                foreach ((string key, string value) in log.ResponseHeaders)
                {
                    logDetailsBuilder.AppendLine($"{key}: {value}");
                }
            }

            if (!string.IsNullOrEmpty(log.ResponseBody))
            {
                logDetailsBuilder.AppendLine(toHeaderString(ApiLogPresenter.DETAIL_HEADER_RESPONSE_BODY));
                logDetailsBuilder.AppendLine(log.PrettyPrintedResponseBody);
            }

            return logDetailsBuilder.ToString().TrimEnd();
        }

        static List<IKeyValueParser> AddLogDetailParsers(List<IKeyValueParser> parsers, ApiLog logDetail)
        {
            string logDetailString = ApiLogPresenter.CreateLogDetailString(logDetail);

            string[] detailString = logDetailString.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            for (var line = 0; line < detailString.Length; ++line)
            {
                if (detailString[line].StartsWith(ApiLogPresenter.DETAIL_HEADER_PREFIX))
                {
                    string detailHeader = detailString[line]
                                          .Replace(ApiLogPresenter.DETAIL_HEADER_PREFIX, "")
                                          .Replace(ApiLogPresenter.DETAIL_HEADER_SUFFIX, "");
                    var header = string.Empty;
                    var hasMultipleDetailElements = false;
                    var isJsonFormatted = false;
                    switch (detailHeader)
                    {
                        case "API Detail":
                            header = "_apiDetail";
                            hasMultipleDetailElements = true;
                            break;
                        case "Request Headers":
                            header = "_requestHeaders";
                            hasMultipleDetailElements = true;
                            break;
                        case "Request Body":
                            header = "_requestBody";
                            isJsonFormatted = logDetail.IsRequestBodyPrettyPrinted;
                            break;
                        case "Response Headers":
                            header = "_responseHeaders";
                            hasMultipleDetailElements = true;
                            break;
                        case "Response Body":
                            header = "_responseBody";
                            isJsonFormatted = logDetail.IsResponseBodyPrettyPrinted;
                            break;
                    }

                    if (string.IsNullOrEmpty(header) || (detailString.Length <= (line + 1)))
                    {
                        continue;
                    }

                    if (hasMultipleDetailElements)
                    {
                        var logDetailParsers = new List<KeyValueParser>();
                        var elementCount = 0;
                        for (int i = line + 1;
                             (i < detailString.Length) && !detailString[i].StartsWith(ApiLogPresenter.DETAIL_HEADER_PREFIX);
                             ++i)
                        {
                            string[] element = detailString[i].Split(':', 2);
                            logDetailParsers.Add(new KeyValueParser(element[0].Trim(), element[1].Trim()));
                            ++elementCount;
                        }

                        parsers.Add(new KeyObjectParser(header, logDetailParsers.ToArray()));
                        line += elementCount;
                    }
                    else
                    {
                        var rawStringValues = new List<string>();
                        for (int i = line + 1;
                             (i < detailString.Length) &&
                             !detailString[i].StartsWith(ApiLogPresenter.DETAIL_HEADER_PREFIX);
                             ++i)
                        {
                            rawStringValues.Add(detailString[i]);
                            ++line;
                        }

                        if (isJsonFormatted)
                        {
                            parsers.Add(new JsonObjectParser(header, string.Join(Environment.NewLine, rawStringValues)));
                        }
                        else
                        {
                            parsers.Add(new KeyValueParser(header, string.Join("", rawStringValues)));
                        }
                    }
                }
            }

            return parsers;
        }

        protected override void OnDestroy()
        {
            _errorLogBuffer = default;
            _normalLogBuffer = default;
            _onErrorLogReceived = default;
            _onLogCopied = default;
            _onLogDownload = default;
            _apiLogMenuInfo = default;

            base.OnDestroy();
        }
    }
}
