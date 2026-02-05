using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor.Compilation;
using System.IO;

namespace NoaDebugger
{
    sealed class PackageEditorWindow : EditorWindow
    {
        public enum TabMenu
        {
            General,
            Settings,
            Tools
        }

        static bool _isWindowOpen;

        Vector2 _scrollPos;

        List<string> _tabMenu;

        TabMenu _selectTab;

        int _selectedTabIndex;

        public static bool IsWait;

        bool _isPlayingLayoutMode;

#pragma warning disable 0414
        bool _isInitialized;
#pragma warning restore 0414

#if NOA_DEBUGGER
        NoaDebuggerSettings _noaDebuggerSettings;

        List<PackageEditorSettingsBase> _settings;

        bool _isSettingsChanged;

#endif
        [MenuItem("Window/NOA Debugger", priority = 100)]
        static void _EditorWindow()
        {
            ShowWindow();
        }

        public static void ShowWindow(TabMenu selectedTab = TabMenu.General)
        {
            if (PackageEditorWindow._isWindowOpen)
            {
                return;
            }

            PackageEditorWindow._isWindowOpen = true;
            var window = CreateInstance<PackageEditorWindow>();
            window.ShowUtility();

            int x = (Screen.currentResolution.width - EditorDefine.NOA_DEBUGGER_EDITOR_WINDOW_WIDTH) / 2;
            int y = (Screen.currentResolution.height - EditorDefine.NOA_DEBUGGER_EDITOR_WINDOW_HEIGHT) / 2;
            window.position = new Rect(x, y, EditorDefine.NOA_DEBUGGER_EDITOR_WINDOW_WIDTH, EditorDefine.NOA_DEBUGGER_EDITOR_WINDOW_HEIGHT);
            var fixedSize = new Vector2() { x = EditorDefine.NOA_DEBUGGER_EDITOR_WINDOW_WIDTH, y = EditorDefine.NOA_DEBUGGER_EDITOR_WINDOW_HEIGHT };
            window.minSize = fixedSize;
            window.maxSize = fixedSize;
            if (!Enum.IsDefined(typeof(TabMenu), selectedTab))
            {
                selectedTab = TabMenu.General;
            }
            window._selectTab = selectedTab;
            window._selectedTabIndex = window._GetTabIndex(window._selectTab);
        }

        void OnEnable()
        {
            PackageEditorWindow._isWindowOpen = true;

            _SwitchEditorLayout();
            _selectedTabIndex = _GetTabIndex(_selectTab);

#if NOA_DEBUGGER

            if (_isInitialized)
            {
                _isInitialized = false;
                NoaPackageManager.InitializeOnPackageUpdate();
            }

            _SettingsAssetLoad();

            if (_noaDebuggerSettings == null)
            {
                return;
            }

            _settings.ForEach(settings => settings.ResetTmpDataWithSettings());
#endif
        }
#if NOA_DEBUGGER
        void _SettingsAssetLoad()
        {
            if(File.Exists(EditorDefine.SettingsDataPath))
            {
                var loadedObjects = InternalEditorUtility.LoadSerializedFileAndForget(EditorDefine.SettingsDataPath);
                _noaDebuggerSettings = loadedObjects[0] as NoaDebuggerSettings;
            }
            _settings = new List<PackageEditorSettingsBase>()
            {
                new PackageEditorLoadingSettings(_noaDebuggerSettings),
                new PackageEditorDisplaySettings(_noaDebuggerSettings),
                new PackageEditorFontSettings(_noaDebuggerSettings),
                new PackageEditorMenuSettings(_noaDebuggerSettings),
                new PackageEditorCustomMenuSettings(_noaDebuggerSettings),
                new PackageEditorInformationSettings(_noaDebuggerSettings),
                new PackageEditorLogSettings(_noaDebuggerSettings),
                new PackageEditorHierarchySettings(_noaDebuggerSettings),
                new PackageEditorCommandSettings(_noaDebuggerSettings),
                new PackageEditorOverlaySettings(_noaDebuggerSettings),
                new PackageEditorUIElementSettings(_noaDebuggerSettings),
                new PackageEditorGameSpeedSettings(_noaDebuggerSettings),
                new PackageEditorShortcutSettings(_noaDebuggerSettings, this),
                new PackageEditorOtherSettings(_noaDebuggerSettings),
                new PackageEditorAutoSaveSettings(_noaDebuggerSettings),
            };
        }

