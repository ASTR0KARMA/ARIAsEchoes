#if NOA_DEBUGGER
using System;
using UnityEditor;
using UnityEngine;

namespace NoaDebugger
{
    abstract class PackageEditorSettingsBase
    {
        readonly protected NoaDebuggerSettings _settings;

        protected PackageEditorSettingsBase(NoaDebuggerSettings settings)
        {
            _settings = settings;
        }

        abstract public void ResetTmpDataWithSettings();

        abstract public void ApplySettings();

        abstract public void ResetDefault();

        abstract public void DrawGUI();

        protected void _DisplaySettingsCategoryHeader(string categoryName, Action reset = null)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField($"<b>{categoryName}</b>", EditorBaseStyle.Headline());

                if (reset != null &&
                    GUILayout.Button("Reset", GUILayout.Width(110)))
                {
                    _OpenSettingsResetDialog($"{categoryName} section", reset);
                }
            }

            EditorGUILayout.Separator();
        }

        protected void _OpenSettingsResetDialog(string resetTarget, Action reset)
        {
            if (EditorUtility.DisplayDialog(
                "Attention", $"Are you sure you want to reset {resetTarget}?", "OK", "Cancel"))
            {
                reset?.Invoke();

                GUI.FocusControl("");
            }
        }
    }
}
#endif
