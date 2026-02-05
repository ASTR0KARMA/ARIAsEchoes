using System;
using System.Collections.Generic;

namespace NoaDebugger
{
    sealed class LogFilterRepository
    {
        readonly string _prefsKey;

        public LogFilterRepository(string prefsKey)
        {
            _prefsKey = prefsKey;
        }

        public void SaveFilter(Dictionary<LogType, bool> flagDictionary)
        {
            InternalLogType flags = _AllTrue();
            foreach (var kv in flagDictionary)
            {
                if (kv.Value)
                {
                    flags |= _GetInternalLogTypeFrom(kv.Key);
                }
                else
                {
                    flags &= ~_GetInternalLogTypeFrom(kv.Key);
                }
            }

            int intFlags = (int)flags;
            NoaDebuggerPrefs.SetInt(_prefsKey, intFlags);
        }

        public Dictionary<LogType, bool> LoadFilter(Dictionary<LogType, bool> flagDictionary)
        {
            int loadedFlagsInt = NoaDebuggerPrefs.GetInt(_prefsKey, (int)_AllTrue());
            var loadedFlags = (InternalLogType)loadedFlagsInt;

            var result = new Dictionary<LogType, bool>(flagDictionary);
            foreach (var kv in flagDictionary)
            {
                var logType = kv.Key;
                if (_IsLogTypeTrue(logType, loadedFlags))
                {
                    result[logType] = true;
                }
                else
                {
                    result[logType] = false;
                }
            }

            return result;
        }

        InternalLogType _GetInternalLogTypeFrom(LogType logType)
        {
            switch (logType)
            {
                case LogType.Log: return InternalLogType.Log;
                case LogType.Warning: return InternalLogType.Warning;
                case LogType.Error: return InternalLogType.Error;
                default:
                    throw new Exception($"Undefined log type ===> {logType}");
            }
        }

        bool _IsLogTypeTrue(LogType logType, InternalLogType targetFlag)
        {
            var internalLogType = _GetInternalLogTypeFrom(logType);
            return (targetFlag & internalLogType) == internalLogType;
        }

        InternalLogType _AllTrue()
        {
            return InternalLogType.Log | InternalLogType.Warning | InternalLogType.Error;
        }

        [Flags]
        enum InternalLogType
        {
            Error = 1 << 0,
            Warning = 1 << 1,
            Log = 1 << 2,
        }
    }
}
