namespace NoaDebugger
{
    sealed class NoaDebuggerSettingsCacheManager
    {
        readonly NoaDebuggerCacheSettingsContainer _cacheSettingsContainer;

        public NoaDebuggerSettingsCacheManager(NoaDebuggerSettings originalSettings)
        {
            _cacheSettingsContainer = new NoaDebuggerCacheSettingsContainer();
            _cacheSettingsContainer.Init();

            _ApplyNoaDebuggerSettings(originalSettings);
        }

        NoaDebuggerSettings _ApplyNoaDebuggerSettings(NoaDebuggerSettings applyTarget)
        {
            _cacheSettingsContainer.ApplyCacheTo(applyTarget);
            return applyTarget;
        }

        public NoaDebuggerSettings GetNoaDebuggerSettingsAppliedCache(NoaDebuggerSettings originalSettings)
        {
            return _ApplyNoaDebuggerSettings(originalSettings);
        }

        public T GetSettings<T>() where T: ToolSettingsBase
        {
            return _cacheSettingsContainer.GetSettings<T>();
        }

        public bool HasUnsavedSettings()
        {
            return _cacheSettingsContainer.HasUnsavedSettings();
        }

        public bool HasUnsavedSettings<TRuntimeSettings>() where TRuntimeSettings : ToolSettingsBase
        {
            return _cacheSettingsContainer.HasUnsavedSettings<TRuntimeSettings>();
        }

        public void Save()
        {
            _cacheSettingsContainer.Save();
        }
    }
}
