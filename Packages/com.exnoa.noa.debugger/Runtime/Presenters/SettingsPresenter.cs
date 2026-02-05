using System;
using UnityEngine;

namespace NoaDebugger
{
    sealed class SettingsPresenter : NoaDebuggerToolBase, INoaDebuggerTool
    {
        [Header("MainView")]
        [SerializeField]
        SettingsView _mainViewPrefab;
        SettingsView _mainView;

        [NonSerialized]
        public bool _isDeviceOrientationChanged;

        public ToolNotificationStatus NotifyStatus => ToolNotificationStatus.None;

        LoadingRuntimeSettings _loadingSettings;
        DisplayRuntimeSettings _displaySettings;
        FontRuntimeSettings _fontSettings;
        MenuRuntimeSettings _menuSettings;
        CustomMenuRuntimeSettings _customMenuSettings;
        InformationRuntimeSettings _informationSettings;
        LogRuntimeSettings _logSettings;
        HierarchyRuntimeSettings _hierarchySettings;
        CommandRuntimeSettings _commandSettings;
        CommonOverlayRuntimeSettings _commonOverlaySettings;
        ProfilerOverlayRuntimeSettings _profilerOverlaySettings;
        ConsoleLogOverlayRuntimeSettings _consoleLogOverlaySettings;
        ApiLogOverlayRuntimeSettings _apiLogOverlaySettings;
        UIElementRuntimeSettings _uiElementSettings;
        GameSpeedRuntimeSettings _gameSpeedSettings;
        ShortcutRuntimeSettings _shortcutSettings;
        OtherRuntimeSettings _otherSettings;

        public void Init()
        {
            _ApplyCacheSettings();

            _SubscribeEvents();
        }

        public sealed class SettingsMenuInfo : IMenuInfo
        {
            public string Name => "Settings";
            public string MenuName => "Noa Debugger";
            public int SortNo => 99;
        }

        SettingsMenuInfo _settingsMenuInfo;

        public IMenuInfo MenuInfo()
        {
            if (_settingsMenuInfo == null)
            {
                _settingsMenuInfo = new SettingsMenuInfo();
            }

            return _settingsMenuInfo;
        }

        public void ShowView(Transform parent)
        {
            if (_mainView == null)
            {
                _mainView = GameObject.Instantiate(_mainViewPrefab, parent);
                _InitView(_mainView);
            }

            _UpdateAllView();
            _mainView.gameObject.SetActive(true);
        }

        SettingsViewLinker Linker
        {
            get
            {
                return new SettingsViewLinker()
                {
                    _loadingSettings = _loadingSettings,
                    _displaySettings = _displaySettings,
                    _fontSettings = _fontSettings,
                    _menuSettings = _menuSettings,
                    _customMenuSettings = _customMenuSettings,
                    _informationSettings = _informationSettings,
                    _logSettings = _logSettings,
                    _hierarchySettings = _hierarchySettings,
                    _commandSettings = _commandSettings,
                    _commonOverlaySettings = _commonOverlaySettings,
                    _profilerOverlaySettings = _profilerOverlaySettings,
                    _consoleLogOverlaySettings = _consoleLogOverlaySettings,
                    _apiLogOverlaySettings = _apiLogOverlaySettings,
                    _uiElementSettings = _uiElementSettings,
                    _gameSpeedSettings = _gameSpeedSettings,
                    _shortcutSettings = _shortcutSettings,
                    _otherSettings = _otherSettings,
                };
            }
        }

        void _ApplyCacheSettings()
        {
            _loadingSettings = NoaDebuggerSettingsManager.GetCacheSettings<LoadingRuntimeSettings>();
            _displaySettings = NoaDebuggerSettingsManager.GetCacheSettings<DisplayRuntimeSettings>();
            _fontSettings = NoaDebuggerSettingsManager.GetCacheSettings<FontRuntimeSettings>();
            _menuSettings = NoaDebuggerSettingsManager.GetCacheSettings<MenuRuntimeSettings>();
            _customMenuSettings = NoaDebuggerSettingsManager.GetCacheSettings<CustomMenuRuntimeSettings>();
            _informationSettings = NoaDebuggerSettingsManager.GetCacheSettings<InformationRuntimeSettings>();
            _logSettings  = NoaDebuggerSettingsManager.GetCacheSettings<LogRuntimeSettings>();
            _hierarchySettings = NoaDebuggerSettingsManager.GetCacheSettings<HierarchyRuntimeSettings>();
            _commandSettings = NoaDebuggerSettingsManager.GetCacheSettings<CommandRuntimeSettings>();
            _commonOverlaySettings = NoaDebuggerSettingsManager.GetCacheSettings<CommonOverlayRuntimeSettings>();
            _consoleLogOverlaySettings = NoaDebuggerSettingsManager.GetCacheSettings<ConsoleLogOverlayRuntimeSettings>();
            _apiLogOverlaySettings = NoaDebuggerSettingsManager.GetCacheSettings<ApiLogOverlayRuntimeSettings>();
            _profilerOverlaySettings = NoaDebuggerSettingsManager.GetCacheSettings<ProfilerOverlayRuntimeSettings>();
            _uiElementSettings = NoaDebuggerSettingsManager.GetCacheSettings<UIElementRuntimeSettings>();
            _gameSpeedSettings = NoaDebuggerSettingsManager.GetCacheSettings<GameSpeedRuntimeSettings>();
            _shortcutSettings = NoaDebuggerSettingsManager.GetCacheSettings<ShortcutRuntimeSettings>();
            _otherSettings = NoaDebuggerSettingsManager.GetCacheSettings<OtherRuntimeSettings>();
        }

