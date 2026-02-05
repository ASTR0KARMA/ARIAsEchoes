using System;

namespace NoaDebugger
{
    /// <summary>
    /// This class allows you to access the functions of controller.
    /// </summary>
    public static class NoaController
    {
        /// <summary>
        /// Count of custom action buttons.
        /// </summary>
        public static readonly int CustomActionButtonCount = 5;

        /// <summary>
        /// Custom action type.
        /// </summary>
        public enum CustomActionType
        {
            /// <summary>
            /// Becomes a button that triggers an action when pressed or long pressed.
            /// </summary>
            Button,

            /// <summary>
            /// Becomes a toggle button.
            /// </summary>
            ToggleButton,

            /// <summary>
            /// Default action type.
            /// </summary>
            Default = Button
        }

        /// <summary>
        /// Screenshot target to capture.
        /// </summary>
        [Flags]
        public enum ScreenshotTarget
        {
            /// <summary>
            /// Does not include any NOA Debugger-related UI.
            /// </summary>
            None = 0,

            /// <summary>
            /// Includes the launch button.
            /// </summary>
            LaunchButton = 1 << 0,

            /// <summary>
            /// Includes floating windows.
            /// </summary>
            FloatingWindows = 1 << 1,

            /// <summary>
            /// Includes NoaUIElement.
            /// </summary>
            UIElement = 1 << 2,

            /// <summary>
            /// Includes the main view.
            /// </summary>
            MainView = 1 << 3,

            /// <summary>
            /// Includes the overlay.
            /// </summary>
            Overlays = 1 << 4,

            /// <summary>
            /// Includes all NOA Debugger-related UI.
            /// </summary>
            All = LaunchButton | FloatingWindows | UIElement | MainView | Overlays
        }

        /// <summary>
        /// Callback that is fired when the controller is displayed.
        /// </summary>
        public static Action OnShow
        {
            get => NoaControllerManager.OnShow;
            set => NoaControllerManager.OnShow = value;
        }

        /// <summary>
        /// Callback that is fired when the controller is closed.
        /// </summary>
        public static Action OnHide
        {
            get => NoaControllerManager.OnHide;
            set => NoaControllerManager.OnHide = value;
        }

        /// <summary>
        /// Callback events when the game state is toggle between paused and resumed.
        /// </summary>
        public static Action<bool> OnTogglePauseResume
        {
            get => NoaControllerManager.OnTogglePauseResume;
            set => NoaControllerManager.OnTogglePauseResume = value;
        }

        /// <summary>
        /// Callback events when the game speed is changed.
        /// </summary>
        public static Action<float> OnGameSpeedChanged
        {
            get => NoaControllerManager.OnGameSpeedChanged;
            set => NoaControllerManager.OnGameSpeedChanged = value;
        }

        /// <summary>
        /// Registers a custom application reset callback.
        /// If the argument is null, unregisters the corresponding callback.
        /// </summary>
        /// <param name="callbacks">A class implementing INoaApplicationResetCallbacks or derived from NoaApplicationResetCallbacks</param>
        public static void SetApplicationResetCallbacks(INoaApplicationResetCallbacks callbacks)
        {
            ApplicationResetCallbacks = callbacks;
        }

        internal  static INoaApplicationResetCallbacks ApplicationResetCallbacks { get; set; } = null;

        /// <summary>
        /// You can set callback events when the application reset.
        /// If the event handler returns true, the transition to the initial scene will occur.
        /// If the event handler returns false, the transition to the initial scene will not occur.
        /// </summary>
        [Obsolete("Inherit from NoaApplicationResetCallbacks and override OnBeforeApplicationReset instead.")]
        public static Func<bool> OnApplicationReset
        {
            get => NoaControllerManager.OnApplicationReset;
            set => NoaControllerManager.OnApplicationReset = value;
        }

