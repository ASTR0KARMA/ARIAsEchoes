using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class SettingsView : NoaDebuggerToolViewBase<SettingsViewLinker>
    {
        [Header("View")]
        [SerializeField] LoadingSettingsGroupView _loadingView;
        [SerializeField] DisplaySettingsGroupView _displayView;
        [SerializeField] FontSettingsGroupView _fontView;
        [SerializeField] MenuSettingsGroupView _menuView;
        [SerializeField] CustomMenuSettingsGroupView _customMenuView;
        [SerializeField] InformationSettingsGroupView _informationView;
        [SerializeField] LogSettingsGroupView _logView;
        [SerializeField] HierarchySettingsGroupView _hierarchyView;
        [SerializeField] CommandSettingsGroupView _commandView;
        [SerializeField] OverlaySettingsGroupView _overlayView;
        [SerializeField] ProfilerOverlaySettingsGroupView _profilerOverlayView;
        [SerializeField] ConsoleLogOverlaySettingsGroupView _consoleLogOverlayView;
        [SerializeField] ApiLogOverlaySettingsGroupView _apiLogOverlayView;
        [SerializeField] UIElementSettingsGroupView _uiElementView;
        [SerializeField] GameSpeedSettingsGroupView _gameSpeedView;
        [SerializeField] ShortcutSettingsGroupView _shortcutView;
        [SerializeField] OtherSettingsGroupView _otherView;

        [Header("Button")]
        [SerializeField] GameObject _footerButtonsArea;
        [SerializeField] Button _saveButton;

        List<SettingsViewBase> _groupViews;

        public event UnityAction<SettingsResetButtonType> OnReset;
        public event UnityAction OnSave;

        protected override void _Init()
        {
            _groupViews = new List<SettingsViewBase>
            {
                _loadingView,
                _fontView,
                _menuView,
                _customMenuView,
                _displayView,
                _informationView,
                _logView,
                _hierarchyView,
                _commandView,
                _overlayView,
                _profilerOverlayView,
                _consoleLogOverlayView,
                _apiLogOverlayView,
                _uiElementView,
                _gameSpeedView,
                _shortcutView,
                _otherView
            };

            _saveButton.onClick.AddListener(_OnSave);
        }

        protected override void _OnShow(SettingsViewLinker linker)
        {
            foreach (var view in _groupViews)
            {
                if (view != null)
                {
                    if (view is MutableSettingsViewBase mutableView)
                    {
                        mutableView.OnReset -= OnReset;
                        mutableView.OnReset += OnReset;
                    }
                }
                view.Initialize(linker);
            }

            SetActiveFooterArea();
        }

        protected override void _OnHide()
        {
            gameObject.SetActive(false);
        }

        void _OnSave()
        {
            OnSave?.Invoke();
            SetActiveFooterArea();
        }

        public void SetActiveFooterArea()
        {
            _footerButtonsArea.SetActive(NoaDebuggerSettingsManager.HasUnsavedNoaDebuggerSettingsCache());
        }
    }


    sealed class SettingsViewLinker : ViewLinkerBase
    {
        public LoadingRuntimeSettings _loadingSettings;

        public DisplayRuntimeSettings _displaySettings;

        public FontRuntimeSettings _fontSettings;

        public MenuRuntimeSettings _menuSettings;

        public CustomMenuRuntimeSettings _customMenuSettings;

        public InformationRuntimeSettings _informationSettings;

        public LogRuntimeSettings _logSettings;

        public HierarchyRuntimeSettings _hierarchySettings;

        public CommandRuntimeSettings _commandSettings;

        public CommonOverlayRuntimeSettings _commonOverlaySettings;

        public ProfilerOverlayRuntimeSettings _profilerOverlaySettings;

        public ConsoleLogOverlayRuntimeSettings _consoleLogOverlaySettings;

        public ApiLogOverlayRuntimeSettings _apiLogOverlaySettings;

        public UIElementRuntimeSettings _uiElementSettings;

        public GameSpeedRuntimeSettings _gameSpeedSettings;

        public ShortcutRuntimeSettings _shortcutSettings;

        public OtherRuntimeSettings _otherSettings;
    }

    public enum SettingsResetButtonType
    {
        Display,
        Information,
        Log,
        Hierarchy,
        Command,
        Overlay,
        ProfilerOverlay,
        ConsoleLogOverlay,
        ApiLogOverlay,
        UiElement,
        GameSpeed,
        Shortcut,
        Other
    }
}
