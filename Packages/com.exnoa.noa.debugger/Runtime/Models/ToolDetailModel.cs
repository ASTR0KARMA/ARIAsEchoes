using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NoaDebugger
{
    sealed class ToolDetailModel : ModelBase
    {
        public const string OPERATIONAL_VALUE = "operational";

        public enum OSType
        {
            Unknown,
            Editor,
            Ios,
            Android,
            Standalone,
            Webgl
        }

        public ToolDetailInformation ToolDetailInformation { private set; get; }

        NoaDebuggerInfo _noaDebuggerInfo;

        public ToolDetailModel()
        {
            _LoadAsset();

            string osVersion = OsType == OSType.Webgl ? SystemInfo.operatingSystem : OsVersion;
            SetOperatingEnv(osVersion, OsType, SystemInfo.deviceModel);
        }


        void _LoadAsset()
        {
            _noaDebuggerInfo = NoaDebuggerInfoManager.GetNoaDebuggerInfo();

            ToolDetailInformation = new ToolDetailInformation()
            {
                _copyright = _noaDebuggerInfo.NoaDebuggerCopyright,
            };
        }

        public void SetOperatingEnv(string osVersion, OSType osType, string device = "", string unityVersion = "")
        {
            ToolDetailInformation._operatingEnv = _CheckSupported(osVersion, osType, device, unityVersion)
                ? ToolDetailModel.OPERATIONAL_VALUE
                : _SupportedVersionText(osVersion, osType);
        }

        string OsVersion
        {
            get
            {
#if UNITY_EDITOR
                return SemanticVersion.ExtractSemanticVersionString(Application.unityVersion);
#elif UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE_WIN || UNITY_WEBGL
                return SemanticVersion.ExtractSemanticVersionString(SystemInfo.operatingSystem);
#else
                return "";
#endif
            }
        }

        OSType OsType
        {
            get
            {
#if UNITY_EDITOR
                return OSType.Editor;
#elif UNITY_IOS
                return OSType.Ios;
#elif UNITY_ANDROID
                return OSType.Android;
#elif UNITY_STANDALONE_WIN
                return OSType.Standalone;
#elif UNITY_WEBGL
                return OSType.Webgl;
#else
                return OSType.Unknown;
#endif
            }
        }

        bool _CheckSupported(string osVersion, OSType osType, string device, string unityVersion)
        {
            switch (osType)
            {
                case OSType.Editor:
                    return _noaDebuggerInfo.IsSupportedUnityVersion(osVersion);

                case OSType.Android:
                    return _noaDebuggerInfo.IsSupportedAndroidVersion(osVersion);

                case OSType.Ios:
                    return _noaDebuggerInfo.IsSupportedIOSVersion(osVersion);

                case OSType.Standalone:
                    return _noaDebuggerInfo.IsSupportedWindowsVersion(osVersion);

                case OSType.Webgl:
                    return _noaDebuggerInfo.IsSupportedBrowser(device, osVersion, unityVersion);

                default:
                    return false;
            }
        }

        string _SupportedVersionText(string osVersion, OSType osType)
        {
            switch (osType)
            {
                case OSType.Editor:
                    return $"Unity {_noaDebuggerInfo.MinimumUnityVersion}+";

                case OSType.Android:
                    return $"Android {_noaDebuggerInfo.MinimumAndroidVersion}+";

                case OSType.Ios:
                    return $"iOS {_noaDebuggerInfo.MinimumIOSVersion}+";

                case OSType.Standalone:
                    return $"Windows {_noaDebuggerInfo.MinimumWindowsVersion}+";

                case OSType.Webgl:
                    return _GetSupportedBrowserText(osVersion);

                default:
                    return String.Empty;
            }
        }

        string _GetSupportedBrowserText(string osVersion)
        {
            foreach (SupportBrowserInfo pcBrowserInfo in _noaDebuggerInfo.SupportedPCBrowsersInfo)
            {
                foreach (string supportedOperatingSystem in pcBrowserInfo.OperatingSystemPrefixList)
                {
                    if (osVersion.StartsWith(supportedOperatingSystem))
                    {
                        return _SupportedBrowserTextFromSupportInfo(
                            _noaDebuggerInfo.SupportedPCBrowsersInfo,
                            hasMobile: false);
                    }
                }
            }

            foreach (SupportBrowserInfo mobileBrowserInfo in _noaDebuggerInfo.SupportedMobileBrowsersInfo)
            {
                foreach (string supportedOperatingSystem in mobileBrowserInfo.OperatingSystemPrefixList)
                {
                    if (osVersion.StartsWith(supportedOperatingSystem))
                    {
                        return _SupportedBrowserTextFromSupportInfo(
                            _noaDebuggerInfo.SupportedMobileBrowsersInfo,
                            hasMobile: true);
                    }
                }
            }

            var mergedList = new List<SupportBrowserInfo>();
            mergedList.AddRange(_noaDebuggerInfo.SupportedPCBrowsersInfo);
            mergedList.AddRange(_noaDebuggerInfo.SupportedMobileBrowsersInfo);

            return _SupportedBrowserTextFromSupportInfo(mergedList.ToArray(), hasMobile: true);
        }

        string _SupportedBrowserTextFromSupportInfo(SupportBrowserInfo[] supportedBrowsersInfo, bool hasMobile)
        {
            var supportedBrowsersGroupingList = supportedBrowsersInfo.GroupBy(x => x.Browser);

            StringBuilder builder = new StringBuilder();

            builder.Append(
                string.Join(
                    ", ",
                    supportedBrowsersGroupingList.Select(
                        browserGroup =>
                            $"{browserGroup.Key} ({string.Join(", ", browserGroup.Select(x => x.DisplayOperatingSystem))})")));

            if (hasMobile)
            {
                builder.Append($", Web builds made with Unity {_noaDebuggerInfo.MinimumUnityVersionForMobileBrowser}+.");
            }

            return builder.ToString();
        }
    }


    sealed class ToolDetailInformation
    {
        public string _operatingEnv;

        public string _copyright;
    }
}
