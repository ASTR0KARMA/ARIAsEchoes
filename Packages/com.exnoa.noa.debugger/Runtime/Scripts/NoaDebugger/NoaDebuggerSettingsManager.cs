using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditorInternal;
#endif
using UnityEngine;
using System.IO;

namespace NoaDebugger
{
    sealed class NoaDebuggerSettingsManager
    {
#if UNITY_EDITOR
        const string TOOL_SETTINGS_ASSET_PATH = "ProjectSettings/NoaDebuggerSettings.asset";
#else
        const string TOOL_SETTINGS_ASSET_NAME = "NoaDebuggerSettings";
#endif

        static NoaDebuggerSettingsManager _instance;
        static NoaDebuggerSettingsManager Instance
        {
            get
            {
                if (_instance == null || _instance._noaDebuggerSettings == null)
                {
                    _instance = new NoaDebuggerSettingsManager();
                }

                return _instance;
            }
        }

        NoaDebuggerSettings _noaDebuggerSettings;
        NoaDebuggerSettingsCacheManager _cacheManager;

        NoaDebuggerSettingsCacheManager CacheManager
        {
            get
            {
                if (_cacheManager == null)
                {
                    _cacheManager = new NoaDebuggerSettingsCacheManager(_noaDebuggerSettings);
                }
                return _cacheManager;
            }
        }

        NoaDebuggerSettingsManager()
        {
            _noaDebuggerSettings = _GetNoaDebuggerSettingsResources();
        }

        public static void InitializeOnRuntime()
        {
            Instance._UpdateNoaDebuggerSettingsFromCacheOrResources();
        }

        public static void ValidateMenuSettings(List<INoaDebuggerTool> allPresenters)
        {
            Instance._ValidateMenuSettings(allPresenters);
        }

        void _ValidateMenuSettings(List<INoaDebuggerTool> allPresenters)
        {
            List<MenuInfo> menuInfos = _noaDebuggerSettings.MenuList.GroupBy(x => x.Name).Select(x => x.First()).ToList();

            for (int i = 0; i < allPresenters.Count; i++)
            {
                var presenter = allPresenters[i];

                if (!menuInfos.Exists(info => info.Name.Equals(presenter.MenuInfo().MenuName)))
                {
                    menuInfos.Add(new MenuInfo()
                    {
                        Name = presenter.MenuInfo().MenuName,
                        Enabled = true
                    });
                }
            }

            for (var i = 0; i < menuInfos.Count; i++)
            {
                var menuInfo = menuInfos[i];

                if (!allPresenters.Exists(tool => tool.MenuInfo().MenuName.Equals(menuInfo.Name)))
                {
                    menuInfos.RemoveAt(i);
                }
            }

            _noaDebuggerSettings.MenuList = menuInfos;
        }

        public static NoaDebuggerSettings GetNoaDebuggerSettings()
        {
            return Instance._noaDebuggerSettings;
        }

        public static NoaDebuggerSettings GetOriginalNoaDebuggerSettings()
        {
            return Instance._GetNoaDebuggerSettingsResources();
        }

        public static void UpdateRuntimeNoaDebuggerSettings()
        {
            Instance._UpdateNoaDebuggerSettingsFromCacheOrResources();
        }

        public static bool HasUnsavedNoaDebuggerSettingsCache()
        {
            return Instance._cacheManager.HasUnsavedSettings();
        }

        public static bool HasUnsavedNoaDebuggerSettingsCache<TRuntimeSettings>()
            where TRuntimeSettings : ToolSettingsBase
        {
            return Instance._cacheManager.HasUnsavedSettings<TRuntimeSettings>();
        }

        public static void SaveNoaDebuggerSettingsCache()
        {
            Instance.CacheManager.Save();
        }

        void _UpdateNoaDebuggerSettingsFromCacheOrResources()
        {
            NoaDebuggerSettings settings = CacheManager.GetNoaDebuggerSettingsAppliedCache(_noaDebuggerSettings);
            _noaDebuggerSettings = settings;
        }

        NoaDebuggerSettings _GetNoaDebuggerSettingsResources()
        {
            NoaDebuggerSettings settings = null;
#if UNITY_EDITOR
            if (File.Exists(TOOL_SETTINGS_ASSET_PATH))
            {
                try
                {
                    var loadedObjects = InternalEditorUtility.LoadSerializedFileAndForget(TOOL_SETTINGS_ASSET_PATH);
                    if (loadedObjects != null && loadedObjects.Length > 0)
                    {
                        settings = loadedObjects[0] as NoaDebuggerSettings;
                    }
                    else
                    {
                        LogModel.LogWarning($"Failed to load settings file: {TOOL_SETTINGS_ASSET_PATH} (empty or invalid)");
                    }
                }
                catch (Exception e)
                {
                    LogModel.LogError($"Failed to load settings file: {TOOL_SETTINGS_ASSET_PATH}. Error: {e.Message}");
                }
            }
#else
            try
            {
                settings = Resources.Load<NoaDebuggerSettings>(TOOL_SETTINGS_ASSET_NAME);
            }
            catch (Exception e)
            {
                LogModel.LogError($"Failed to load settings resource: {TOOL_SETTINGS_ASSET_NAME}. Error: {e.Message}");
            }
#endif
            if (settings == null)
            {
#if NOA_DEBUGGER
                settings = ScriptableObject.CreateInstance<NoaDebuggerSettings>().Init();
#endif
            }

            return settings;
        }

        public static T GetCacheSettings<T>() where T: ToolSettingsBase
        {
            return Instance.CacheManager.GetSettings<T>();
        }

        public static void Dispose()
        {
            NoaDebuggerSettingsManager._instance = null;
        }
    }
}
