using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using UnityEditorInternal;

namespace NoaDebugger
{
#if UNITY_EDITOR
    sealed class NoaDebuggerSettingsBuildHelper
    {
        static readonly string SettingsDataResourcesPath = $"{NoaPackageManager.NoaDebuggerPackagePath}/{EditorDefine.SettingsDataResourcesPath}";

        public static void CopySettingsToResources()
        {
            try
            {
                if (!File.Exists(EditorDefine.SettingsDataPath))
                {
                    LogModel.LogWarning($"NoaDebuggerSettings not found: {EditorDefine.SettingsDataPath}");
                    return;
                }

                string targetDir = Path.GetDirectoryName(SettingsDataResourcesPath);
                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }

                var loadedObjects = InternalEditorUtility.LoadSerializedFileAndForget(EditorDefine.SettingsDataPath);
                var sourceAsset = loadedObjects[0] as NoaDebuggerSettings;
                if (sourceAsset == null)
                {
                    LogModel.LogError($"Failed to load NoaDebuggerSettings: {EditorDefine.SettingsDataPath}");
                    return;
                }

                File.Copy(EditorDefine.SettingsDataPath, SettingsDataResourcesPath, true);

                AssetDatabase.Refresh();
                AssetDatabase.ImportAsset(SettingsDataResourcesPath, ImportAssetOptions.ForceUpdate);
                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                LogModel.LogError($"Error copying NoaDebuggerSettings: {e.Message}\n{e.StackTrace}");
            }
        }

        public static void DeleteSettingsFromResources()
        {
            try
            {
                if (File.Exists(SettingsDataResourcesPath))
                {
                    AssetDatabase.DeleteAsset(SettingsDataResourcesPath);
                    AssetDatabase.Refresh();
                }
            }
            catch (Exception e)
            {
                LogModel.LogError($"[NoaDebuggerBuildHelper] Error deleting NoaDebuggerSettings: {e.Message}\n{e.StackTrace}");
            }
        }
    }
#endif
}