        /// <summary>
        /// Registers a custom toggle UI callback.
        /// If the argument is null, unregisters the corresponding callback.
        /// </summary>
        /// <param name="callbacks">A class implementing INoaToggleUICallbacks or derived from NoaToggleUICallbacks</param>
        public static void SetToggleUICallbacks(INoaToggleUICallbacks callbacks)
        {
            ToggleUICallbacks = callbacks;
        }

        internal static INoaToggleUICallbacks ToggleUICallbacks { get; set; } = null;

        /// <summary>
        /// Callback that is fired when toggling the show state of the NOA Debugger-related UI.
        /// If the callback returns true, toggle is executed, otherwise, if it returns false, toggle will not occur.
        /// </summary>
        [Obsolete("Inherit from NoaToggleUICallbacks and override OnBeforeToggleUI instead.")]
        public static Func<bool, bool> OnToggleNoaDebuggerUI
        {
            get => NoaDebuggerVisibilityManager.OnToggleNoaDebuggerUI;
            set => NoaDebuggerVisibilityManager.OnToggleNoaDebuggerUI = value;
        }

        /// <summary>
        /// Registers a custom screenshot callback.
        /// If the argument is null, unregisters the corresponding callback.
        /// </summary>
        /// <param name="callbacks">A class implementing INoaScreenshotCallbacks or derived from NoaScreenshotCallbacks</param>
        public static void SetScreenshotCallbacks(INoaScreenshotCallbacks callbacks)
        {
            ScreenshotCallbacks = callbacks;
        }

        internal static INoaScreenshotCallbacks ScreenshotCallbacks { get; private set; } = null;

        /// <summary>
        /// Callback that is fired before capturing a screenshot.
        /// The return value of the callback controls the contents of the screenshot.
        /// </summary>
        [Obsolete("Inherit from NoaScreenshotCallbacks and override OnBeforeScreenshot instead.")]
        public static Func<ScreenshotTarget> OnBeforeScreenshot
        {
            get => NoaControllerManager.OnBeforeScreenshot;
            set => NoaControllerManager.OnBeforeScreenshot = value;
        }

        /// <summary>
        /// Callback that is fired when capturing a screenshot.
        /// If the callback returns true, capturing a screenshot will occur,
        /// otherwise, if it returns false, capturing a screenshot will not occur and GetCapturedScreenshot() returns null.
        /// </summary>
        [Obsolete("Inherit from NoaScreenshotCallbacks and override OnCaptureScreenshot instead.")]
        public static Func<bool> OnCaptureScreenshot
        {
            get => NoaControllerManager.OnCaptureScreenshot;
            set => NoaControllerManager.OnCaptureScreenshot = value;
        }

        /// <summary>
        /// Callback that is fired after capturing a screenshot.
        /// You can get the screenshot by calling GetCapturedScreenshot().
        /// This is fired even if OnCaptureScreenshot returns false, but the image data will be null.
        /// </summary>
        [Obsolete("Inherit from NoaScreenshotCallbacks and override OnAfterScreenshot instead.")]
        public static Action OnAfterScreenshot
        {
            get => NoaControllerManager.OnAfterScreenshot;
            set => NoaControllerManager.OnAfterScreenshot = value;
        }

        /// <summary>
        /// Callback that is fired when stepping frames with the tool.
        /// </summary>
        public static Action OnFrameStepping
        {
            get => NoaControllerManager.OnFrameStepping;
            set => NoaControllerManager.OnFrameStepping = value;
        }

        /// <summary>
        /// Returns true if the controller is displayed.
        /// </summary>
        public static bool IsVisible => NoaDebuggerVisibilityManager.IsControllerActive;

        /// <summary>
        /// Returns true if the game is playing, false if game is paused.
        /// </summary>
        public static bool IsGamePlaying => NoaControllerManager.IsGamePlaying;

        /// <summary>
        /// Returns the game speed set by the tool.
        /// </summary>
        public static float GameSpeed => NoaControllerManager.GameSpeed;

