using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NoaDebugger
{
    sealed class InformationPresenter : NoaDebuggerToolBase, INoaDebuggerTool
    {
        class InformationExportData : IExportData
        {
            const string EXPORT_FILE_PREFIX = "information";

            readonly DownloadInfo _downloadData = new DownloadInfo(InformationExportData.EXPORT_FILE_PREFIX);

            public DownloadInfo GetDownloadInfo()
            {
                return _downloadData;
            }
        }

        [Header("MainView")]
        [SerializeField]
        InformationView _mainViewPrefab;
        InformationView _mainView;

        bool IsShowView => _mainView != null && _mainView.gameObject.activeSelf;

        [SerializeField, Header("DownloadDialog")]
        DownloadDialog _dialogPrefab;

        DownloadDialogPresenter _downloadDialogPresenter;
        ApplicationInformationModel _applicationInformationModel;
        DeviceInformationModel _deviceInformationModel;
        CustomInformationModel _customInfoModel;
        AutoRefreshSwitcher _autoRefreshSwitcher;

        bool _isSelectionMode = false;

        bool _isRebuildCustomView = false;

        [Obsolete]
        SystemInformationModel _systemInfoModel;
        [Obsolete]
        UnityInformationModel _unityInfoModel;

        InformationSelectModel _informationSelectModel;

        InformationView.ToggleTabType _tabType = InformationView.ToggleTabType.Application;

        public ToolNotificationStatus NotifyStatus => ToolNotificationStatus.None;

        public Func<string, string, bool> _onDownload;

        public UnityAction<Dictionary<string,List<InformationGroup>>,string> _onCopied;

        public UnityAction<Dictionary<string,List<InformationGroup>>> _onSend;

        public void Init()
        {
            _applicationInformationModel = new ApplicationInformationModel();
            _deviceInformationModel = new DeviceInformationModel();
            _customInfoModel = new CustomInformationModel();

            _autoRefreshSwitcher = new AutoRefreshSwitcher(
                _OnAutoRefresh,
                NoaDebuggerPrefsDefine.PrefsKeyInformationAutoRefresh,
                NoaDebuggerDefine.InformationAutoRefreshInterval);

#pragma warning disable CS0618 
#pragma warning disable CS0612 
            _systemInfoModel = new SystemInformationModel();
            _unityInfoModel = new UnityInformationModel();
#pragma warning restore CS0618 
#pragma warning restore CS0612 
            _informationSelectModel = new InformationSelectModel();
            _informationSelectModel.OnSelectChild += _OnSelectChild;
            _informationSelectModel.SelectTab(_tabType);
        }

        class InformationMenuInfo : IMenuInfo
        {
            public string Name => "Information";
            public string MenuName => "Information";
            public int SortNo => NoaDebuggerDefine.INFORMATION_MENU_SORT_NO;
        }

        InformationMenuInfo _informationMenuInfo;

        public IMenuInfo MenuInfo()
        {
            return _informationMenuInfo ??= new InformationMenuInfo();
        }

        public void ShowView(Transform parent)
        {
            if (_mainView == null)
            {
                _mainView = GameObject.Instantiate(_mainViewPrefab, parent);
                _InitView(_mainView);
            }

            _UpdateTabView();
        }

        void _InitView(InformationView view)
        {
            view.OnClickTab += _OnClickTabButton;
            view.OnToggleSelectionButton += _OnSelectionButton;
            view.OnTapRefreshButton += _OnRefreshButton;
            view.OnLongTapRefreshButton += _OnUpdateAutoRefreshState;
            view.OnClickSelectAll += _OnSelectAll;
            view.OnClickDeselectAll += _OnDeselectAll;
            view.OnClickCopy += _OnCopy;
            view.OnClickDownload += _OnDownload;
            view.OnClickSend += _OnSend;
        }

        void _OnClickTabButton(InformationView.ToggleTabType tabType)
        {
            _tabType = tabType;
            _informationSelectModel.SelectTab(_tabType);
            _UpdateTabView();
        }

        void _OnSelectionButton(bool isSelectionMode)
        {
            _isSelectionMode = isSelectionMode;

            _UpdateTabView();
        }

        void _OnRefreshButton(bool isAutoRefresh)
        {
            if (isAutoRefresh)
            {
                _OnUpdateAutoRefreshState();
            }

            _UpdateTabView();
        }

        void _OnUpdateAutoRefreshState()
        {
            bool isAutoRefresh = !_autoRefreshSwitcher.IsAutoRefresh;
            _autoRefreshSwitcher.UpdateAutoRefresh(isAutoRefresh);
        }

        void _OnAutoRefresh()
        {
            if (IsShowView)
            {
                _UpdateTabView();
            }
        }

        void _UpdateTabView()
        {
            var viewData = new InformationViewLinker()
            {
                _tabType = _tabType,
                _isSelectionMode = _isSelectionMode,
                _isAutoRefresh = _autoRefreshSwitcher.IsAutoRefresh,
                _totalSelectCount = _informationSelectModel.TotalSelectCount(),
                _isSendCallbackEnabled = _onSend != null
            };

            switch (_tabType)
            {
                case InformationView.ToggleTabType.Application:
                    viewData._applicationInformationViewLinker = _CreateApplicationInformationViewLinker(_applicationInformationModel);
                    _SetInformationCommonLinker(viewData._applicationInformationViewLinker);

                    break;

                case InformationView.ToggleTabType.Device:
                    viewData._deviceInformationViewLinker = _CreateDeviceInformationViewLinker(_deviceInformationModel);
                    _SetInformationCommonLinker(viewData._deviceInformationViewLinker);

                    break;

                case InformationView.ToggleTabType.Custom:
                    viewData._customInformationViewLinker = _CreateCustomInformationViewLinker(_customInfoModel, _isRebuildCustomView);
                    _isRebuildCustomView = false;
                    _SetInformationCommonLinker(viewData._customInformationViewLinker);

                    break;
            }

            _mainView.Show(viewData);
            _mainView.gameObject.SetActive(true);
        }

        ApplicationInformationViewLinker _CreateApplicationInformationViewLinker(ApplicationInformationModel applicationModel)
        {
            var model = _informationSelectModel.GetSelectableModel<InformationApplicationSelectModel>();
            model.UpdateApplicationInfo(applicationModel);

            return new ApplicationInformationViewLinker
            {
                _build = model.BuildInfo,
                _runtime = model.RuntimeInfo,
                _screen = model.ScreenInfo,
                _graphicsSettings = model.GraphicsSettingsInfo,
                _logging = model.LoggingInfo,
                _other = model.OtherInfo
            };
        }

        DeviceInformationViewLinker _CreateDeviceInformationViewLinker(DeviceInformationModel deviceModel)
        {
            var model = _informationSelectModel.GetSelectableModel<InformationDeviceSelectModel>();
            model.UpdateDeviceInfo(deviceModel);

            return new DeviceInformationViewLinker
            {
                _device = model.DeviceGeneralInfo,
                _os = model.OSInfo,
                _processor = model.ProcessorInfo,
                _graphicsDevice = model.GraphicsDeviceInfo,
                _systemMemory = model.SystemMemoryInfo,
                _display = model.DisplayInfo,
                _graphicsSupport = model.GraphicsSupportInfo,
                _textureFormatSupport = model.TextureFormatSupportInfo,
                _featureSupport = model.FeatureSupportInfo,
                _network = model.NetworkInfo,
                _system = model.SystemInfo,
                _input = model.InputInfo
            };
        }

        CustomInformationViewLinker _CreateCustomInformationViewLinker(CustomInformationModel customModel, bool isRebuildCustomView)
        {
            var model = _informationSelectModel.GetSelectableModel<InformationCustomSelectModel>();
            model.UpdateCustomInfo(customModel);

            var allGroups = customModel.GetAllGroups();
            var linker = new CustomInformationViewLinker()
            {
                _groups = allGroups,
                _isBuild = isRebuildCustomView
            };

            return linker;
        }

        void _SetInformationCommonLinker(InformationCommonViewLinker linker)
        {
            linker._onSelectChild = _informationSelectModel.OnSelectChild;
            linker._isSelection = _isSelectionMode;
        }

        void _OnSelectChild(string groupName, string key, bool isSelect)
        {
            if (isSelect)
            {
                _informationSelectModel.Select(groupName, key);
            }
            else
            {
                _informationSelectModel.Deselect(groupName, key);
            }

            _UpdateTabView();
        }

        void _OnSelectAll()
        {
            _informationSelectModel.SelectAll();
            _UpdateTabView();
        }

        void _OnDeselectAll()
        {
            _informationSelectModel.DeselectAll();
            _UpdateTabView();
        }

        void _OnCopy()
        {
            string clipboardText = _informationSelectModel.CreateExportJsonString();
            ClipboardModel.Copy(clipboardText);
            NoaDebugger.ShowToast(new ToastViewLinker {_label = NoaDebuggerDefine.ClipboardCopiedText});

            var informationGroups = _informationSelectModel.CreateExportAllSelectedInformationGroup();
            _onCopied?.Invoke(informationGroups, clipboardText);
        }

        void _OnDownload()
        {
            if (_downloadDialogPresenter == null)
            {
                _downloadDialogPresenter = new DownloadDialogPresenter(_dialogPrefab);
                _downloadDialogPresenter.OnExecDownload += _OnExecDownload;
            }

            _downloadDialogPresenter.ShowDialog();
        }

        void _OnExecDownload(string label, Action<FileDownloader.DownloadExecutedInfo> completed)
        {
            DownloadInfo downloadInfo = new InformationExportData().GetDownloadInfo();
            string fileName = downloadInfo.GetExportFileName(label, DateTime.Now);
            string json = _informationSelectModel.CreateExportJsonString(label);

            FileDownloader.DownloadFileWithUserCallbacks(
                fileName,
                json,
                completed,
                _downloadDialogPresenter.HideDialog,
                NoaInformation.DownloadCallbacks,
                _onDownload);
        }

        void _OnSend()
        {
            if (_onSend == null)
            {
                return;
            }

            var informationGroups = _informationSelectModel.CreateExportAllSelectedInformationGroup();
            _onSend.Invoke(informationGroups);

            NoaDebugger.ShowToast(new ToastViewLinker()
            {
                _label = NoaDebuggerDefine.InformationSentText,
            });
        }

        public void AlignmentUI(bool isReverse)
        {
            _mainView.AlignmentUI(isReverse);
        }

        public void AddCustomGroup(string groupName, string displayName, int order = Int32.MaxValue)
        {
            _customInfoModel.AddGroup(groupName, displayName, order);
            _isRebuildCustomView = true;
        }

        public void AddCustomGroupKeyValue(string groupName, InformationCustomKeyValue customKeyValue)
        {
            _customInfoModel.AddKeyValues(groupName, customKeyValue);
            _isRebuildCustomView = true;
        }

        public void RefreshCustomView()
        {
            if (_tabType == InformationView.ToggleTabType.Custom)
            {
                _UpdateTabView();
            }
        }

        public NoaCustomInformationIntValue GetCustomIntValue(string groupName, string keyName)
        {
            return _customInfoModel.GetIntValue(groupName, keyName);
        }

        public NoaCustomInformationFloatValue GetCustomFloatValue(string groupName, string keyName)
        {
            return _customInfoModel.GetFloatValue(groupName, keyName);
        }

        public NoaCustomInformationBoolValue GetCustomBoolValue(string groupName, string keyName)
        {
            return _customInfoModel.GetBoolValue(groupName, keyName);
        }

        public NoaCustomInformationStringValue GetCustomStringValue(string groupName, string keyName)
        {
            return _customInfoModel.GetStringValue(groupName, keyName);
        }

        public NoaCustomInformationEnumValue GetCustomEnumValue(string groupName, string keyName)
        {
            return _customInfoModel.GetEnumValue(groupName, keyName);
        }

        public NoaCustomInformationGroup GetNoaCustomGroup(string groupName)
        {
            InformationCustomGroup groupInfo = _customInfoModel.GetGroup(groupName);

            if (groupInfo == null)
            {
                return null;
            }

            return new NoaCustomInformationGroup(groupInfo.Name, groupInfo);
        }

        public List<NoaCustomInformationGroup> GetAllCustomGroups()
        {
            var allGroups = new List<NoaCustomInformationGroup>();

            foreach (var groupInfo in _customInfoModel.GetAllGroups())
            {
                allGroups.Add(new NoaCustomInformationGroup(groupInfo.Name, groupInfo));
            }

            return allGroups;
        }

        public void RemoveCustomKeyValue(string groupName, string keyName)
        {
            _customInfoModel.RemoveKeyValue(groupName, keyName);
            _isRebuildCustomView = true;
        }

        public void RemoveCustomGroup(string groupName)
        {
            _customInfoModel.RemoveGroup(groupName);
            _isRebuildCustomView = true;
        }

        public void RemoveAllCustomGroup()
        {
            _customInfoModel.RemoveAll();
            _isRebuildCustomView = true;
        }

        void _OnHidden()
        {
            if (_mainView != null)
            {
                _mainView.gameObject.SetActive(false);
            }

            if (_downloadDialogPresenter != null)
            {
                _downloadDialogPresenter.Dispose();
            }
        }

        public void OnHidden()
        {
            _OnHidden();
        }

        public void OnToolDispose()
        {
            _OnHidden();
            _applicationInformationModel = null;
            _deviceInformationModel = null;
            _customInfoModel = null;
#pragma warning disable CS0612 
            _systemInfoModel = null;
            _unityInfoModel = null;
#pragma warning restore CS0612 
        }

        [Obsolete]
        public SystemInformation CreateSystemInformation()
        {
            _systemInfoModel.OnUpdate();

            return new SystemInformation(_systemInfoModel);
        }

        [Obsolete]
        public UnityInformation CreateUnityInformation()
        {
            _unityInfoModel.OnUpdate();

            return new UnityInformation(_unityInfoModel);
        }

        public ApplicationInformation CreateApplicationInformation()
        {
            _applicationInformationModel.OnUpdate();

            return new ApplicationInformation(_applicationInformationModel);
        }

        public DeviceInformation CreateDeviceInformation()
        {
            _deviceInformationModel.OnUpdate();

            return new DeviceInformation(_deviceInformationModel);
        }

        void OnDestroy()
        {
            _mainViewPrefab = default;
            _mainView = default;
            _dialogPrefab = default;
            _downloadDialogPresenter = default;
            _applicationInformationModel = default;
            _deviceInformationModel = default;
            _customInfoModel = default;
            _autoRefreshSwitcher = default;
            _informationMenuInfo = default;
#pragma warning disable CS0612 
            _systemInfoModel = default;
            _unityInfoModel = default;
#pragma warning restore CS0612 
        }
    }
}
