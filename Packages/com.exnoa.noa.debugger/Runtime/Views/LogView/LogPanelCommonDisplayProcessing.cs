using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class LogPanelCommonDisplayProcessing
    {
        readonly Image _logType;
        readonly TextMeshProUGUI _logString;
        readonly GameObject _logCounterRoot;
        readonly TextMeshProUGUI _logCounter;

        public LogPanelCommonDisplayProcessing(
            Image logType, TextMeshProUGUI logString,
            GameObject logCounterRoot, TextMeshProUGUI logCounter)
        {
            _logType = logType;
            _logString = logString;
            _logCounterRoot = logCounterRoot;
            _logCounter = logCounter;
        }

        public void RefreshLogType(LogViewLinker.LogPanelInfo logInfo)
        {
            _logType.color = _GetLogTypeColor(logInfo._logType);
        }

        public void RefreshLogString(LogViewLinker.LogPanelInfo logInfo)
        {
            RefreshLogString(logInfo._logString);
        }

        public void RefreshLogString(string logString)
        {
            _logString.text = NoaDebuggerText.GetHasFontAssetString(_logString.font, logString);
        }

        public void RefreshLogCounter(LogViewLinker.LogPanelInfo logInfo)
        {
            if (logInfo._numberOfMatchingLogs > 1)
            {
                _logCounter.text = logInfo._numberOfMatchingLogs >= NoaDebuggerDefine.MaxNumberOfMatchingLogsToDisplay ? $"{NoaDebuggerDefine.MaxNumberOfMatchingLogsToDisplay}+" : logInfo._numberOfMatchingLogs.ToString();
                _logCounterRoot.SetActive(true);
            }
            else
            {
                _logCounterRoot.SetActive(false);
            }
        }

        static Color _GetLogTypeColor(LogType type)
        {
            return type switch
            {
                LogType.Error => NoaDebuggerDefine.LogColors.LogError,
                LogType.Warning => NoaDebuggerDefine.LogColors.LogWarning,
                LogType.Log => NoaDebuggerDefine.LogColors.LogMessage,
                _ => NoaDebuggerDefine.LogColors.LogMessage
            };
        }
    }
}
