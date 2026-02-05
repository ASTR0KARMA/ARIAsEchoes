namespace NoaDebugger
{
    /// <summary>
    /// By implementing this interface and registering it with the public API, application reset callbacks can be customized.
    /// For users who want a highly flexible implementation with detailed customization.
    /// </summary>
    public interface INoaApplicationResetCallbacks
    {
        /// <summary>
        /// Whether to execute NOA Debugger’s application reset process.
        /// </summary>
        bool IsAllowBaseApplicationReset { get; }

        /// <summary>
        /// Called before the application reset process.
        /// </summary>
        void OnBeforeApplicationReset();

        /// <summary>
        /// Called after the application reset process.
        /// This method is executed even if NOA Debugger’s application reset process was not executed.
        /// </summary>
        void OnAfterApplicationReset();
    }

    /// <summary>
    /// By inheriting this class and registering it with the public API, application reset callbacks can be customized.
    /// </summary>
    public class NoaApplicationResetCallbacks : INoaApplicationResetCallbacks
    {
        /// <summary>
        /// See <see cref="INoaApplicationResetCallbacks.IsAllowBaseApplicationReset"/> for details.
        /// </summary>
        public virtual bool IsAllowBaseApplicationReset => true;

        /// <summary>
        /// See <see cref="INoaApplicationResetCallbacks.OnBeforeApplicationReset"/> for details.
        /// </summary>
        public virtual void OnBeforeApplicationReset() { }

        /// <summary>
        /// See <see cref="INoaApplicationResetCallbacks.OnAfterApplicationReset"/> for details.
        /// </summary>
        public virtual void OnAfterApplicationReset() { }
    }
}

