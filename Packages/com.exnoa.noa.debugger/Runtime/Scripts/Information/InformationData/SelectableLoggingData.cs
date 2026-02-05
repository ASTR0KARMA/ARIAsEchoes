namespace NoaDebugger
{
    sealed class SelectableLoggingData : SelectableKeyValueBase, ISelectableGroup<InformationLoggingData>
    {
        const string GROUP_NAME = "Logging";
        const string KEY_ENABLED = "Enabled";
        const string KEY_FILTER_LOG_TYPE = "Filter Log Type";
        const string KEY_EXCEPTION_STACK_TRACE_LOG_TYPE = "Exception Stack Trace Log Type";
        const string KEY_ASSERT_STACK_TRACE_LOG_TYPE = "Assert Stack Trace Log Type";
        const string KEY_ERROR_STACK_TRACE_LOG_TYPE = "Error Stack Trace Log Type";
        const string KEY_WARNING_STACK_TRACE_LOG_TYPE = "Warning Stack Trace Log Type";
        const string KEY_LOG_STACK_TRACE_LOG_TYPE = "Log Stack Trace Log Type";
        const string KEY_CONSOLE_LOG_PATH = "Console Log Path";

        public string GroupName => GROUP_NAME;

        public SelectableLoggingData(InformationLoggingData logging)
        {
            AddItem(KEY_ENABLED, logging._enabled);
            AddItem(KEY_FILTER_LOG_TYPE, logging._filterLogType);
            AddItem(KEY_EXCEPTION_STACK_TRACE_LOG_TYPE, logging._exceptionStackTraceLogType);
            AddItem(KEY_ASSERT_STACK_TRACE_LOG_TYPE, logging._assertStackTraceLogType);
            AddItem(KEY_ERROR_STACK_TRACE_LOG_TYPE, logging._errorStackTraceLogType);
            AddItem(KEY_WARNING_STACK_TRACE_LOG_TYPE, logging._warningStackTraceLogType);
            AddItem(KEY_LOG_STACK_TRACE_LOG_TYPE, logging._logStackTraceLogType);
            AddItem(KEY_CONSOLE_LOG_PATH, logging._consoleLogPath);
        }

        public void Update(InformationLoggingData logging)
        {
            UpdateItem(KEY_ENABLED, logging._enabled);
            UpdateItem(KEY_FILTER_LOG_TYPE, logging._filterLogType);
            UpdateItem(KEY_EXCEPTION_STACK_TRACE_LOG_TYPE, logging._exceptionStackTraceLogType);
            UpdateItem(KEY_ASSERT_STACK_TRACE_LOG_TYPE, logging._assertStackTraceLogType);
            UpdateItem(KEY_ERROR_STACK_TRACE_LOG_TYPE, logging._errorStackTraceLogType);
            UpdateItem(KEY_WARNING_STACK_TRACE_LOG_TYPE, logging._warningStackTraceLogType);
            UpdateItem(KEY_LOG_STACK_TRACE_LOG_TYPE, logging._logStackTraceLogType);
            UpdateItem(KEY_CONSOLE_LOG_PATH, logging._consoleLogPath);
        }
    }
}
