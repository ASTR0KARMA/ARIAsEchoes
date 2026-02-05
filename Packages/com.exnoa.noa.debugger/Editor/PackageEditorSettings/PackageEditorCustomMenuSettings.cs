#if NOA_DEBUGGER
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.IO;

namespace NoaDebugger
{
    sealed class PackageEditorCustomMenuSettings : PackageEditorSettingsBase
    {
        List<CustomMenuInfo> _customMenuList;

        ReorderableList _reorderableCustomMenuList;

        public PackageEditorCustomMenuSettings(NoaDebuggerSettings settings) : base(settings)
        {
            _rootPath = settings?.CustomMenuFolderPath;
        }

        string _rootPath;

        public override void ResetTmpDataWithSettings()
        {
            _customMenuList = new List<CustomMenuInfo>();
            foreach (var menuInfo in _settings.CustomMenuList)
            {
                _customMenuList.Add(new CustomMenuInfo()
                {
                    _script = menuInfo._script,
                    _sortNo = menuInfo._sortNo,
                    _scriptName = menuInfo._scriptName
                });
            }

            void onDrawCustomMenuElement(Rect rect, int index, bool isActive, bool isFocused)
            {
                CustomMenuInfo data = _customMenuList[index];
                Rect scriptRect = rect;
                scriptRect.width *= 0.5f;
                scriptRect.x += 200;
                data._script = EditorGUI.ObjectField(scriptRect, data._script, typeof(MonoScript), false) as MonoScript;

                string viewName = "";
                if (data._script != null)
                {
                    if (data.IsInvalidScript())
                    {
                        data._script = null;
                        EditorUtility.DisplayDialog("Error", "Can't Set No Inheritance Script", "OK");
                    }

                    viewName = data.GetViewName();
                    data.RefreshScriptName();
                }

                EditorGUI.LabelField(rect, viewName);
            }

            _reorderableCustomMenuList = new ReorderableList(_customMenuList, typeof(CustomMenuInfo))
            {
                headerHeight = 0,
                footerHeight = 20,
                displayAdd = true,
                displayRemove = true,
                drawElementCallback = onDrawCustomMenuElement,
                drawHeaderCallback = rect =>
                                     {
                                         EditorGUI.LabelField(rect, "");
                                     },
            };
        }

        public override void ApplySettings()
        {
            List<CustomMenuInfo> monoInfos = new List<CustomMenuInfo>();
            for (int i = 0; i < _customMenuList.Count; i++)
            {
                var monoInfo = _customMenuList[i];

                monoInfos.Add(
                    new CustomMenuInfo()
                    {
                        _script = monoInfo._script,
                        _sortNo = i,
                        _scriptName = monoInfo._scriptName
                    });
            }

            _settings.CustomMenuList = monoInfos;
            _settings.CustomMenuFolderPath = _rootPath;
            NoaDebuggerSettings settings = NoaDebuggerSettingsManager.GetNoaDebuggerSettings();
            settings.CustomMenuList = monoInfos;
            settings.CustomMenuFolderPath = _rootPath;
        }

        public override void ResetDefault()
        {
        }

        public override void DrawGUI()
        {
            _DisplaySettingsCategoryHeader("Custom Menu");

            string displayPath = string.IsNullOrEmpty(_rootPath)
                ? EditorDefine.CustomMenuResourcesFolderPath
                : _rootPath;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Selected Path:", GUILayout.Width(90f));
			EditorGUILayout.LabelField(displayPath);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create Default Folder", GUILayout.Width(140f)))
            {
                if (!string.IsNullOrEmpty(_rootPath))
                {
                    if (!Directory.Exists(_rootPath))
                    {
                        Directory.CreateDirectory(_rootPath);
                        AssetDatabase.Refresh();
                        LogModel.Log($"Folder created: {_rootPath}");
                    }
                    else
                    {
                        LogModel.LogWarning($"Folder already exists: {_rootPath}");
                    }
                }
            }

            if (GUILayout.Button("Select Folder", GUILayout.Width(95f)))
            {
                string selectedPath = EditorUtility.SaveFolderPanel("Target folder", "Assets", string.Empty);

                if (!string.IsNullOrEmpty(selectedPath))
                {
                    if (selectedPath.StartsWith(Application.dataPath))
                    {
                        string folderName = Path.GetFileName(selectedPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                        if (folderName == "Resources")
                        {
                            _rootPath = selectedPath.Replace(Application.dataPath, "Assets");
                        }
                        else
                        {
                            LogModel.LogWarning("Please select a folder named 'Resources'.");
                        }
                    }
                    else
                    {
                        LogModel.LogWarning("Please select a folder under the Assets directory.");
                    }
                }
            }
			EditorGUILayout.EndHorizontal();

            EditorGUILayout.HelpBox("Please specify a custom menu folder separate from the app's Resources folder.", MessageType.Info);

            _reorderableCustomMenuList.DoLayoutList();
        }
    }
}
#endif
