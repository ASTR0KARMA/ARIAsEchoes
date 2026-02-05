namespace NoaDebugger
{
    /// <summary>
    /// By implementing this interface and registering it with the public API, UI visibility toggle callbacks can be customized.
    /// For users who want a highly flexible implementation with detailed customization.
    /// </summary>
    public interface INoaToggleUICallbacks
    {
        /// <summary>
        /// Whether to execute NOA Debugger’s UI visibility toggle process.
        /// </summary>
        bool IsAllowBaseToggleUI { get; }

        /// <summary>
        /// Called before the NOA Debugger-related UI visibility toggle process.
        /// </summary>
        /// <param name="nextIsVisible">Next, whether the UI is visible.</param>
        void OnBeforeToggleUI(bool nextIsVisible);

        /// <summary>
        /// Called after the NOA Debugger-related UI visibility toggle process.
        /// This method is executed even if the default NOA Debugger UI visibility toggle process was not executed.
        /// </summary>
        /// <param name="isVisible">Whether the UI is now visible.</param>
        void OnAfterToggleUI(bool isVisible);
    }

    /// <summary>
    /// By inheriting this class and registering it with the public API, UI visibility toggle callbacks can be customized.
    /// </summary>
    public class NoaToggleUICallbacks : INoaToggleUICallbacks
    {
        /// <summary>
        /// See <see cref="INoaToggleUICallbacks.IsAllowBaseToggleUI"/> for details.
        /// </summary>
        public virtual bool IsAllowBaseToggleUI => true;

        /// <summary>
        /// See <see cref="INoaToggleUICallbacks.OnBeforeToggleUI"/> for details.
        /// </summary>
        public virtual void OnBeforeToggleUI(bool nextIsVisible) { }

        /// <summary>
        /// See <see cref="INoaToggleUICallbacks.OnAfterToggleUI"/> for details.
        /// </summary>
        public virtual void OnAfterToggleUI(bool isVisible) { }
    }
}

