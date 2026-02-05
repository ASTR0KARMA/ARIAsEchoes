using System;
using NoaDebugger.DebugCommand;
using UnityEngine;

namespace NoaDebugger
{
    static class NoaDebuggerDefine
    {
        public static readonly char[] NewLine = new[] {'\n', '\r'};

        public static string Ellipsis => "\u2026";

        public static readonly string InternalErrorStacktraceRegexPattern = @"^NoaDebugger\.";

        public static readonly string DebugCommandInvocationStacktraceRegexPattern =
            @"^NoaDebugger\.DebugCommand\."
            + @"("
            + @"GetOnlyPropertyCommand[:\.]GetValue"
            + @"|"
            + @"MutablePropertyCommandBase`1\[T\][:\.]Invoke[GS]etter"
            + @"|"
            + @"(Method|Coroutine|HandleMethod)Command[:\.]Invoke"
            + @")\b";

        public static readonly string RootObjectName = "NoaDebuggerRoot";

        public static string EditorPrefsKeyPackageSettingsData =>
            $"{Application.productName}:PACKAGE_SETTINGS_DATA";

        public const ButtonPosition DEFAULT_START_BUTTON_POSITION = ButtonPosition.LowerLeft;

        public const float DEFAULT_START_BUTTON_SCALE = 1.0f;

        public static readonly float StartButtonScaleMin = 0.5f;

        public static readonly float StartButtonScaleMax = 1.0f;

        public const ButtonMovementType DEFAULT_START_BUTTON_MOVEMENT_TYPE = ButtonMovementType.Draggable;

        public const bool DEFAULT_SAVE_START_BUTTON_POSITION = true;

        public const float DEFAULT_TOOL_START_BUTTON_ALPHA = 0.6f;

        public static readonly float ToolStartButtonAlphaMin = 0.0f;

        public static readonly float ToolStartButtonAlphaMax = 1.0f;

        public const float DEFAULT_CANVAS_ALPHA = 0.7f;

        public const float DEFAULT_CONTROLLER_BACKGROUND_ALPHA = 0.7f;

        public static readonly float CanvasAlphaMin = 0.1f;

        public static readonly float CanvasAlphaMax = 1;

        public const float DEFAULT_NOA_DEBUGGER_CANVAS_SCALE = 1.0f;

        public static readonly float CanvasScaleMin = 0.8f;

        public static readonly float CanvasScaleMax = 1.2f;

        public const int DEFAULT_NOA_DEBUGGER_CANVAS_SORT_ORDER = 1000;

        public const bool DEFAULT_IS_UI_REVERSE_PORTRAIT = true;

        public const bool DEFAULT_IS_SHOW_SIDE_MENU_CLOSE_BUTTON = true;

        public static readonly float InformationAutoRefreshInterval = 1f;

        public static readonly int ProfilerChartHistoryCount = 600;

        public static readonly string[] VSyncCountChoices =
        {
            "Don't Sync",
            "Every V Blank",
            "Every Second V Blank",
            "Every Third V Blank",
            "Every Fourth V Blank"
        };

        public const bool DEFAULT_SAVE_INFORMATION_VALUE = false;

        public static readonly int MaxNumberOfMatchingLogsToDisplay = 99;

        public const int DEFAULT_CONSOLE_LOG_COUNT = 999;

        public static readonly int ConsoleLogCountMin = 99;

        public static readonly int ConsoleLogCountMax = 999;

        public static readonly int ConsoleLogSummaryStringLengthMax = 256;

        public static readonly string UnavaliableStackTraceLabel = "-- No stack trace available --";

        public const int DEFAULT_API_LOG_COUNT = 999;

        public static readonly int ApiLogCountMin = 99;

        public static readonly int ApiLogCountMax = 999;

        public static readonly int SnapshotLogMaxLabelCharNum = 30;

        public static readonly float PressTimeSeconds = 0.5f;

        public static readonly int PressActionIntervalChangeCount = 5;

        public static readonly float PressActionFirstInterval = 0.2f;

        public static readonly float PressActionSecondInterval = 0.1f;

        public static readonly float DragThresholdDistanceOnScreen = 50f;

        public const int DEFAULT_INT_SETTINGS_INCREMENT = 1;