        void _SubscribeEvents()
        {
            _displaySettings.OnUpdateSettings -= _OnUpdateDisplaySettings;
            _displaySettings.OnUpdateSettings += _OnUpdateDisplaySettings;
            _logSettings.OnUpdateSettings -= _OnUpdateLogSettings;
            _logSettings.OnUpdateSettings += _OnUpdateLogSettings;
            _hierarchySettings.OnUpdateSettings -= _OnUpdateHierarchySettings;
            _hierarchySettings.OnUpdateSettings += _OnUpdateHierarchySettings;
            _commandSettings.OnUpdateSettings -= _OnUpdateCommandSettings;
            _commandSettings.OnUpdateSettings += _OnUpdateCommandSettings;
            _gameSpeedSettings.OnUpdateSettings -= _OnUpdateGameSpeedSettings;
            _gameSpeedSettings.OnUpdateSettings += _OnUpdateGameSpeedSettings;
            _uiElementSettings.OnUpdateSettings -= _OnUpdateUIElementSettings;
            _uiElementSettings.OnUpdateSettings += _OnUpdateUIElementSettings;
            _shortcutSettings.OnUpdateSettings -= _OnUpdateShortcutSettings;
            _shortcutSettings.OnUpdateSettings += _OnUpdateShortcutSettings;
            _commonOverlaySettings.OnUpdateSettings -= _OnUpdateCommonOverlaySettings;
            _commonOverlaySettings.OnUpdateSettings += _OnUpdateCommonOverlaySettings;
        }

        void _UnsubscribeEvents()
        {
            if (_displaySettings != null)
            {
                _displaySettings.OnUpdateSettings -= _OnUpdateDisplaySettings;
            }
            if (_logSettings != null)
            {
                _logSettings.OnUpdateSettings -= _OnUpdateLogSettings;
            }
            if (_hierarchySettings != null)
            {
                _hierarchySettings.OnUpdateSettings -= _OnUpdateHierarchySettings;
            }
            if (_commandSettings != null)
            {
                _commandSettings.OnUpdateSettings -= _OnUpdateCommandSettings;
            }
            if (_gameSpeedSettings != null)
            {
                _gameSpeedSettings.OnUpdateSettings -= _OnUpdateGameSpeedSettings;
            }
            if (_uiElementSettings != null)
            {
                _uiElementSettings.OnUpdateSettings -= _OnUpdateUIElementSettings;
            }
            if (_shortcutSettings != null)
            {
                _shortcutSettings.OnUpdateSettings -= _OnUpdateShortcutSettings;
            }
            if (_commonOverlaySettings != null)
            {
                _commonOverlaySettings.OnUpdateSettings -= _OnUpdateCommonOverlaySettings;
            }
        }

        void _UnsubscribeViewEvents()
        {
            if (_mainView != null)
            {
                _mainView.OnSave -= _OnSave;
                _mainView.OnReset -= _OnReset;
            }
        }

        void _InitView(SettingsView view)
        {
            view.OnSave += _OnSave;
            view.OnReset += _OnReset;

            SettingsEventModel.OnSettingsValueChanged -= view.SetActiveFooterArea;
            SettingsEventModel.OnSettingsValueChanged += view.SetActiveFooterArea;
        }

        void _UpdateAllView()
        {
            if (_mainView == null)
            {
                return;
            }

            _mainView.Show(Linker);
        }

        void _OnUpdateDisplaySettings()
        {
            SettingsEventModel.OnUpdateDisplaySettings?.Invoke();
        }

