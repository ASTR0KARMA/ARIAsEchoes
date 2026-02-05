using System;
using System.Collections;
using UnityEngine;

namespace NoaDebugger
{
    sealed class ProfilerPresenter : NoaDebuggerToolBase, INoaDebuggerTool, INoaDebuggerOverlayTool
    {
        [Header("MainView")]
        [SerializeField]
        ProfilerView _mainViewPrefab;
        ProfilerView _mainView;

        [Header("Overlay")]
        [SerializeField]
        ProfilerOverlayView _overlayPrefab;
        [SerializeField]
        ProfilerOverlaySettingsView _overlaySettingsPrefab;

        ProfilerOverlayPresenter _overlayPresenter;

        public Func<bool> _onGCCollect;

        public Func<bool> _onUnloadAsset;

        public ToolNotificationStatus NotifyStatus => ToolNotificationStatus.None;

        class ProfilerMenuInfo : IMenuInfo
        {
            public string Name => "Profiler";
            public string MenuName => "Profiler";
            public int SortNo => NoaDebuggerDefine.PROFILER_MENU_SORT_NO;
        }

        ProfilerMenuInfo _profilerMenuInfo;

        public IMenuInfo MenuInfo()
        {
            if (_profilerMenuInfo == null)
            {
                _profilerMenuInfo = new ProfilerMenuInfo();
            }

            return _profilerMenuInfo;
        }


        public FpsModel FpsModel { get; private set; }

        public FrameTimeModel FrameTimeModel { get; private set; }

        public TargetFrameRateModel TargetFrameRateModel { get; private set; }

        public MemoryModel MemoryModel { get; private set; }

        public RenderingModel RenderingModel { get; private set; }

        public BatteryModel BatteryModel { get; private set; }

        public ThermalModel ThermalModel { get; private set; }


        public void Init()
        {
            FpsModel = new FpsModel();
            FpsModel.OnFpsInfoChanged = _UpdateFpsView;

            FrameTimeModel = new FrameTimeModel();
            FrameTimeModel.OnFrameTimeInfoChanged = _UpdateFrameTimeView;

            TargetFrameRateModel = new TargetFrameRateModel();

            MemoryModel = new MemoryModel();
            MemoryModel.OnMemoryInfoChanged = _UpdateMemoryView;

            RenderingModel = new RenderingModel();
            RenderingModel.OnRenderingInfoChanged = _UpdateRenderingView;

            BatteryModel = new BatteryModel();

            ThermalModel = new ThermalModel();

            _overlayPresenter = new ProfilerOverlayPresenter(
                _overlayPrefab, _overlaySettingsPrefab, prefsKeyPrefix:"Profiler");
            _overlayPresenter.OnUpdateSettings += _OnUpdateOverlaySettings;
            _overlayPresenter.OnInitAction += _OnInitOverlay;
        }


        public void ShowView(Transform parent)
        {
            if (_mainView == null)
            {
                _mainView = GameObject.Instantiate(_mainViewPrefab, parent);
                _InitView(_mainView);
            }

            _UpdateAllView();

            if (!_overlayPresenter.IsOverlaySettingsEnable)
            {
                _mainView.gameObject.SetActive(true);
            }
        }

        void _InitView(ProfilerView view)
        {
            view.OnFpsProfilingStateChanged += _OnFpsProfilingStateChanged;
            view.OnFrameTimeProfilingStateChanged += _OnFrameTimeProfilingStateChanged;
            view.OnVSyncCountChanged += _OnVSyncCountChanged;
            view.OnTargetFrameRateChanged += TargetFrameRateModel.SetTargetFrameRate;
            view.OnMemoryProfilingStateChanged += _OnMemoryProfilingStateChanged;
            view.OnMemoryGraphStateChanged += _OnMemoryGraphShowingStateChanged;
            view.OnMemoryProfilingTypeChanged += _OnMemoryProfilingTypeChanged;
            view.OnGCCollectExecuted += _OnGCCollectExecuted;
            view.OnUnloadAssetExecuted += _OnUnloadAssetExecuted;
            view.OnRenderingProfilingStateChanged += _OnRenderingProfilingStateChanged;
            view.OnRenderingGraphStateChanged += _OnRenderingGraphShowingStateChanged;
            view.OnRenderingGraphSelected += _OnRenderingGraphSelected;
        }

        public void AlignmentUI(bool isReverse)
        {
            _mainView.AlignmentUI(isReverse);
            _overlayPresenter.AlignmentUI(isReverse);
        }


