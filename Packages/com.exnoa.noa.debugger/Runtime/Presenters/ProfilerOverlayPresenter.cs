namespace NoaDebugger
{
    sealed class ProfilerOverlayPresenter: OverlayPresenterBase
        <ProfilerOverlayView, ProfilerOverlaySettingsView, ProfilerOverlayRuntimeSettings, ProfilerOverlayViewLinker>
    {
        FpsUnchangingInfo _fpsInfo;

        ProfilerFrameTimeViewInformation _frameTimeInfo;

        MemoryUnchangingInfo _memoryInfo;

        RenderingUnchangingInfo _renderingInfo;

        public ProfilerOverlayPresenter(
            ProfilerOverlayView overlayPrefab,
            ProfilerOverlaySettingsView overlaySettingsPrefab,
            string prefsKeyPrefix)
            : base(overlayPrefab, overlaySettingsPrefab, prefsKeyPrefix)
        {
            _overlayToolSettings = NoaDebuggerSettingsManager.GetCacheSettings<ProfilerOverlayRuntimeSettings>();
        }

        protected override ViewLinkerBase _CreateOverlayViewLinker()
        {
            return new ProfilerOverlayViewLinker()
            {
                _axis = _settings.ProfilerOverlayAxis,
                _scale = _settings.ProfilerOverlayScale,
                _position = _settings.ProfilerOverlayPosition,
                _fpsInfo = _fpsInfo,
                _frameTimeInfo = _frameTimeInfo,
                _memoryInfo = _memoryInfo,
                _renderingInfo = _renderingInfo
            };
        }

        void _SetProfilerInfo(ProfilerViewLinker linker)
        {
            _fpsInfo = linker._fpsInfo;
            _frameTimeInfo = linker._frameTimeInfo;
            _memoryInfo = linker._memoryInfo;
            _renderingInfo = linker._renderingInfo;
        }

        public void ShowOverlayView(ProfilerViewLinker linker)
        {
            _SetProfilerInfo(linker);
            ShowOverlayView();
        }

        public void UpdateOverlayView(ProfilerViewLinker linker)
        {
            _SetProfilerInfo(linker);
            UpdateOverlayView();
        }
    }
}