        void _SettingsAssetSave()
        {
            if (_noaDebuggerSettings == null)
            {
                _SettingsAssetLoad();
            }

            _settings.ForEach(settings => settings.ApplySettings());

            InternalEditorUtility.SaveToSerializedFileAndForget(
                new UnityEngine.Object[] { _noaDebuggerSettings },
                EditorDefine.SettingsDataPath,
                true
            );

            _isSettingsChanged = false;
        }
#endif
        void OnGUI()
        {
#if NOA_DEBUGGER

            if (_noaDebuggerSettings == null)
            {
                OnEnable();
            }
#endif

            using (new EditorGUILayout.VerticalScope(EditorBaseStyle.Box))
            {
                EditorUtil.DrawTitle("NOA Debugger Editor");
            }

            using (new EditorGUILayout.VerticalScope(EditorBaseStyle.Box))
            {
                EditorGUILayout.LabelField($"<b>Version : {NoaPackageManager.NoaDebuggerPackageInfo.version}</b> | <i>{NoaPackageManager.NoaDebuggerPackageInfo.name}</i>", EditorBaseStyle.RichText());
            }

            EditorGUI.BeginDisabledGroup(IsWait);

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                _selectedTabIndex = GUILayout.Toolbar(_selectedTabIndex, _tabMenu.ToArray(), EditorStyles.toolbarButton);

                if(check.changed)
                {
                    _selectTab = (TabMenu)Enum.Parse(typeof(TabMenu), _tabMenu[_selectedTabIndex]);
                }
            }

            using (new EditorGUILayout.VerticalScope(EditorBaseStyle.Box))
            {
                if (_selectTab == TabMenu.General)
                {
                    EditorGUILayout.LabelField("<b>Package</b>", EditorBaseStyle.Headline());

                    EditorGUILayout.Separator();

                    if (GUILayout.Button("Initialize"))
                    {
                        if (NoaPackageManager.HasRuntimePackage())
                        {
                            IsWait = true;
                            NoaPackageManager.ClearEditorPrefs();
                            NoaPackageManager.InitializeOnPackageUpdate();
                            _isInitialized = true;
                        }
                    }

                    EditorGUILayout.Separator();

                    if (GUILayout.Button("Exclude from compile"))
                    {
                        if (EditorUtility.DisplayDialog("Attention", "Are you sure you want to exclude \"NOA Debugger\" from compilation?", "OK", "Cancel"))
                        {
                            IsWait = true;
                            NoaPackageManager.ExcludeFromCompile();
                        }
                    }

                    if (GUILayout.Button("Include in compile"))
                    {
                        if (EditorUtility.DisplayDialog("Attention", "Are you sure you want to include \"NOA Debugger\" in the compilation?", "OK", "Cancel"))
                        {
                            IsWait = true;
                            NoaPackageManager.IncludeInCompile();
                        }
                    }

                    EditorGUILayout.Separator();

                    if (GUILayout.Button("Delete"))
                    {
                        _DeletePackage();
                    }

                    EditorBaseStyle.DrawUILine();

                    EditorGUILayout.LabelField("<b>Document</b>", EditorBaseStyle.Headline());

                    EditorGUILayout.Separator();

                    if (GUILayout.Button("Open README.md"))
                    {
                        var path = $"{NoaPackageManager.NoaDebuggerPackagePath}/README.md";
                        EditorUtil.OpenFile(path);
                    }

                    EditorBaseStyle.DrawUILine();

                    EditorGUILayout.LabelField("<b>Support</b>", EditorBaseStyle.Headline());

                    EditorGUILayout.Separator();

                    GUILayout.Label("We are committed to supporting a diverse community, so feel free to post in English or Japanese. Our team will respond to your inquiries in the language you used. Please note that our support hours are from 10:00 to 18:00 JST, Monday through Friday.", EditorStyles.wordWrappedLabel);

                    EditorGUILayout.Separator();

                    if (GUILayout.Button("Open Unity Discussions"))
                    {
                        Application.OpenURL(EditorDefine.UnityDiscussionsUrl);
                    }

                    EditorBaseStyle.DrawUILine();

                    EditorGUILayout.LabelField("<b>Review</b>", EditorBaseStyle.Headline());

                    EditorGUILayout.Separator();

                    GUILayout.Label("If you find NOA Debugger helpful, we would greatly appreciate it if you could leave a rating and review on the Asset Store. Your feedback is invaluable and helps us improve. Thank you for your support!", EditorStyles.wordWrappedLabel);

                    EditorGUILayout.Separator();

                    if (GUILayout.Button("Rate and review on Asset Store"))
                    {
                        Application.OpenURL(EditorDefine.AssetStoreReviewUrl);
                    }
                }
#if NOA_DEBUGGER
                EditorGUI.BeginChangeCheck();

                if (_selectTab == TabMenu.Settings)
                {
                    PackageEditorAutoSaveSettings autoSaveSettings = null;

                    using (var scrollView = new EditorGUILayout.ScrollViewScope(_scrollPos))
                    {
                        _scrollPos = scrollView.scrollPosition;

                        float originalValue = EditorGUIUtility.labelWidth;
                        EditorGUIUtility.labelWidth = EditorDefine.NOA_DEBUGGER_EDITOR_LABEL_WIDTH;


                        int settingsCount = 0;
                        foreach (var settings in _settings)
                        {
                            if (settings is PackageEditorAutoSaveSettings autoSave)
                            {
                                autoSaveSettings = autoSave;
                                continue;
                            }

                            settings.DrawGUI();

                            int settingsCountLimit = _settings.Count - 2;
                            if (settingsCount < settingsCountLimit)
                            {
                                EditorBaseStyle.DrawUILine();
                            }
                            settingsCount++;
                        }

                        EditorGUIUtility.labelWidth = originalValue;
                    }

                    EditorBaseStyle.DrawUILine();
                    autoSaveSettings.DrawGUI();

                    if (EditorGUI.EndChangeCheck())
                    {
                        _isSettingsChanged = true;

                        if (autoSaveSettings.IsAutoSave)
                        {
                            _SettingsAssetSave();
                        }
                    }

                    if (!autoSaveSettings.IsAutoSave && _isSettingsChanged)
                    {
                        var color = GUI.backgroundColor;
                        GUI.backgroundColor = NoaDebuggerDefine.TextColors.LogLightBlue;
                        if (GUILayout.Button("Save", EditorBaseStyle.ButtonHighlighted))
                        {
                            _SettingsAssetSave();
                        }
                        GUI.backgroundColor = color;
                    }
                }


                if (_selectTab == TabMenu.Tools)
                {
                    EditorGUILayout.LabelField("<b>Views</b>", EditorBaseStyle.Headline());
                    EditorGUILayout.Separator();
                    GUILayout.Label("Opens the Debug Command window.", EditorStyles.wordWrappedLabel);
                    GUILayout.Label("Debug commands can be set and registered within an application that incorporates the NOA Debugger, allowing for the execution of methods, display of properties, etc. For more information, please refer to the instruction guide.", EditorStyles.wordWrappedLabel);
                    EditorGUILayout.Separator();

                    if (GUILayout.Button("Open Debug Command View"))
                    {
                        CommandEditorWindow.ShowWindow(this);
                    }
                }
#endif
            }

