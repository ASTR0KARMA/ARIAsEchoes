#if NOA_DEBUGGER
using UnityEditor;
using UnityEditorInternal;

namespace NoaDebugger
{
    sealed class InputSettingsChecker
    {
        [InitializeOnLoadMethod]
        static void Initialize()
        {
            EditorApplication.delayCall += InputSettingsChecker.CheckInputSettingsOnce;
        }

        static void CheckInputSettingsOnce()
        {
            EditorApplication.delayCall -= InputSettingsChecker.CheckShortcut;

            UnityInputUtils.CheckInputSystemAvailable();

            InputSettingsChecker.CheckShortcut();
        }

        static void CheckShortcut()
        {
            NoaDebuggerSettings settings = NoaDebuggerSettingsManager.GetNoaDebuggerSettings();

            if (settings == null)
            {
                return;
            }

            bool isUpdated = NoaDebuggerShortcutSettings.CheckUpdateDirtyShortcutSettings(settings);

            if (isUpdated)
            {
                InternalEditorUtility.SaveToSerializedFileAndForget(
                    new UnityEngine.Object[] { settings },
                    EditorDefine.SettingsDataPath,
                    true
                );

                string input = UnityInputUtils.IsEnableInputSystem ? "Input System" : "Input Manager";

                LogModel.Log($"Shortcut settings for the {input} have been initialized.");
            }
        }
    }
}
#endif
