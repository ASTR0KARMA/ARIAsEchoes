using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class NoaDebuggerInfo : ScriptableObject
    {
        [SerializeField]
        [Tooltip("The version of NOA Debugger. The value of \"version\" in package.json is automatically applied.")]
        string _noaDebuggerVersion;

        [SerializeField]
        [Tooltip("The minimum version of Unity supported. The value of \"unity\" in package.json is automatically applied.")]
        string _minimumUnityVersion;

        [SerializeField]
        [Tooltip("The minimum version of Unity supported on mobile browser.")]
        string _minimumUnityVersionForMobileBrowser;

        [SerializeField]
        [Tooltip("The minimum version of iOS supported.")]
        string _minimumIOSVersion;

        [SerializeField]
        [Tooltip("The minimum version of Android supported.")]
        string _minimumAndroidVersion;

        [SerializeField]
        [Tooltip("The minimum version of Windows supported.")]
        string _minimumWindowsVersion;

        [SerializeField]
        [Tooltip("The list of PC browsers and OS supported on Web environments.")]
        SupportBrowserInfo[] _supportedPCBrowsersInfo;

        [SerializeField]
        [Tooltip("The list of mobile browsers and OS supported on Web environments.")]
        SupportBrowserInfo[] _supportedMobileBrowsersInfo;

        [SerializeField]
        [Tooltip("Used only during editor execution to verify the correctness of the operation assurance environment definition file settings from package.json. It cannot be referenced during device build.")]
        TextAsset _packageInfo;

        [SerializeField]
        [Tooltip("The copyright of NOA Debugger.")]
        string _noaDebuggerCopyright;

        public struct UnityPackageInfo
        {
            public string version;
            public string unity;
        }

        [Conditional("NOA_DEBUGGER_DEBUG")]
        void OnValidate()
        {
            SemanticVersion.Parse(_noaDebuggerVersion);
            SemanticVersion.Parse(_minimumUnityVersion);
            SemanticVersion.Parse(_minimumUnityVersionForMobileBrowser);
            SemanticVersion.Parse(_minimumIOSVersion);
            SemanticVersion.Parse(_minimumAndroidVersion);
            SemanticVersion.Parse(_minimumWindowsVersion);

            if (_packageInfo != null)
            {
                var packageInfo = JsonUtility.FromJson<UnityPackageInfo>(_packageInfo.text);
                Assert.AreEqual(packageInfo.version, _noaDebuggerVersion);
                Assert.AreEqual(packageInfo.unity, _minimumUnityVersion);
            }
        }

        public string NoaDebuggerVersion
        {
            get => _noaDebuggerVersion;
#if UNITY_EDITOR
            set => _noaDebuggerVersion = value;
#endif
        }

        public string MinimumUnityVersion
        {
            get => _minimumUnityVersion;
#if UNITY_EDITOR
            set => _minimumUnityVersion = value;
#endif
        }

        public string MinimumUnityVersionForMobileBrowser => _minimumUnityVersionForMobileBrowser;

        public string MinimumIOSVersion => _minimumIOSVersion;

        public string MinimumAndroidVersion => _minimumAndroidVersion;

        public string MinimumWindowsVersion => _minimumWindowsVersion;

        public SupportBrowserInfo[] SupportedPCBrowsersInfo => _supportedPCBrowsersInfo;

        public SupportBrowserInfo[] SupportedMobileBrowsersInfo => _supportedMobileBrowsersInfo;

        public string NoaDebuggerCopyright => _noaDebuggerCopyright;

        public bool IsSupportedUnityVersion(string version)
        {
            return NoaDebuggerInfo.CompareVersions(_minimumUnityVersion, version) <= 0;
        }

        public bool IsSupportedIOSVersion(string version)
        {
            return NoaDebuggerInfo.CompareVersions(_minimumIOSVersion, version) <= 0;
        }

        public bool IsSupportedAndroidVersion(string version)
        {
            return NoaDebuggerInfo.CompareVersions(_minimumAndroidVersion, version) <= 0;
        }

        public bool IsSupportedWindowsVersion(string version)
        {
            return NoaDebuggerInfo.CompareVersions(_minimumWindowsVersion, version) <= 0;
        }

        public bool IsSupportedBrowser(string browser, string operatingSystem, string unityVersion = null)
        {
            if (_IsSupportedBrowser(browser, operatingSystem, _supportedPCBrowsersInfo))
            {
                return true;
            }

            if (_IsSupportedBrowser(browser, operatingSystem, _supportedMobileBrowsersInfo))
            {
                if (string.IsNullOrEmpty(unityVersion))
                {
                    unityVersion = SemanticVersion.ExtractSemanticVersionString(Application.unityVersion);
                }

                if (NoaDebuggerInfo.CompareVersions(_minimumUnityVersionForMobileBrowser, unityVersion) <= 0)
                {
                    return true;
                }
            }

            return false;
        }

        bool _IsSupportedBrowser(string browser, string operatingSystem, SupportBrowserInfo[] supportedBrowsersInfo)
        {
            foreach (SupportBrowserInfo supportedBrowserInfo in supportedBrowsersInfo)
            {
                if (browser.StartsWith(supportedBrowserInfo.Browser))
                {
                    foreach (string supportedOperatingSystem in supportedBrowserInfo.OperatingSystemPrefixList)
                    {
                        if (operatingSystem.StartsWith(supportedOperatingSystem))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        static int CompareVersions(string version1, string version2)
        {
            SemanticVersion sevVer1 = SemanticVersion.Parse(version1);
            SemanticVersion semVer2 = SemanticVersion.Parse(version2);
            return sevVer1.CompareTo(semVer2);
        }
    }

    [Serializable]
    sealed class SupportBrowserInfo
    {
        [SerializeField]
        string[] _operatingSystemPrefixList;

        [SerializeField]
        string _displayOperatingSystem;

        [SerializeField]
        string _browser;

        public string[] OperatingSystemPrefixList => _operatingSystemPrefixList;

        public string DisplayOperatingSystem => _displayOperatingSystem;

        public string Browser => _browser;
    }
}