            EditorGUI.EndDisabledGroup();


            GUILayout.FlexibleSpace();

            using (new EditorGUILayout.VerticalScope(EditorBaseStyle.Box))
            {
                EditorGUILayout.LabelField("<i>Copyright (c) 2024 EXNOA LLC. All Rights Reserved.</i>", EditorBaseStyle.RichText());
            }
        }

        void Awake()
        {
            CompilationPipeline.compilationStarted  += PackageEditorWindow._OnCompilationStarted;
            CompilationPipeline.compilationFinished += PackageEditorWindow._OnCompilationFinished;
            IsWait = false;
        }

        void OnDestroy()
        {
            PackageEditorWindow._isWindowOpen = false;
#if NOA_DEBUGGER
            _noaDebuggerSettings = null;
#endif
            CompilationPipeline.compilationStarted  -= PackageEditorWindow._OnCompilationStarted;
            CompilationPipeline.compilationFinished -= PackageEditorWindow._OnCompilationFinished;
        }

        void Update()
        {
            if (Application.isPlaying && !_isPlayingLayoutMode)
            {
                _SwitchPlayingLayout();
                Repaint();
            }

            if(!Application.isPlaying && _isPlayingLayoutMode)
            {
                _SwitchEditorLayout();
                Repaint();
            }
        }

        void _SwitchEditorLayout()
        {
            _isPlayingLayoutMode = false;

            _tabMenu = new List<string>();
            _tabMenu.Add(TabMenu.General.ToString());
#if NOA_DEBUGGER
            _tabMenu.Add(TabMenu.Settings.ToString());
            _tabMenu.Add(TabMenu.Tools.ToString());
#endif
            _selectedTabIndex = _GetTabIndex(_selectTab);
        }

        void _SwitchPlayingLayout()
        {
            _isPlayingLayoutMode = true;
#if NOA_DEBUGGER
            _tabMenu = new List<string>();
            _tabMenu.Add(TabMenu.Tools.ToString());
            _selectTab = TabMenu.Tools;
            _selectedTabIndex = _GetTabIndex(_selectTab);
#endif
        }

        int _GetTabIndex(TabMenu selectTab)
        {
            if(_tabMenu == null || _tabMenu.Count == 0)
                return 0;

            int index = Array.IndexOf(_tabMenu.ToArray(), selectTab.ToString());

            if (index == -1)
                return 0;

            return index;
        }

        static void _OnCompilationStarted(object obj)
        {
            IsWait = true;
        }

        static void _OnCompilationFinished(object obj)
        {
            IsWait = false;
        }

        void _DeletePackage()
        {
            void delete()
            {
                IsWait = true;
                NoaPackageManager.DeletePackage();
                AssetDatabase.Refresh();
                Close();
            }

            if (EditorUtility.DisplayDialog(
                "Delete Package",
                "Are you sure you want to delete the \"NOA Debugger\" package?",
                "OK",
                "Cancel"))
            {
                string sampleProjectPath = NoaPackageManager.SampleDirectoryPathToDelete;
                if (string.IsNullOrEmpty(sampleProjectPath))
                {
                    delete();
                }
                else
                {
                    if (EditorUtility.DisplayDialog(
                        "Delete Sample Project",
                        $"The directory containing the sample project below will also be deleted.\n\n{sampleProjectPath}\n\nAre you sure?",
                        "OK",
                        "Cancel"))
                    {
                        NoaPackageManager.DeletePackageSample();
                        delete();
                    }
                }
            }
        }
    }
}
