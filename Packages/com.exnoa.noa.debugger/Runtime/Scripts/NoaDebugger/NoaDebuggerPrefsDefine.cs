using System.Collections.Generic;

namespace NoaDebugger
{
    static class NoaDebuggerPrefsDefine
    {
        public static readonly string PrefsKeyStartButtonPortrait = "StartButtonPortrait";

        public static readonly string PrefsKeyStartButtonLandscape = "StartButtonLandscape";

        public static readonly string PrefsKeyInformationAutoRefresh = "InformationAutoRefresh";

        public static readonly KeyValuePair<string, bool> IsFpsProfilingKeyValue = new ("IsFpsProfiling", true);

        public static readonly KeyValuePair<string, bool> IsFrameTimeProfilingKeyValue = new ("IsFrameTimeProfiling", true);

        public static readonly KeyValuePair<string, bool> IsMemoryProfilingKeyValue = new ("IsMemoryProfiling", true);

        public static readonly KeyValuePair<string, bool> IsMemoryGraphShowingKeyValue = new ("IsMemoryGraphShowing", true);

        public static readonly KeyValuePair<string, NoaProfiler.MemoryProfilingType> MemoryProfilingTypeKeyValue = new ("MemoryProfilingType", NoaProfiler.MemoryProfilingType.Unity);

        public static readonly KeyValuePair<string, bool> IsRenderProfilingKeyValue = new ("IsRenderProfiling", true);

        public static readonly KeyValuePair<string, bool> IsRenderGraphShowingKeyValue = new ("IsRenderGraphShowing", true);

        public static readonly KeyValuePair<string, RenderingGraphTarget> RenderGraphTargetKeyValue = new ("RenderGraphTarget", RenderingGraphTarget.SetPassCalls);

        public static readonly string PrefsKeyIsProfilerWindowInfo = "IsProfilerWindowInfo";

        public static readonly string PrefsKeyIsSnapshotWindowInfo = "IsSnapshotWindowInfo";

        public static readonly string PrefsKeyIsConsoleLogFilterFlags = "IsConsoleLogFilterFlags";

        public static readonly string PrefsKeyIsApiLogFilterFlags = "IsApiLogFilterFlags";

        public static readonly string PrefsKeyIsConsoleLogWindowInfo = "IsConsoleLogWindowInfo";

        public static readonly string PrefsKeyIsApiLogWindowInfo = "IsApiLogWindowInfo";

        public static readonly string PrefsKeyIsTimerWindowInfo = "IsTimerWindowInfo";

        public static readonly string PrefsKeyIsHierarchyWindowInfo = "IsHierarchyWindowInfo";

        public static readonly string PrefsKeyIsDebugCommandWindowInfo = "IsDebugCommandWindowInfo";

        public static readonly string PrefsKeyLastDebugCommandCategoryName = "LastDebugCommandCategoryName";

        public static readonly string PrefsKeyDebugCommandGroupFilter = "DebugCommandGroupFilter";

        public static readonly string PrefsKeyDebugCommandGroupCollapsedStatus = "DebugCommandGroupCollapsedStatus";

        public static readonly string PrefsKeyDebugCommandGroupCollapsedStatusEditor = "DebugCommandGroupCollapsedStatusEditor";

        public static readonly string PrefsKeyIsConsoleLogCollecting = "IsConsoleLogCollecting";

        public static readonly string PrefsKeyIsApiLogCollecting = "IsApiLogCollecting";

        public static readonly string PrefsKeyDebugCommandAutoRefresh = "DebugCommandAutoRefresh";

        public static readonly string PrefsKeyPrefixConsoleLogOverlaySettings = "ConsoleLog";

        public static readonly string PrefsKeyPrefixApiLogOverlaySettings = "ApiLog";

        public static readonly string PrefsKeySuffixOverlayEnabled = "OverlayEnabled";

        public static readonly string PrefsKeySuffixOverlaySettings = "OverlaySettings";

        public static readonly string PrefsKeySuffixFeatureSettings = "FeatureSettings";

        public static readonly string PrefsKeyNoaDebuggerSettings = "NoaDebuggerSettings";

        public static readonly string PrefsKeyDebugCommandPropertiesPrefix = "DebugCommandProperty";

        public static readonly string PrefsKeyNoaPrefsDataPrefix = "NoaPrefsData";

        public static readonly string PrefsKeyDelimiter = "_";

        public static readonly string PrefsKeyGameSpeed = "GameSpeed";
    }
}
