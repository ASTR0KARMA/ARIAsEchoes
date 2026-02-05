using UnityEngine;
using System.IO;

namespace NoaDebugger
{
    static class EditorDefine
    {
        public const string NOA_DEBUGGER_RUNTIME_ASSEMBLY_DEFINITION_FILE = "Runtime/Noa.NoaDebugger.Runtime.asmdef";
        public const int NOA_DEBUGGER_EDITOR_WINDOW_WIDTH = 450;
        public const int NOA_DEBUGGER_EDITOR_WINDOW_HEIGHT = 610;
        public const int NOA_DEBUGGER_EDITOR_LABEL_WIDTH = 190;
        public static readonly string EditorPrefsKeyPackageVersion = $"{Application.productName}:PACKAGE_VERSION";

        static readonly string ProjectRoot = Directory.GetParent(Application.dataPath)!.FullName;
        public static readonly string SettingsDataPath = "ProjectSettings/NoaDebuggerSettings.asset";
        public static readonly string SettingsDataFullPath = Path.Combine(ProjectRoot, EditorDefine.SettingsDataPath);
        public static readonly string SettingsDataResourcesPath = "Runtime/Resources/NoaDebuggerSettings.asset";

        public static readonly string CustomMenuResourcesFolderPath = "Assets/NoaDebuggerCustomMenu/Resources";

        public static readonly string HiddenCustomMenuPathFile = "Assets/.NoaDebuggerCustomMenuPath";

        public static readonly string UnityDiscussionsUrl = "https://discussions.unity.com/t/noa-debugger-for-unity-feedback-questions-and-feature-requests";

        public static readonly string AssetStoreReviewUrl = "https://u3d.as/3cCN#reviews";
    }
}
