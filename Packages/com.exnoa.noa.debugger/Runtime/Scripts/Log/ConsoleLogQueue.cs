using System;
using System.Collections.Concurrent;

namespace NoaDebugger
{
    sealed class ConsoleLogQueue
    {
        static ConcurrentQueue<(UnityEngine.LogType Type, string Message, string StackTrace)> LogQueue { get; } = new ();

        public static void EnqueueLog(UnityEngine.LogType type, string message, string stackTrace) => LogQueue.Enqueue((type, message, stackTrace));

        public static void FlushLog(Action<UnityEngine.LogType, string, string> logCallback)
        {
            while(LogQueue.TryDequeue(out var log))
            {
                logCallback(log.Type, log.Message, log.StackTrace);
            }
        }

        public static void ClearLog() => LogQueue.Clear();
    }
}
