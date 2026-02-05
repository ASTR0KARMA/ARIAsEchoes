using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

namespace NoaDebugger
{
    /// <summary>
    /// This class allows you to access the information of the ConsoleLog functionality
    /// </summary>
    public sealed class NoaConsoleLog
    {
        /// <summary>
        /// Returns the list of logs being held
        /// </summary>
        public static LinkedList<ConsoleLogEntry> LogList => _GetLogList();

        /// <summary>
        /// Callback for when an error is detected
        /// </summary>
        public static UnityAction<ConsoleLogEntry> OnError
        {
            get
            {
                var presenter = NoaDebugger.GetPresenter<ConsoleLogPresenter>();

                return presenter != null ? presenter._onErrorLogReceived : null;
            }
            set
            {
                var presenter = NoaDebugger.GetPresenter<ConsoleLogPresenter>();

                if (presenter == null)
                {
                    return;
                }

                if (value == null)
                {
                    presenter._onErrorLogReceived = null;
                }
                else
                {
                    presenter._onErrorLogReceived += value;
                }
            }
        }

        /// <summary>
        /// Callback for determining whether to display notifications when an error is detected
        /// </summary>
        public static Func<ConsoleLogEntry, bool> OnFilterErrorNotification
        {
            get
            {
                var presenter = NoaDebugger.GetPresenter<ConsoleLogPresenter>();

                return presenter != null ? presenter._onFilterErrorNotification : null;
            }
            set
            {
                var presenter = NoaDebugger.GetPresenter<ConsoleLogPresenter>();

                if (presenter == null)
                {
                    return;
                }

                if (value == null)
                {
                    presenter._onFilterErrorNotification = null;
                }
                else
                {
                    presenter._onFilterErrorNotification += value;
                }
            }
        }

        /// <summary>
        /// Event triggered when a console log is copied to the clipboard.
        /// Users can register their custom actions to be executed when a log entry is copied to the clipboard.
        /// The event provides the log entry data and the copied text data.
        /// </summary>
        public static UnityAction<ConsoleLogEntry, string> OnLogCopied
        {
            get
            {
                var presenter = NoaDebugger.GetPresenter<ConsoleLogPresenter>();

                return presenter != null ? presenter._onLogCopied : null;
            }
            set
            {
                var presenter = NoaDebugger.GetPresenter<ConsoleLogPresenter>();

                if (presenter == null)
                {
                    return;
                }

                if (value == null)
                {
                    presenter._onLogCopied = null;
                }
                else
                {
                    presenter._onLogCopied += value;
                }
            }
        }

        /// <summary>
        /// Registers a custom download callback.
        /// If the argument is null, unregisters the corresponding callback.
        /// </summary>
        /// <param name="commonCallbacks">A class implementing INoaDownloadCallbacks or derived from NoaDownloadCallbacks</param>
        /// <param name="consoleLogCallbacks">A class implementing INoaConsoleLogDownloadCallbacks or derived from NoaConsoleLogDownloadCallbacks</param>
        public static void SetDownloadCallbacks(INoaDownloadCallbacks commonCallbacks, INoaConsoleLogDownloadCallbacks consoleLogCallbacks)
        {
            CommonDownloadCallbacks = commonCallbacks;
            ConsoleLogDownloadCallbacks = consoleLogCallbacks;
        }

        internal static INoaDownloadCallbacks CommonDownloadCallbacks { get; private set; } = null;

        internal static INoaConsoleLogDownloadCallbacks ConsoleLogDownloadCallbacks { get; private set; } = null;

        /// <summary>
        /// Event triggered when logs are downloaded.
        /// Users can register their custom actions to be executed upon log download.
        /// The event provides the filename and the JSON data as strings.
        /// If the event handler returns true, the logs will be downloaded locally.
        /// If the event handler returns false, the logs will not be downloaded locally.
        /// </summary>
        [Obsolete("Inherit from NoaDownloadCallbacks and override OnBeforeDownload instead.")]
        public static Func<string, string, bool> OnLogDownload
        {
            get
            {
                var presenter = NoaDebugger.GetPresenter<ConsoleLogPresenter>();

                return presenter != null ? presenter._onLogDownload : null;
            }
            set
            {
                var presenter = NoaDebugger.GetPresenter<ConsoleLogPresenter>();

                if (presenter == null)
                {
                    return;
                }

                if (value == null)
                {
                    presenter._onLogDownload = null;
                }
                else
                {
                    presenter._onLogDownload += value;
                    presenter._onLogDownloadWithLogEntries = null;
                }
            }
        }