        public bool GetPinStatus()
        {
            return _overlayPresenter.IsOverlayEnable;
        }

        public void TogglePin(Transform parent)
        {
            _overlayPresenter.ToggleActiveOverlayView(parent);
        }

        public void InitOverlay(Transform parent)
        {
            if (_overlayPresenter.IsOverlayEnable)
            {
                _overlayPresenter.InstantiateOverlay(parent);
                _UpdateAllView();
            }
        }

        public bool GetSettingsStatus()
        {
            return _overlayPresenter.IsOverlaySettingsEnable;
        }

        public void ToggleOverlaySettings(Transform parent)
        {
            _overlayPresenter.ToggleActiveOverlaySettingsView(_mainView.gameObject, parent);
        }

        void _OnUpdateOverlaySettings()
        {
            _UpdateAllView();
        }

        void _OnInitOverlay(ProfilerOverlayView overlay)
        {
            overlay.OnEnabledAction += _OnOverlayEnabled;
            _UpdateAllView();
        }

        void _OnOverlayEnabled()
        {
            _UpdateAllView();
        }


        void _UpdateAllView()
        {
            var fps = FpsModel.FpsInfo;
            var frameTime = FrameTimeModel.FrameTimeInfo;
            var memory = MemoryModel.MemoryInfo;
            var rendering = RenderingModel.RenderingInfo;

            var viewData = new ProfilerViewLinker
            {
                _fpsInfo = CreateFpsViewInfo(fps),
                _frameTimeInfo = CreateFrameTimeInfo(frameTime),
                _memoryInfo = CreateMemoryViewInfo(memory),
                _renderingInfo = CreateRenderingViewInfo(rendering),
            };

            if (_overlayPresenter.IsShowOverlay)
            {
                _overlayPresenter.UpdateOverlayView(viewData);
            }

            _UpdateViewFromViewData(viewData);
        }

        void _UpdateViewFromViewData(ProfilerViewLinker viewData)
        {
            if (NoaDebugger.IsShowTargetToolMainView(this))
            {
                viewData._vSyncCountIndex = QualitySettings.vSyncCount;

                viewData._targetFrameRateChoices = TargetFrameRateModel.GetChoicesArrayIfNeedUpdate();
                viewData._defaultTargetFrameRateIndex = TargetFrameRateModel.GetCurrentIndex();
            }

            if (_mainView != null)
            {
                _mainView.Show(viewData);
            }

            if (_overlayPresenter.IsShowOverlay)
            {
                _overlayPresenter.ShowOverlayView(viewData);
            }
        }

        void _UpdateFpsView()
        {
            var fps = FpsModel.FpsInfo;
            var viewData = new ProfilerViewLinker
            {
                _fpsInfo = CreateFpsViewInfo(fps)
            };

            _UpdateViewFromViewData(viewData);
        }

        public static FpsUnchangingInfo CreateFpsViewInfo(FpsInfo info)
        {
            return new FpsUnchangingInfo(info);
        }

        void _UpdateFrameTimeView()
        {
            var frameTime = FrameTimeModel.FrameTimeInfo;
            var viewData = new ProfilerViewLinker
            {
                _frameTimeInfo = CreateFrameTimeInfo(frameTime)
            };

            _UpdateViewFromViewData(viewData);
        }

        public static ProfilerFrameTimeViewInformation CreateFrameTimeInfo(FrameTimeInfo info)
        {
            return new ProfilerFrameTimeViewInformation
            {
                _histories = info.HistoryBuffer,
                _isEnabled = info.IsEnabled,
                _isActive = info.IsActive
            };
        }

        void _UpdateMemoryView()
        {
            var memory = MemoryModel.MemoryInfo;
            var viewData = new ProfilerViewLinker()
            {
                _memoryInfo = CreateMemoryViewInfo(memory)
            };

            _UpdateViewFromViewData(viewData);
        }

        public static MemoryUnchangingInfo CreateMemoryViewInfo(MemoryInfo info)
        {
            return new MemoryUnchangingInfo(info);
        }

        void _UpdateRenderingView()
        {
            var rendering = RenderingModel.RenderingInfo;
            var viewData = new ProfilerViewLinker
            {
                _renderingInfo = CreateRenderingViewInfo(rendering)
            };

            _UpdateViewFromViewData(viewData);
        }

        void _OnUpdateProfilerSettings()
        {
            _overlayPresenter.ApplySettings();
        }

        public static RenderingUnchangingInfo CreateRenderingViewInfo(RenderingInfo info)
        {
            return new RenderingUnchangingInfo(info);
        }