        public const float DEFAULT_FLOAT_SETTINGS_INCREMENT = 0.1f;

        public const bool DEFAULT_AUTO_CREATE_EVENT_SYSTEM = true;

        public const ErrorNotificationType DEFAULT_ERROR_NOTIFICATION_TYPE = ErrorNotificationType.Full;

        public const bool DEFAULT_AUTO_INITIALIZE = true;

        public const string DEFAULT_CUSTOM_MENU_RESOURCES_FOLDER_PATH = "Assets/NoaDebuggerCustomMenu/Resources";

        public const bool DEFAULT_IS_CUSTOM_FONT_SETTINGS_ENABLED = false;

        public const float DEFAULT_FONT_SIZE_RATE = 1f;

        public const int DEFAULT_HIERARCHY_LEVELS = 3;

        public static readonly int HierarchyLevelsMin = 1;

        public static readonly int HierarchyLevelsMax = 10;

        public static readonly float DebugCommandAutoRefreshInterval = 1f;

        public const CommandDisplayFormat DEFAULT_COMMAND_FORMAT_LANDSCAPE = CommandDisplayFormat.Panel;

        public const CommandDisplayFormat DEFAULT_COMMAND_FORMAT_PORTRAIT = CommandDisplayFormat.List;

        public static readonly bool DefaultOverlayEnabled = false;

        public const float DEFAULT_OVERLAY_BACKGROUND_OPACITY = 0.6f;

        public static readonly float OverlayBackgroundOpacityMin = 0.1f;

        public static readonly float OverlayBackgroundOpacityMax = 1.0f;

        public const bool DEFAULT_OVERLAY_SAFE_AREA_APPLICABLE = true;

        public const int DEFAULT_OVERLAY_PADDING_X = 0;

        public const int DEFAULT_OVERLAY_PADDING_Y = 0;

        public const int OVERLAY_PADDING_MIN = int.MinValue;

        public const int OVERLAY_PADDING_MAX = int.MaxValue;

        public const NoaDebug.OverlayPosition DEFAULT_PROFILER_OVERLAY_POSITION = NoaDebug.OverlayPosition.UpperRight;

        public const float DEFAULT_PROFILER_OVERLAY_SCALE = 1.0f;

        public static readonly float ProfilerOverlayScaleMin = 0.5f;

        public static readonly float ProfilerOverlayScaleMax = 1.5f;

        public static readonly float ProfilerOverlayScaleIncrement = 0.1f;

        public const NoaProfiler.OverlayAxis DEFAULT_PROFILER_OVERLAY_AXIS = NoaProfiler.OverlayAxis.Vertical;

        public static readonly bool DefaultProfilerOverlayFeatureEnabled = true;

        public static readonly NoaProfiler.OverlayTextType DefaultProfilerOverlayTextType = NoaProfiler.OverlayTextType.Full;

        public static readonly bool DefaultProfilerOverlayGraphEnabled = true;

        public const NoaDebug.OverlayPosition DEFAULT_LOG_OVERLAY_POSITION = NoaDebug.OverlayPosition.UpperLeft;

        public const float DEFAULT_LOG_OVERLAY_FONT_SCALE = 1.0f;

        public static readonly float LogOverlayFontScaleMin = 0.5f;

        public static readonly float LogOverlayFontScaleMax = 1.5f;

        public static readonly float LogOverlayFontScaleIncrement = 0.1f;

        public const int DEFAULT_LOG_OVERLAY_MAXIMUM_LOG_COUNT = 10;

        public static readonly int LogOverlayMaximumLogCountMin = 10;

        public static readonly int LogOverlayMaximumLogCountMax = 30;

        public static readonly int LogOverlayMaximumLogCountIncrement = 10;

        public const float DEFAULT_LOG_OVERLAY_MINIMUM_OPACITY = 0.5f;

        public static readonly float LogOverlayMinimumOpacityMin = 0;

        public static readonly float LogOverlayMinimumOpacityMax = 1.0f;

        public static readonly float LogOverlayMinimumOpacityIncrement = 0.1f;

        public const float DEFAULT_LOG_OVERLAY_ACTIVE_DURATION = 3.0f;

        public static readonly float LogOverlayActiveDurationMin = 1.0f;

