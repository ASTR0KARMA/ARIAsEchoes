using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class NoaDebuggerView : ViewBase<NoaDebuggerViewLinker>
    {
        public event Action<int> ChangeToolListener;

        public event Action OnToolDetailButton;

        public event Action OnCloseButton;

        public event Action OnFloatingWindowPinButton;

        public event Action OnOverlayPinButton;

        public event Action OnOverlaySettingsButton;

        public event Action OnControllerButton;

        public event Action<bool> OnSettingsButton;

        public event Action<bool> OnChangeButton;

        public bool RequiresRebuildSideMenu { get; set; } = true;

        [Header("Content")]
        [SerializeField] Transform _content;

        public Transform ContentRoot => _content;

        [SerializeField] Image _background;

        [Header("LayoutGroup")]
        [SerializeField] HorizontalLayoutGroup _sideMenuLayoutGroup;

        [Header("LayoutElements")]
        [SerializeField] LayoutElement _contentLayoutElement;

        public Transform UIParent => _background.transform;

        [Header("Button")]
        [SerializeField]
        Button _toolDetailButton;
        [SerializeField]
        Button _closeButton;
        [SerializeField]
        ToggleButtonBase _floatingWindowPinButton;
        [SerializeField]
        GameObject _overlayPinRoot;
        [SerializeField]
        ToggleButtonBase _overlayPinButton;
        [SerializeField]
        ToggleButtonBase _overlaySettingsButton;
        [SerializeField]
        Button _controllerButton;
        [SerializeField]
        ToggleColorButton _changeButton;
        [SerializeField]
        ToggleColorButton _settingsButton;
        [SerializeField]
        Button _sideMenuCloseButton;

        [Header("Text")]
        [SerializeField]
        TextMeshProUGUI _toolName;
        [SerializeField]
        TextMeshProUGUI _noaDebuggerVersion;

        [Header("Header")]
        [SerializeField]
        GameObject _headerSelectLine;

        [Header("Tab")]
        [SerializeField]
        ScrollRect _tabScroll;
        [SerializeField]
        MenuTabComponent _tabBase;
        MenuTabComponent[] _tabs;
        bool _createdTabs;

        [Header("Root content")]
        [SerializeField]
        RectTransform _rootContent;
        [Header("Side menu")]
        [SerializeField]
        RectTransform _sideMenu;
        [SerializeField]
        Button _sideMenuShowButton;
        [SerializeField]
        Button _sideMenuHideButton;
        [SerializeField]
        GameObject _sideMenuSpace;
        [SerializeField]
        Image _menuViewHeader;
        [SerializeField]
        GameObject _menuViewHeaderLine;

        [Header("UIReverseComponents")]
        [SerializeField]
        UIReverseComponents _reverseComponents;

        NoaDebuggerSettings _settings;

        protected override void _Init()
        {
            _toolDetailButton.onClick.AddListener(() => OnToolDetailButton?.Invoke());
            _closeButton.onClick.AddListener(() => OnCloseButton?.Invoke());
            _floatingWindowPinButton._onClick.AddListener((isOn) => OnFloatingWindowPinButton?.Invoke());
            _overlayPinButton._onClick.AddListener((isOn) => OnOverlayPinButton?.Invoke());
            _overlaySettingsButton._onClick.AddListener((isOn) => OnOverlaySettingsButton?.Invoke());
            _controllerButton.onClick.AddListener(() => OnControllerButton?.Invoke());
            _changeButton._onClick.AddListener((isOn) => OnChangeButton?.Invoke(isOn));
            _settingsButton._onClick.AddListener((isOn) => OnSettingsButton?.Invoke(isOn));
            _sideMenuCloseButton.onClick.AddListener(() => OnCloseButton?.Invoke());
            _sideMenuShowButton.onClick.AddListener(_ShowSideMenu);
            _sideMenuHideButton.onClick.AddListener(_HideSideMenu);

            ApplyNoaDebuggerSettings();
        }

        protected override void _OnShow(NoaDebuggerViewLinker linker)
        {
            CreateMenu(linker._targetTools);

            _sideMenuCloseButton.gameObject.SetActive(_settings.IsShowSideMenuCloseButton);

            _settings.BackgroundAlpha = Mathf.Clamp(
                _settings.BackgroundAlpha,
                NoaDebuggerDefine.CanvasAlphaMin,
                NoaDebuggerDefine.CanvasAlphaMax);

            Color backgroundColor = _background.color;
            backgroundColor.a = _settings.BackgroundAlpha;
            _background.color = backgroundColor;

            INoaDebuggerTool target = linker._forceTargetTool ?? linker._targetTools[linker._activeToolIndex];
            _toolName.text = target.MenuInfo().Name;
            target.ShowView(_content);

            if (linker._isPortrait)
            {
                _EnableSideMenuVerticalParts();

                SetActiveSideMenu(linker._isMenuActive);
            }
            else
            {
                _DisableSideMenuVerticalParts();
                _ShowSideMenu();
            }

            _AlignmentUI(linker._isPortrait, target);

            UpdateMenu(linker);

            bool isSelectToolDetailsMenuTab = linker._forceTargetTool != null && linker._forceTargetTool is ToolDetailPresenter;
            _headerSelectLine.SetActive(isSelectToolDetailsMenuTab);

            _ShowPinArea(target);

            NoaDebuggerInfo info = NoaDebuggerInfoManager.GetNoaDebuggerInfo();
            _noaDebuggerVersion.text = info.NoaDebuggerVersion;
            _changeButton.Init(linker._isCustom);
            _settingsButton.Init(linker._isSettings);
        }

        void _AlignmentUI(bool isPortrait, INoaDebuggerTool target)
        {
            bool isUIReverse = isPortrait && _settings.IsUIReversePortrait;

            _reverseComponents.Alignment(isUIReverse);

            Vector2 tmp = _tabScroll.content.pivot;
            tmp.y = isUIReverse ? 0 : 1;
            _tabScroll.content.pivot = tmp;

            if (_tabs != null)
            {
                foreach (MenuTabComponent tab in _tabs)
                {
                    tab.AlignmentUI(isUIReverse);
                }
            }

            if(RequiresRebuildSideMenu)
            {
                GlobalCoroutine.Run(_SetScrollPosition());
                RequiresRebuildSideMenu = false;
            }

            target.AlignmentUI(isUIReverse);
        }

        void _ShowPinArea(INoaDebuggerTool target)
        {
            _floatingWindowPinButton.gameObject.SetActive(false);
            _overlayPinRoot.SetActive(false);

            if (NoaDebuggerManager.IsWorldSpaceRenderingEnabled)
            {
                return;
            }

            if (target is INoaDebuggerOverlayTool overlayTool)
            {
                _overlayPinRoot.SetActive(true);
                RefreshOverlayButtons(overlayTool);
            }
            if (target is INoaDebuggerFloatingTool floatingTool)
            {
                _floatingWindowPinButton.gameObject.SetActive(true);
                _floatingWindowPinButton.Init(floatingTool.GetPinStatus());
            }
        }

        public void ApplyNoaDebuggerSettings()
        {
            _settings = NoaDebuggerSettingsManager.GetNoaDebuggerSettings();
        }

        public void RefreshOverlayButtons(INoaDebuggerOverlayTool overlayTool)
        {
            _overlayPinButton.Init(overlayTool.GetPinStatus());
            _overlaySettingsButton.Init(overlayTool.GetSettingsStatus());
        }

        IEnumerator _SetScrollPosition()
        {
            yield return null;

            _tabScroll.verticalNormalizedPosition = 1;
        }

        public void UpdateMenu(NoaDebuggerViewLinker linker)
        {
            if (_tabs != null)
            {
                for (int i = 0; i < _tabs.Length; i++)
                {
                    var tab = _tabs[i];
                    bool isSelect = i == linker._activeToolIndex;
                    tab.ChangeTabSelect(isSelect);
                    tab.ShowNoticeBadge(linker._targetTools[i].NotifyStatus);
                }
            }
        }

        void _OnChangeTool(int index)
        {
            ChangeToolListener?.Invoke(index);
        }

        public void HideContents()
        {
            foreach (Transform content in _content)
            {
                content.gameObject.SetActive(false);
            }
        }

        public void SetChangeButtonInteractable(bool interactable)
        {
            _changeButton.Interactable = interactable;
        }

        void _DestroyAllMenuButton()
        {
            if (_tabs == null)
            {
                return;
            }

            foreach (MenuTabComponent menuTab in _tabs)
            {
                menuTab._tabButton.onClick.RemoveAllListeners();
                GameObject.Destroy(menuTab.gameObject);
            }

            _tabs = null;
        }

        public void CreateMenu(INoaDebuggerTool[] tools, bool forceCreate = false)
        {
            if (_createdTabs && !forceCreate)
            {
                return;
            }

            _createdTabs = true;

            if (forceCreate)
            {
                _DestroyAllMenuButton();
            }

            if (tools.Length == 0)
            {
                _tabBase.gameObject.SetActive(false);

                return;
            }

            _tabs = new MenuTabComponent[tools.Length];

            for (int i = 0; i < tools.Length; i++)
            {
                int index = i;
                var tool = tools[index];
                MenuTabComponent button = GameObject.Instantiate(_tabBase, _tabScroll.content);
                button.gameObject.SetActive(true);
                button.name = tool.MenuInfo().MenuName;
                button._label.text = tool.MenuInfo().MenuName;

                button._tabButton.onClick.AddListener(
                    () =>
                    {
                        _OnChangeTool(index);
                    });

                _tabs[index] = button;
            }

            _tabBase.gameObject.SetActive(false);
        }

        public void InitTabs()
        {
            if (_tabs == null)
            {
                return;
            }

            foreach (var tab in _tabs)
            {
                tab.ChangeTabSelect(false);
            }
        }

        public void SetActiveSideMenu(bool isActive)
        {
            if (isActive)
            {
                _ShowSideMenu();
            }
            else
            {
                _HideSideMenu();
            }
        }

        public void ToggleSettingsButton(bool isOn)
        {
            _settingsButton.Init(isOn);
        }

        void _EnableSideMenuVerticalParts()
        {
            _sideMenuShowButton.gameObject.SetActive(true);
            _sideMenuSpace.gameObject.SetActive(true);
            _menuViewHeader.enabled = true;
            _menuViewHeaderLine.SetActive(false);
            _sideMenuHideButton.gameObject.SetActive(true);
        }

        void _DisableSideMenuVerticalParts()
        {
            _rootContent.anchoredPosition = Vector2.zero;
            _sideMenuShowButton.gameObject.SetActive(false);
            _sideMenuSpace.gameObject.SetActive(false);
            _menuViewHeader.enabled = false;
            _menuViewHeaderLine.SetActive(true);
            _sideMenuHideButton.gameObject.SetActive(false);

            _contentLayoutElement.minWidth = -1.0f;
        }

        void _ShowSideMenu()
        {
            _sideMenu.gameObject.SetActive(true);
            _sideMenuHideButton.image.raycastTarget = true;

            if(_sideMenuHideButton.gameObject.activeSelf)
            {
                _contentLayoutElement.minWidth = _rootContent.rect.width;
            }
        }

        void _HideSideMenu()
        {
            _sideMenu.gameObject.SetActive(false);
            _sideMenuHideButton.image.raycastTarget = false;
            _contentLayoutElement.minWidth = -1;
        }

        protected override void _OnHide()
        {
            if (_sideMenuHideButton.IsActive())
            {
                _HideSideMenu();
            }

            base._OnHide();
        }

        void OnDestroy()
        {
            ChangeToolListener = default;
            OnToolDetailButton = default;
            OnCloseButton = default;
            OnFloatingWindowPinButton = default;
            OnOverlayPinButton = default;
            OnOverlaySettingsButton = default;
            OnControllerButton = default;
            OnChangeButton = default;
            OnSettingsButton = default;
            _content = default;
            _background = default;
            _toolDetailButton = default;
            _closeButton = default;
            _floatingWindowPinButton = default;
            _overlayPinButton = default;
            _overlaySettingsButton = default;
            _controllerButton = default;
            _changeButton = default;
            _settingsButton = default;
            _sideMenuCloseButton = default;
            _toolName = default;
            _noaDebuggerVersion = default;
            _headerSelectLine = default;
            _tabScroll = default;
            _tabBase = default;
            _tabs = default;
            _rootContent = default;
            _sideMenu = default;
            _sideMenuShowButton = default;
            _sideMenuHideButton = default;
            _sideMenuSpace = default;
            _menuViewHeader = default;
            _menuViewHeaderLine = default;
        }
    }

    sealed class NoaDebuggerViewLinker : ViewLinkerBase
    {
        public INoaDebuggerTool[] _targetTools;
        public int _activeToolIndex;
        public bool _isCustom;
        public bool _isPortrait;
        public bool _isSettings;

        public bool _isMenuActive = false;

        public INoaDebuggerTool _forceTargetTool;
    }
}
