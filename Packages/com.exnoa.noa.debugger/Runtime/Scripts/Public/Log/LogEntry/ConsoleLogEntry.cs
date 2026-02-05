using System;

namespace NoaDebugger
{
    /// <summary>
    /// The details of a ConsoleLog.
    /// </summary>
    public class ConsoleLogDetail : ILogDetail
    {
        /// <summary>
        /// The detail text of the log.
        /// </summary>
        public string LogDetailString { get; }

        /// <summary>
        /// The detail text of the log for display.
        /// </summary>
        internal string LogDetailStringForDisplay { get; }

        /// <summary>
        /// Registration via API.
        /// </summary>
        internal bool IsRegisteredApi { get; }

        /// <summary>
        /// Generates a ConsoleLogDetail.
        /// </summary>
        /// <param name="logString">Specifies the log string.</param>
        /// <param name="stackTrace">Specifies the stack trace.</param>
        /// <param name="isRegisteredApi">Specify true if registering via API.</param>
        internal ConsoleLogDetail(string logString, string stackTrace, bool isRegisteredApi)
        {
            bool hasStackTrace = !string.IsNullOrEmpty(stackTrace);
            LogDetailString = hasStackTrace
                ? $"{logString}\n{stackTrace}"
                : logString;

            string displayStackTrace = hasStackTrace
                ? stackTrace
                : NoaDebuggerDefine.UnavaliableStackTraceLabel;
            LogDetailStringForDisplay = $"{logString}\n<color=#{NoaDebuggerDefine.TextColors.StackTraceColorCode}>{displayStackTrace}</color>";
            IsRegisteredApi = isRegisteredApi;
        }

        /// <summary>
        /// Gets text to be copied to the clipboard.
        /// </summary>
        /// <returns>The text to be copied.</returns>
        string ILogDetail.GetClipboardText()
        {
            return LogDetailString.TrimEnd();
        }
    }

    /// <summary>
    /// An entry in the ConsoleLog.
    /// </summary>
    public sealed class ConsoleLogEntry : LogEntry<ConsoleLogDetail>
    {
        /// <summary>
        /// The stack trace string.
        /// </summary>
        public string StackTraceString { private set; get; }

        /// <summary>
        /// Overrides the LogDetail of LogEntry.
        /// </summary>
        public override ConsoleLogDetail LogDetail { protected set; get; }

        /// <summary>
        /// Generates a ConsoleLogEntry.
        /// </summary>
        /// <param name="serialNumber">Specify the sequential number of the log</param>
        /// <param name="logString">Specifies the string output in the log</param>
        /// <param name="stackTraceString">Specifies the StackTrace string</param>
        /// <param name="logViewString">Specifies the log display string on the view</param>
        /// <param name="logDetail">Specifies the log detail</param>
        /// <param name="logType">Specifies the type of log</param>
        /// <param name="receivedAt">Specifies the date and time the log was retrieved</param>
        internal ConsoleLogEntry(int serialNumber, string logString, string stackTraceString, string logViewString, ConsoleLogDetail logDetail,
                                 LogType logType, DateTime receivedAt)
            : base(serialNumber, logString, logViewString, logType, receivedAt)
        {
            StackTraceString = stackTraceString;
            LogDetail = logDetail;
        }
    }
}