        /// <summary>
        /// Shows the controller.
        /// </summary>
        public static void Show()
        {
            NoaDebuggerVisibilityManager.SetControllerVisible(true);
        }

        /// <summary>
        /// Hides the controller.
        /// </summary>
        public static void Hide()
        {
            NoaDebuggerVisibilityManager.SetControllerVisible(false);
        }

        /// <summary>
        /// Sets the action to be performed when a custom action button (0-4) on the controller is pressed.
        /// </summary>
        /// <param name="buttonIndex">Target custom action button (0-4).</param>
        /// <param name="action">Action to be performed.</param>
        /// <param name="messageFunc">Delegate that returns the toast message to be displayed. If omitted, a default toast message is displayed. If the delegate returns null, no toast message is displayed.</param>
        public static void SetCustomTapAction(int buttonIndex, Action action, Func<string> messageFunc = null)
        {
            NoaControllerManager.SetCustomTapAction(buttonIndex, action, messageFunc);
        }

        /// <summary>
        /// Sets the action to be performed when a custom action button (0-4) on the controller is long pressed.
        /// </summary>
        /// <param name="buttonIndex">Target custom action button (0-4).</param>
        /// <param name="action">Action to be performed.</param>
        /// <param name="messageFunc">Delegate that returns the toast message to be displayed. If omitted, a default toast message is displayed. If the delegate returns null, no toast message is displayed.</param>
        public static void SetCustomLongPressAction(int buttonIndex, Action action, Func<string> messageFunc = null)
        {
            NoaControllerManager.SetCustomLongPressAction(buttonIndex, action, messageFunc);
        }

        /// <summary>
        /// Sets the action to be performed when a custom action button (0-4) on the controller is toggled.
        /// </summary>
        /// <param name="buttonIndex">Target custom action button (0-4).</param>
        /// <param name="action">Action to be performed.</param>
        /// <param name="messageFunc">Delegate that returns the toast message to be displayed. If omitted, a default toast message is displayed. If the delegate returns null, no toast message is displayed.</param>
        /// <param name="initialState">Initial toggle state. If null, the current state is preserved.</param>
        public static void SetCustomToggleAction(int buttonIndex, Action<bool> action,
                                                 Func<bool, string> messageFunc = null,
                                                 bool? initialState = null)
        {
            NoaControllerManager.SetCustomToggleAction(buttonIndex, action, messageFunc, initialState);
        }

        /// <summary>
        /// Switches the function of a custom action button (0-4) on the controller.
        /// </summary>
        /// <param name="buttonIndex">Target custom action button (0-4).</param>
        /// <param name="actionType">Custom action type.</param>
        public static void SetCustomActionType(int buttonIndex, CustomActionType actionType)
        {
            NoaControllerManager.SetCustomActionType(buttonIndex, actionType);
        }

        /// <summary>
        /// Gets the current function type of a custom action button (0-4) on the controller.
        /// </summary>
        /// <param name="buttonIndex">Target custom action button (0-4).</param>
        /// <returns>The custom action type currently assigned to the specified button.</returns>
        public static CustomActionType GetCustomActionType(int buttonIndex)
        {
            return NoaControllerManager.GetCustomActionType(buttonIndex);
        }

        /// <summary>
        /// Executes the action set for when a custom action button (0-4) on the controller is pressed.
        /// If no press action is set, nothing happens.
        /// </summary>
        /// <param name="buttonIndex">Target custom action button (0-4).</param>
        public static void RunCustomTapAction(int buttonIndex)
        {
            NoaControllerManager.RunCustomTapAction(buttonIndex);
        }

        /// <summary>
        /// Executes the action set for when a custom action button (0-4) on the controller is long pressed.
        /// </summary>
        /// <param name="buttonIndex">Target custom action button (0-4).</param>
        /// <remarks>
        /// If no long press action is set, nothing happens.
        /// </remarks>
        public static void RunCustomLongPressAction(int buttonIndex)
        {
            NoaControllerManager.RunCustomLongPressAction(buttonIndex);
        }

