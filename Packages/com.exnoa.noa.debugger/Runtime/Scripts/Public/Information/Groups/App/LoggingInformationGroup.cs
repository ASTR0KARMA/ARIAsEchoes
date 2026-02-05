using UnityEngine;

namespace NoaDebugger
{
    /// <summary>
    /// Represents a collection of the logging information.
    /// </summary>
    public sealed class LoggingInformationGroup
    {
        /// <summary>
        /// True if runtime debug logging is enabled; otherwise false.
        /// </summary>
        public bool Enabled
        {
            get => Debug.unityLogger.logEnabled;
            set => Debug.unityLogger.logEnabled = value;
        }

        /// <summary>
        /// Gets or sets the types of debug log message is enabled.
        /// </summary>
        public UnityEngine.LogType FilterLogType
        {
            get => Debug.unityLogger.filterLogType;
            set => Debug.unityLogger.filterLogType = value;
        }

        /// <summary>
        /// Gets or sets the logging options of exception logs.
        /// </summary>
        public StackTraceLogType ExceptionStackTraceLogType
        {
            get => Application.GetStackTraceLogType(UnityEngine.LogType.Exception);
            set => Application.SetStackTraceLogType(UnityEngine.LogType.Exception, value);
        }

        /// <summary>
        /// Gets or sets the logging options of assert logs.
        /// </summary>
        public StackTraceLogType AssertStackTraceLogType
        {
            get => Application.GetStackTraceLogType(UnityEngine.LogType.Assert);
            set => Application.SetStackTraceLogType(UnityEngine.LogType.Assert, value);
        }

        /// <summary>
        /// Gets or sets the logging options of error logs.
        /// </summary>
        public StackTraceLogType ErrorStackTraceLogType
        {
            get => Application.GetStackTraceLogType(UnityEngine.LogType.Error);
            set => Application.SetStackTraceLogType(UnityEngine.LogType.Error, value);
        }

        /// <summary>
        /// Gets or sets the logging options of warning logs.
        /// </summary>
        public StackTraceLogType WarningStackTraceLogType
        {
            get => Application.GetStackTraceLogType(UnityEngine.LogType.Warning);
            set => Application.SetStackTraceLogType(UnityEngine.LogType.Warning, value);
        }

        /// <summary>
        /// Gets or sets the logging options of regular logs.
        /// </summary>
        public StackTraceLogType LogStackTraceLogType
        {
            get => Application.GetStackTraceLogType(UnityEngine.LogType.Log);
            set => Application.SetStackTraceLogType(UnityEngine.LogType.Log, value);
        }

        /// <summary>
        /// Gets the path to the console log file,
        /// or an empty string if the current platform does not support log files.
        /// </summary>
        public string ConsoleLogPath { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingInformationGroup"/>.
        /// </summary>
        internal LoggingInformationGroup()
        {
            const string groupName = "Logging";
            EnabledParameter = new BoolInformationParameter(groupName, "Enabled", Enabled, value => Enabled = value);
            FilterLogTypeParameter = new EnumInformationParameter(groupName, "FilterLogType", FilterLogType, value => FilterLogType = (UnityEngine.LogType)value);
            ExceptionStackTraceLogTypeParameter = new EnumInformationParameter(groupName, "ExceptionStackTraceLogType", ExceptionStackTraceLogType, value => ExceptionStackTraceLogType = (StackTraceLogType)value);
            AssertStackTraceLogTypeParameter = new EnumInformationParameter(groupName, "AssertStackTraceLogType", AssertStackTraceLogType, value => AssertStackTraceLogType = (StackTraceLogType)value);
            ErrorStackTraceLogTypeParameter = new EnumInformationParameter(groupName, "ErrorStackTraceLogType", ErrorStackTraceLogType, value => ErrorStackTraceLogType = (StackTraceLogType)value);
            WarningStackTraceLogTypeParameter = new EnumInformationParameter(groupName, "WarningStackTraceLogType", WarningStackTraceLogType, value => WarningStackTraceLogType = (StackTraceLogType)value);
            LogStackTraceLogTypeParameter = new EnumInformationParameter(groupName, "LogStackTraceLogType", LogStackTraceLogType, value => LogStackTraceLogType = (StackTraceLogType)value);
            ConsoleLogPath = Application.consoleLogPath;
        }

        internal void UpdateSettings()
        {
            EnabledParameter.ChangeValue(Enabled);
            FilterLogTypeParameter.ChangeValue(FilterLogType);
            ExceptionStackTraceLogTypeParameter.ChangeValue(ExceptionStackTraceLogType);
            AssertStackTraceLogTypeParameter.ChangeValue(AssertStackTraceLogType);
            ErrorStackTraceLogTypeParameter.ChangeValue(ErrorStackTraceLogType);
            WarningStackTraceLogTypeParameter.ChangeValue(WarningStackTraceLogType);
            LogStackTraceLogTypeParameter.ChangeValue(LogStackTraceLogType);
        }

        internal BoolInformationParameter EnabledParameter { get; }
        internal EnumInformationParameter FilterLogTypeParameter { get; }
        internal EnumInformationParameter ExceptionStackTraceLogTypeParameter { get; }
        internal EnumInformationParameter AssertStackTraceLogTypeParameter { get; }
        internal EnumInformationParameter ErrorStackTraceLogTypeParameter { get; }
        internal EnumInformationParameter WarningStackTraceLogTypeParameter { get; }
        internal EnumInformationParameter LogStackTraceLogTypeParameter { get; }
    }
}
