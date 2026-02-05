using System;
using TMPro;
using UnityEngine;

namespace NoaDebugger
{
    /// <summary>
    /// Operations of NoaDebugger can be done through this class.
    /// </summary>
    public static class NoaDebug
    {
        /// <summary>
        /// Types of NOA Debugger hierarchy.
        /// </summary>
        public enum HierarchyLevelGroup
        {
            /// <summary>
            /// The hierarchy for the floating window.
            /// </summary>
            FloatingWindow,

            /// <summary>
            /// The hierarchy for the NoaUIElement.
            /// </summary>
            UIElement,

            /// <summary>
            /// The hierarchy for the main view.
            /// </summary>
            MainView,

            /// <summary>
            /// The hierarchy for dialogs.
            /// </summary>
            Dialog,

            /// <summary>
            /// The hierarchy for overlays.
            /// </summary>
            Overlay,
        }

        /// <summary>
        /// Types of overlay features.
        /// </summary>
        public enum OverlayFeatures
        {
            Profiler = 1,
            ConsoleLog = 3,
            ApiLog = 4,
        }

        /// <summary>
        /// Enum representing different anchor positions for overlays.
        /// </summary>
        public enum OverlayPosition
        {
            UpperLeft,
            UpperCenter,
            UpperRight,
            MiddleLeft,
            MiddleRight,
            LowerLeft,
            LowerCenter,
            LowerRight
        }

        /// <summary>
        /// Enum representing menu types.
        /// </summary>
        public enum MenuType
        {
            Information,
            Profiler,
            Snapshot,
            ConsoleLog,
            APILog,
            Hierarchy,
            Command
        }

        /// <summary>
        /// Callback events when displaying the tool.
        /// </summary>
        public static Action<int> OnShow
        {
            get => NoaDebuggerManager.OnShow;
            set => NoaDebuggerManager.OnShow = value;
        }

        /// <summary>
        /// Callback events when closing the tool.
        /// </summary>
        public static Action<int> OnHide
        {
            get => NoaDebuggerManager.OnHide;
            set => NoaDebuggerManager.OnHide = value;
        }

        /// <summary>
        /// Callback events when switching menus.
        /// </summary>
        public static Action<int, bool> OnMenuChanged
        {
            get => NoaDebuggerManager.OnMenuChanged;
            set => NoaDebuggerManager.OnMenuChanged = value;
        }

        /// <summary>
        /// Returns Transform at the top level of the tool.
        /// </summary>
        public static Transform RootTransform => NoaDebuggerManager.RootTransform;

        /// <summary>
        /// Returns true if the tool is initialized.
        /// </summary>
        public static bool IsInitialized => NoaDebuggerManager.IsDebuggerInitialized();

        /// <summary>
        /// Returns true if shortcuts are enabled.
        /// </summary>
        public static bool IsShortcutsEnabled => NoaDebuggerManager.IsShortcutsEnabled;

        /// <summary>
        /// Returns true if NOA Debugger is shown on the world coordinate.
        /// </summary>
        public static bool IsWorldSpaceRenderingEnabled => NoaDebuggerManager.IsWorldSpaceRenderingEnabled;

        /// <summary>
        /// Returns true if the tool is visible.
        /// </summary>
        public static bool IsDebuggerVisible => NoaDebuggerVisibilityManager.IsMainViewActive;

        /// <summary>
        /// Returns true if the trigger button is visible.
        /// </summary>
        public static bool IsTriggerButtonVisible => NoaDebuggerVisibilityManager.IsTriggerButtonActive;

        /// <summary>
        /// Returns true if the overlay is visible.
        /// </summary>
        public static bool IsOverlayVisible => NoaDebuggerVisibilityManager.IsOverlayVisible;

        /// <summary>
        /// Returns true if the floating window is visible.
        /// </summary>
        public static bool IsFloatingWindowVisible => NoaDebuggerVisibilityManager.IsFloatingWindowVisible;

        /// <summary>
        /// Initialize the tool.
        /// </summary>
        public static void Initialize()
        {
            NoaDebuggerManager.InitializeDebugger();
        }

        /// <summary>
        /// Start up the tool.
        /// Opens the last displayed tool.
        /// </summary>
        public static void Show()
        {
            NoaDebuggerManager.ShowDebugger();
        }

        /// <summary>
        /// Start up the tool.
        /// Opens the tab at the specified position by specifying the index number.
        /// If index is null, the last displayed tool will open.
        /// </summary>
        /// <param name="index">index number</param>
        /// <param name="isCustomMenu">Whether it is a CustomMenu</param>
        public static void Show(int? index, bool isCustomMenu = false)
        {
            NoaDebuggerManager.ShowDebugger(index, isCustomMenu);
        }

        /// <summary>
        /// Opens the specified menu.
        /// </summary>
        /// <param name="menuType">menu type</param>
        public static void Show(MenuType menuType)
        {
            NoaDebuggerManager.ShowDebugger(menuType);
        }

        /// <summary>
        /// Close the tool.
        /// </summary>
        public static void Hide()
        {
            NoaDebuggerManager.HideDebugger();
        }

        /// <summary>
        /// You can set the tool screen display / non-display.
        /// </summary>
        /// <param name="isActive">display / non-display</param>
        public static void SetDebuggerActive(bool isActive)
        {
            NoaDebuggerVisibilityManager.SetMainViewVisible(isActive);
        }

        /// <summary>
        /// Sets overlay active.
        /// </summary>
        /// <param name="isActive">True shows overlay, false hides them.</param>
        public static void SetOverlayActive(bool isActive)
        {
            NoaDebuggerVisibilityManager.SetOverlayVisibleSetting(isActive);
        }

