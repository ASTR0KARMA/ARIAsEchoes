using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Scripting;
using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class ProfilerView : NoaDebuggerToolViewBase<ProfilerViewLinker>
    {
        [Header("FPS")]
        [SerializeField]
        ToggleButtonBase _fpsProfilingButton;

        [SerializeField]
        FpsTextDrawerComponent _fpsTextComponent;

        [SerializeField]
        FpsGaugeDrawerComponent _fpsGaugeComponent;

        [SerializeField]
        TMP_Dropdown _vSyncCountDropdown;

        [SerializeField]
        TMP_Dropdown _targetFrameRateDropdown;

        [Header("FrameTime")]
        [SerializeField]
        ToggleButtonBase _frameTimeProfilingButton;

        [SerializeField]
        FrameTimeChartDrawerComponent _frameTimeChartComponent;

        [Header("Memory")]
        [SerializeField]
        ToggleButtonBase _memoryProfilingButton;

        [SerializeField]
        ToggleButtonBase _memoryGraphButton;

        [SerializeField]
        MemoryTextDrawerComponent _memoryTextComponent;

        [SerializeField]
        MemoryChartDrawerComponent _memoryChartComponent;

        [SerializeField]
        MemoryGaugeDrawerComponent _memoryGaugeComponent;

        [SerializeField]
        Button _gcCollectButton;

        [SerializeField]
        Button _unloadAssetButton;

        [SerializeField]
        TMP_Dropdown _memoryProfilingTypeDropdown;

        [Header("Rendering")]
        [SerializeField]
        ToggleButtonBase _renderingProfilingButton;

        [SerializeField]
        ToggleButtonBase _renderingGraphButton;

        [SerializeField]
        RenderingTextDrawerComponent _renderingTextComponent;

        [SerializeField]
        RenderingChartDrawerComponent _renderingChartComponent;

        [SerializeField]
        ToggleButtonGroup _renderingValueOptionGroup;

        [SerializeField]
        ToggleButtonBase _renderingValueOptionSetPassCalls;

        [SerializeField]
        ToggleButtonBase _renderingValueOptionDrawCalls;

        [SerializeField]
        ToggleButtonBase _renderingValueOptionBatches;

        [SerializeField]
        ToggleButtonBase _renderingValueOptionTriangles;

        [SerializeField]
        ToggleButtonBase _renderingValueOptionVertices;

        public event UnityAction<bool> OnFpsProfilingStateChanged;
        public event UnityAction<bool> OnFrameTimeProfilingStateChanged;
        public event UnityAction<int> OnVSyncCountChanged;
        public event UnityAction<int> OnTargetFrameRateChanged;
        public event UnityAction<bool> OnMemoryProfilingStateChanged;
        public event UnityAction<bool> OnMemoryGraphStateChanged;
        public event UnityAction<NoaProfiler.MemoryProfilingType> OnMemoryProfilingTypeChanged;
        public event UnityAction OnGCCollectExecuted;
        public event UnityAction OnUnloadAssetExecuted;
        public event UnityAction<bool> OnRenderingProfilingStateChanged;
        public event UnityAction<bool> OnRenderingGraphStateChanged;
        public event UnityAction<RenderingGraphTarget> OnRenderingGraphSelected;

        protected override void _Init()
        {
            Assert.IsNotNull(_fpsProfilingButton);
            Assert.IsNotNull(_fpsTextComponent);
            Assert.IsNotNull(_fpsGaugeComponent);
            Assert.IsNotNull(_vSyncCountDropdown);
            Assert.IsNotNull(_targetFrameRateDropdown);
            Assert.IsNotNull(_frameTimeProfilingButton);
            Assert.IsNotNull(_frameTimeChartComponent);
            Assert.IsNotNull(_memoryProfilingButton);
            Assert.IsNotNull(_memoryGraphButton);
            Assert.IsNotNull(_memoryTextComponent);
            Assert.IsNotNull(_memoryChartComponent);
            Assert.IsNotNull(_memoryGaugeComponent);
            Assert.IsNotNull(_gcCollectButton);
            Assert.IsNotNull(_unloadAssetButton);
            Assert.IsNotNull(_memoryProfilingTypeDropdown);
            Assert.IsNotNull(_renderingProfilingButton);
            Assert.IsNotNull(_renderingGraphButton);
            Assert.IsNotNull(_renderingTextComponent);
            Assert.IsNotNull(_renderingChartComponent);
            Assert.IsNotNull(_renderingValueOptionGroup);
            Assert.IsNotNull(_renderingValueOptionSetPassCalls);
            Assert.IsNotNull(_renderingValueOptionDrawCalls);
            Assert.IsNotNull(_renderingValueOptionBatches);
            Assert.IsNotNull(_renderingValueOptionTriangles);
            Assert.IsNotNull(_renderingValueOptionVertices);
        }

        protected override void _OnStart()
        {
            _frameTimeChartComponent.SetActiveChart(_frameTimeProfilingButton.IsOn);
            _memoryChartComponent.SetActiveChart(_memoryGraphButton.IsOn && _memoryGraphButton.Interactable);
            _renderingChartComponent.SetActiveChart(_renderingGraphButton.IsOn);

            _fpsProfilingButton._onClick.RemoveAllListeners();
            _fpsProfilingButton._onClick.AddListener(_OnClickFpsProfiling);
            _frameTimeProfilingButton._onClick.RemoveAllListeners();
            _frameTimeProfilingButton._onClick.AddListener(_OnClickFrameTimeProfiling);
            _memoryProfilingButton._onClick.RemoveAllListeners();
            _memoryProfilingButton._onClick.AddListener(_OnClickMemoryProfiling);
            _memoryGraphButton._onClick.RemoveAllListeners();
            _memoryGraphButton._onClick.AddListener(_OnClickMemoryGraph);
            _gcCollectButton.onClick.RemoveAllListeners();
            _gcCollectButton.onClick.AddListener(_OnClickGCCollect);
            _unloadAssetButton.onClick.RemoveAllListeners();
            _unloadAssetButton.onClick.AddListener(_OnClickUnloadAsset);
            _renderingProfilingButton._onClick.RemoveAllListeners();
            _renderingProfilingButton._onClick.AddListener(_OnClickRenderProfiling);
            _renderingGraphButton._onClick.RemoveAllListeners();
            _renderingGraphButton._onClick.AddListener(_OnClickRenderGraph);

            _SetDropdown(
                NoaDebuggerDefine.VSyncCountChoices,
                _vSyncCountDropdown,
                _OnVSyncCountChangedChanged,
                QualitySettings.vSyncCount);
            _targetFrameRateDropdown.onValueChanged.RemoveAllListeners();
            _targetFrameRateDropdown.onValueChanged.AddListener(_OnTargetFrameRateChanged);
            _SetDropdown(
                Enum.GetNames(typeof(NoaProfiler.MemoryProfilingType))
                    .Select(ProfilerDrawerHelper.GetMemoryProfilingTypeText)
                    .ToArray(),
                _memoryProfilingTypeDropdown,
                _OnMemoryProfilingTypeChanged);

            _renderingValueOptionSetPassCalls._onClick.AddListener(_OnClickRenderingSetPassCalls);
            _renderingValueOptionDrawCalls._onClick.AddListener(_OnClickRenderingDrawCalls);
            _renderingValueOptionBatches._onClick.AddListener(_OnClickRenderingBatches);
            _renderingValueOptionTriangles._onClick.AddListener(_OnClickRenderingTriangles);
            _renderingValueOptionVertices._onClick.AddListener(_OnClickRenderingVertices);
        }

        protected override void _OnShow(ProfilerViewLinker linker)
        {
            if (linker._fpsInfo != null)
            {
                _OnShowFps(linker._fpsInfo);
            }

            if (linker._frameTimeInfo != null)
            {
                _OnShowFrameTime(linker._frameTimeInfo);
            }

            if (linker._memoryInfo != null)
            {
                _OnShowMemory(linker._memoryInfo);
            }

            if (linker._renderingInfo != null)
            {
                _OnShowRendering(linker._renderingInfo);
            }

            if (linker._vSyncCountIndex != null)
            {
                _vSyncCountDropdown.SetValueWithoutNotify(linker._vSyncCountIndex.Value);
            }

            if (linker._targetFrameRateChoices != null && linker._defaultTargetFrameRateIndex != null)
            {
                _SetDropdownOptions(
                    linker._targetFrameRateChoices,
                    _targetFrameRateDropdown,
                    linker._defaultTargetFrameRateIndex.Value);
            }

            if (linker._defaultTargetFrameRateIndex != null)
            {
                _targetFrameRateDropdown.SetValueWithoutNotify(linker._defaultTargetFrameRateIndex.Value);
            }
        }

        void _OnShowFps(FpsUnchangingInfo info)
        {
            _fpsTextComponent.OnShowFpsText(info, NoaDebuggerDefine.TextColors.ProfilerFps);
            _fpsGaugeComponent.OnShowFpsGauge(info);

            _fpsProfilingButton.Init(info.IsProfiling);
        }

        void _OnShowFrameTime(ProfilerFrameTimeViewInformation info)
        {
            if (info._isEnabled && info._isActive)
            {
                _frameTimeChartComponent.OnShowFrameTime(info);
            }

            _frameTimeProfilingButton.Init(info._isEnabled);

            _frameTimeChartComponent.SetActiveChart(info._isEnabled);
        }

        void _OnShowMemory(MemoryUnchangingInfo info)
        {
            _memoryTextComponent.OnShowMemoryText(info, NoaDebuggerDefine.TextColors.ProfilerMemory);
            _memoryChartComponent.OnShowMemoryChart(info);
            _memoryGaugeComponent.OnShowMemoryGauge(info);

            bool isInteractable = info.IsValid || info.IsViewHyphen;
            _memoryProfilingButton.Init(info.IsProfiling);
            _memoryProfilingButton.Interactable = isInteractable;
            _memoryGraphButton.Init(info.IsGraphShowing);
            _memoryGraphButton.Interactable = isInteractable;

            bool isGraphShowing = info.IsGraphShowing && isInteractable;
            _memoryChartComponent.SetActiveChart(isGraphShowing);

            _gcCollectButton.interactable = GarbageCollector.GCMode != GarbageCollector.Mode.Disabled;

            _memoryProfilingTypeDropdown.SetValueWithoutNotify((int)info.ProfilingType);
        }

        void _OnShowRendering(RenderingUnchangingInfo info)
        {
            _renderingTextComponent.OnShowRenderingText(info, NoaDebuggerDefine.TextColors.ProfilerRendering);
            _renderingChartComponent.OnShowRenderingChart(info);

            _renderingProfilingButton.Init(info.IsProfiling);
            _renderingGraphButton.Init(info.IsGraphShowing);

            _renderingChartComponent.SetActiveChart(info.IsGraphShowing);

            _renderingValueOptionSetPassCalls.Init(info.GraphTarget == RenderingGraphTarget.SetPassCalls);
            _renderingValueOptionDrawCalls.Init(info.GraphTarget == RenderingGraphTarget.DrawCalls);
            _renderingValueOptionBatches.Init(info.GraphTarget == RenderingGraphTarget.Batches);
            _renderingValueOptionTriangles.Init(info.GraphTarget == RenderingGraphTarget.Triangles);
            _renderingValueOptionVertices.Init(info.GraphTarget == RenderingGraphTarget.Vertices);
        }

        void _OnClickFpsProfiling(bool isOn)
        {
            OnFpsProfilingStateChanged?.Invoke(isOn);
        }

        void _OnClickFrameTimeProfiling(bool isOn)
        {
            OnFrameTimeProfilingStateChanged?.Invoke(isOn);
        }

        void _OnVSyncCountChangedChanged(int index)
        {
            OnVSyncCountChanged?.Invoke(index);
        }

        void _OnTargetFrameRateChanged(int index)
        {
            OnTargetFrameRateChanged?.Invoke(index);
        }

        void _OnClickMemoryProfiling(bool isOn)
        {
            OnMemoryProfilingStateChanged?.Invoke(isOn);
        }

        void _OnClickMemoryGraph(bool isOn)
        {
            OnMemoryGraphStateChanged?.Invoke(isOn);
        }

        void _OnClickGCCollect()
        {
            OnGCCollectExecuted?.Invoke();
        }

        void _OnClickUnloadAsset()
        {
            OnUnloadAssetExecuted?.Invoke();
        }

        void _OnMemoryProfilingTypeChanged(int index)
        {
            OnMemoryProfilingTypeChanged?.Invoke((NoaProfiler.MemoryProfilingType)index);
        }

        void _OnClickRenderProfiling(bool isOn)
        {
            OnRenderingProfilingStateChanged?.Invoke(isOn);
        }

        void _OnClickRenderGraph(bool isOn)
        {
            OnRenderingGraphStateChanged?.Invoke(isOn);
        }

        void _OnClickRenderingCheckbox(bool isOn, RenderingGraphTarget target)
        {
            if (isOn)
            {
                OnRenderingGraphSelected?.Invoke(target);
            }
        }

        void _OnClickRenderingSetPassCalls(bool isOn)
        {
            _OnClickRenderingCheckbox(isOn, RenderingGraphTarget.SetPassCalls);
        }

        void _OnClickRenderingDrawCalls(bool isOn)
        {
            _OnClickRenderingCheckbox(isOn, RenderingGraphTarget.DrawCalls);
        }

        void _OnClickRenderingBatches(bool isOn)
        {
            _OnClickRenderingCheckbox(isOn, RenderingGraphTarget.Batches);
        }

        void _OnClickRenderingTriangles(bool isOn)
        {
            _OnClickRenderingCheckbox(isOn, RenderingGraphTarget.Triangles);
        }

        void _OnClickRenderingVertices(bool isOn)
        {
            _OnClickRenderingCheckbox(isOn, RenderingGraphTarget.Vertices);
        }

        void _SetDropdown(string[] optionNames, TMP_Dropdown target, UnityAction<int> onChanged, int defaultIndex = 0)
        {
            _SetDropdownOptions(optionNames, target, defaultIndex);
            target.onValueChanged.RemoveAllListeners();
            target.onValueChanged.AddListener(onChanged);
        }

        void _SetDropdownOptions(string[] optionNames, TMP_Dropdown target, int defaultIndex = 0)
        {
            var optionDataList = new List<TMP_Dropdown.OptionData>(optionNames.Length);
            foreach(string optionName in optionNames)
            {
                TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(optionName);
                optionDataList.Add(option);
            }

            target.options = optionDataList;
            target.SetValueWithoutNotify(defaultIndex);
        }

        public void SetGCCollectButtonInteractable(bool interactable)
        {
            _gcCollectButton.interactable = interactable;
        }

        public void SetUnloadAssetButtonInteractable(bool interactable)
        {
            _unloadAssetButton.interactable = interactable;
        }

        void OnDestroy()
        {
            _fpsProfilingButton = default;
            _fpsTextComponent = default;
            _fpsGaugeComponent = default;
            _vSyncCountDropdown = default;
            _targetFrameRateDropdown = default;
            _frameTimeProfilingButton = default;
            _frameTimeChartComponent = default;
            _memoryProfilingButton = default;
            _memoryGraphButton = default;
            _memoryTextComponent = default;
            _memoryChartComponent = default;
            _memoryGaugeComponent = default;
            _gcCollectButton = default;
            _unloadAssetButton = default;
            _memoryProfilingTypeDropdown = default;
            _renderingProfilingButton = default;
            _renderingGraphButton = default;
            _renderingTextComponent = default;
            _renderingChartComponent = default;
            _renderingValueOptionGroup = default;
            _renderingValueOptionSetPassCalls = default;
            _renderingValueOptionDrawCalls = default;
            _renderingValueOptionBatches = default;
            _renderingValueOptionTriangles = default;
            _renderingValueOptionVertices = default;
            OnFpsProfilingStateChanged = default;
            OnFrameTimeProfilingStateChanged = default;
            OnVSyncCountChanged = default;
            OnTargetFrameRateChanged = default;
            OnMemoryProfilingStateChanged = default;
            OnMemoryGraphStateChanged = default;
            OnGCCollectExecuted = default;
            OnUnloadAssetExecuted = default;
            OnRenderingProfilingStateChanged = default;
            OnRenderingGraphStateChanged = default;
            OnRenderingGraphSelected = default;
        }
    }

    sealed class ProfilerViewLinker : ViewLinkerBase
    {
        public FpsUnchangingInfo _fpsInfo;

        public ProfilerFrameTimeViewInformation _frameTimeInfo;

        public MemoryUnchangingInfo _memoryInfo;

        public RenderingUnchangingInfo _renderingInfo;

        public int? _vSyncCountIndex = null;

        public string[] _targetFrameRateChoices;

        public int? _defaultTargetFrameRateIndex;
    }

    sealed class ProfilerFrameTimeViewInformation
    {
        public RingBuffer<float[]> _histories;

        public bool _isEnabled;

        public bool _isActive;
    }
}
