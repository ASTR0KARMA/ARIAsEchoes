#if UNITY_IOS && !UNITY_EDITOR
using System.Runtime.InteropServices;

namespace NoaDebugger
{
    sealed partial class MemoryModel
    {
        [DllImport("__Internal", EntryPoint = "NoaDebuggerGetCurrentMemoryByte")]
        static extern long _NoaDebuggerGetCurrentMemoryByte();

        public static partial long? _GetCurrentNativeMemoryByte()
        {
            return _NoaDebuggerGetCurrentMemoryByte();
        }
    }
}
#endif
