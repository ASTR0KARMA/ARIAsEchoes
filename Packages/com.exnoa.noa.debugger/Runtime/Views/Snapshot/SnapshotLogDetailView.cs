using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class SnapshotLogDetailView : ViewBase<SnapshotViewLinker>
    {
        [Header("Header")]
        [SerializeField]
        GameObject _header;
        [SerializeField]
        Image _icon;

        [Header("Drawer")]
        [SerializeField]
        Transform _drawersParent;
        [SerializeField]
        SnapshotFpsTextDrawerComponent _fpsComponent;
        [SerializeField]
        SnapshotMemoryTextDrawerComponent _memoryComponent;
        [SerializeField]
        RenderingTextDrawerComponent _renderingComponent;
        [SerializeField]
        BatteryTextDrawerComponent _batteryComponent;
        [SerializeField]
        ThermalTextDrawerComponent _thermalComponent;

        [Header("Snapshot")]
        [SerializeField]
        NoaDebuggerText _label;
        [SerializeField]
        SnapshotModel.ToggleState _toggleState;
        [SerializeField]
        ScrollRect _profilingViewRoot;
        [SerializeField]
        NoaDebuggerText _emptyText;

        [SerializeField]
        SnapshotAdditionalInfoView _additionalInfoView;

        List<SnapshotAdditionalInfoView> _additionalInfoViewPrev;
        List<SnapshotAdditionalInfoView> _additionalInfoViewAfter;

        [Header("Button")]
        [SerializeField]
        Button _copyButton;

        public UnityAction<int> OnClickCopyButton { get; set; }
        SnapshotLogRecordInformation _selectedLog;

        void Awake()
        {
            Assert.IsNotNull(_header);
            Assert.IsNotNull(_icon);
            Assert.IsNotNull(_fpsComponent);
            Assert.IsNotNull(_memoryComponent);
            Assert.IsNotNull(_renderingComponent);
            Assert.IsNotNull(_batteryComponent);
            Assert.IsNotNull(_thermalComponent);
            Assert.IsNotNull(_label);
            Assert.IsNotNull(_profilingViewRoot);
            Assert.IsNotNull(_emptyText);
            Assert.IsNotNull(_additionalInfoView);
            Assert.IsNotNull(_copyButton);

            _copyButton.onClick.RemoveAllListeners();
            _copyButton.onClick.AddListener(_OnCopy);
        }

        protected override void _OnShow(SnapshotViewLinker linker)
        {
            if (linker._logList == null)
            {
                return;
            }

            bool isComparison = linker._isComparison;
            _selectedLog = linker._logList.FirstOrDefault(
                logData => logData.IsSelected ||
                           isComparison &&
                           logData.ToggleState == _toggleState);
            bool isShowProfilerInfo = _selectedLog?.Snapshot != null;
            bool isShowAdditionalInfo = _selectedLog?.AdditionalInfo?.Count > 0;
            bool showSnapshotLogDetail = isShowProfilerInfo || isShowAdditionalInfo;
            _profilingViewRoot.gameObject.SetActive(true);
            _profilingViewRoot.viewport.gameObject.SetActive(showSnapshotLogDetail);

            _label.text = _selectedLog?.Label;
            _header.SetActive(!isComparison);

            _profilingViewRoot.enabled = showSnapshotLogDetail;
            _emptyText.gameObject.SetActive(!showSnapshotLogDetail);
            if (!showSnapshotLogDetail)
            {
                _profilingViewRoot.enabled = false;
                _profilingViewRoot.horizontalScrollbar.gameObject.SetActive(false);
                _profilingViewRoot.verticalScrollbar.gameObject.SetActive(false);
                return;
            }

            _icon.gameObject.SetActive(isComparison);
            switch (_toggleState)
            {
                case SnapshotModel.ToggleState.SelectedFirst:
                    _icon.color = NoaDebuggerDefine.ImageColors.SnapshotFirstSelected;
                    break;

                case SnapshotModel.ToggleState.SelectedSecond:
                    _icon.color = NoaDebuggerDefine.ImageColors.SnapshotSecondSelected;
                    break;
            }

            _fpsComponent.gameObject.SetActive(isShowProfilerInfo);
            _memoryComponent.gameObject.SetActive(isShowProfilerInfo);
            _renderingComponent.gameObject.SetActive(isShowProfilerInfo);
            _batteryComponent.gameObject.SetActive(isShowProfilerInfo);
            _thermalComponent.gameObject.SetActive(isShowProfilerInfo);

            if (isShowProfilerInfo)
            {
                _OnShowProfilingInfo(_selectedLog.Snapshot);
            }

            if (_additionalInfoViewPrev != null)
            {
                foreach (var additionalInfoView in _additionalInfoViewPrev)
                {
                    Destroy(additionalInfoView.gameObject);
                }

                _additionalInfoViewPrev.Clear();
            }

            if (isShowAdditionalInfo)
            {
                if (_additionalInfoViewAfter == null)
                {
                    _additionalInfoViewAfter = new List<SnapshotAdditionalInfoView>();
                }

                foreach (var additionalInfo in _selectedLog.AdditionalInfo)
                {
                    var additionalInfoView = Instantiate(_additionalInfoView, _drawersParent);
                    var additionalInfoViewLinker = new SnapshotAdditionalInfoViewLinker()
                    {
                        _category = SnapshotPresenter.ConvertCategoryName(additionalInfo.Key),
                        _categoryItems = additionalInfo.Value.CategoryItems
                    };

                    additionalInfoView.Show(additionalInfoViewLinker);
                    _additionalInfoViewAfter.Add(additionalInfoView);
                }

                _additionalInfoViewPrev = new List<SnapshotAdditionalInfoView>(_additionalInfoViewAfter.ToArray());
                _additionalInfoViewAfter.Clear();
            }

            _copyButton.gameObject.SetActive(true);
        }

        void _OnShowProfilingInfo(ProfilerSnapshotData snapshotData)
        {
            if (snapshotData.FpsInfo != null)
            {
                _fpsComponent.OnShowFpsText(snapshotData.FpsInfo);
            }

            if (snapshotData.MemoryInfo != null)
            {
                _memoryComponent.OnShowMemoryText(snapshotData.MemoryInfo);
            }

            if (snapshotData.RenderingInfo != null)
            {
                _renderingComponent.OnShowRenderingText(snapshotData.RenderingInfo);
            }

#pragma warning disable 612
            if (snapshotData.BatteryInfo != null)
            {
                _batteryComponent.OnShowBatteryText(snapshotData.BatteryInfo);
            }

            if (snapshotData.ThermalInfo != null)
            {
                _thermalComponent.OnShowThermalText(snapshotData.ThermalInfo);
            }
#pragma warning restore 612
        }

        void OnDestroy()
        {
            _header = default;
            _icon = default;
            _fpsComponent = default;
            _memoryComponent = default;
            _renderingComponent = default;
            _batteryComponent = default;
            _thermalComponent = default;
            _label = default;
            _profilingViewRoot = default;
            _emptyText = default;
            _additionalInfoView = default;
            _additionalInfoViewPrev = default;
            _additionalInfoViewAfter = default;
            _selectedLog = default;
            OnClickCopyButton = default;
        }

        void _OnCopy()
        {
            if (_selectedLog == null)
            {
                return;
            }
            OnClickCopyButton?.Invoke(_selectedLog.Id);

            _copyButton.gameObject.SetActive(false);
        }
    }
}