        /// <summary>
        /// Sets overlay enabled for specified feature.
        /// </summary>
        /// <param name="feature">Specifies the target to change the enabled state.</param>
        /// <param name="isEnabled">True enables specified feature overlay, false disables them.</param>
        public static void SetOverlayEnabled(OverlayFeatures feature, bool isEnabled)
        {
            NoaDebuggerManager.SetOverlayEnabled(feature, isEnabled);
        }

        /// <summary>
        /// Gets whether the specified overlay feature is enabled.
        /// </summary>
        /// <param name="feature">Specifies the target to check.</param>
        /// <returns>true if the specified feature is enabled.</returns>
        public static bool GetOverlayEnabled(OverlayFeatures feature)
        {
            return NoaDebuggerManager.GetOverlayEnabled(feature);
        }

        /// <summary>
        /// You can set the floating window of the tool display / non-display.
        /// </summary>
        /// <param name="isActive">display / non-display</param>
        public static void SetFloatingWindowActive(bool isActive)
        {
            NoaDebuggerVisibilityManager.SetFloatingWindowVisibleSetting(isActive);
        }

        /// <summary>
        /// You can set the display/tracking of the tool's trigger button.
        /// </summary>
        /// <param name="isActive">display / non-display</param>
        public static void SetTriggerButtonActive(bool isActive)
        {
            NoaDebuggerVisibilityManager.SetTriggerButtonVisibleSetting(isActive);
        }

        /// <summary>
        /// Configure the fonts used for the tool.
        /// </summary>
        /// <param name="fontAsset">Font asset to use.</param>
        /// <param name="fontMaterial">Material to use. If omitted, the default material of the specified font asset will be applied.</param>
        /// <param name="fontSizeRate">Font size Multiplier. If omitted, an appropriate value will be automatically calculated and set.</param>
        public static void SetFont(TMP_FontAsset fontAsset, Material fontMaterial = null, float fontSizeRate = -1.0f)
        {
            NoaDebuggerManager.SetFont(fontAsset, fontMaterial, fontSizeRate);
        }

        /// <summary>
        /// Generates an object at the same hierarchy level as the NOA Debugger UI.
        /// </summary>
        /// <param name="prefab">Target object.</param>
        /// <param name="group">Hierarchy level.</param>
        public static GameObject Instantiate(GameObject prefab, HierarchyLevelGroup group)
        {
            Transform parent = group switch
            {
                HierarchyLevelGroup.FloatingWindow => NoaDebuggerManager.FloatingWindowRootTransform,
                HierarchyLevelGroup.UIElement => NoaDebuggerManager.UIElementRootTransform,
                HierarchyLevelGroup.MainView => NoaDebuggerManager.MainViewRootTransform,
                HierarchyLevelGroup.Dialog => NoaDebuggerManager.DialogRootTransform,
                HierarchyLevelGroup.Overlay => NoaDebuggerManager.OverlayRootTransform,
                _ => null
            };

            if (parent == null)
            {
                Debug.LogWarning($"Unknown group: {group}");

                return null;
            }

            return UnityEngine.Object.Instantiate(prefab, parent);
        }

        /// <summary>
        /// Sets NOA Debugger’s canvas scale.
        /// </summary>
        /// <param name="scale">NOA Debugger’s canvas scale.</param>
        public static void SetCanvasScale(float scale)
        {
            NoaDebuggerManager.SetCanvasScale(scale);
        }

        /// <summary>
        /// Sets shortcuts enabled.
        /// </summary>
        /// <param name="isEnabled">True enables shortcuts, false disables them.</param>
        public static void SetShortcutsEnabled(bool isEnabled)
        {
            NoaDebuggerManager.SetShortcutsEnabled(isEnabled);
        }

        /// <summary>
        /// Display tools on world coordinates.
        /// </summary>
        /// <param name="worldCamera">Specifies the camera to be used for rendering and event detection of the NOA Debugger UI. If null is specified, a camera with the MainCamera tag will be used.</param>
        public static void EnableWorldSpaceRendering(Camera worldCamera = null)
        {
            NoaDebuggerManager.EnableWorldSpaceRendering(worldCamera);
        }

        /// <summary>
        /// Display the tool on the 2D screen coordinates.
        /// </summary>
        public static void DisableWorldSpaceRendering()
        {
            NoaDebuggerManager.DisableWorldSpaceRendering();
        }

        /// <summary>
        /// Copies the specified text to the clipboard.
        /// </summary>
        /// <param name="text">Specified text.</param>
        public static void CopyToClipboard(string text)
        {
            NoaDebuggerManager.CopyToClipboard(text);
        }

        /// <summary>
        /// Downloads the specified text as a file with the specified name.
        /// </summary>
        /// <param name="text">The text to download.</param>
        /// <param name="fileName">The name of the file to download. Includes the file extension.</param>
        /// <param name="mimeType">The MIME type corresponding to the file extension. Used only on Android and Web.</param>
        public static void DownloadText(string text, string fileName, string mimeType)
        {
            NoaDebuggerManager.DownloadText(text, fileName, mimeType);
        }

        /// <summary>
        /// Destroys the tool.
        /// </summary>
        public static void Destroy()
        {
            NoaDebuggerManager.DestroyDebugger();
        }

        /// <summary>
        /// Captures a screenshot and returns the image data through a callback function.
        /// </summary>
        /// <param name="callback">The callback function which will receive the image data as a byte array.</param>
        [Obsolete("Use NoaController.CaptureScreenshot() instead.")]
        public static void TakeScreenshot(Action<byte[]> callback)
        {
            NoaControllerManager.TakeScreenshot(callback);
        }
    }
}