        void _OnUpdateGameSpeedSettings()
        {
            SettingsEventModel.OnUpdateGameSpeedSettings?.Invoke();
        }

        void _OnUpdateLogSettings()
        {
            SettingsEventModel.OnUpdateLogSettings?.Invoke();
        }

        void _OnUpdateHierarchySettings()
        {
            SettingsEventModel.OnUpdateHierarchySettings?.Invoke();
        }

        void _OnUpdateCommandSettings()
        {
            SettingsEventModel.OnUpdateCommandSettings?.Invoke();
        }

        void _OnUpdateUIElementSettings()
        {
            SettingsEventModel.OnUpdateUIElementSettings?.Invoke();
        }

        void _OnUpdateCommonOverlaySettings()
        {
            SettingsEventModel.OnUpdateCommonOverlaySettings?.Invoke();
        }

        void _OnUpdateShortcutSettings()
        {
            SettingsEventModel.OnUpdateShortcutSettings?.Invoke();
        }

        void _OnReset(SettingsResetButtonType type)
        {
            switch (type)
            {
                case SettingsResetButtonType.Display:
                    _displaySettings?.Reset();
                    break;

                case SettingsResetButtonType.Information:
                    _informationSettings?.Reset();
                    break;

                case SettingsResetButtonType.Log:
                    _logSettings?.Reset();
                    break;

                case SettingsResetButtonType.Hierarchy:
                    _hierarchySettings?.Reset();
                    break;

                case SettingsResetButtonType.Command:
                    _commandSettings?.Reset();
                    break;

                case SettingsResetButtonType.Overlay:
                    _commonOverlaySettings?.Reset();
                    _profilerOverlaySettings?.Reset();
                    _consoleLogOverlaySettings?.Reset();
                    _apiLogOverlaySettings?.Reset();
                    break;

                case SettingsResetButtonType.ProfilerOverlay:
                    _profilerOverlaySettings?.Reset();
                    break;

                case SettingsResetButtonType.ConsoleLogOverlay:
                    _consoleLogOverlaySettings?.Reset();
                    break;

                case SettingsResetButtonType.ApiLogOverlay:
                    _apiLogOverlaySettings?.Reset();
                    break;

                case SettingsResetButtonType.UiElement:
                    _uiElementSettings?.Reset();
                    break;

                case SettingsResetButtonType.GameSpeed:
                    _gameSpeedSettings?.Reset();
                    break;

                case SettingsResetButtonType.Shortcut:
                    _shortcutSettings?.Reset();
                    break;

                case SettingsResetButtonType.Other:
                    _otherSettings?.Reset();
                    break;
            }

            _UpdateAllView();
        }

        void _OnSave()
        {
            NoaDebuggerSettingsManager.SaveNoaDebuggerSettingsCache();
        }

        public void AlignmentUI(bool isReverse)
        {
            _mainView.AlignmentUI(isReverse);
        }


        void _OnHidden()
        {
            if (_mainView != null)
            {
                _mainView.Hide();
            }
        }

        public void OnHidden()
        {
            _OnHidden();
        }

        public void OnToolDispose()
        {
            _OnHidden();

            _UnsubscribeEvents();
            _UnsubscribeViewEvents();

            _mainViewPrefab = default;
            _mainView = default;
            _settingsMenuInfo = default;

            _loadingSettings = default;
            _displaySettings = default;
            _fontSettings = default;
            _menuSettings = default;
            _customMenuSettings = default;
            _informationSettings = default;
            _logSettings = default;
            _hierarchySettings = default;
            _commandSettings = default;
            _commonOverlaySettings = default;
            _profilerOverlaySettings = default;
            _consoleLogOverlaySettings = default;
            _apiLogOverlaySettings = default;
            _uiElementSettings = default;
            _gameSpeedSettings = default;
            _shortcutSettings = default;
            _otherSettings = default;
        }

        void OnDestroy()
        {
            _UnsubscribeEvents();
            _UnsubscribeViewEvents();

            _mainViewPrefab = default;
            _mainView = default;
            _settingsMenuInfo = default;

            _loadingSettings = default;
            _displaySettings = default;
            _fontSettings = default;
            _menuSettings = default;
            _customMenuSettings = default;
            _informationSettings = default;
            _logSettings = default;
            _hierarchySettings = default;
            _commandSettings = default;
            _commonOverlaySettings = default;
            _profilerOverlaySettings = default;
            _consoleLogOverlaySettings = default;
            _apiLogOverlaySettings = default;
            _uiElementSettings = default;
            _gameSpeedSettings = default;
            _shortcutSettings = default;
            _otherSettings = default;
        }
    }
}