        public static readonly float LogOverlayActiveDurationMax = 5.0f;

        public static readonly float LogOverlayActiveDurationIncrement = 1f;

        public const bool DEFAULT_LOG_OVERLAY_SHOW_TIMESTAMP = true;

        public const bool DEFAULT_LOG_OVERLAY_SHOW_LOGS = false;

        public const bool DEFAULT_UI_ELEMENT_SAFE_AREA_APPLICABLE = true;

        public const int DEFAULT_UI_ELEMENT_PADDING_X = 0;

        public const int DEFAULT_UI_ELEMENT_PADDING_Y = 0;

        public const int UI_ELEMENT_PADDING_MIN = int.MinValue;

        public const int UI_ELEMENT_PADDING_MAX = int.MaxValue;

        public const float DEFAULT_GAME_SPEED = 1.0f;

        public const float MIN_GAME_SPEED = 0.1f;

        public const bool DEFAULT_GAME_SPEED_CHANGE_APPLICABLE = true;

        public const float DEFAULT_GAME_SPEED_INCREMENT = 1.0f;

        public const float MIN_GAME_SPEED_INCREMENT = 0.1f;

        public const float MAX_GAME_SPEED_INCREMENT = 1.0f;

        public const float DEFAULT_MAX_GAME_SPEED = 3.0f;

        public const float MIN_CONFIGURABLE_MAX_GAME_SPEED = 1.0f;

        public const float MAX_CONFIGURABLE_MAX_GAME_SPEED = 5.0f;

        public const int GAME_SPEED_SETTINGS_SIGNIFICANT_FRACTIONAL_DIGITS = 1;

        public const bool DEFAULT_AUTO_SAVE = true;

        public const bool DEFAULT_ENABLE_ALL_SHORTCUTS = true;

        public const int NUMERIC_INPUT_DRAG_SENSITIVITY = 2;

        public const string SETTINGS_ENABLED_VALUE = "enabled";

        public const string SETTINGS_DISABLED_VALUE = "disabled";

        public const string MISSING_VALUE = "Not Supported";

        public static readonly string SupportedValue = "Supported";

        public static readonly string HyphenValue = "-";

        public static readonly char NoHasFontAssetReplacementCharacter = '_';

        public static readonly string ClipboardCopiedText = "Copied to clipboard";

        public static readonly string ClipboardCopyFailedText = "Copying to clipboard failed.";

        public static readonly string DownloadCompletedText = "Download completed";

        public static readonly string DownloadCanceledText = "Download canceled";

        public static readonly string DownloadFailedText = "Download failed";

        public static readonly string DeleteSaveDataText = "Save data deleted";

        public static readonly string ShowErrorText = "Sorry, an error occurred";

        public static readonly string CaptureScreenshotText = "Captured a screenshot";

        public static readonly string InformationSentText = "Information sent";

        public static readonly string LogSentText = "Log sent";

        public static readonly string HideNoaDebuggerUIText = "Hid the NOA Debugger-related UI.\nTo show it again, tap where the trigger button was located.";

        public static readonly string TapCustomActionText = "Tapped custom action (F{0})";

        public static readonly string LongPressCustomActionText = "Long pressed custom action (F{0})";

        public static readonly string ToggledOnCustomActionText = "Toggled on custom action (F{0})";

        public static readonly string ToggledOffCustomActionText = "Toggled off custom action (F{0})";

        public static readonly string TransitionToInitialSceneText = "Transition to application initial scene";

        public static readonly string CustomApplicationResetText = "Execute custom application reset";

        public static readonly string UnmarkedSnapshotCategoryNameToSetAdditionalInfo = "Others";

        public static readonly string[] SnapshotDuplicateCategoryNames =
            {"FPS", "Memory", "Rendering", "Battery", "Thermal"};

        public static readonly string[] SnapshotDownloadLogInfoLabels = {"_label", "_time", "_elapsedTime"};

        public static readonly string SnapshotDuplicateCategoryNamePrefix = "[Additional]";

        public static readonly char DecimalPoint = '.';

        public static readonly int ShortcutInvalidKey = -1;

