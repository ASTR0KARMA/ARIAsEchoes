namespace NoaDebugger
{
    /// <summary>
    /// By implementing this interface and registering it with the public API, screenshot callbacks can be customized.
    /// For users who want a highly flexible implementation with detailed customization.
    /// </summary>
    public interface INoaScreenshotCallbacks
    {
        /// <summary>
        /// Whether to execute NOA Debugger’s screenshot process.
        /// </summary>
        bool IsAllowBaseScreenshot { get; }

        /// <summary>
        /// Screenshot target to be captured.
        /// </summary>
        NoaController.ScreenshotTarget ScreenshotTarget { get; }

        /// <summary>
        /// This method is executed before controlling the display of the NOA Debugger UI based on the Target.
        /// By changing the Target here, OnCaptureScreenshot is executed with unnecessary NOA Debugger UI elements hidden.
        /// </summary>
        void OnBeforePrepareScreenshot();

        /// <summary>
        /// Called when capturing the screenshot.
        /// </summary>
        void OnCaptureScreenshot();

        /// <summary>
        /// Called after the screenshot process.
        /// This method is executed even if NOA Debugger’s screenshot process was not executed.
        /// </summary>
        void OnAfterScreenshot();
    }

    /// <summary>
    /// By inheriting this class and registering it with the public API, screenshot callbacks can be customized.
    /// </summary>
    public class NoaScreenshotCallbacks : INoaScreenshotCallbacks
    {
        /// <summary>
        /// See <see cref="INoaScreenshotCallbacks.IsAllowBaseScreenshot"/> for details.
        /// </summary>
        public virtual bool IsAllowBaseScreenshot => true;

        /// <summary>
        /// See <see cref="INoaScreenshotCallbacks.ScreenshotTarget"/> for details.
        /// </summary>
        public virtual NoaController.ScreenshotTarget ScreenshotTarget => NoaController.ScreenshotTarget.None;

        /// <summary>
        /// See <see cref="INoaScreenshotCallbacks.OnBeforePrepareScreenshot"/> for details.
        /// </summary>
        public virtual void OnBeforePrepareScreenshot() { }

        /// <summary>
        /// See <see cref="INoaScreenshotCallbacks.OnCaptureScreenshot"/> for details.
        /// </summary>
        public virtual void OnCaptureScreenshot() { }

        /// <summary>
        /// See <see cref="INoaScreenshotCallbacks.OnAfterScreenshot"/> for details.
        /// </summary>
        public virtual void OnAfterScreenshot() { }
    }
}