        public static BatteryUnchangingInfo CreateBatteryViewInfo(BatteryInfo info)
        {
            return new BatteryUnchangingInfo(info);
        }

        public static ThermalUnchangingInfo CreateThermalViewInfo(ThermalInfo info)
        {
            return new ThermalUnchangingInfo(info);
        }


        void _OnFpsProfilingStateChanged(bool isProfiling)
        {
            ChangeFpsProfiling(isProfiling);
        }

        void _OnFrameTimeProfilingStateChanged(bool isProfiling)
        {
            ChangeFrameTimeProfiling(isProfiling);
        }

        void _OnVSyncCountChanged(int vSyncCount)
        {
            QualitySettings.vSyncCount = vSyncCount;
        }

        void _OnMemoryProfilingStateChanged(bool isProfiling)
        {
            ChangeMemoryProfiling(isProfiling);
        }

        void _OnMemoryGraphShowingStateChanged(bool isShowing)
        {
            MemoryModel.ChangeGraphShowingState(isShowing);
            _UpdateMemoryView();
        }

        void _OnMemoryProfilingTypeChanged(NoaProfiler.MemoryProfilingType profilingType)
        {
            MemoryModel.ChangeProfilingType(profilingType);
            _UpdateMemoryView();
        }

        void _OnGCCollectExecuted()
        {
            GlobalCoroutine.Run(_ExecuteGCCollect());
        }

        IEnumerator _ExecuteGCCollect()
        {
            _mainView.SetGCCollectButtonInteractable(false);

            yield return new WaitForEndOfFrame();

            INoaGCCollectCallbacks callbacks = NoaProfiler.GCCollectCallbacks;
            if (callbacks != null)
            {
                callbacks.OnBeforeGCCollect();
                if (!callbacks.IsAllowBaseGCCollect)
                {
                    _mainView.SetGCCollectButtonInteractable(true);

                    yield return null;

                    callbacks.OnAfterGCCollect();

                    yield break;
                }
            }
            else if (_onGCCollect != null && !_onGCCollect.Invoke())
            {
                _mainView.SetGCCollectButtonInteractable(true);
                yield break;
            }

            yield return null;

            GC.Collect();

            GC.WaitForPendingFinalizers();

            GC.Collect();

            _mainView.SetGCCollectButtonInteractable(true);

            yield return null;

            if (callbacks != null)
            {
                callbacks.OnAfterGCCollect();
            }
        }

        void _OnUnloadAssetExecuted()
        {
            GlobalCoroutine.Run(_ExecuteUnloadUnusedAssets());
        }

        IEnumerator _ExecuteUnloadUnusedAssets()
        {
            _mainView.SetUnloadAssetButtonInteractable(false);

            yield return new WaitForEndOfFrame();

            INoaUnloadAssetsCallbacks callbacks = NoaProfiler.UnloadAssetsCallbacks;
            if (callbacks != null)
            {
                callbacks.OnBeforeUnloadAssets();
                if (!callbacks.IsAllowBaseUnloadAssets)
                {
                    _mainView.SetUnloadAssetButtonInteractable(true);

                    yield return null;

                    callbacks.OnAfterUnloadAssets();

                    yield break;
                }
            }
            else if (_onUnloadAsset != null && !_onUnloadAsset.Invoke())
            {
                _mainView.SetUnloadAssetButtonInteractable(true);
                yield break;
            }

            yield return null;

            AsyncOperation asyncOp = Resources.UnloadUnusedAssets();
            while (!asyncOp.isDone)
            {
                yield return null;
            }

            _mainView.SetUnloadAssetButtonInteractable(true);

            yield return null;

            if (callbacks != null)
            {
                callbacks.OnAfterUnloadAssets();
            }
        }

        void _OnRenderingProfilingStateChanged(bool isProfiling)
        {
            ChangeRenderingProfiling(isProfiling);
        }

        void _OnRenderingGraphShowingStateChanged(bool isShowing)
        {
            RenderingModel.ChangeGraphShowingState(isShowing);
            _UpdateRenderingView();
        }

        void _OnRenderingGraphSelected(RenderingGraphTarget target)
        {
            if (target != RenderingModel.RenderingInfo.GraphTarget)
            {
                RenderingModel.SwitchGraphTarget(target);
                _UpdateRenderingView();
            }
        }


        void _OnHidden()
        {
            if (_mainView != null)
            {
                _mainView.gameObject.SetActive(false);
            }

            if (_overlayPresenter != null)
            {
                _overlayPresenter.DestroyOverlaySettings();
            }
        }

