#if NOA_DEBUGGER
using NoaDebugger.DebugCommand;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NoaDebugger
{
    public class CommandEditorWindow : EditorWindow
    {
        int _selectedCategory;

        Vector2 _commandsScrollPosition;

        Vector2 _detailsScrollPosition;

        string[] _displayCategoryNames = null;

        string[] _rawCategoryNames = null;

        bool HasCategory => _displayCategoryNames != null && _displayCategoryNames.Length > 0;

        CommandGroup[] _commandGroupData = null;

        bool HasCommand => _commandGroupData != null && _commandGroupData.Length > 0;

        CommandCollapsedStatusSaveData _savedCollapseStatus = null;

        bool _isShowsDetails = false;

        bool _isAutoRefresh = true;

        void OnEnable()
        {
            DebugCommandPresenter.OnRefreshProperty -= Repaint;
            DebugCommandPresenter.OnRefreshProperty += Repaint;

            if (_savedCollapseStatus == null)
            {
                _savedCollapseStatus = new CommandCollapsedStatusSaveData(
                    NoaDebuggerPrefsDefine.PrefsKeyDebugCommandGroupCollapsedStatusEditor);
            }
        }

        public static void ShowWindow(EditorWindow calledWindow = null)
        {
            var window = EditorWindow.GetWindow<CommandEditorWindow>();
            window.titleContent = new GUIContent("Debug Command");
            window.ShowTab();

            if(calledWindow != null)
            {
                float x = calledWindow.position.x;
                float y = calledWindow.position.y;
                float width = window.position.width;
                float height = Screen.currentResolution.height;
                window.position = new Rect(x, y, width, height);
            }
        }

        void OnInspectorUpdate()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (_isAutoRefresh)
            {
                Repaint();
            }

            _isAutoRefresh = DebugCommandPresenter.IsAutoRefresh;
        }

        void OnGUI()
        {
            if (!Application.isPlaying)
            {
                ShowBeforePlayingDisplay();
                RemoveCacheData();
                return;
            }

            RefreshCategoryNames();

            if (HasCategory)
            {
                ShowCategoryList();
            }
            else
            {
                ShowEmptyCategoryList();
                return;
            }


            if(!HasCommand)
            {
                ShowNoCommandsMessage();
                return;
            }

            ShowCommands();
            ShowDetailsIfRequired();
        }


        void ShowBeforePlayingDisplay()
        {
            EditorUtil.DrawTitle("NOA Debugger Tools");

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.HelpBox($"The command is displayed when the Editor is played.{Environment.NewLine}For details on debug commands, please refer to the DebugCommand section in the README.", MessageType.Info);
            }

            if(GUILayout.Button("Open README.md"))
            {
                var path = $"{NoaPackageManager.NoaDebuggerPackagePath}/README.md";
                EditorUtil.OpenFile(path);
            }
        }

        void ShowEmptyCategoryList()
        {
            _displayCategoryNames = Array.Empty<string>();
            ShowCategoryList();

            using (new EditorGUILayout.VerticalScope(EditorBaseStyle.Box))
            {
                EditorGUILayout.LabelField("No categories.");
            }
        }

        void ShowNoCommandsMessage()
        {
            using (new EditorGUILayout.VerticalScope(EditorBaseStyle.Box))
            {
                EditorGUILayout.LabelField("No commands.");
            }
        }

        void ShowCategoryList()
        {
            int beforeSelectCategory =  _selectedCategory;

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Category", GUILayout.Width(EditorGUIUtility.fieldWidth + 4));
                _selectedCategory = EditorGUILayout.Popup(_selectedCategory, _displayCategoryNames);

                var refreshIcon = EditorGUIUtility.IconContent("Refresh");
                GUIStyle style = EditorStyles.iconButton;
                style.margin.top = 3;
                if (GUILayout.Button(refreshIcon, style, GUILayout.Width(EditorGUIUtility.fieldWidth - 32)))
                {
                    RefreshCategoryNames();
                }

                _isAutoRefresh = EditorGUILayout.Toggle(_isAutoRefresh, GUILayout.Width(18));

                if (_isAutoRefresh != DebugCommandPresenter.IsAutoRefresh)
                {
                    DebugCommandPresenter.IsAutoRefresh = _isAutoRefresh;
                }

                EditorGUILayout.LabelField("Details", GUILayout.Width(EditorGUIUtility.fieldWidth - 8));
                _isShowsDetails = EditorGUILayout.Toggle(_isShowsDetails, GUILayout.Width(18));

            }

            if (beforeSelectCategory != _selectedCategory)
            {
                RefreshCommandGroup();
            }
        }

        void ShowCommands()
        {
            using (var scrollView = new EditorGUILayout.ScrollViewScope(_commandsScrollPosition))
            {
                _commandsScrollPosition = scrollView.scrollPosition;

                foreach(var group in _commandGroupData)
                {
                    group.Draw();
                }
            }
        }

        void ShowDetailsIfRequired()
        {
            if (!_isShowsDetails)
            {
                return;
            }

            EditorBaseStyle.DrawUILine();
            using (var scrollView = new EditorGUILayout.ScrollViewScope(_detailsScrollPosition, GUILayout.Height(position.size.y / 2)))
            {
                _detailsScrollPosition = scrollView.scrollPosition;

                foreach(var group in _commandGroupData)
                {
                    group.DrawCommandDetails();
                }

            }
        }


        void RefreshCategoryNames()
        {
            var tempRawCategoryNames = _rawCategoryNames;
            _rawCategoryNames = DebugCommandPresenter.GetCategoryNames();
            _displayCategoryNames = DebugCommandPresenter.GetDisplayCategoryNames();

            if (_commandGroupData == null ||
                tempRawCategoryNames == null ^ _rawCategoryNames == null ||
                tempRawCategoryNames != null && _rawCategoryNames != null &&
                !tempRawCategoryNames.SequenceEqual(_rawCategoryNames))
            {
                RefreshCommandGroup();
            }
        }

        void RefreshCommandGroup()
        {
            if (!HasCategory)
            {
                return;
            }

            if (_selectedCategory >= _rawCategoryNames.Length)
            {
                _selectedCategory = 0;
            }

            string categoryName = _rawCategoryNames[_selectedCategory];
            var commandGroupData = DebugCommandPresenter.GetCategoryGroup(categoryName);

            if (commandGroupData == null)
            {
                _commandGroupData = Array.Empty<CommandGroup>();

                return;
            }

            int index = 0;
            _commandGroupData = new CommandGroup[commandGroupData.Count];
            foreach(var group in commandGroupData.OrderBy(x => x.Value._order ?? Int32.MaxValue))
            {
                _commandGroupData[index] = new CommandGroup(categoryName, group.Key, group.Value, this, _savedCollapseStatus);
                index++;
            }
        }

        void RemoveCacheData()
        {
            if (_displayCategoryNames != null)
            {
                _displayCategoryNames = null;
                _rawCategoryNames = null;
                _commandGroupData = null;
            }
        }


        class CommandGroup
        {
            readonly string _categoryName;
            readonly string _groupName;
            readonly IEditorCommandComponent[] _commandElements;
            readonly EditorWindow _window;
            readonly CommandCollapsedStatusSaveData _savedCollapseStatus;
            bool _isShowDetail = false;
            bool _isShowsGroup;

            bool IsShowGroup
            {
                get => _isShowsGroup;
                set
                {
                    if (_isShowsGroup == value)
                    {
                        return;
                    }

                    _savedCollapseStatus.UpdateStatus(_categoryName, _groupName, !value);
                    _savedCollapseStatus.Save();
                    _isShowsGroup = value;
                }
            }

            public CommandGroup(string categoryName, string name, CommandGroupData data, EditorWindow window, CommandCollapsedStatusSaveData savedCollapseStatus)
            {
                _categoryName = categoryName;
                _groupName = name;
                _window = window;
                _savedCollapseStatus = savedCollapseStatus;

                _isShowsGroup = !savedCollapseStatus.GetIsCollapsed(categoryName, name, data._isCollapsedDefault);

                _commandElements = new IEditorCommandComponent[data._commandList.Count];
                for(int i = 0; i < data._commandList.Count; i++)
                {
                    var command = data._commandList[i];
                    var visitor = new EditorCommandComponentVisitor();
                    command.Accept(visitor);

                    if (!visitor.IsSuccess)
                    {
                        throw new Exception($"Unsupported type. ==> {command}");
                    }

                    _commandElements[i] = visitor.Result;
                }
            }

            public void Draw()
            {
                using (new EditorGUILayout.VerticalScope(EditorBaseStyle.Box))
                {
                    IsShowGroup = EditorGUILayout.Foldout(IsShowGroup, _groupName, true);

                    if (!IsShowGroup)
                    {
                        return;
                    }

                    DrawCommands();
                }
            }

            void DrawCommands()
            {
                foreach (var command in _commandElements)
                {
                    command.Draw(_window.position.width);
                }
            }

            public void DrawCommandDetails()
            {
                using (new EditorGUILayout.VerticalScope(EditorBaseStyle.Box))
                {
                    _isShowDetail = EditorGUILayout.Foldout(_isShowDetail, _groupName, true);

                    if (!_isShowDetail)
                    {
                        return;
                    }

                    foreach (var command in _commandElements)
                    {
                        command.DrawDetail();
                    }
                }
            }
        }
    }
}
#endif
