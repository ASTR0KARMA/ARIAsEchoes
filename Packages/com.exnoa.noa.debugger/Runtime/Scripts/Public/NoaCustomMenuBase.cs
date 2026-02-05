using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NoaDebugger
{
    /// <summary>
    /// Class that provides a custom menu.
    /// </summary>
    public abstract class NoaCustomMenuBase : INoaDebuggerTool
    {
        GameObject _viewObj;
        GameObject _mainView;

        bool _hasError;

        protected NoaCustomMenuBase()
        {
#pragma warning disable
            _viewObj = Resources.Load<GameObject>(ViewPrefabPath);
#pragma warning restore

#if UNITY_EDITOR
            _hasError = !_ValidateSetup();
#endif
        }

        /// <summary>
        /// Fixed to None as notification functionality will not be used.
        /// </summary>
        ToolNotificationStatus INoaDebuggerTool.NotifyStatus => ToolNotificationStatus.None;

        /// <summary>
        /// Initialization process.
        /// </summary>
        void INoaDebuggerTool.Init()
        {
            OnInitialize();
        }

#if UNITY_EDITOR
        /// <summary>
        /// Checks if the custom menu has any errors.
        /// Called by NoaDebugger before adding to the menu list.
        /// </summary>
        /// <returns>True if there are errors; otherwise, false.</returns>
        public bool HasError()
        {
            return _hasError;
        }

        /// <summary>
        /// Validates the custom menu setup.
        /// </summary>
        /// <returns>True if valid; otherwise, false.</returns>
        bool _ValidateSetup()
        {
            if (_viewObj == null)
            {
                LogModel.LogWarning($"Prefab Not Found:{this.GetType().Name}:{ViewPrefabPath}");
                return false;
            }

            if (!_IsInCustomMenuFolder(_viewObj, out string customMenuFolderPath))
            {
                LogModel.LogError($"View prefab '{_viewObj.name}' is not located under the CustomMenuFolderPath. Expected path: {customMenuFolderPath}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if the specified GameObject asset exists under the CustomMenuFolderPath defined in NoaDebuggerSettings.
        /// </summary>
        /// <param name="viewObj">The GameObject to check.</param>
        /// <param name="customMenuFolderPath">The custom menu folder path used for validation.</param>
        /// <returns>True if the asset is under the CustomMenuFolderPath; otherwise, false.</returns>
        bool _IsInCustomMenuFolder(GameObject viewObj, out string customMenuFolderPath)
        {
            customMenuFolderPath = NoaDebuggerSettingsManager.GetNoaDebuggerSettings()?.CustomMenuFolderPath ?? NoaDebuggerDefine.DEFAULT_CUSTOM_MENU_RESOURCES_FOLDER_PATH;

            string assetPath = AssetDatabase.GetAssetPath(viewObj);
            if (string.IsNullOrEmpty(assetPath))
            {
                return false;
            }

            string normalizedCustomPath = customMenuFolderPath.Replace('\\', '/').TrimEnd('/');
            string normalizedAssetPath = assetPath.Replace('\\', '/');
            return normalizedAssetPath.StartsWith(normalizedCustomPath + "/", System.StringComparison.OrdinalIgnoreCase);
        }
#endif

        /// <summary>
        /// Menu information class of the provider class.
        /// </summary>
        internal class CustomMenuInfo : IMenuInfo
        {
            public string Name => MenuName;
            public string MenuName { get; }

            public int SortNo => NoaDebuggerDefine.CUSTOM_MENU_SORT_NO;

            /// <summary>
            /// Initialization process.
            /// </summary>
            /// <param name="menuName">Menu display name.</param>
            public CustomMenuInfo(string menuName)
            {
                MenuName = menuName;
            }

            /// <summary>
            /// Initialization process (Made becuase Default was needed. Not used)
            /// It's necessary as a setup because it's processed in NoaDebuggerSettings.GetIMenuInfoList
            /// </summary>
            public CustomMenuInfo() { }
        }

        /// <summary>
        /// Holds the menu information.
        /// </summary>
        CustomMenuInfo _customMenuInfo;

        /// <summary>
        /// Menu information.
        /// </summary>
        IMenuInfo INoaDebuggerTool.MenuInfo()
        {
            return _customMenuInfo ??= new CustomMenuInfo(MenuName);
        }

        /// <summary>
        /// Refer to the summary in the inherited parent.
        /// </summary>
        void INoaDebuggerTool.ShowView(Transform parent)
        {
            if (_viewObj == null)
            {
                LogModel.LogWarning($"Prefab Not Found:{this.GetType().Name}:{ViewPrefabPath}");
                return;
            }

            if (_mainView == null)
            {
                _mainView = Object.Instantiate(_viewObj, parent);
            }

            _ShowView(_mainView);
        }

        /// <summary>
        /// Display process for the specified view.
        /// </summary>
        /// <param name="view">Specify the view to be displayed.</param>
        void _ShowView(GameObject view)
        {
            _mainView.gameObject.SetActive(true);
            OnShow(view);
        }

        /// <summary>
        /// Alignment process of the menu UI.
        /// </summary>
        void INoaDebuggerTool.AlignmentUI(bool isReverse)
        {
        }

        /// <summary>
        /// Hide the view.
        /// </summary>
        void INoaDebuggerTool.OnHidden()
        {
            OnHide();

            if (_mainView != null)
            {
                _mainView.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Refer to the summary in the inherited parent.
        /// </summary>
        void INoaDebuggerTool.OnToolDispose()
        {
            OnDispose();

            if (_viewObj == null)
            {
                return;
            }

            _viewObj = null;
        }

        /// <summary>
        /// Callback when the tool is displayed.
        /// </summary>
        /// <param name="view">The GameObject that will be displayed is entered.</param>
        protected virtual void OnShow(GameObject view) { }

        /// <summary>
        /// Callback when the tool is Hide.
        /// </summary>
        protected virtual void OnHide() { }

        /// <summary>
        /// Initialization process.
        /// </summary>
        protected virtual void OnInitialize() { }

        /// <summary>
        /// Disabling process.
        /// </summary>
        protected virtual void OnDispose() { }

        /// <summary>
        /// Path of the Prefab to be displayed.
        /// </summary>
        protected abstract string ViewPrefabPath { get; }

        /// <summary>
        /// Name of the menu to be displayed.
        /// </summary>
        protected abstract string MenuName { get; }
    }
}
