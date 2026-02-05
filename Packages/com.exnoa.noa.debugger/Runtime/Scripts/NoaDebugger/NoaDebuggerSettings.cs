using JetBrains.Annotations;
using NoaDebugger.DebugCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace NoaDebugger
{
    sealed class NoaDebuggerSettings : ScriptableObject
    {
        const string DEFAULT_OLD_CUSTOM_MENU_FOLDER_PATH = "Assets/NoaDebuggerSettings";

        const string DEFAULT_OLD_NOA_DEBUGGER_SETTINGS_PATH = "Assets/NoaDebuggerSettings/Resources/NoaDebuggerSettings.asset";

        const string DEFAULT_CUSTOM_MENU_FOLDER_PATH = "Assets/NoaDebuggerCustomMenu";

        const string DEFAULT_NOA_DEBUGGER_SETTINGS_PATH = "ProjectSettings/NoaDebuggerSettings.asset";

        public ButtonPosition StartButtonPosition
        {
            get => _startButtonPosition;
            set => _startButtonPosition = value;
        }
        [SerializeField]
        ButtonPosition _startButtonPosition = NoaDebuggerDefine.DEFAULT_START_BUTTON_POSITION;

        public float StartButtonScale
        {
            get => _startButtonScale;
            set => _startButtonScale = value;
        }
        [SerializeField]
        float _startButtonScale = NoaDebuggerDefine.DEFAULT_START_BUTTON_SCALE;

        public ButtonMovementType StartButtonMovementType
        {
            get => _startButtonMovementType;
            set => _startButtonMovementType = value;
        }
        [SerializeField]
        ButtonMovementType _startButtonMovementType = NoaDebuggerDefine.DEFAULT_START_BUTTON_MOVEMENT_TYPE;

        public bool SaveStartButtonPosition
        {
            get => _saveStartButtonPosition;
            set => _saveStartButtonPosition = value;
        }
        [SerializeField]
        bool _saveStartButtonPosition = NoaDebuggerDefine.DEFAULT_SAVE_START_BUTTON_POSITION;

        public float ToolStartButtonAlpha
        {
            get => _toolStartButtonAlpha;
            set => _toolStartButtonAlpha = value;
        }
        [SerializeField]
        float _toolStartButtonAlpha = NoaDebuggerDefine.DEFAULT_TOOL_START_BUTTON_ALPHA;

        public float BackgroundAlpha
        {
            get => _backgroundAlpha;
            set => _backgroundAlpha = value;
        }
        [SerializeField]
        float _backgroundAlpha = NoaDebuggerDefine.DEFAULT_CANVAS_ALPHA;

        public float FloatingWindowAlpha
        {
            get => _floatingWindowAlpha;
            set => _floatingWindowAlpha = value;
        }
        [SerializeField]
        float _floatingWindowAlpha = NoaDebuggerDefine.DEFAULT_CANVAS_ALPHA;

        public float ControllerBackgroundAlpha
        {
            get => _controllerBackgroundAlpha;
            set => _controllerBackgroundAlpha = value;
        }
        [SerializeField]
        float _controllerBackgroundAlpha = NoaDebuggerDefine.DEFAULT_CONTROLLER_BACKGROUND_ALPHA;

        public float NoaDebuggerCanvasScale
        {
            get => _noaDebuggerCanvasScale;
            set => _noaDebuggerCanvasScale = value;
        }
        [SerializeField]
        float _noaDebuggerCanvasScale = NoaDebuggerDefine.DEFAULT_NOA_DEBUGGER_CANVAS_SCALE;

        public int NoaDebuggerCanvasSortOrder
        {
            get => _noaDebuggerCanvasSortOrder;
            set => _noaDebuggerCanvasSortOrder = value;
        }
        [SerializeField]
        int _noaDebuggerCanvasSortOrder = NoaDebuggerDefine.DEFAULT_NOA_DEBUGGER_CANVAS_SORT_ORDER;

        public bool IsUIReversePortrait
        {
            get => _isUIReversePortrait;
            set => _isUIReversePortrait = value;
        }
        [SerializeField]
        bool _isUIReversePortrait = NoaDebuggerDefine.DEFAULT_IS_UI_REVERSE_PORTRAIT;

        public bool IsShowSideMenuCloseButton
        {
            get => _isShowSideMenuCloseButton;
            set => _isShowSideMenuCloseButton = value;
        }
        [SerializeField]
        bool _isShowSideMenuCloseButton = NoaDebuggerDefine.DEFAULT_IS_SHOW_SIDE_MENU_CLOSE_BUTTON;

        public List<MenuInfo> MenuList
        {
            get => _menuList;
            set => _menuList = value;
        }
        [SerializeField]
        List<MenuInfo> _menuList;

        public bool AutoCreateEventSystem
        {
            get => _autoCreateEventSystem;
            set => _autoCreateEventSystem = value;
        }
        [SerializeField]
        bool _autoCreateEventSystem = NoaDebuggerDefine.DEFAULT_AUTO_CREATE_EVENT_SYSTEM;

        public ErrorNotificationType ErrorNotificationType
        {
            get => _errorNotificationType;
            set => _errorNotificationType = value;
        }
        [SerializeField]
        ErrorNotificationType _errorNotificationType = NoaDebuggerDefine.DEFAULT_ERROR_NOTIFICATION_TYPE;

        public bool AutoInitialize
        {
            get => _autoInitialize;
            set => _autoInitialize = value;
        }
        [SerializeField]
        bool _autoInitialize = NoaDebuggerDefine.DEFAULT_AUTO_INITIALIZE;

        public string CustomMenuFolderPath
        {
            get => _customMenuFolderPath;
            set => _customMenuFolderPath = value;
        }
        [SerializeField]
        string _customMenuFolderPath = NoaDebuggerDefine.DEFAULT_CUSTOM_MENU_RESOURCES_FOLDER_PATH;

        public List<CustomMenuInfo> CustomMenuList
        {
            get => _customMenuList;
            set => _customMenuList = value;
        }
        [SerializeField]
        List<CustomMenuInfo> _customMenuList;

        public bool SaveInformationValue
        {
            get => _saveInformationValue;
            set => _saveInformationValue = value;
        }
        [SerializeField]
        bool _saveInformationValue = NoaDebuggerDefine.DEFAULT_SAVE_INFORMATION_VALUE;

        public int ConsoleLogCount
        {
            get => _consoleLogCount;
            set => _consoleLogCount = value;
        }
        [SerializeField]
        int _consoleLogCount = NoaDebuggerDefine.DEFAULT_CONSOLE_LOG_COUNT;

        public int ApiLogCount
        {
            get => _apiLogCount;
            set => _apiLogCount = value;
        }
        [SerializeField]
        int _apiLogCount = NoaDebuggerDefine.DEFAULT_API_LOG_COUNT;

        public bool IsCustomFontSpecified => IsCustomFontSettingsEnabled && FontAsset != null;

        public bool IsCustomFontSettingsEnabled
        {
            get => _isCustomFontSettingsEnabled;
            set => _isCustomFontSettingsEnabled = value;
        }
        [SerializeField]
        bool _isCustomFontSettingsEnabled = NoaDebuggerDefine.DEFAULT_IS_CUSTOM_FONT_SETTINGS_ENABLED;

        public TMP_FontAsset FontAsset
        {
            get => _fontAsset;
            set => _fontAsset = value;
        }
        [SerializeField]
        TMP_FontAsset _fontAsset;

        public Material FontMaterial
        {
            get => _fontMaterial;
            set => _fontMaterial = value;
        }
        [SerializeField]
        Material _fontMaterial;

        public float FontSizeRate
        {
            get => _fontSizeRate;
            set => _fontSizeRate = value;
        }
        [SerializeField]
        float _fontSizeRate = NoaDebuggerDefine.DEFAULT_FONT_SIZE_RATE;

        public int HierarchyLevels
        {
            get => _hierarchyLevels;
            set => _hierarchyLevels = value;
        }
        [SerializeField]
        int _hierarchyLevels = NoaDebuggerDefine.DEFAULT_HIERARCHY_LEVELS;

        public CommandDisplayFormat CommandFormatLandscape
        {
            get => _commandFormatLandscape;
            set => _commandFormatLandscape = value;
        }
        [SerializeField]
        CommandDisplayFormat _commandFormatLandscape = NoaDebuggerDefine.DEFAULT_COMMAND_FORMAT_LANDSCAPE;

        public CommandDisplayFormat CommandFormatPortrait
        {
            get => _commandFormatPortrait;
            set => _commandFormatPortrait = value;
        }
        [SerializeField]
        CommandDisplayFormat _commandFormatPortrait = NoaDebuggerDefine.DEFAULT_COMMAND_FORMAT_PORTRAIT;

        public float OverlayBackgroundOpacity
        {
            get => _overlayBackgroundOpacity;
            set => _overlayBackgroundOpacity = value;
        }
        [SerializeField]
        float _overlayBackgroundOpacity = NoaDebuggerDefine.DEFAULT_OVERLAY_BACKGROUND_OPACITY;

        public bool AppliesOverlaySafeArea
        {
            get => _appliesOverlaySafeArea;
            set => _appliesOverlaySafeArea = value;
        }
        [SerializeField]
        bool _appliesOverlaySafeArea = NoaDebuggerDefine.DEFAULT_OVERLAY_SAFE_AREA_APPLICABLE;

        public Vector2 OverlayPadding
        {
            get => _overlayPadding;
            set => _overlayPadding = value;
        }
        [SerializeField]
        Vector2 _overlayPadding = new Vector2(NoaDebuggerDefine.DEFAULT_OVERLAY_PADDING_X, NoaDebuggerDefine.DEFAULT_OVERLAY_PADDING_Y);

        public NoaDebug.OverlayPosition ProfilerOverlayPosition
        {
            get => _profilerOverlayPosition;
            set => _profilerOverlayPosition = value;
        }
        [SerializeField]
        NoaDebug.OverlayPosition _profilerOverlayPosition = NoaDebuggerDefine.DEFAULT_PROFILER_OVERLAY_POSITION;

        public float ProfilerOverlayScale
        {
            get => _profilerOverlayScale;
            set => _profilerOverlayScale = value;
        }
        [SerializeField]
        float _profilerOverlayScale = NoaDebuggerDefine.DEFAULT_PROFILER_OVERLAY_SCALE;

        public NoaProfiler.OverlayAxis ProfilerOverlayAxis
        {
            get => _profilerOverlayAxis;
            set => _profilerOverlayAxis = value;
        }
        [SerializeField]
        NoaProfiler.OverlayAxis _profilerOverlayAxis = NoaDebuggerDefine.DEFAULT_PROFILER_OVERLAY_AXIS;

        public ProfilerOverlayFeatureSettings ProfilerOverlayFpsSettings
        {
            get => _profilerOverlayFpsSettings;
            set => _profilerOverlayFpsSettings = value;
        }
        [SerializeField]
        ProfilerOverlayFeatureSettings _profilerOverlayFpsSettings;

        public ProfilerOverlayFeatureSettings ProfilerOverlayMemorySettings
        {
            get => _profilerOverlayMemorySettings;
            set => _profilerOverlayMemorySettings = value;
        }
        [SerializeField]
        ProfilerOverlayFeatureSettings _profilerOverlayMemorySettings;

        public ProfilerOverlayFeatureSettings ProfilerOverlayRenderingSettings
        {
            get => _profilerOverlayRenderingSettings;
            set => _profilerOverlayRenderingSettings = value;
        }
        [SerializeField]
        ProfilerOverlayFeatureSettings _profilerOverlayRenderingSettings;

        public NoaDebug.OverlayPosition ConsoleLogOverlayPosition
        {
            get => _consoleLogOverlayPosition;
            set => _consoleLogOverlayPosition = value;
        }
        [SerializeField]
        NoaDebug.OverlayPosition _consoleLogOverlayPosition = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_POSITION;

        public float ConsoleLogOverlayFontScale
        {
            get => _consoleLogOverlayFontSize;
            set => _consoleLogOverlayFontSize = value;
        }
        [SerializeField]
        float _consoleLogOverlayFontSize = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_FONT_SCALE;

        public int ConsoleLogOverlayMaximumLogCount
        {
            get => _consoleLogOverlayMaximumLogCount;
            set => _consoleLogOverlayMaximumLogCount = value;
        }
        [SerializeField]
        int _consoleLogOverlayMaximumLogCount = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_MAXIMUM_LOG_COUNT;

        public float ConsoleLogOverlayMinimumOpacity
        {
            get => _consoleLogOverlayMinimumOpacity;
            set => _consoleLogOverlayMinimumOpacity = value;
        }
        [SerializeField]
        float _consoleLogOverlayMinimumOpacity = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_MINIMUM_OPACITY;

        public float ConsoleLogOverlayActiveDuration
        {
            get => _consoleLogOverlayActiveDuration;
            set => _consoleLogOverlayActiveDuration = value;
        }
        [SerializeField]
        float _consoleLogOverlayActiveDuration = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_ACTIVE_DURATION;

        public bool ConsoleLogOverlayShowTimestamp
        {
            get => _consoleLogOverlayShowTimestamp;
            set => _consoleLogOverlayShowTimestamp = value;
        }
        [SerializeField]
        bool _consoleLogOverlayShowTimestamp = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_SHOW_TIMESTAMP;

        public bool ConsoleLogOverlayShowMessageLogs
        {
            get => _consoleLogOverlayShowMessageLogs;
            set => _consoleLogOverlayShowMessageLogs = value;
        }
        [SerializeField]
        bool _consoleLogOverlayShowMessageLogs = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_SHOW_LOGS;

        public bool ConsoleLogOverlayShowWarningLogs
        {
            get => _consoleLogOverlayShowWarningLogs;
            set => _consoleLogOverlayShowWarningLogs = value;
        }
        [SerializeField]
        bool _consoleLogOverlayShowWarningLogs = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_SHOW_LOGS;

        public bool ConsoleLogOverlayShowErrorLogs
        {
            get => _consoleLogOverlayShowErrorLogs;
            set => _consoleLogOverlayShowErrorLogs = value;
        }
        [SerializeField]
        bool _consoleLogOverlayShowErrorLogs = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_SHOW_LOGS;

        public NoaDebug.OverlayPosition ApiLogOverlayPosition
        {
            get => _apiLogOverlayPosition;
            set => _apiLogOverlayPosition = value;
        }
        [SerializeField]
        NoaDebug.OverlayPosition _apiLogOverlayPosition = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_POSITION;

        public float ApiLogOverlayFontScale
        {
            get => _apiLogOverlayFontSize;
            set => _apiLogOverlayFontSize = value;
        }
        [SerializeField]
        float _apiLogOverlayFontSize = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_FONT_SCALE;

        public int ApiLogOverlayMaximumLogCount
        {
            get => _apiLogOverlayMaximumLogCount;
            set => _apiLogOverlayMaximumLogCount = value;
        }
        [SerializeField]
        int _apiLogOverlayMaximumLogCount = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_MAXIMUM_LOG_COUNT;

        public float ApiLogOverlayMinimumOpacity
        {
            get => _apiLogOverlayMinimumOpacity;
            set => _apiLogOverlayMinimumOpacity = value;
        }
        [SerializeField]
        float _apiLogOverlayMinimumOpacity = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_MINIMUM_OPACITY;

        public float ApiLogOverlayActiveDuration
        {
            get => _apiLogOverlayActiveDuration;
            set => _apiLogOverlayActiveDuration = value;
        }
        [SerializeField]
        float _apiLogOverlayActiveDuration = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_ACTIVE_DURATION;

        public bool ApiLogOverlayShowTimestamp
        {
            get => _apiLogOverlayShowTimestamp;
            set => _apiLogOverlayShowTimestamp = value;
        }
        [SerializeField]
        bool _apiLogOverlayShowTimestamp = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_SHOW_TIMESTAMP;

        public bool ApiLogOverlayShowMessageLogs
        {
            get => _apiLogOverlayShowMessageLogs;
            set => _apiLogOverlayShowMessageLogs = value;
        }
        [SerializeField]
        bool _apiLogOverlayShowMessageLogs = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_SHOW_LOGS;

        public bool ApiLogOverlayShowErrorLogs
        {
            get => _apiLogOverlayShowErrorLogs;
            set => _apiLogOverlayShowErrorLogs = value;
        }
        [SerializeField]
        bool _apiLogOverlayShowErrorLogs = NoaDebuggerDefine.DEFAULT_LOG_OVERLAY_SHOW_LOGS;

        public bool AppliesUIElementSafeArea
        {
            get => _appliesUIElementSafeArea;
            set => _appliesUIElementSafeArea = value;
        }
        [SerializeField]
        bool _appliesUIElementSafeArea = NoaDebuggerDefine.DEFAULT_UI_ELEMENT_SAFE_AREA_APPLICABLE;

        public Vector2 UIElementPadding
        {
            get => _uiElementPadding;
            set => _uiElementPadding = value;
        }
        [SerializeField]
        Vector2 _uiElementPadding = new Vector2(NoaDebuggerDefine.DEFAULT_UI_ELEMENT_PADDING_X, NoaDebuggerDefine.DEFAULT_UI_ELEMENT_PADDING_Y);

        public bool AppliesGameSpeedChange
        {
            get => _appliesGameSpeedChange;
            set => _appliesGameSpeedChange = value;
        }
        [SerializeField]
        bool _appliesGameSpeedChange = NoaDebuggerDefine.DEFAULT_GAME_SPEED_CHANGE_APPLICABLE;

        public float GameSpeedIncrement
        {
            get => _gameSpeedIncrement;
            set => _gameSpeedIncrement = value;
        }
        [SerializeField]
        float _gameSpeedIncrement = NoaDebuggerDefine.DEFAULT_GAME_SPEED_INCREMENT;

        public float MaxGameSpeed
        {
            get => _maxGameSpeed;
            set => _maxGameSpeed = value;
        }
        [SerializeField]
        float _maxGameSpeed = NoaDebuggerDefine.DEFAULT_MAX_GAME_SPEED;

        public bool EnableAllShortcuts
        {
            get => _enableAllShortcuts;
            set => _enableAllShortcuts = value;
        }
        [SerializeField]
        bool _enableAllShortcuts = NoaDebuggerDefine.DEFAULT_ENABLE_ALL_SHORTCUTS;

        public List<ShortcutAction> EnabledShortcutActions
        {
            get
            {
                if (UnityInputUtils.IsEnableInputSystem)
                {
                    return _shortcutActions;
                }
                else
                {
                    return _shortcutActionsOnInputManager;
                }
            }
            set
            {
                if (UnityInputUtils.IsEnableInputSystem)
                {
                    _shortcutActions = value;
                }
                else
                {
                    _shortcutActionsOnInputManager = value;
                }
            }
        }

        public List<ShortcutAction> ShortcutActionsOnInputManager
        {
            get => _shortcutActionsOnInputManager;
            set => _shortcutActionsOnInputManager = value;
        }
        [SerializeField]
        List<ShortcutAction> _shortcutActionsOnInputManager;

        public List<ShortcutAction> ShortcutActions
        {
            get => _shortcutActions;
            set => _shortcutActions = value;
        }
        [SerializeField]
        List<ShortcutAction> _shortcutActions;

        public bool AutoSave
        {
            get => _autoSave;
            set => _autoSave = value;
        }
        [SerializeField]
        bool _autoSave = NoaDebuggerDefine.DEFAULT_AUTO_SAVE;

#if NOA_DEBUGGER
        public NoaDebuggerSettings Init()
        {
            var settingsContainer = new NoaDebuggerSettingsContainer(this);
            settingsContainer.Init();

#if UNITY_EDITOR

            _SaveSettingsData();
#endif
            return this;
        }

#if UNITY_EDITOR
        public NoaDebuggerSettings Update()
        {
            var jsonDictionary = _LoadSettingsData();

            if (jsonDictionary == null)
            {
                jsonDictionary = _SaveSettingsData();
            }

            _UpdateMenu();
            _UpdateShortcut();

            bool needsFileMigration = File.Exists(DEFAULT_OLD_NOA_DEBUGGER_SETTINGS_PATH);
            bool needsFolderMigration = Directory.Exists(DEFAULT_OLD_CUSTOM_MENU_FOLDER_PATH);

            if (needsFileMigration || needsFolderMigration)
            {
                try
                {
                    int step = 0;
                    int totalStep = 3;
                    _ShowProgressBar(step, totalStep);

                    _UpdateSettings();

                    _ShowProgressBar(step++, totalStep);

                    _UpdateCustomMenuFolderPath();

                    _ShowProgressBar(step++, totalStep);

                    _SaveSettingsData();

                    _ShowProgressBar(step++, totalStep);
                }
                catch (Exception e)
                {
                    LogModel.LogError($"Migration failed: {e}");
                }
                finally
                {
                    EditorUtility.ClearProgressBar();
                }
            }
            else
            {
                _SaveSettingsData();
            }

            return this;
        }

        void _UpdateSettings()
        {
            if(File.Exists(DEFAULT_OLD_NOA_DEBUGGER_SETTINGS_PATH))
            {
                try
                {
                    string oldPath = DEFAULT_OLD_NOA_DEBUGGER_SETTINGS_PATH;
                    string newPath = DEFAULT_NOA_DEBUGGER_SETTINGS_PATH;

                    string parentDir = Path.GetDirectoryName(newPath);
                    if (!string.IsNullOrEmpty(parentDir) && !Directory.Exists(parentDir))
                    {
                        Directory.CreateDirectory(parentDir);
                    }

                    if (File.Exists(oldPath))
                    {
                        string oldMetaPath = oldPath + ".meta";
                        if (File.Exists(oldMetaPath))
                        {
                            File.Delete(oldMetaPath);
                        }

                        File.Copy(oldPath, newPath, true);
                        LogModel.Log($"Copied file to {newPath}");

                        AssetDatabase.DeleteAsset(oldPath);
                        LogModel.Log($"Deleted old file: {oldPath}");
                    }

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

                    EditorUtility.DisplayDialog(
                        "NoaDebugger Settings Moved",
                        $"NoaDebugger settings have been moved to a new location.\n\nFrom: {oldPath}\n\nTo: {newPath}",
                        "OK"
                    );
                }
                catch (Exception e)
                {
                    LogModel.LogError($"Failed to move settings file: {e}");
                }
            }
        }

        void _ShowProgressBar(int step, int totalStep)
        {
            EditorUtility.DisplayProgressBar(
                "NoaDebugger Settings Migration",
                $"Migrating settings file... ({step++}/{totalStep})",
                (float)step/ totalStep
            );
        }

        void _UpdateCustomMenuFolderPath()
        {
            var directoryInfo = new DirectoryInfo(DEFAULT_OLD_CUSTOM_MENU_FOLDER_PATH);
            if(directoryInfo.Exists)
            {
                if (!_HasAnyFilesInOldCustomMenuFolder())
                {
                    try
                    {
                        AssetDatabase.DeleteAsset(DEFAULT_OLD_CUSTOM_MENU_FOLDER_PATH);
                        AssetDatabase.Refresh();
                    }
                    catch (Exception e)
                    {
                        LogModel.LogError($"Failed to delete folder: {e.Message}");
                    }

                    return;
                }

                var destinationDirectory = new DirectoryInfo(DEFAULT_CUSTOM_MENU_FOLDER_PATH);

                if(destinationDirectory.Exists)
                {
                    LogModel.LogWarning($"Destination already exists: {DEFAULT_CUSTOM_MENU_FOLDER_PATH}");
                    return;
                }

                try
                {
                    string oldPath = DEFAULT_OLD_CUSTOM_MENU_FOLDER_PATH;
                    string newPath = DEFAULT_CUSTOM_MENU_FOLDER_PATH;

                    string parentDir = Path.GetDirectoryName(newPath);
                    if (!string.IsNullOrEmpty(parentDir) && !Directory.Exists(parentDir))
                    {
                        Directory.CreateDirectory(parentDir);
                        AssetDatabase.Refresh();
                    }

                    string error = AssetDatabase.MoveAsset(oldPath, newPath);

                    if (string.IsNullOrEmpty(error))
                    {
                        LogModel.Log($"Successfully moved folder to {newPath}");
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();

                        EditorUtility.DisplayDialog(
                            "NoaDebugger Settings Folder Moved",
                            $"NoaDebugger settings folder has been moved to a new location.\n\nFrom: {DEFAULT_OLD_CUSTOM_MENU_FOLDER_PATH}\n\nTo: {DEFAULT_CUSTOM_MENU_FOLDER_PATH}",
                            "OK"
                        );
                    }
                    else
                    {
                        LogModel.LogError($"Failed to move folder: {error}");
                    }
                }
                catch(Exception e)
                {
                    LogModel.LogError($"Failed to move folder: {e}");
                }
            }
        }

        bool _HasAnyFilesInOldCustomMenuFolder()
        {
            var directoryInfo = new DirectoryInfo(DEFAULT_OLD_CUSTOM_MENU_FOLDER_PATH);
            if (!directoryInfo.Exists)
            {
                return false;
            }

            return directoryInfo.GetFiles("*", SearchOption.AllDirectories).Any(file => !file.Name.EndsWith(".meta"));
        }

        void _UpdateMenu()
        {
            _menuList = NoaDebuggerMenuSettings.GetUpdatedMenuSettings(_menuList);
        }

        void _UpdateShortcut()
        {
            if (!UnityInputUtils.IsEnableInputSystem && !_shortcutActionsOnInputManager.Any())
            {
                _shortcutActionsOnInputManager = new List<ShortcutAction>(_shortcutActions);

                _shortcutActions.Clear();
            }

            _shortcutActionsOnInputManager = NoaDebuggerShortcutSettings.GetUpdatedShortcutSettings(
                _shortcutActionsOnInputManager,
                forInputSystem: false);
            _shortcutActions = NoaDebuggerShortcutSettings.GetUpdatedShortcutSettings(
                _shortcutActions,
                forInputSystem: true);
        }

        public void UpdateCustomMenu()
        {
            foreach (CustomMenuInfo customMenuInfo in CustomMenuList)
            {
                customMenuInfo.RefreshScriptName();
            }
        }

        Dictionary<string, object> _SaveSettingsData()
        {
            var fields = typeof(NoaDebuggerSettings).GetFields(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            var dictionary = new Dictionary<string, object>();

            foreach (var field in fields)
            {
                dictionary.Add(field.Name, field.FieldType.Name);
            }

            var jsonDictionary = new JsonDictionary<string, object>(dictionary);
            var json = JsonUtility.ToJson(jsonDictionary, true);
            EditorPrefs.SetString(NoaDebuggerDefine.EditorPrefsKeyPackageSettingsData, json);

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return jsonDictionary.Dictionary;
        }

        Dictionary<string, object> _LoadSettingsData()
        {
            var json = EditorPrefs.GetString(NoaDebuggerDefine.EditorPrefsKeyPackageSettingsData);

            var jsonDictionary = JsonUtility.FromJson<JsonDictionary<string, object>>(json);

            return jsonDictionary?.Dictionary;
        }
#endif 
#endif 
    }

    [Serializable]
    sealed class ProfilerOverlayFeatureSettings
    {
        [SerializeField]
        public bool Enabled = NoaDebuggerDefine.DefaultProfilerOverlayFeatureEnabled;
        [SerializeField]
        public NoaProfiler.OverlayTextType TextType = NoaDebuggerDefine.DefaultProfilerOverlayTextType;
        [SerializeField]
        public bool Graph = NoaDebuggerDefine.DefaultProfilerOverlayGraphEnabled;
    }

    [Serializable]
    sealed class MenuInfo
    {
        [SerializeField]
        public string Name;
        [SerializeField]
        public bool Enabled;
        [SerializeField]
        public int SortNo;
    }

    [Serializable]
    sealed class CustomMenuInfo
    {
        [SerializeField]
        public int _sortNo;

        public String _scriptName;

#if UNITY_EDITOR
        [SerializeField]
        public MonoScript _script;

        public bool IsInvalidScript()
        {
            if (_script == null || _script.GetClass() == null)
            {
                return true;
            }

            if (_script.GetClass().BaseType != typeof(NoaCustomMenuBase))
            {
                return true;
            }

            return false;
        }

        public void RefreshScriptName()
        {
            if (_script == null)
            {
                return;
            }

            if (IsInvalidScript())
            {
                return;
            }

            _scriptName = _script.GetClass().AssemblyQualifiedName;
        }

        public string GetViewName()
        {
            return _script == null ? "" : _script.GetClass().Name;
        }
#else
        public string GetViewName()
        {
            if(string.IsNullOrEmpty(_scriptName))
            {
                return "";
            }
            else
            {
                Type t = Type.GetType(_scriptName);
                return t.Name;
            }
        }
#endif
    }

    [Serializable]
    sealed class JsonDictionary<TKey, TValue> : ISerializationCallbackReceiver
    {
        [Serializable]
        struct KeyValuePair
        {
            [SerializeField]
            [UsedImplicitly]
            TKey key;
            [SerializeField]
            [UsedImplicitly]
            TValue value;

            public TKey Key => key;
            public TValue Value => value;

            public KeyValuePair(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
            }
        }

        [SerializeField]
        [UsedImplicitly]
        KeyValuePair[] dictionary = default;

        Dictionary<TKey, TValue> _dictionary;

        public Dictionary<TKey, TValue> Dictionary => _dictionary;

        public JsonDictionary(Dictionary<TKey, TValue> dictionary)
        {
            _dictionary = dictionary;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            dictionary = _dictionary.Select(x => new KeyValuePair(x.Key, x.Value)).ToArray();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            _dictionary = dictionary.ToDictionary(x => x.Key, x => x.Value);
            dictionary = null;
        }
    }
}