        /// <summary>
        /// Event triggered when logs are downloaded.
        /// Users can register their custom actions to be executed upon log download.
        /// The event provides the filename and the list of log entries as parameters.
        /// If the event handler returns true, the log entries will be downloaded locally.
        /// If the event handler returns false, the log entries will not be downloaded locally.
        /// </summary>
        [Obsolete("Inherit from NoaConsoleLogDownloadCallbacks and override OnBeforeConversion instead.")]
        public static Func<string, List<ConsoleLogEntry>, bool> OnLogDownloadWithLogEntries
        {
            get
            {
                var presenter = NoaDebugger.GetPresenter<ConsoleLogPresenter>();
                return presenter != null ? presenter._onLogDownloadWithLogEntries : null;
            }
            set
            {
                var presenter = NoaDebugger.GetPresenter<ConsoleLogPresenter>();

                if (presenter == null)
                {
                    return;
                }

                if (value == null)
                {
                    presenter._onLogDownloadWithLogEntries = null;
                }
                else
                {
                    presenter._onLogDownloadWithLogEntries += value;
                    presenter._onLogDownload = null;
                }
            }
        }

        /// <summary>
        /// Users can register custom actions to be executed when the log sending feature is initiated.
        /// The event provides a list of the selected logs.
        /// </summary>
        public static UnityAction<List<ConsoleLogEntry>> OnLogSend
        {
            get
            {
                var presenter = NoaDebugger.GetPresenter<ConsoleLogPresenter>();
                return presenter != null ? presenter._onLogSend : null;
            }
            set
            {
                var presenter = NoaDebugger.GetPresenter<ConsoleLogPresenter>();

                if (presenter == null)
                {
                    return;
                }

                if (value == null)
                {
                    presenter._onLogSend = null;
                }
                else
                {
                    presenter._onLogSend += value;
                }
            }
        }



        /// <summary>
        /// Returns the list of logs being held
        /// </summary>
        /// <returns>Returns the list of logs being held</returns>
        static LinkedList<ConsoleLogEntry> _GetLogList()
        {
            ConsoleLogQueue.FlushLog(_AddUnsafe);

            ConsoleLogPresenter presenter = NoaDebugger.GetPresenter<ConsoleLogPresenter>();

            return presenter != null ? presenter.GetLogList() : null;
        }

        /// <summary>
        /// Adds logs to the ConsoleLog functionality
        /// </summary>
        /// <param name="logType">Type of log</param>
        /// <param name="message">Log text</param>
        /// <param name="stackTrace">Log stack trace</param>
        static void _AddUnsafe(UnityEngine.LogType logType, string message, string stackTrace = null)
        {
            ConsoleLogPresenter presenter = NoaDebugger.GetPresenter<ConsoleLogPresenter>();
            if (presenter == null)
            {
                return;
            }

            if (stackTrace == null)
            {
                stackTrace = StackTraceUtility.ExtractStackTrace();
            }

            presenter.ReceiveLogGenerateStackTraceFirstLine(message, stackTrace, logType);
        }

        /// <summary>
        /// Adds logs to the ConsoleLog functionality
        /// </summary>
        /// <param name="logType">Type of log</param>
        /// <param name="message">Log text</param>
        /// <param name="stackTrace">Log stack trace</param>
        public static void Add(UnityEngine.LogType logType, string message, string stackTrace = null)
        {
            ConsoleLogPresenter presenter = NoaDebugger.GetPresenter<ConsoleLogPresenter>();
            if(!presenter || NoaDebuggerManager.MainThreadId != Thread.CurrentThread.ManagedThreadId)
            {
                ConsoleLogQueue.EnqueueLog(logType, message, stackTrace);
                return;
            }

            ConsoleLogQueue.FlushLog(_AddUnsafe);

            _AddUnsafe(logType, message, stackTrace);
        }

        /// <summary>
        /// Deletes all logs
        /// </summary>
        public static void Clear()
        {
            ConsoleLogQueue.ClearLog();

            ConsoleLogPresenter presenter = NoaDebugger.GetPresenter<ConsoleLogPresenter>();

            if (presenter == null)
            {
                return;
            }

            presenter.ClearLog();
        }
    }
}
