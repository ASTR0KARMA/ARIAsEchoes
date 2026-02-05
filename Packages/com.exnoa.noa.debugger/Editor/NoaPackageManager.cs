using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Compilation;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEditorInternal;
using UnityEngine;

namespace NoaDebugger
{
    static class NoaPackageManager
    {
        static string _noaDebuggerDirectoryPath;

        public static string NoaDebuggerPackagePath
        {
            get
            {
                if (string.IsNullOrEmpty(NoaPackageManager._noaDebuggerDirectoryPath))
                {
                    string folderPath = null;
                    Type type = typeof(NoaPackageManager);
                    string[] guids = AssetDatabase.FindAssets($"t:script {type.Name}");

                    foreach (string guid in guids)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guid);

                        if (AssetDatabase.IsValidFolder(path))
                        {
                            continue;
                        }

                        var scriptAsset = AssetDatabase.LoadMainAssetAtPath(path) as MonoScript;

                        if (scriptAsset != null && scriptAsset.GetClass() == type)
                        {
                            folderPath = Path.GetDirectoryName(path);

                            break;
                        }
                    }

                    NoaPackageManager._noaDebuggerDirectoryPath = Path.GetDirectoryName(folderPath);
                }

                return NoaPackageManager._noaDebuggerDirectoryPath;
            }
        }

        const string DEMO_SCRIPT_NAME = "NoaDebuggerDemo.Demo";

        static string _demoScriptDirectoryPath;

        static string DemoScriptDirectoryPath
        {
            get
            {
                if (string.IsNullOrEmpty(NoaPackageManager._demoScriptDirectoryPath))
                {
                    string[] guids = AssetDatabase.FindAssets("t:script Demo", new[] { "Assets" });

                    foreach (string guid in guids)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guid);

                        if (AssetDatabase.IsValidFolder(path))
                        {
                            continue;
                        }

                        var scriptAsset = AssetDatabase.LoadMainAssetAtPath(path) as MonoScript;

                        if (scriptAsset != null && scriptAsset.GetClass()?.FullName == NoaPackageManager.DEMO_SCRIPT_NAME)
                        {
                            NoaPackageManager._demoScriptDirectoryPath = Path.GetDirectoryName(path);
                            break;
                        }
                    }
                }

                return NoaPackageManager._demoScriptDirectoryPath;
            }
        }

        static string _sampleDirectoryPathToDelete;

        public static string SampleDirectoryPathToDelete
        {
            get
            {
                if (string.IsNullOrEmpty(NoaPackageManager._sampleDirectoryPathToDelete))
                {
                    var demoPath = NoaPackageManager.DemoScriptDirectoryPath;
                    if (string.IsNullOrEmpty(demoPath))
                    {
                        return null;
                    }

                    var currentDir = Directory.GetParent(demoPath);
                    if (currentDir == null)
                    {
                        return null;
                    }

                    string rootPath = currentDir.FullName;
                    var parentDir = currentDir.Parent;

                    while (parentDir != null)
                    {
                        if (parentDir.Name == "Assets")
                        {
                            break;
                        }

                        var parentEntries = Directory.GetFileSystemEntries(parentDir.FullName)
                            .Where(entry => !entry.EndsWith(".meta"))
                            .ToArray();

                        if (parentEntries.Length > 1)
                        {
                            break;
                        }

                        rootPath = parentDir.FullName;
                        parentDir = parentDir.Parent;
                    }

                    NoaPackageManager._sampleDirectoryPathToDelete = rootPath.Replace("\\", "/")
                        .Replace(Application.dataPath, "Assets");
                }

                return NoaPackageManager._sampleDirectoryPathToDelete;
            }
        }

        static UnityPackageInfo _noaDebuggerPackageInfo;

        public static UnityPackageInfo NoaDebuggerPackageInfo
        {
            get
            {
                if (NoaPackageManager._noaDebuggerPackageInfo == null)
                {
                    var packageJsonPath = $"{NoaPackageManager.NoaDebuggerPackagePath}/package.json";

                    if (!File.Exists(packageJsonPath))
                    {
                        return null;
                    }

                    string text = File.ReadAllText(packageJsonPath);
                    NoaPackageManager._noaDebuggerPackageInfo = JsonUtility.FromJson<UnityPackageInfo>(text);
                }

                return NoaPackageManager._noaDebuggerPackageInfo;
            }
        }

        public static void ExcludeFromCompile()
        {
            if (NoaPackageManager.NoaDebuggerPackagePath == null)
            {
                return;
            }

            ScriptingDefineSymbolUtil.RemoveNoaDebuggerSymbol();

            AssetDatabase.Refresh();

            if (!Directory.Exists($"{NoaPackageManager.NoaDebuggerPackagePath}/.Runtime"))
            {
                ExcludeDir($"{NoaPackageManager.NoaDebuggerPackagePath}/Runtime");
            }

#if NOA_DEBUGGER
            var customMenuFolderPath = NoaDebuggerSettingsManager.GetNoaDebuggerSettings()?.CustomMenuFolderPath ?? EditorDefine.CustomMenuResourcesFolderPath;

            if (Directory.Exists(customMenuFolderPath))
            {
                File.WriteAllText(EditorDefine.HiddenCustomMenuPathFile, customMenuFolderPath);

                string parentDir = Path.GetDirectoryName(customMenuFolderPath);
                string folderName = Path.GetFileName(customMenuFolderPath);
                string hiddenCustomMenuFolderPath = Path.Combine(parentDir, "." + folderName);

                if (!Directory.Exists(hiddenCustomMenuFolderPath))
                {
                    ExcludeDir(customMenuFolderPath);
                }
            }
#endif

            AssetDatabase.Refresh();
        }

        static void _ExcludeFile(string path)
        {
            if (!File.Exists(path))
            {
                return;
            }

            string fileName = Path.GetFileName(path);
            string directoryName = Path.GetDirectoryName(path);
            string renamedFilePath = Path.Combine(directoryName, $".{fileName}");

            if (File.Exists(renamedFilePath))
            {
                File.Delete(renamedFilePath);
            }
            File.Move(path, renamedFilePath);

            string metaPath = $"{path}.meta";
            if (File.Exists(metaPath))
            {
                string renamedMetaPath = $"{renamedFilePath}.meta";
                if (File.Exists(renamedMetaPath))
                {
                    File.Delete(renamedMetaPath);
                }
                File.Move(metaPath, renamedMetaPath);
            }

            if (path.StartsWith("Assets"))
            {
                AssetDatabase.Refresh();
            }
        }

        public static void ExcludeDir(string path)
        {
            var directoryInfo = new DirectoryInfo(path);

            if (!directoryInfo.Exists)
            {
                return;
            }

            string renamedDirPath = directoryInfo.FullName.Replace(directoryInfo.Name, $".{directoryInfo.Name}");
            directoryInfo.MoveTo(renamedDirPath);

            var fileInfo = new FileInfo($"{path}.meta");
            string renamedFilePath = fileInfo.FullName.Replace(fileInfo.Name, $".{fileInfo.Name}");
            fileInfo.MoveTo(renamedFilePath);
        }

        public static void IncludeInCompile()
        {
            if (NoaPackageManager.NoaDebuggerPackagePath == null)
            {
                return;
            }

            var runtimeHiddenPath = $"{NoaPackageManager.NoaDebuggerPackagePath}/.Runtime";

            if (!Directory.Exists(runtimeHiddenPath))
            {
                PackageEditorWindow.IsWait = false;

                return;
            }

            IncludeDir(runtimeHiddenPath);

            if (File.Exists(EditorDefine.HiddenCustomMenuPathFile))
            {
                var customMenuFolderPath = File.ReadAllText(EditorDefine.HiddenCustomMenuPathFile);

                string parentDir = Path.GetDirectoryName(customMenuFolderPath);
                string folderName = Path.GetFileName(customMenuFolderPath);
                string hiddenCustomMenuFolderPath = Path.Combine(parentDir, "." + folderName);

                if (Directory.Exists(hiddenCustomMenuFolderPath))
                {
                    IncludeDir(hiddenCustomMenuFolderPath);
                }

                File.Delete(EditorDefine.HiddenCustomMenuPathFile);
            }

            AssetDatabase.Refresh();

            ScriptingDefineSymbolUtil.AddNoaDebuggerSymbol();
            AssetDatabase.Refresh();
        }

        static void _IncludeFile(string path)
        {
            if (!File.Exists(path))
            {
                return;
            }

            string fileName = Path.GetFileName(path);
            string renameFileName = fileName.Substring(1);
            string directoryName = Path.GetDirectoryName(path);
            string renamedFilePath = Path.Combine(directoryName, renameFileName);

            if (File.Exists(renamedFilePath))
            {
                File.Delete(renamedFilePath);
            }
            File.Move(path, renamedFilePath);

            string metaPath = $"{path}.meta";
            if (File.Exists(metaPath))
            {
                string renamedMetaPath = $"{renamedFilePath}.meta";
                if (File.Exists(renamedMetaPath))
                {
                    File.Delete(renamedMetaPath);
                }
                File.Move(metaPath, renamedMetaPath);
            }

            if (path.StartsWith("Assets"))
            {
                AssetDatabase.Refresh();
            }
        }

        public static void IncludeDir(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }

            var directoryInfo = new DirectoryInfo(path);
            string renamedDirName = directoryInfo.Name.Substring(1);
            string renamedDirPath = directoryInfo.FullName.Replace(directoryInfo.Name, renamedDirName);
            directoryInfo.MoveTo(renamedDirPath);

            var fileInfo = new FileInfo($"{path}.meta");
            string renamedFileName = fileInfo.Name.Substring(1);
            string renamedFilePath = fileInfo.FullName.Replace(fileInfo.Name, renamedFileName);
            fileInfo.MoveTo(renamedFilePath);
        }

        public static bool HasRuntimePackage()
        {
            var runtimePath = $"{NoaPackageManager.NoaDebuggerPackagePath}/Runtime";

            return Directory.Exists(runtimePath);
        }

        public static void InitializeOnPackageUpdate()
        {
            if (NoaPackageManager.NoaDebuggerPackagePath == null)
            {
                return;
            }

            int step = 0;
            int totalStep = 2;
            _ShowProgressBar(step, totalStep);

#if NOA_DEBUGGER
            if (!File.Exists(EditorDefine.SettingsDataFullPath))
            {
                string tempAsset = "temp_settings.asset";
                string tempAssetPath = $"Assets/{tempAsset}";

                try
                {
                    var scriptableObject = ScriptableObject.CreateInstance<NoaDebuggerSettings>().Init();

                    InternalEditorUtility.SaveToSerializedFileAndForget(
                        new UnityEngine.Object[] { scriptableObject },
                        EditorDefine.SettingsDataPath,
                        true
                    );

                    LogModel.Log($"Saved to ProjectSettings: {EditorDefine.SettingsDataFullPath}");
                }
                catch (System.Exception e)
                {
                    LogModel.LogError($"Failed to save: {e.Message}");
                }
                finally
                {
                    AssetDatabase.DeleteAsset(tempAssetPath);
                }
            }

            NoaDebuggerSettingsManager.GetNoaDebuggerSettings()?.Update();
#endif
            _ShowProgressBar(step++, totalStep);

            if (!EditorPrefs.HasKey(EditorDefine.EditorPrefsKeyPackageVersion))
            {
                ScriptingDefineSymbolUtil.AddNoaDebuggerSymbolIfOpenDialog();

                EditorPrefs.SetString(
                    EditorDefine.EditorPrefsKeyPackageVersion, NoaPackageManager.NoaDebuggerPackageInfo.version);
            }

            else if (EditorPrefs.GetString(EditorDefine.EditorPrefsKeyPackageVersion) !=
                     NoaPackageManager.NoaDebuggerPackageInfo.version)
            {
                ScriptingDefineSymbolUtil.AddNoaDebuggerSymbol();

                EditorPrefs.SetString(
                    EditorDefine.EditorPrefsKeyPackageVersion, NoaPackageManager.NoaDebuggerPackageInfo.version);
            }

            _ShowProgressBar(step++, totalStep);
            EditorUtility.ClearProgressBar();
        }

        static void _ShowProgressBar(int step, int totalStep)
        {
            EditorUtility.DisplayProgressBar(
                "NoaDebugger Initializing",
                $"Initialize Progress... ({step}/{totalStep})",
                (float)step/ totalStep
            );
        }

        public static void UpdateCustomMenu()
        {
#if NOA_DEBUGGER
            NoaDebuggerSettingsManager.GetNoaDebuggerSettings()?.UpdateCustomMenu();
#endif
        }

        public static void DeletePackage()
        {
            ScriptingDefineSymbolUtil.RemoveNoaDebuggerSymbol();
            Delete(NoaPackageManager.NoaDebuggerPackagePath);
            File.Delete($"{NoaPackageManager.NoaDebuggerPackagePath}.meta");
        }

        public static void DeletePackageSample()
        {
            if (string.IsNullOrEmpty(NoaPackageManager.SampleDirectoryPathToDelete))
            {
                return;
            }

            Delete(NoaPackageManager.SampleDirectoryPathToDelete);
            File.Delete($"{NoaPackageManager.SampleDirectoryPathToDelete}.meta");
        }

        static void Delete(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }

            var filePaths = Directory.GetFiles(path);

            foreach (string filePath in filePaths)
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                File.Delete(filePath);
            }

            var directoryPaths = Directory.GetDirectories(path);

            foreach (var directoryPath in directoryPaths)
            {
                Delete(directoryPath);
            }

            Directory.Delete(path, false);
        }

        public static void ClearEditorPrefs()
        {
            EditorPrefs.DeleteKey(EditorDefine.EditorPrefsKeyPackageVersion);

            EditorUtility.RequestScriptReload();
            AssetDatabase.Refresh();
        }
    }
}