        public void OnHidden()
        {
            _OnHidden();
        }

        public void OnToolDispose()
        {
            _OnHidden();

            if (FpsModel != null)
            {
                FpsModel.Dispose();
                FpsModel = null;
            }
            if (FrameTimeModel != null)
            {
                FrameTimeModel.Dispose();
                FrameTimeModel = null;
            }
            if (TargetFrameRateModel != null)
            {
                TargetFrameRateModel.Dispose();
                TargetFrameRateModel = null;
            }
            if (MemoryModel != null)
            {
                MemoryModel.Dispose();
                MemoryModel = null;
            }
            if (RenderingModel != null)
            {
                RenderingModel.Dispose();
                RenderingModel = null;
            }
            if (BatteryModel != null)
            {
                BatteryModel.Dispose();
                BatteryModel = null;
            }
            if (ThermalModel != null)
            {
                ThermalModel.Dispose();
                ThermalModel = null;
            }
        }


        public ProfilerSnapshotData CaptureSnapshot()
        {
            return new ProfilerSnapshotData(this);
        }


        public FpsInfo GetFpsInfo()
        {
            return FpsModel.FpsInfo;
        }

        public bool IsFpsProfiling()
        {
            return FpsModel.FpsInfo.IsProfiling;
        }

        public void ChangeFpsProfiling(bool isProfiling)
        {
            FpsModel.ChangeFpsProfilingState(isProfiling);
            FrameTimeModel.ToggleActive(isProfiling);
            _UpdateFpsView();
        }


        public void ChangeFrameTimeProfiling(bool isProfiling)
        {
            FrameTimeModel.ToggleEnabled(isProfiling);
            _UpdateFrameTimeView();
        }


        public MemoryInfo GetMemoryInfo()
        {
            return MemoryModel.MemoryInfo;
        }

        public bool IsMemoryProfiling()
        {
            return MemoryModel.MemoryInfo.IsProfiling;
        }

        public void ChangeMemoryProfiling(bool isProfiling)
        {
            MemoryModel.ChangeProfilingState(isProfiling);
            _UpdateMemoryView();
        }

        public void ChangeMemoryProfilingType(NoaProfiler.MemoryProfilingType profilingType)
        {
            MemoryModel.ChangeProfilingType(profilingType);
            _UpdateMemoryView();
        }

        public float GetTotalNativeMemoryMB()
        {
            return MemoryModel.MemoryInfo.TotalNativeMemoryMB;
        }

        public void SetTotalNativeMemoryMB(float totalMemoryMB = -1)
        {
            MemoryModel.MemoryInfo.SetTotalNativeMemoryMB(totalMemoryMB);
        }


        public RenderingInfo GetRenderingInfo()
        {
            return RenderingModel.RenderingInfo;
        }

        public bool IsRenderingProfiling()
        {
            return RenderingModel.RenderingInfo.IsProfiling;
        }

        public void ChangeRenderingProfiling(bool isProfiling)
        {
            RenderingModel.ChangeProfilingState(isProfiling);
            _UpdateRenderingView();
        }


        public BatteryInfo GetBatteryInfo()
        {
            return BatteryModel.BatteryInfo;
        }

        public bool IsBatteryProfiling()
        {
            if (!BatteryModel.CanProfiling())
            {
                return false;
            }

            return BatteryModel.BatteryInfo.IsProfiling;
        }

        public void ChangeBatteryProfiling(bool isProfiling)
        {
            BatteryModel.ChangeBatteryProfilingState(isProfiling);
        }


        public ThermalInfo GetThermalInfo()
        {
            return ThermalModel.ThermalInfo;
        }

        public bool IsThermalProfiling()
        {
            if (!ThermalModel.CanProfiling())
            {
                return false;
            }

            return ThermalModel.ThermalInfo.IsProfiling;
        }

        public void ChangeThermalProfiling(bool isProfiling)
        {
            ThermalModel.ChangeProfilingState(isProfiling);
        }

        void OnDestroy()
        {
            _mainViewPrefab = default;
            _mainView = default;
            _overlayPrefab = default;
            _overlaySettingsPrefab = default;
            _overlayPresenter = default;
            _profilerMenuInfo = default;
            _onGCCollect = default;
            _onUnloadAsset = default;
            FpsModel = default;
            FrameTimeModel = default;
            TargetFrameRateModel = default;
            MemoryModel = default;
            RenderingModel = default;
            BatteryModel = default;
            ThermalModel = default;
        }
    }
}
