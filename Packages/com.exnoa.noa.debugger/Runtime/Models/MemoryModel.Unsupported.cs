#if (!UNITY_ANDROID && !UNITY_IOS && !UNITY_STANDALONE_WIN) || UNITY_EDITOR
namespace NoaDebugger
{
    sealed partial class MemoryModel
    {
        public static partial long? _GetCurrentNativeMemoryByte()
        {
            return -1;
        }
    }
}
#endif
