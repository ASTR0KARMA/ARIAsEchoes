using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NoaDebugger
{
    [RequireComponent(typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler))]
    sealed class NoaDebugger : MonoBehaviour
    {
        static readonly string DeviceOrientationKey = "NoaDebuggerDeviceOrientation";

        static NoaDebugger _instance;

        [SerializeField]
        NoaDebuggerView _mainView;

        [SerializeField]
        Transform _presenterRoot;
        INoaDebuggerTool[] _allNoaDebuggerTools; 
        INoaDebuggerTool[] _filteredNoaDebuggerTools; 
        INoaDebuggerTool[] _noaDebuggerTools; 
        public List<string> _initializedNoaDebuggerToolNames;
        int _activeToolIndex;
        int _lastActiveToolIndex;
        bool _isSearchingToolWithoutError;
        [SerializeField]
        ToolDetailPresenter _toolDetailPresenter;
        [SerializeField]
        SettingsPresenter _settingsPresenter;

        INoaDebuggerTool[] _allCustomNoaDebuggerTools; 
        List<Type> _customMenuType;

        [SerializeField, Header("SafeArea")]
        SafeAreaAdjuster _safeAreaRoot;
        [SerializeField, Header("FloatingWindow")]
        GameObject _floatingWindowRoot;
        [SerializeField, Header("Dialog")]
        Transform _dialogRoot;

        [SerializeField, Header("Toast")]
        Transform _toastRoot;
        [SerializeField]
        ToastView _toastPrefab;
        ToastView _toastInstance;

        [SerializeField, Header("Overlay")]
        OverlayManager _overlayManager;

        [SerializeField, Header("NoaUIElement")]
        NoaUIElementManager _noaUIElementManager;

        [SerializeField, Header("Operation environments")]
        RuntimeVersionChecker _runtimeVersionChecker;

        [SerializeField, Header("Manager")]
        Transform _managerRoot;

        const int TOOL_DETAIL_ACTIVE_INDEX = -1;

        const int TOOL_SETTINGS_ACTIVE_INDEX = -2;

        bool _isShowCustomTab;
        bool _isPortrait;
        RectTransform _rectTransform;
        Canvas _canvas;
        CanvasScaler _canvasScaler;
        Vector2 _defaultReferenceResolution;
        ShortcutActionHandler _shortcutActionHandler;

        public Action<int> OnShow { get; set; }

        public Action<int> OnHide { get; set; }

        public bool IsInitialized { get; private set; } = false;

        public bool IsShortcutsEnabled => _shortcutActionHandler.IsShortcutsEnabled;

        public bool IsWorldSpaceRenderingEnabled => _canvas != null && _canvas.renderMode == RenderMode.WorldSpace;

        Action<int, bool> _onMenuChanged;

        public NoaUIElementManager NoaUIElementManager => _noaUIElementManager;

        public static Transform MainViewRoot => NoaDebugger._instance._mainView.UIParent;

        public static Transform FloatingWindowRoot => NoaDebugger._instance._floatingWindowRoot.transform;

        public static Transform DialogRoot => NoaDebugger._instance._dialogRoot;

        public static ToastView ToastInstance => NoaDebugger._instance._toastInstance;

        public static Transform UIElementRoot => NoaDebugger._instance._noaUIElementManager.RootTransform;

        public static Transform OverlayRoot => NoaDebugger._instance._overlayManager.OverlayRoot;


        public static bool IsPortrait => NoaDebugger._instance._isPortrait;

        void Awake()
        {
            Assert.IsNotNull(_mainView);
            Assert.IsNotNull(_presenterRoot);
            Assert.IsNotNull(_toolDetailPresenter);
            Assert.IsNotNull(_settingsPresenter);
            Assert.IsNotNull(_safeAreaRoot);
            Assert.IsNotNull(_overlayManager);
            Assert.IsNotNull(_floatingWindowRoot);
            Assert.IsNotNull(_dialogRoot);
            Assert.IsNotNull(_toastRoot);
            Assert.IsNotNull(_toastPrefab);
            Assert.IsNotNull(_noaUIElementManager);
            Assert.IsNotNull(_runtimeVersionChecker);
            Assert.IsNotNull(_managerRoot);
            _rectTransform = GetComponent<RectTransform>();
            _canvas = GetComponent<Canvas>();
            _canvasScaler = GetComponent<CanvasScaler>();
        }

        public Action<int, bool> OnMenuChanged
        {
            get => _onMenuChanged;
            set => _onMenuChanged = value;
        }

        public void Init()
        {
            _Init();
        }

        void _Init()
        {
            DeviceOrientationManager.Init(_managerRoot);
            ApplicationBackgroundManager.Instantiate(_managerRoot);
            NoaDebuggerPrefsBehaviour.Initialize(_managerRoot);
            GlobalCoroutine.Init(_managerRoot);
            FrameTimeMeasurer.Instantiate(_managerRoot);
            EventSystemManager.Init(_managerRoot);
            Input.Initialize();
            _instance = this;
            NoaDebuggerVisibilityManager.OnInitNoaDebuggerVisibility();
            _overlayManager.Initialize(_mainView.ContentRoot, _CurrentTool);
            _noaUIElementManager.ResetRootRectSize();

            _allNoaDebuggerTools = _FindNoaDebuggerTools();
            _customMenuType = new List<Type>();
            _allCustomNoaDebuggerTools = _FindCustomNoaDebuggerTools();
            _initializedNoaDebuggerToolNames = new List<string>();
            _isShowCustomTab = false;

            string menuName = _toolDetailPresenter.MenuInfo().MenuName;
            _toolDetailPresenter.Init();
            _initializedNoaDebuggerToolNames.Add(menuName);

            string settingsMenuName = _settingsPresenter.MenuInfo().MenuName;
            _settingsPresenter.Init();
            _initializedNoaDebuggerToolNames.Add(settingsMenuName);

            _UpdateNoaDebuggerTools();

            _InitNoaDebuggerTools();

            DeviceOrientationManager.SetAction(NoaDebugger.DeviceOrientationKey, _OnDeviceOrientation);
            _mainView.ChangeToolListener += _ChangeToolButton;
            _mainView.OnToolDetailButton += _ShowToolDetail;
            _mainView.OnCloseButton += NoaDebuggerManager.HideDebugger;
            _mainView.OnFloatingWindowPinButton += _PinFloatingWindowTool;
            _mainView.OnOverlayPinButton += _overlayManager.PinOverlayTool;
            _mainView.OnOverlaySettingsButton += _overlayManager.ToggleOverlaySettings;
            _mainView.OnControllerButton += _ShowController;
            _mainView.OnChangeButton += _ChangeTab;
            _mainView.OnSettingsButton += _ShowSettings;
            _mainView.SetChangeButtonInteractable(_allCustomNoaDebuggerTools.Length > 0);

            var noaDebuggerSettings = NoaDebuggerSettingsManager.GetNoaDebuggerSettings();

            _defaultReferenceResolution = _canvasScaler.referenceResolution;
            _ApplyCanvasScale(noaDebuggerSettings.NoaDebuggerCanvasScale);

            _canvas.sortingOrder = noaDebuggerSettings.NoaDebuggerCanvasSortOrder;

            if (noaDebuggerSettings.EnableAllShortcuts)
            {
                _shortcutActionHandler = gameObject.AddComponent<ShortcutActionHandler>();
                _shortcutActionHandler.IsShortcutsEnabled = true;
            }

            SettingsEventModel.OnUpdateShortcutSettings -= _OnUpdateShortcutSettings;
            SettingsEventModel.OnUpdateShortcutSettings += _OnUpdateShortcutSettings;
            SettingsEventModel.OnUpdateCommonOverlaySettings -= _OnUpdateCommonOverlaySettings;
            SettingsEventModel.OnUpdateCommonOverlaySettings += _OnUpdateCommonOverlaySettings;

            IsInitialized = true;
        }

        void _InitNoaDebuggerTools()
        {
            foreach (INoaDebuggerTool tool in _noaDebuggerTools)
            {
                string menuName = tool.MenuInfo().MenuName;

                if (_initializedNoaDebuggerToolNames.Contains(menuName))
                {
                    continue;
                }

                tool.Init();

                if (tool is INoaDebuggerOverlayTool overlayTool)
                {
                    overlayTool.InitOverlay(_overlayManager.OverlayRoot);
                }
                if (tool is INoaDebuggerFloatingTool floatingTool)
                {
                    floatingTool.InitFloatingWindow(_floatingWindowRoot.transform);
                }

                _initializedNoaDebuggerToolNames.Add(menuName);
            }

            foreach (INoaDebuggerTool tool in _allCustomNoaDebuggerTools)
            {
                tool.Init();
            }
        }

        public void _DestroyNoaDebugger()
        {
            GlobalCoroutine.Dispose();
            _DisableNoaDebugger();

            DeviceOrientationManager.DeleteAction(NoaDebugger.DeviceOrientationKey);

            for (int i = 0; i < _filteredNoaDebuggerTools.Length; i++)
            {
                _filteredNoaDebuggerTools[i].OnToolDispose();
            }

            for (int i = 0; i < _allCustomNoaDebuggerTools.Length; i++)
            {
                _allCustomNoaDebuggerTools[i].OnToolDispose();
            }

            _toolDetailPresenter.OnToolDispose();
            NoaDebuggerSettingsManager.Dispose();
            NoaDebuggerInfoManager.Dispose();
            NoaDebuggerVisibilityManager.Dispose();
        }

        void _OnDeviceOrientation(bool isPortrait)
        {
            _safeAreaRoot.Adjust();
            _overlayManager.ResetRootRectSize();
            _noaUIElementManager.ResetRootRectSize();
            _isPortrait = isPortrait;

            if (NoaDebuggerVisibilityManager.IsMainViewActive)
            {
                _mainView.RequiresRebuildSideMenu = true;
                _ChangeTool(_activeToolIndex);
            }
        }

        INoaDebuggerTool[] _FindNoaDebuggerTools()
        {
            var toolList = new List<INoaDebuggerTool>(_presenterRoot.childCount);

            foreach (Transform child in _presenterRoot)
            {
                var toolBase = child.GetComponent<NoaDebuggerToolBase>();

                if (toolBase == null)
                {
                    continue;
                }

                toolList.Add(toolBase as INoaDebuggerTool);
            }

            return toolList.ToArray();
        }

        INoaDebuggerTool[] _FindCustomNoaDebuggerTools()
        {
            var toolList = new List<INoaDebuggerTool>();
            List<CustomMenuInfo> customMenuInfos = GetCustomMenuInfo();

            foreach (CustomMenuInfo customMenuInfo in customMenuInfos)
            {
                Type t = Type.GetType(customMenuInfo._scriptName);

                if (t == null)
                {
                    continue;
                }

                bool isCommand = t.BaseType == typeof(NoaCustomMenuBase);

                if (!isCommand)
                {
                    continue;
                }

                var customPresenter = Activator.CreateInstance(t);
                var tool = customPresenter as INoaDebuggerTool;
#if UNITY_EDITOR
                var customMenu = tool as NoaCustomMenuBase;
                if (customMenu != null && customMenu.HasError())
                {
                    continue;
                }
#endif
                toolList.Add(tool);
                _customMenuType.Add(t);
            }

            return toolList.ToArray();
        }

        void _ChangeTool(int index, bool isMenuActive = false, bool isChangeMenu = false)
        {
            index = _GetToolIndexAfterValidation(index);

            var linker = new NoaDebuggerViewLinker
            {
                _activeToolIndex = index,
                _targetTools = _noaDebuggerTools,
                _isCustom = _isShowCustomTab,
                _isPortrait = _isPortrait,
                _isMenuActive = isMenuActive,
                _isSettings = index == TOOL_SETTINGS_ACTIVE_INDEX,
            };

            switch (index)
            {
                case TOOL_DETAIL_ACTIVE_INDEX:
                    linker._forceTargetTool = _toolDetailPresenter as INoaDebuggerTool;

                    break;
                case TOOL_SETTINGS_ACTIVE_INDEX:
                    linker._forceTargetTool = _settingsPresenter as INoaDebuggerTool;
                    break;
            }

            if (isChangeMenu || index != _activeToolIndex)
            {
                _HideTool(_activeToolIndex);
                _onMenuChanged?.Invoke(index, _isShowCustomTab);
            }

            _activeToolIndex = index;
            _mainView.Show(linker);
        }

        void _HideTool(int index)
        {
            INoaDebuggerTool currentTool = null;

            if (index == TOOL_DETAIL_ACTIVE_INDEX)
            {
                currentTool = _toolDetailPresenter;
            }
            else if (index == TOOL_SETTINGS_ACTIVE_INDEX)
            {
                currentTool = _settingsPresenter;
            }
            else if (index < _noaDebuggerTools.Length)
            {
                currentTool = _noaDebuggerTools[index];
            }

            if (currentTool != null)
            {
                currentTool.OnHidden();
            }
        }

        void _ChangeToolButton(int index)
        {
            _ChangeTool(index);
        }

        void _ShowToolDetail()
        {
            if (_activeToolIndex == TOOL_DETAIL_ACTIVE_INDEX)
            {
                return;
            }

            _HideTool(_activeToolIndex);

            _onMenuChanged?.Invoke(TOOL_DETAIL_ACTIVE_INDEX, _isShowCustomTab);

            _activeToolIndex = TOOL_DETAIL_ACTIVE_INDEX;

            var linker = new NoaDebuggerViewLinker
            {
                _activeToolIndex = 0,
                _targetTools = _noaDebuggerTools,
                _isCustom = _isShowCustomTab,
                _isPortrait = _isPortrait,
                _forceTargetTool = _toolDetailPresenter,
                _isSettings = false
            };

            _mainView.Show(linker);
            _mainView.InitTabs();
        }

        public void CloseNoaDebugger()
        {
            _HideTool(_activeToolIndex);

            _SetLastActiveToolIndex();
            NoaDebuggerVisibilityManager.OnHideMainViewVisibility();

            OnHide?.Invoke(_activeToolIndex);
        }

        void _SetLastActiveToolIndex()
        {
            int toolIndex;

            if (NoaDebuggerManager.IsError())
            {
                _SwitchShowTools(false);

                if (_isSearchingToolWithoutError)
                {
                    int loopIndex = (_lastActiveToolIndex + 1) % _noaDebuggerTools.Length;
                    toolIndex = loopIndex;
                }

                else
                {
                    int consoleLogIndex = _GetTargetToolIndex<ConsoleLogPresenter>();
                    toolIndex = consoleLogIndex;

                    if (_activeToolIndex == consoleLogIndex)
                    {
                        _isSearchingToolWithoutError = true;
                        toolIndex = -1; 
                    }
                }
            }
            else
            {
                _isSearchingToolWithoutError = false;
                toolIndex = _activeToolIndex;
            }

            _lastActiveToolIndex = toolIndex;
        }

        int _GetTargetToolIndex<T>() where T : INoaDebuggerTool
        {
            int index = -1;

            for (int i = 0; i < _noaDebuggerTools.Length; i++)
            {
                if (_noaDebuggerTools[i] is T)
                {
                    index = i;
                }
            }

            return index;
        }

        void _PinFloatingWindowTool()
        {
            INoaDebuggerTool currentTool = _CurrentTool();
            if (currentTool is INoaDebuggerFloatingTool floatingTool)
            {
                floatingTool.TogglePin(_floatingWindowRoot.transform);
            }
        }

        void _ShowController()
        {
            NoaDebuggerManager.HideDebugger();
            NoaDebuggerVisibilityManager.SetControllerVisible(true);
        }

        void _ChangeTab(bool isShowCustomTab)
        {
            _HideTool(_activeToolIndex);

            _SwitchShowTools(isShowCustomTab);

            _activeToolIndex = 0;

            if (_noaDebuggerTools.Length == 0)
            {
                _activeToolIndex = -1;
            }

            _ChangeTool(_activeToolIndex, isMenuActive: true, isChangeMenu: true);
        }

        void _ShowSettings(bool isShow)
        {
            if (_activeToolIndex == TOOL_SETTINGS_ACTIVE_INDEX)
            {
                if (_isPortrait)
                {
                    _mainView.SetActiveSideMenu(false);
                }

                _mainView.ToggleSettingsButton(true);

                return;
            }

            _HideTool(_activeToolIndex);

            _onMenuChanged?.Invoke(TOOL_SETTINGS_ACTIVE_INDEX, _isShowCustomTab);

            _activeToolIndex = TOOL_SETTINGS_ACTIVE_INDEX;

            var linker = new NoaDebuggerViewLinker
            {
                _activeToolIndex = _activeToolIndex,
                _targetTools = _noaDebuggerTools,
                _isCustom = _isShowCustomTab,
                _isPortrait = _isPortrait,
                _forceTargetTool = _settingsPresenter,
                _isSettings = true
            };

            _mainView.Show(linker);
            _mainView.InitTabs();
        }

        void _SwitchShowTools(bool isShowCustom)
        {
            _isShowCustomTab = isShowCustom;

            if (isShowCustom)
            {
                _SwitchCustomNoaDebuggerTools();
            }
            else
            {
                _SwitchNoaDebuggerTools();
            }

            _mainView.CreateMenu(_noaDebuggerTools, true);
        }

        void _UpdateNoaDebuggerTools()
        {
            NoaDebuggerSettingsManager.ValidateMenuSettings(AllPresenters().ToList());

            List<MenuInfo> menuSettings = NoaDebuggerSettingsManager.GetNoaDebuggerSettings().MenuList;
            var sortedNoaDebuggerTools = new List<INoaDebuggerTool>();

            foreach (MenuInfo menuSetting in menuSettings)
            {
                INoaDebuggerTool target = _allNoaDebuggerTools.FirstOrDefault(
                    tool => tool.MenuInfo().MenuName.Equals(menuSetting.Name));

                if (target != null)
                {
                    if (menuSetting.Enabled)
                    {
                        sortedNoaDebuggerTools.Add(target);
                    }
                    else
                    {
                        target.OnToolDispose();

                        continue;
                    }
                }
            }

            _filteredNoaDebuggerTools = sortedNoaDebuggerTools.ToArray();
            _noaDebuggerTools = _filteredNoaDebuggerTools;
        }

        void _SwitchNoaDebuggerTools()
        {
            _noaDebuggerTools = _filteredNoaDebuggerTools;
        }

        void _SwitchCustomNoaDebuggerTools()
        {
            _noaDebuggerTools = _allCustomNoaDebuggerTools;
        }

        INoaDebuggerTool _CurrentTool()
        {
            if (_noaDebuggerTools.Length == 0 || _activeToolIndex == TOOL_DETAIL_ACTIVE_INDEX)
            {
                return _toolDetailPresenter;
            }

            if (_activeToolIndex == NoaDebugger.TOOL_SETTINGS_ACTIVE_INDEX)
            {
                return _settingsPresenter;
            }

            return _noaDebuggerTools[_activeToolIndex];
        }

        void _RefreshNoaDebugger()
        {
            bool isOnSettings = _activeToolIndex == TOOL_SETTINGS_ACTIVE_INDEX;

            var linker = new NoaDebuggerViewLinker
            {
                _activeToolIndex = _activeToolIndex,
                _targetTools = _noaDebuggerTools,
                _isCustom = _isShowCustomTab,
                _isPortrait = _isPortrait,
                _isSettings = isOnSettings
            };

            if (isOnSettings)
            {
                linker._forceTargetTool = _settingsPresenter;
            }

            _mainView.ApplyNoaDebuggerSettings();
            _UpdateNoaDebuggerTools();

            var settings = NoaDebuggerSettingsManager.GetNoaDebuggerSettings();
            _ApplyCanvasScale(settings.NoaDebuggerCanvasScale);

            _mainView.Show(linker);
            _mainView.InitTabs();
        }

        void _OnUpdateShortcutSettings()
        {
            var settings = NoaDebuggerSettingsManager.GetNoaDebuggerSettings();
            if (_shortcutActionHandler == null)
            {
                _shortcutActionHandler = gameObject.AddComponent<ShortcutActionHandler>();
            }
            _shortcutActionHandler.Init();
            _shortcutActionHandler.IsShortcutsEnabled = settings.EnableAllShortcuts;
        }

        void _OnUpdateCommonOverlaySettings()
        {
            _overlayManager.ResetRootRectSize();
        }

        public static bool IsShowTargetToolMainView(INoaDebuggerTool tool)
        {
            INoaDebuggerTool current = NoaDebugger._instance._CurrentTool();

            return NoaDebuggerVisibilityManager.IsMainViewActive && current.GetType() == tool.GetType();
        }

        public static void ShowNoaDebugger(int index = 0, bool? isCustomMenu = false)
        {
            if (_instance == null)
            {
                return;
            }

            if (isCustomMenu != null)
            {
                NoaDebugger._instance._SwitchShowTools(isCustomMenu.Value);
            }
            NoaController.Hide();

            NoaDebuggerVisibilityManager.OnShowMainViewVisibility();
            NoaDebugger._instance._ChangeTool(index);

            NoaDebuggerInfo info = NoaDebuggerInfoManager.GetNoaDebuggerInfo();

            if (info != null)
            {
                NoaDebugger._instance._runtimeVersionChecker.DoCheck(info);
            }

            NoaDebugger._instance.OnShow?.Invoke(NoaDebugger._instance._activeToolIndex);
        }

        int _GetToolIndexAfterValidation(int index)
        {
            if (_instance._noaDebuggerTools.Length == 0)
            {
                index = TOOL_DETAIL_ACTIVE_INDEX;
            }

            if (index != NoaDebugger.TOOL_DETAIL_ACTIVE_INDEX
                && index != TOOL_SETTINGS_ACTIVE_INDEX
                && (index < 0 || index >= NoaDebugger._instance._noaDebuggerTools.Length))
            {
                index = 0;
            }

            return index;
        }

        public static void ShowNoaDebuggerLastActiveTool()
        {
            ShowNoaDebugger(NoaDebugger._instance._lastActiveToolIndex, NoaDebugger._instance._isShowCustomTab);
        }

        void _DisableNoaDebugger()
        {
            NoaDebuggerVisibilityManager.OnDisableNoaDebugger();
            _dialogRoot.gameObject.SetActive(false);
            _HideTool(_activeToolIndex);
            _mainView.HideContents();
            CloseNoaDebugger();
        }

        public static void ShowToast(ToastViewLinker linker)
        {
            if (NoaDebugger._instance._toastInstance == null)
            {
                NoaDebugger._instance._toastInstance = GameObject.Instantiate(
                    NoaDebugger._instance._toastPrefab,
                    NoaDebugger._instance._toastRoot);
            }

            NoaDebugger._instance._toastInstance.Show(linker);
        }

        public static TPresenter GetPresenter<TPresenter>() where TPresenter : INoaDebuggerTool
        {
            if (_instance == null)
            {
                return default(TPresenter);
            }

            if (NoaDebuggerManager.IsError())
            {
                return default(TPresenter);
            }

            INoaDebuggerTool[] toolsArray = NoaDebugger._instance._filteredNoaDebuggerTools;

            foreach (var tool in toolsArray)
            {
                if (tool is TPresenter presenter)
                {
                    return presenter;
                }
            }

            return default(TPresenter);
        }

        public static INoaDebuggerTool[] AllPresenters()
        {
            return NoaDebugger._instance._allNoaDebuggerTools;
        }

        public static INoaDebuggerTool[] AllCustomPresenters()
        {
            return NoaDebugger._instance._allCustomNoaDebuggerTools;
        }

        public static void OnChangeNotificationStatus<TPresenter>() where TPresenter : INoaDebuggerTool
        {
            if (NoaDebugger._instance == null)
            {
                return;
            }

            if (NoaDebuggerVisibilityManager.IsMainViewActive &&
                NoaDebugger._instance._CurrentTool().GetType() != typeof(TPresenter))
            {
                var linker = new NoaDebuggerViewLinker
                {
                    _targetTools = NoaDebugger._instance._noaDebuggerTools,
                    _activeToolIndex = NoaDebugger._instance._activeToolIndex,
                    _isCustom = NoaDebugger._instance._isShowCustomTab,
                    _isPortrait = NoaDebugger._instance._isPortrait
                };

                NoaDebugger._instance._mainView.UpdateMenu(linker);
            }
        }

        public static bool IsErrorNotice
        {
            get
            {
                bool isError = false;
                isError |= NoaDebugger._instance._toolDetailPresenter.NotifyStatus == ToolNotificationStatus.Error;

                isError |= NoaDebugger._instance._filteredNoaDebuggerTools.FirstOrDefault(
                    tool => tool.NotifyStatus == ToolNotificationStatus.Error) != null;

                return isError;
            }
        }

        List<CustomMenuInfo> GetCustomMenuInfo()
        {
            List<CustomMenuInfo> allCustomNoaDebuggerTools =
                NoaDebuggerSettingsManager.GetNoaDebuggerSettings().CustomMenuList;

            return allCustomNoaDebuggerTools;
        }

        public static bool IsInternalError(string stackTrace)
        {
            const RegexOptions options = RegexOptions.Multiline;

            return Regex.IsMatch(stackTrace, NoaDebuggerDefine.InternalErrorStacktraceRegexPattern, options)
                   && !Regex.IsMatch(stackTrace, NoaDebuggerDefine.DebugCommandInvocationStacktraceRegexPattern, options);
        }

        public static bool ContainsCustomClassNameByText(string text)
        {
            if (text.Contains(nameof(NoaCustomMenuBase)))
            {
                return true;
            }

            foreach (Type customMenuType in NoaDebugger._instance._customMenuType)
            {
                if (text.Contains(customMenuType.Name))
                {
                    return true;
                }
            }

            return false;
        }

        public static void RefreshNoaDebugger()
        {
            _instance._RefreshNoaDebugger();
        }

        public void SetOverlayEnabled(NoaDebug.OverlayFeatures feature, bool isEnabled)
        {
            INoaDebuggerOverlayTool overlayTool = _overlayManager.GetOverlayToolFromOverlayFeatures(feature);

            if (overlayTool == null)
            {
                return;
            }

            _overlayManager.SetOverlayEnabled(
                overlayTool,
                isEnabled,
                () =>
                {
                    if (_CurrentTool() == overlayTool)
                    {
                        _mainView.RefreshOverlayButtons(overlayTool);
                    }
                });
        }

        public bool GetOverlayEnabled(NoaDebug.OverlayFeatures feature)
        {
            INoaDebuggerOverlayTool overlayTool = _overlayManager.GetOverlayToolFromOverlayFeatures(feature);

            if (overlayTool == null)
            {
                return false;
            }

            return overlayTool.GetPinStatus();
        }

        public void SetCanvasScale(float scale)
        {
            float safeScale = Mathf.Clamp(scale, NoaDebuggerDefine.CanvasScaleMin, NoaDebuggerDefine.CanvasScaleMax);

            var settings = NoaDebuggerSettingsManager.GetCacheSettings<DisplayRuntimeSettings>();
            settings.NoaDebuggerCanvasScale.ChangeValue(safeScale);
            settings.Save();

            _ApplyCanvasScale(safeScale);
        }

        void _ApplyCanvasScale(float scale)
        {
            Vector2 newReferenceResolution = _defaultReferenceResolution / scale;
            _canvasScaler.referenceResolution = newReferenceResolution;
        }

        public void SetShortcutsEnabled(bool isEnabled)
        {
            if (_shortcutActionHandler != null)
            {
                _shortcutActionHandler.IsShortcutsEnabled = isEnabled;
            }
        }

        public void EnableWorldSpaceRendering(Camera worldCamera = null)
        {
            NoaDebuggerVisibilityManager.OnEnableWorldSpaceVisibility();

            _canvas.renderMode = RenderMode.WorldSpace;
            _canvas.worldCamera = worldCamera;

            _rectTransform.sizeDelta = _defaultReferenceResolution;

            _OnDeviceOrientation(false);
            DeviceOrientationManager.DeleteAction(NoaDebugger.DeviceOrientationKey);
        }

        public void DisableWorldSpaceRendering()
        {
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvas.worldCamera = null;

            NoaDebuggerVisibilityManager.OnDisableWorldSpaceVisibility();

            _OnDeviceOrientation(DeviceOrientationManager.IsPortrait);

            if (!DeviceOrientationManager.ContainsKey(NoaDebugger.DeviceOrientationKey))
            {
                DeviceOrientationManager.SetAction(NoaDebugger.DeviceOrientationKey, _OnDeviceOrientation);
            }
        }

        public void Dispose()
        {
            _mainView = default;
            _presenterRoot = default;
            _allNoaDebuggerTools = default;
            _filteredNoaDebuggerTools = default;
            _noaDebuggerTools = default;
            _initializedNoaDebuggerToolNames = default;
            _toolDetailPresenter = default;
            _allCustomNoaDebuggerTools = default;
            _customMenuType = default;
            _safeAreaRoot = default;
            _floatingWindowRoot = default;
            _dialogRoot = default;
            _toastRoot = default;
            _toastPrefab = default;
            _toastInstance = default;
            _overlayManager = default;
            _noaUIElementManager = default;
            _runtimeVersionChecker = default;
            _managerRoot = default;
            _rectTransform = default;
            _canvas = default;
            _canvasScaler = default;
            _onMenuChanged = default;
            OnShow = default;
            OnHide = default;
            SettingsEventModel.OnUpdateShortcutSettings -= _OnUpdateShortcutSettings;
            SettingsEventModel.OnUpdateCommonOverlaySettings -= _OnUpdateCommonOverlaySettings;
            NoaDebugger._instance = null;
        }
    }
}
