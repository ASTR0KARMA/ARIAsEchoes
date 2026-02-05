using System;

namespace NoaDebugger
{
    sealed class InformationLoggingData
    {
        public IMutableParameter<bool> _enabled;
        public IMutableParameter<Enum> _filterLogType;
        public IMutableParameter<Enum> _exceptionStackTraceLogType;
        public IMutableParameter<Enum> _assertStackTraceLogType;
        public IMutableParameter<Enum> _errorStackTraceLogType;
        public IMutableParameter<Enum> _warningStackTraceLogType;
        public IMutableParameter<Enum> _logStackTraceLogType;
        public string _consoleLogPath;
    }
}
