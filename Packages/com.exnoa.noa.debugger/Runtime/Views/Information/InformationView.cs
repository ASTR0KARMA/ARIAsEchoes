using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class InformationView : NoaDebuggerToolViewBase<InformationViewLinker>
    {
        [Header("Tab menu")]
        [SerializeField]
        ToggleButtonBase _appButton;

        [SerializeField]
        ToggleButtonBase _deviceButton;

        [SerializeField]
        ToggleButtonBase _customButton;

        [SerializeField]
        NoaDebuggerText _selectCountText;

        [SerializeField]
        ToggleColorButton _selectionButton;

        [SerializeField]
        LongTapToggleColorButton _refreshButton;

        [Header("Tab view")]
        [SerializeField]
        ApplicationInformationView _applicationInfoView;

        [SerializeField]
        DeviceInformationView _deviceInfoView;

        [SerializeField]
        CustomInformationView _customInfoView;

        [Header("Footer")]
        [SerializeField]
        GameObject _footer;

        [SerializeField]
        Button _selectAllButton;

        [SerializeField]
        Button _deselectAllButton;

        [SerializeField]
        DisableButtonBase _copyButton;

        [SerializeField]
        DisableButtonBase _downloadButton;

        [SerializeField]
        DisableButtonBase _sendButton;

        public enum ToggleTabType
        {
            Application = 0,
            Device = 1,
            Custom = 2,
        }

        public event UnityAction<ToggleTabType> OnClickTab;

        public event UnityAction<bool> OnToggleSelectionButton;

        public event UnityAction<bool> OnTapRefreshButton;

        public event UnityAction OnLongTapRefreshButton;

        public event UnityAction OnClickSelectAll;

        public event UnityAction OnClickDeselectAll;

        public event UnityAction OnClickCopy;

        public event UnityAction OnClickDownload;

        public event UnityAction OnClickSend;

        protected override void _Init()
        {
            _appButton._onClick.RemoveAllListeners();
            _deviceButton._onClick.RemoveAllListeners();
            _customButton._onClick.RemoveListener(_OnClickCustomTab);
            _selectionButton._onClick.RemoveAllListeners();
            _refreshButton.OnClick.RemoveAllListeners();
            _refreshButton.OnToggle.RemoveAllListeners();
            _selectAllButton.onClick.RemoveAllListeners();
            _deselectAllButton.onClick.RemoveAllListeners();
            _copyButton._onClick.RemoveAllListeners();
            _downloadButton._onClick.RemoveAllListeners();
            _sendButton._onClick.RemoveAllListeners();
            _appButton._onClick.AddListener(_OnClickAppTab);
            _deviceButton._onClick.AddListener(_OnClickDeviceTab);
            _customButton._onClick.AddListener(_OnClickCustomTab);
            _selectionButton._onClick.AddListener(_OnClickSelection);
            _refreshButton.OnClick.AddListener(_OnClickReload);
            _refreshButton.OnToggle.AddListener(_OnLongTapRefreshButton);
            _selectAllButton.onClick.AddListener(_OnClickSelectAll);
            _deselectAllButton.onClick.AddListener(_OnClickDeselectAll);
            _copyButton._onClick.AddListener(_OnClickCopy);
            _downloadButton._onClick.AddListener(_OnClickDownload);
            _sendButton._onClick.AddListener(_OnClickSend);

            _downloadButton.Interactable = FileDownloader.CanDownload();

            _footer.SetActive(false);
        }

        protected override void _OnShow(InformationViewLinker linker)
        {
            switch (linker._tabType)
            {
                case ToggleTabType.Application:
                    _appButton.Init(true);
                    _deviceButton.Init(false);
                    _customButton.Init(false);
                    _deviceInfoView.Hide();
                    _applicationInfoView.Show(linker._applicationInformationViewLinker);
                    _customInfoView.Hide();

                    break;

                case ToggleTabType.Device:
                    _appButton.Init(false);
                    _deviceButton.Init(true);
                    _customButton.Init(false);
                    _applicationInfoView.Hide();
                    _deviceInfoView.Show(linker._deviceInformationViewLinker);
                    _customInfoView.Hide();

                    break;

                case ToggleTabType.Custom:
                    _appButton.Init(false);
                    _deviceButton.Init(false);
                    _customButton.Init(true);
                    _applicationInfoView.Hide();
                    _deviceInfoView.Hide();
                    _customInfoView.Show(linker._customInformationViewLinker);

                    break;
            }

            _footer.gameObject.SetActive(linker._isSelectionMode);
            bool isButtonEnabled = linker._totalSelectCount > 0;
            _copyButton.Interactable = isButtonEnabled;
            _downloadButton.Interactable = isButtonEnabled;
            bool isSendButtonEnabled = isButtonEnabled && linker._isSendCallbackEnabled;
            _sendButton.Interactable = isSendButtonEnabled;
            _refreshButton.Init(linker._isAutoRefresh);
            _selectCountText.text = linker._totalSelectCount.ToString();
        }

        void _OnClickAppTab(bool isOn)
        {
            if (isOn)
            {
                OnClickTab?.Invoke(ToggleTabType.Application);
            }
        }

        void _OnClickDeviceTab(bool isOn)
        {
            if (isOn)
            {
                OnClickTab?.Invoke(ToggleTabType.Device);
            }
        }

        void _OnClickCustomTab(bool isOn)
        {
            if (isOn)
            {
                OnClickTab?.Invoke(ToggleTabType.Custom);
            }
        }

        void _OnClickSelection(bool isOn)
        {
            OnToggleSelectionButton?.Invoke(isOn);
        }

        void _OnClickReload()
        {
            OnTapRefreshButton?.Invoke(_refreshButton.IsOn);
        }

        void _OnLongTapRefreshButton(bool isToggleOn)
        {
            OnLongTapRefreshButton?.Invoke();
        }

        void _OnClickSelectAll()
        {
            OnClickSelectAll?.Invoke();
        }

        void _OnClickDeselectAll()
        {
            OnClickDeselectAll?.Invoke();
        }

        void _OnClickCopy()
        {
            OnClickCopy?.Invoke();
        }

        void _OnClickDownload()
        {
            OnClickDownload?.Invoke();
        }

        void _OnClickSend()
        {
            OnClickSend?.Invoke();
        }
    }

    sealed class InformationViewLinker : ViewLinkerBase
    {
        public ApplicationInformationViewLinker _applicationInformationViewLinker;

        public DeviceInformationViewLinker _deviceInformationViewLinker;

        public CustomInformationViewLinker _customInformationViewLinker;

        public InformationView.ToggleTabType _tabType;

        public bool _isSelectionMode;

        public bool _isAutoRefresh;

        public int _totalSelectCount;

        public bool _isSendCallbackEnabled;
    }
}
