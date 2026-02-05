#if UNITY_WEBGL && !UNITY_EDITOR
#define JAVASCRIPT_FEATURE_ENABLED
#endif

using UnityEngine;

namespace NoaDebugger
{
    sealed class UserAgentModel
    {
#if JAVASCRIPT_FEATURE_ENABLED
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern string NoaDebuggerGetUserAgent();

        private static readonly System.Lazy<bool> _isiOSorMacSafari = new (
            () =>
            {
                var userAgent = NoaDebuggerGetUserAgent() ?? string.Empty;
                var pattern = @"iPhone|iPad|iPod|(?=.*Macintosh)(?=.*Version).*Safari";
                return System.Text.RegularExpressions.Regex.IsMatch(userAgent, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }
        );

        public static bool IsWebGLandiOSorMacSafari => _isiOSorMacSafari.Value;

        public static bool IsMatchRegexPattern(string pattern)
        {
            var userAgent = NoaDebuggerGetUserAgent() ?? string.Empty;
            return System.Text.RegularExpressions.Regex.IsMatch(userAgent, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }

        private static readonly System.Lazy<bool> _isMobileBrowser = new (
            () =>
            {
                var userAgent = NoaDebuggerGetUserAgent() ?? string.Empty;
                var pattern = @"iPhone|iPad|iPod|Android";
                return System.Text.RegularExpressions.Regex.IsMatch(userAgent, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }
        );

        public static bool IsMobileBrowser => _isMobileBrowser.Value;
#else
    public static bool IsWebGLandiOSorMacSafari => false;

    public static bool IsMatchRegexPattern(string pattern)
    {
        return false;
    }

    public static bool IsMobileBrowser => false;
#endif
    }
}
