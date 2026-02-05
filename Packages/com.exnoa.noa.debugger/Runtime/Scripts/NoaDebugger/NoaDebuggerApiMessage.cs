namespace NoaDebugger
{
    static class NoaDebuggerApiMessage
    {
        static readonly string ApplyFontFailedText = "Failed to apply font";

        public static string GetFailedMessage(NoaDebuggerApiStatus.SetFont status)
        {
            return status switch
            {
                NoaDebuggerApiStatus.SetFont.Failed => $"{NoaDebuggerApiMessage.ApplyFontFailedText}.",
                NoaDebuggerApiStatus.SetFont.FailedNoaDebuggerIsNull => $"{NoaDebuggerApiMessage.ApplyFontFailedText}: NoaDebugger is null.",
                NoaDebuggerApiStatus.SetFont.FailedFontAssetIsNull => $"{NoaDebuggerApiMessage.ApplyFontFailedText}: fontAsset is null, reverting to default font.",
                NoaDebuggerApiStatus.SetFont.FailedIsNotPlayMode => $"{NoaDebuggerApiMessage.ApplyFontFailedText}: is not playing mode.",
                _ => null,
            };
        }
    }
}
