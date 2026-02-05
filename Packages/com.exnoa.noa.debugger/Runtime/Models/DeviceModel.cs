using UnityEngine;

namespace NoaDebugger
{
    sealed class DeviceModel : ModelBase
    {
        static bool IsUserAgentAndroid => UserAgentModel.IsMatchRegexPattern(@"Android");

        static bool IsUserAgentWindows =>
            Application.platform == RuntimePlatform.WebGLPlayer &&
            UserAgentModel.IsMatchRegexPattern(@"Windows NT|Win64");

        static bool IsUserAgentMac =>
            Application.platform == RuntimePlatform.WebGLPlayer &&
            UserAgentModel.IsMatchRegexPattern(@"(?=.*Macintosh)");

        static bool IsUserAgentIOS => UserAgentModel.IsMatchRegexPattern(@"iPhone|iPad|iPod");

        public static bool IsWindows
        {
            get
            {
                bool isWindows = Application.platform == RuntimePlatform.WindowsEditor ||
                                 Application.platform == RuntimePlatform.WindowsPlayer ||
                                 IsUserAgentWindows;

                return isWindows;
            }
        }

        public static bool IsMac
        {
            get
            {
                bool isMac = Application.platform == RuntimePlatform.OSXEditor ||
                             Application.platform == RuntimePlatform.OSXPlayer ||
                             IsUserAgentMac;

                return isMac;
            }
        }

        public static bool IsAndroid
        {
            get
            {
                bool isAndroid = Application.platform == RuntimePlatform.Android ||
                                 IsUserAgentAndroid;

                return isAndroid;
            }
        }

        public static bool IsIOS
        {
            get
            {
                bool isIos = Application.platform == RuntimePlatform.IPhonePlayer ||
                             IsUserAgentIOS;

                return isIos;
            }
        }
    }
}
