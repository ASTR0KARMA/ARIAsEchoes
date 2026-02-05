using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace NoaDebugger
{
    sealed class NoaDebuggerManager
    {
        static readonly string PrefabPath = "Prefabs/NoaDebuggerRoot";
        static readonly string ErrorObserverKey = "NoaDebuggerErrorObserver";
        static readonly string MonitorFontOnUpdateKey = "NoaDebuggerManagerMonitorFontOnUpdate";

        static NoaDebugger _noaDebugger;
        static NoaDebuggerManager _instance;

        bool _isError;

        bool _isInitializedOnce = false;

        NoaDebuggerButton _noaDebuggerButton;
        NoaDebuggerButtonEffectManager _buttonEffectManager;

        Action<int> _onShow;

        Action<int> _onHide;

        Action<int, bool> _onMenuChanged;

        public static Action<int> OnShow
        {
            get => NoaDebuggerManager._instance._onShow;
            set => NoaDebuggerManager._instance._onShow = value;
        }

        public static Action<int> OnHide
        {
            get => NoaDebuggerManager._instance._onHide;
            set => NoaDebuggerManager._instance._onHide = value;
        }

        public static Action<int, bool> OnMenuChanged
        {
            get => NoaDebuggerManager._instance._onMenuChanged;
            set => NoaDebuggerManager._instance._onMenuChanged = value;
        }

        public static Transform RootTransform =>
            NoaDebuggerManager.IsDebuggerInitialized()
                ? NoaDebuggerManager._noaDebugger.transform
                : null;

        public static bool IsShortcutsEnabled => NoaDebuggerManager.IsDebuggerInitialized() && NoaDebuggerManager._noaDebugger.IsShortcutsEnabled;

        public static bool IsWorldSpaceRenderingEnabled =>
            NoaDebuggerManager.IsDebuggerInitialized() && NoaDebuggerManager._noaDebugger.IsWorldSpaceRenderingEnabled;

        public static Transform MainViewRootTransform =>
            NoaDebuggerManager.IsDebuggerInitialized()
                ? NoaDebugger.MainViewRoot
                : null;

        public static Transform OverlayRootTransform =>
            NoaDebuggerManager.IsDebuggerInitialized()
                ? NoaDebugger.OverlayRoot
                : null;

        public static Transform FloatingWindowRootTransform =>
            NoaDebuggerManager.IsDebuggerInitialized()
                ? NoaDebugger.FloatingWindowRoot
                : null;

        public static Transform DialogRootTransform =>
            NoaDebuggerManager.IsDebuggerInitialized()
                ? NoaDebugger.DialogRoot
                : null;

        public static Transform UIElementRootTransform =>
            NoaDebuggerManager.IsDebuggerInitialized()
                ? NoaDebugger.UIElementRoot
                : null;

        public static int MainThreadId { get; private set; }

#if NOA_DEBUGGER
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void _AutoInitialize()
        {
            NoaDebuggerManager._instance = new NoaDebuggerManager();
            NoaDebuggerSettingsManager.InitializeOnRuntime();
            NoaDebuggerSettings settings = NoaDebuggerSettingsManager.GetNoaDebuggerSettings();
            NoaDebuggerText.Init(settings);

            if (settings.AutoInitialize)
            {
                InitializeDebugger();
            }

            ClipboardModel.Initialize();
        }
#endif
        static void _Instantiate()
        {
            if (NoaDebuggerManager.IsDebuggerInitialized())
            {
                return;
            }

            var prefab = Resources.Load<GameObject>(PrefabPath);
            GameObject obj = GameObject.Instantiate(prefab);
            obj.name = NoaDebuggerDefine.RootObjectName;
            GameObject.DontDestroyOnLoad(obj);
            NoaDebuggerManager._noaDebugger = obj.GetComponent<NoaDebugger>();

            obj.AddComponent<NoaDebuggerDestroyReceiver>();

            NoaDebuggerManager._instance._Init();
            NoaDebuggerManager._instance._isInitializedOnce = true;
        }

        void _Init()
        {
            NoaDebuggerManager._noaDebugger.Init();

            NoaDebuggerManager._noaDebugger.OnShow = NoaDebuggerManager.RunOnShow;
            NoaDebuggerManager._noaDebugger.OnHide = NoaDebuggerManager.RunOnHide;
            NoaDebuggerManager._noaDebugger.OnMenuChanged = NoaDebuggerManager.RunOnMenuChanged;
            SettingsEventModel.OnUpdateDisplaySettings -= _OnUpdateDisplaySettings;
            SettingsEventModel.OnUpdateDisplaySettings += _OnUpdateDisplaySettings;
            SettingsEventModel.OnUpdateUIElementSettings -= _OnUpdateUIElementSettings;
            SettingsEventModel.OnUpdateUIElementSettings += _OnUpdateUIElementSettings;

            Application.logMessageReceived += _HandleException;

            UpdateManager.SetAction(NoaDebuggerManager.ErrorObserverKey, _ErrorObserver);

            Transform buttonObj = NoaDebuggerManager._noaDebugger.transform.Find("Root/NoaDebuggerButton");
            _buttonEffectManager = buttonObj.GetComponent<NoaDebuggerButtonEffectManager>();
            _buttonEffectManager.Init();
            _noaDebuggerButton = _buttonEffectManager.NoaDebuggerButton;
            _buttonEffectManager.PlayOnLocationAnimation();

            NoaControllerManager.Initialize(_noaDebuggerButton.ControllerView, _buttonEffectManager);
        }

        void _ErrorObserver()
        {
            if (_isError)
            {
                NoaDebuggerManager._CloseNoaDebugger();

                var linker = new ToastViewLinker
                {
                    _label = NoaDebuggerDefine.ShowErrorText,
                };

                NoaDebugger.ShowToast(linker);
                _isError = false;
            }
        }

        void _HandleException(string logString, string stackTrace, UnityEngine.LogType type)
        {
            if (type != UnityEngine.LogType.Exception)
            {
                return;
            }

            if (!NoaDebugger.IsInternalError(stackTrace))
            {
                return;
            }

            _isError = true;

            LogModel.CollectNoaDebuggerErrorLog(logString, stackTrace);
        }

        void _OnUpdateDisplaySettings()
        {
            _ApplyDisplaySettingsToNoaDebuggerButton();

            NoaDebugger.RefreshNoaDebugger();
        }

        void _ApplyDisplaySettingsToNoaDebuggerButton()
        {
            bool isPortrait = NoaDebugger.IsPortrait;
            _noaDebuggerButton.ApplySettings(isPortrait);
        }

        void _OnUpdateUIElementSettings()
        {
            ResetRootRectSize();
        }

        static void _CloseNoaDebugger()
        {
            NoaDebuggerManager._noaDebugger.CloseNoaDebugger();
        }

        public static void OnChangeNotificationStatus<TPresenter>(ToolNotificationStatus notifyStatus)
            where TPresenter : INoaDebuggerTool
        {
            if (notifyStatus == ToolNotificationStatus.Error && _instance._noaDebuggerButton != null)
            {
                if (!NoaDebuggerManager._instance._buttonEffectManager.IsPlayingAnimation())
                {
                    _instance._buttonEffectManager.ResetButtonColor();
                }
                _instance._buttonEffectManager.PlayOnErrorAnimation();
            }

            NoaDebugger.OnChangeNotificationStatus<TPresenter>();
        }

        public static void DetectError()
        {
            if (_instance == null)
            {
                return;
            }

            _instance._isError = true;
        }

        public static void InitializeDebugger(Action onInitComplete = null)
        {
            if (NoaDebuggerManager.IsDebuggerInitialized())
            {
                return;
            }

            MainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;

            UnityInputUtils.CheckInputSystemAvailable();

            InitializeNoaDebuggerBehaviour.OnInitComplete = onInitComplete; 
            GameObject obj = new GameObject("InitializeNoaDebuggerBehaviour");
            obj.AddComponent<InitializeNoaDebuggerBehaviour>();
        }

        public static void ShowDebugger(int? index = null, bool? isCustomMenu = null)
        {
            if (NoaDebuggerVisibilityManager.IsMainViewActive)
            {
                return;
            }

            void showDebugger()
            {
                if (NoaDebuggerManager._instance._isError)
                {
                    return;
                }

                NoaDebuggerManager._ShowDebugger(index, isCustomMenu);
            }

            bool isInit = NoaDebuggerManager.IsDebuggerInitialized();

            if (!isInit)
            {
                NoaDebuggerManager.InitializeDebugger(showDebugger);
                return;
            }

            showDebugger();
        }

        public static void ShowDebugger(NoaDebug.MenuType menuType)
        {
            var menuInfo = _GetMenuInfoByMenuType(menuType);

            if (menuInfo == null || !menuInfo.Enabled)
            {
                return;
            }

            ShowDebugger(menuInfo.SortNo, false);
        }

        static MenuInfo _GetMenuInfoByMenuType(NoaDebug.MenuType menuType)
        {
            List<MenuInfo> menuSettings = NoaDebuggerSettingsManager.GetNoaDebuggerSettings().MenuList;
            string targetName = menuType.ToString();
            return menuSettings.FirstOrDefault(menu => menu.Name == targetName);
        }

        static void _ShowDebugger(int? index, bool? isCustomMenu)
        {
            if (index != null && isCustomMenu != null)
            {
                NoaDebugger.ShowNoaDebugger(index.Value, isCustomMenu.Value);
            }
            else
            {
                NoaDebugger.ShowNoaDebuggerLastActiveTool();
            }
        }

        public static void HideDebugger()
        {
            if (NoaDebuggerManager._noaDebugger == null)
            {
                return;
            }

            NoaDebuggerManager._CloseNoaDebugger();
        }

        public static void SetOverlayEnabled(NoaDebug.OverlayFeatures feature, bool isEnabled)
        {
            if (NoaDebuggerManager._noaDebugger == null)
            {
                return;
            }

            NoaDebuggerManager._noaDebugger.SetOverlayEnabled(feature, isEnabled);
        }

        public static bool GetOverlayEnabled(NoaDebug.OverlayFeatures feature)
        {
            if (NoaDebuggerManager._noaDebugger == null)
            {
                return false;
            }

            return NoaDebuggerManager._noaDebugger.GetOverlayEnabled(feature);
        }

        public static void SetFont(TMP_FontAsset fontAsset, Material fontMaterial, float fontSizeRate)
        {
            if (fontAsset != null && fontMaterial == null)
            {
                fontMaterial = fontAsset.material;
            }
            NoaDebuggerText.ChangeFont(fontAsset, fontMaterial, fontSizeRate);
            NoaDebuggerApiStatus.SetFont status = NoaDebuggerManager._ApplyFontToInstantiatedObjects();

            if (status == NoaDebuggerApiStatus.SetFont.Succeed)
            {
                UpdateManager.AddOrOverwriteAction(NoaDebuggerManager.MonitorFontOnUpdateKey, NoaDebuggerManager._MonitorFontOnUpdate);
            }
            else
            {
                LogModel.LogWarning(NoaDebuggerApiMessage.GetFailedMessage(status));

                if (status != NoaDebuggerApiStatus.SetFont.FailedNoaDebuggerIsNull)
                {
                    NoaDebuggerManager._ResetFont();
                }
            }
        }

        static void _MonitorFontOnUpdate()
        {
            if (!NoaDebuggerText.HasFontAsset)
            {
                NoaDebuggerManager._ResetFont();

                LogModel.LogWarning("fontAsset is null, reverting to default font.");

                UpdateManager.DeleteAction(NoaDebuggerManager.MonitorFontOnUpdateKey);
            }
        }

        static NoaDebuggerApiStatus.SetFont _ApplyFontToInstantiatedObjects()
        {
            if (NoaDebuggerManager._noaDebugger == null)
            {
                return NoaDebuggerApiStatus.SetFont.FailedNoaDebuggerIsNull;
            }

            NoaDebuggerText[] allTexts = NoaDebuggerManager._noaDebugger.GetComponentsInChildren<NoaDebuggerText>(includeInactive:true);

            foreach (NoaDebuggerText text in allTexts)
            {
                NoaDebuggerApiStatus.SetFont status = text.ApplyFont(isForce:true);
                if (status != NoaDebuggerApiStatus.SetFont.Succeed)
                {
                    return status;
                }
            }

            return NoaDebuggerApiStatus.SetFont.Succeed;
        }

        static void _ResetFont()
        {
            NoaDebuggerText.ResetFont();
            NoaDebuggerManager._ApplyFontToInstantiatedObjects();
        }

        public static void SetCanvasScale(float scale)
        {
            if (!NoaDebuggerManager.IsDebuggerInitialized())
            {
                return;
            }

            NoaDebuggerManager._noaDebugger.SetCanvasScale(scale);
        }

        public static void SetShortcutsEnabled(bool isEnabled)
        {
            if (!NoaDebuggerManager.IsDebuggerInitialized())
            {
                return;
            }

            NoaDebuggerManager._noaDebugger.SetShortcutsEnabled(isEnabled);
        }

        public static void EnableWorldSpaceRendering(Camera worldCamera = null)
        {
            if (!NoaDebuggerManager.IsDebuggerInitialized())
            {
                return;
            }

            NoaDebuggerManager._noaDebugger.EnableWorldSpaceRendering(worldCamera);
        }

        public static void DisableWorldSpaceRendering()
        {
            if (!NoaDebuggerManager.IsDebuggerInitialized())
            {
                return;
            }

            NoaDebuggerManager._noaDebugger.DisableWorldSpaceRendering();
        }

        public static void CopyToClipboard(string text)
        {
            ClipboardModel.Copy(text);

            NoaDebugger.ShowToast(new ToastViewLinker {_label = NoaDebuggerDefine.ClipboardCopiedText});
        }

        public static void DownloadText(string text, string fileName, string mimeType)
        {
            FileDownloader.DownloadFile(fileName, text, mimeType, FileDownloader.OnDownloadFinished);
        }

        public static void DestroyDebugger()
        {
            if (NoaDebuggerManager.IsDebuggerInitialized())
            {
                UpdateManager.DeleteAction(NoaDebuggerManager.ErrorObserverKey);
                OnHide = null;

                NoaDebuggerManager._noaDebugger._DestroyNoaDebugger();
                Application.logMessageReceived -= _instance._HandleException;

                GameObject.Destroy(NoaDebuggerManager._noaDebugger.gameObject);
                NoaDebuggerManager._instance._buttonEffectManager.Dispose();
                NoaDebuggerManager._instance._noaDebuggerButton = null;
                NoaDebuggerManager._instance._buttonEffectManager = null;
                NoaDebuggerManager._noaDebugger.Dispose();
                NoaDebuggerManager._noaDebugger = null;
            }
        }

        public static bool IsDebuggerInitialized()
        {
            return NoaDebuggerManager._noaDebugger != null && NoaDebuggerManager._noaDebugger.IsInitialized;
        }

        public static bool IsError()
        {
            return NoaDebuggerManager._instance._isError;
        }

        static void RunOnShow(int index)
        {
            OnShow?.Invoke(index);
        }

        static void RunOnHide(int index)
        {
            OnHide?.Invoke(index);
        }

        static void RunOnMenuChanged(int index, bool isCustomMenu)
        {
            OnMenuChanged?.Invoke(index, isCustomMenu);
        }

        public static INoaUIElementView[] GetAllUIElements()
        {
            if (!NoaDebuggerManager.IsDebuggerInitialized())
            {
                return Array.Empty<INoaUIElementView>();
            }

            return NoaDebuggerManager._noaDebugger.NoaUIElementManager.RegisteredElements;
        }

        public static void RegisterUIElement(INoaUIElement element)
        {
            if (!NoaDebuggerManager.IsDebuggerInitialized())
            {
                return;
            }

            NoaDebuggerManager._noaDebugger.NoaUIElementManager.RegisterUIElement(element);
        }

        public static void UnregisterUIElement(string key)
        {
            if (!NoaDebuggerManager.IsDebuggerInitialized())
            {
                return;
            }

            NoaDebuggerManager._noaDebugger.NoaUIElementManager.UnregisterUIElement(key);
        }

        public static void UnregisterAllUIElements()
        {
            if (!NoaDebuggerManager.IsDebuggerInitialized())
            {
                return;
            }

            NoaDebuggerManager._noaDebugger.NoaUIElementManager.UnregisterAllUIElements();
        }

        public static bool IsUIElementRegistered(string key = null)
        {
            if (!NoaDebuggerManager.IsDebuggerInitialized())
            {
                return false;
            }

            return NoaDebuggerManager._noaDebugger.NoaUIElementManager.IsUIElementRegistered(key);
        }

        public static void SetUIElementVisibility(string key, bool visible)
        {
            if (!NoaDebuggerManager.IsDebuggerInitialized())
            {
                return;
            }

            NoaDebuggerManager._noaDebugger.NoaUIElementManager.SetUIElementVisibility(key, visible);
        }

        public static bool IsUIElementVisible(string key = null)
        {
            if (!NoaDebuggerManager.IsDebuggerInitialized())
            {
                return false;
            }

            if (string.IsNullOrEmpty(key))
            {
                return NoaDebuggerVisibilityManager.IsAllUIElementActive;
            }

            return NoaDebuggerManager._noaDebugger.NoaUIElementManager.IsUIElementVisible(key);
        }

        public static void SetVerticalAlignment(AnchorType anchorType)
        {
            if (!NoaDebuggerManager.IsDebuggerInitialized())
            {
                return;
            }

            NoaDebuggerManager._noaDebugger.NoaUIElementManager.SetVerticalAlignment(anchorType);
        }

        public static void SetHorizontalAlignment(AnchorType anchorType)
        {
            if (!NoaDebuggerManager.IsDebuggerInitialized())
            {
                return;
            }

            NoaDebuggerManager._noaDebugger.NoaUIElementManager.SetHorizontalAlignment(anchorType);
        }

        public static void ResetRootRectSize()
        {
            NoaDebuggerManager._noaDebugger.NoaUIElementManager.ResetRootRectSize();
        }

        class InitializeNoaDebuggerBehaviour : MonoBehaviour
        {
            public static Action OnInitComplete { get; set; }

            void Awake()
            {
                StartCoroutine(_Init());
            }

            IEnumerator _Init()
            {
                if (NoaDebuggerManager._instance._isInitializedOnce)
                {
                    yield return null;
                }

                NoaDebuggerManager._Instantiate();
                OnInitComplete?.Invoke();
                OnInitComplete = null;
                GameObject.Destroy(gameObject);
            }
        }

        class NoaDebuggerDestroyReceiver : MonoBehaviour
        {
            void OnDestroy()
            {
                DestroyDebugger();
            }
        }
    }
}