        public const int INFORMATION_MENU_SORT_NO = 0;
        public const int PROFILER_MENU_SORT_NO = 1;
        public const int SNAPSHOT_MENU_SORT_NO = 2;
        public const int CONSOLE_LOG_MENU_SORT_NO = 3;
        public const int API_LOG_MENU_SORT_NO = 4;
        public const int HIERARCHY_MENU_SORT_NO = 5;
        public const int COMMAND_MENU_SORT_NO = 6;

        public const int CUSTOM_MENU_SORT_NO = 0;

        public static readonly ShortcutActionType[] SortedShortcutActionType =
        {
            ShortcutActionType.ToggleDebugger,
            ShortcutActionType.ToggleOverlay,
            ShortcutActionType.ToggleFloatingWindow,
            ShortcutActionType.ToggleTriggerButton,
            ShortcutActionType.ToggleUIElements,
            ShortcutActionType.ToggleAllUI,
            ShortcutActionType.ResetApplication,
            ShortcutActionType.CaptureScreenshot,
            ShortcutActionType.TogglePauseResume,
            ShortcutActionType.DecreaseGameSpeed,
            ShortcutActionType.IncreaseGameSpeed,
            ShortcutActionType.FrameStepping,
            ShortcutActionType.ResetGameSpeed,
            ShortcutActionType.CustomAction1,
            ShortcutActionType.CustomAction2,
            ShortcutActionType.CustomAction3,
            ShortcutActionType.CustomAction4,
            ShortcutActionType.CustomAction5,
        };

        [Flags]
        public enum ShortCutTriggerType
        {
            ShortPress = 1 << 0,  
            LongPress = 1 << 1,   
            HoldDown = 1 << 2,    
            ShortAndLongPress = ShortPress | LongPress, 
            ShortAndHoldDown = ShortPress | HoldDown, 
        }

        public static ShortCutTriggerType GetTriggerType(ShortcutActionType actionType)
        {
            return actionType switch
            {
                ShortcutActionType.ToggleDebugger => ShortCutTriggerType.ShortPress,
                ShortcutActionType.ToggleOverlay => ShortCutTriggerType.ShortPress,
                ShortcutActionType.ToggleFloatingWindow => ShortCutTriggerType.ShortPress,
                ShortcutActionType.ToggleTriggerButton => ShortCutTriggerType.ShortPress,
                ShortcutActionType.ToggleUIElements => ShortCutTriggerType.ShortPress,
                ShortcutActionType.ToggleAllUI => ShortCutTriggerType.ShortPress,
                ShortcutActionType.ResetApplication => ShortCutTriggerType.LongPress,
                ShortcutActionType.CaptureScreenshot => ShortCutTriggerType.ShortPress,
                ShortcutActionType.TogglePauseResume => ShortCutTriggerType.ShortPress,
                ShortcutActionType.DecreaseGameSpeed => ShortCutTriggerType.ShortAndLongPress,
                ShortcutActionType.IncreaseGameSpeed => ShortCutTriggerType.ShortAndLongPress,
                ShortcutActionType.FrameStepping => ShortCutTriggerType.ShortAndHoldDown,
                ShortcutActionType.ResetGameSpeed => ShortCutTriggerType.LongPress,
                ShortcutActionType.CustomAction1 => ShortCutTriggerType.ShortAndLongPress,
                ShortcutActionType.CustomAction2 => ShortCutTriggerType.ShortAndLongPress,
                ShortcutActionType.CustomAction3 => ShortCutTriggerType.ShortAndLongPress,
                ShortcutActionType.CustomAction4 => ShortCutTriggerType.ShortAndLongPress,
                ShortcutActionType.CustomAction5 => ShortCutTriggerType.ShortAndLongPress,
#if NOA_DEBUGGER_DEBUG
                _ => throw new Exception("TriggerTypeが設定されていないActionTypeです。定義を追加してください。")
#else
                _ => ShortCutTriggerType.ShortPress
#endif
            };
        }

        public struct TextColors
        {
            public static readonly Color Default = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
            public static readonly Color Success = new Color32(0x4A, 0xFF, 0xF3, 0xFF);
            public static readonly Color Warning = new Color32(0xFF, 0x8D, 0x00, 0xFF);
            public static readonly Color Dynamic = new Color32(0x00, 0xFF, 0x5A, 0xFF);
            public static readonly Color InProgress = new Color32(0xFF, 0xFF, 0x00, 0xFF);