        /// <summary>
        /// Toggles the custom action button (0-4) on the controller.
        /// </summary>
        /// <param name="buttonIndex">Target custom action button (0-4).</param>
        /// <param name="isOn">Toggle state.</param>
        /// <remarks>
        /// If no toggle action is set, nothing happens.
        /// </remarks>
        public static void SetCustomToggle(int buttonIndex, bool isOn)
        {
            NoaControllerManager.SetCustomToggle(buttonIndex, isOn);
        }

        /// <summary>
        /// Returns the toggle state of the custom action button (0-4) on the controller.
        /// </summary>
        /// <param name="buttonIndex">Target custom action button (0-4).</param>
        /// <returns>
        /// If no toggle action is set, it returns false.
        /// </returns>
        public static bool GetCustomToggle(int buttonIndex)
        {
            return NoaControllerManager.GetCustomToggle(buttonIndex);
        }

        /// <summary>
        /// Toggles the game state between paused and playing.
        /// </summary>
        public static void TogglePauseResume()
        {
            NoaControllerManager.TogglePauseResume();
        }

        /// <summary>
        /// Increases the game speed.
        /// </summary>
        public static void IncreaseGameSpeed()
        {
            NoaControllerManager.IncreaseGameSpeed();
        }

        /// <summary>
        /// Decreases the game speed.
        /// </summary>
        public static void DecreaseGameSpeed()
        {
            NoaControllerManager.DecreaseGameSpeed();
        }

        /// <summary>
        /// Sets the game speed to its minimum value.
        /// </summary>
        public static void MinimizeGameSpeed()
        {
            NoaControllerManager.MinimizeGameSpeed();
        }

        /// <summary>
        /// Sets the game speed to its maximum value.
        /// </summary>
        public static void MaximizeGameSpeed()
        {
            NoaControllerManager.MaximizeGameSpeed();
        }

        /// <summary>
        /// Resets the game speed to default.
        /// </summary>
        public static void ResetGameSpeed()
        {
            NoaControllerManager.ResetGameSpeed();
        }

        /// <summary>
        /// Pause the game and stepping frame by frame.
        /// </summary>
        public static void FrameStepping()
        {
            NoaControllerManager.FrameStepping();
        }

        /// <summary>
        /// Transitions to the application initial scene.
        /// </summary>
        /// <remarks>
        /// You can cancel the process by returning a specific value from the callback that is invoked before transition.
        /// </remarks>
        public static void ResetApplication()
        {
            NoaControllerManager.ResetApplication();
        }

        /// <summary>
        /// Toggles the show state of the NOA Debugger-related UI.
        /// </summary>
        /// <remarks>
        /// You can cancel the process by returning a specific value from the callback that is invoked before toggle.
        /// </remarks>
        public static void ToggleNoaDebuggerUI()
        {
            NoaDebuggerVisibilityManager.ToggleNoaDebuggerUI();
        }

        /// <summary>
        /// Captures a screenshot.
        /// </summary>
        /// <remarks>
        /// You can get the captured screenshot by calling the GetCapturedScreenshot().
        /// </remarks>
        public static void CaptureScreenshot()
        {
            NoaControllerManager.CaptureScreenshot();
        }

        /// <summary>
        /// Returns the captured screenshot.
        /// </summary>
        /// <returns>The captured screenshot or null if there is no captured screenshot.</returns>
        /// <remarks>
        /// You can get the captured screenshot anytime until calling the ClearCapturedScreenshot().
        /// </remarks>
        public static byte[] GetCapturedScreenshot()
        {
            return NoaControllerManager.GetCapturedScreenshot();
        }

        /// <summary>
        /// Clears the captured screenshot.
        /// </summary>
        public static void ClearCapturedScreenshot()
        {
            NoaControllerManager.ClearCapturedScreenshot();
        }
    }
}
