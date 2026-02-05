namespace NoaDebugger
{
    static class NoaDebuggerApiStatus
    {
        public enum SetFont
        {
            Succeed,

            Failed,

            FailedNoaDebuggerIsNull,

            FailedFontAssetIsNull,

            FailedIsNotPlayMode,
        }
    }
}