            public static readonly Color ProfilerFps = new Color32(0x00, 0xFB, 0x57, 0xFF);
            public static readonly Color ProfilerMemory = new Color32(0x47, 0xBC, 0xFF, 0xFF);
            public static readonly Color ProfilerRendering = new Color32(0xFF, 0x7A, 0x27, 0xFF);

            public static readonly Color LogGray = new Color32(0xBA, 0xBA, 0xBA, 0xFF);
            public static readonly Color LogLightBlue = new Color32(0x42, 0xA9, 0xBC, 0xFF);
            public static readonly Color LogOrange = new Color32(0xC3, 0x8C, 0x50, 0xFF);
            public static readonly Color LogYellow = new Color32(0xC0, 0xB9, 0x3D, 0xFF);
            public static readonly Color LogBlue = new Color32(0x55, 0x71, 0xD4, 0xFF);
            public static readonly Color LogPurple = new Color32(0xB4, 0x18, 0xBC, 0xFF);
            public static readonly Color LogGreen = new Color32(0x57, 0xB2, 0x50, 0xFF);
            public static readonly Color LogRed = new Color32(0xDE, 0x19, 0x26, 0xFF);

            public static readonly string StackTraceColorCode = "BABABA";
        }

        public struct ImageColors
        {
            public static readonly Color Default = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
            public static readonly Color Disabled = new Color32(0xA7, 0xA7, 0xA7, 0xFF);
            public static readonly Color Clear = new Color32(0xFF, 0xFF, 0xFF, 0x00);
            public static readonly Color Warning = new Color32(0xFF, 0x8D, 0x00, 0xFF);
            public static readonly Color SnapshotFirstSelected = new Color32(0x4D, 0xD6, 0xE8, 0xFF);
            public static readonly Color SnapshotSecondSelected = new Color32(0x26, 0x5B, 0x78, 0xFF);
        }

        public struct BackgroundColors
        {
            public static readonly Color NoaDebuggerButtonAlert = new Color32(0xFF, 0x00, 0x00, 0xFF);
            public static readonly Color NoaDebuggerButtonDefault = new Color32(0x4B, 0x4B, 0x4B, 0xFF);
            public static readonly Color NoaDebuggerButtonInControllerDefault = new Color32(0x4B, 0x4B, 0x4B, 0x9B);
            public static readonly Color LogBright = new Color32(0x7A, 0x7A, 0x7A, 0xC0);
            public static readonly Color LogDark = new Color32(0x67, 0x67, 0x67, 0x9B);
            public static readonly Color LogBrown = new Color32(0xFF, 0x80, 0x80, 0x9B);
            public static readonly Color LogDarkBrown = new Color32(0xE6, 0xA5, 0x72, 0x9B);
            public static readonly Color LogGreen = new Color32(0x8A, 0xCC, 0x66, 0x9B);
            public static readonly Color LogLightBlue = new Color32(0x80, 0xB5, 0xFF, 0x9B);
            public static readonly Color LogDarkPurple = new Color32(0x99, 0x4C, 0x92, 0x9B);
            public static readonly Color LogBlack = new Color32(0x33, 0x10, 0x04, 0x9B);
            public static readonly Color LogDarkGreen = new Color32(0x4C, 0x34, 0x0F, 0x9B);
            public static readonly Color LogYellowGreen = new Color32(0x99, 0x93, 0x00, 0x9B);
            public static readonly Color LogBlue = new Color32(0x1F, 0x5B, 0x99, 0x9B);
            public static readonly Color LogPurple = new Color32(0xFF, 0x4C, 0xEE, 0x9B);
        }

        public enum LogType
        {
            Error,
            Warning,
            Log,
        }

        public struct LogColors
        {
            public static readonly Color LogError = new Color32(188, 0, 0, 255);
            public static readonly Color LogWarning = new Color32(203, 198, 17, 255);
            public static readonly Color LogMessage = new Color32(200, 200, 200, 255);
        }
    }

    public enum ErrorNotificationType
    {
        Full, 
        Flashing, 
        None 
    }
}
