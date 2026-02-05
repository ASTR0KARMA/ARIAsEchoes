using System;
using System.Collections.Generic;
using System.Linq;

namespace NoaDebugger
{
    sealed class NoaDebuggerSettingsContainer
    {
        readonly List<NoaDebuggerSettingsBase> _settings;
        public NoaDebuggerSettingsContainer(NoaDebuggerSettings settings)
        {
            _settings = new List<NoaDebuggerSettingsBase>()
            {
                new NoaDebuggerLoadingSettings(settings),
                new NoaDebuggerDisplaySettings(settings),
                new NoaDebuggerFontSettings(settings),
                new NoaDebuggerMenuSettings(settings),
                new NoaDebuggerCustomMenuSettings(settings),
                new NoaDebuggerLogSettings(settings),
                new NoaDebuggerHierarchySettings(settings),
                new NoaDebuggerCommandSettings(settings),
                new NoaDebuggerOverlaySettings(settings),
                new NoaDebuggerUIElementSettings(settings),
                new NoaDebuggerGameSpeedSettings(settings),
                new NoaDebuggerShortcutSettings(settings),
                new NoaDebuggerOtherSettings(settings),
                new NoaDebuggerAutoSaveSettings(settings),
            };
        }

        public void Init()
        {
            _settings.ForEach(setting => setting.Init());
        }
    }

    sealed class NoaDebuggerCacheSettingsContainer
    {
        readonly Dictionary<Type, ToolSettingsBase> _settings;

        public NoaDebuggerCacheSettingsContainer()
        {
            _settings = new Dictionary<Type, ToolSettingsBase>()
            {
                [typeof(LoadingRuntimeSettings)] = new LoadingRuntimeSettings(),
                [typeof(DisplayRuntimeSettings)] = new DisplayRuntimeSettings(),
                [typeof(FontRuntimeSettings)] = new FontRuntimeSettings(),
                [typeof(MenuRuntimeSettings)] = new MenuRuntimeSettings(),
                [typeof(CustomMenuRuntimeSettings)] = new CustomMenuRuntimeSettings(),
                [typeof(InformationRuntimeSettings)] = new InformationRuntimeSettings(),
                [typeof(LogRuntimeSettings)] = new LogRuntimeSettings(),
                [typeof(HierarchyRuntimeSettings)] = new HierarchyRuntimeSettings(),
                [typeof(CommandRuntimeSettings)] = new CommandRuntimeSettings(),
                [typeof(CommonOverlayRuntimeSettings)] = new CommonOverlayRuntimeSettings(),
                [typeof(ProfilerOverlayRuntimeSettings)] = new ProfilerOverlayRuntimeSettings(),
                [typeof(ConsoleLogOverlayRuntimeSettings)] = new ConsoleLogOverlayRuntimeSettings(),
                [typeof(ApiLogOverlayRuntimeSettings)] = new ApiLogOverlayRuntimeSettings(),
                [typeof(UIElementRuntimeSettings)] = new UIElementRuntimeSettings(),
                [typeof(GameSpeedRuntimeSettings)] = new GameSpeedRuntimeSettings(),
                [typeof(ShortcutRuntimeSettings)] = new ShortcutRuntimeSettings(),
                [typeof(OtherRuntimeSettings)] = new OtherRuntimeSettings(),
            };
        }

        public void Init()
        {
            foreach (var (type, setting) in _settings)
            {
                setting.Initialize();
            }
        }

        public bool HasUnsavedSettings()
        {
            return _settings.Any(x => x.Value.IsValueChanged());
        }

        public bool HasUnsavedSettings<TRuntimeSettings>() where TRuntimeSettings : ToolSettingsBase
        {
            var pair = _settings.FirstOrDefault(x => typeof(TRuntimeSettings).IsAssignableFrom(x.Key));
            return pair.Value?.IsValueChanged() ?? false;
        }

        public void Save()
        {
            bool hasChanges = false;
            var updatedSettings = new List<ToolSettingsBase>();
            foreach (var (type, setting) in _settings)
            {
                if (setting.IsValueChanged())
                {
                    setting.SaveCore();
                    hasChanges = true;
                    updatedSettings.Add(setting);
                }
            }

            if (hasChanges)
            {
                NoaDebuggerSettingsManager.UpdateRuntimeNoaDebuggerSettings();
                foreach (var setting in updatedSettings)
                {
                    setting.InvokeOnUpdateSettings();
                }
            }
        }

        public void ApplyCacheTo(NoaDebuggerSettings originalSettings)
        {
            foreach (var (type, setting) in _settings)
            {
                setting.ApplyCache(originalSettings);
            }
        }

        public T GetSettings<T>() where T : ToolSettingsBase
        {
            var type = typeof(T);
            if (_settings.TryGetValue(type, out var setting))
            {
                return setting as T;
            }

            LogModel.ThrowException($"指定された型の設定が存在しません ===> {type}");
            return null;
        }
    }
}
