namespace NoaDebugger
{
    /// <summary>
    /// By implementing this interface and registering it with the public API, garbage collection callbacks can be customized.
    /// For users who want a highly flexible implementation with detailed customization.
    /// </summary>
    public interface INoaGCCollectCallbacks
    {
        /// <summary>
        /// Whether to execute NOA Debugger’s garbage collection process.
        /// </summary>
        public bool IsAllowBaseGCCollect { get; }

        /// <summary>
        /// Called before executing NOA Debugger’s garbage collection.
        /// </summary>
        public void OnBeforeGCCollect();

        /// <summary>
        /// Called after the garbage collection process.
        /// This method is executed even if NOA Debugger’s garbage collection process was not executed.
        /// </summary>
        public void OnAfterGCCollect();
    }

    /// <summary>
    /// By inheriting this class and registering it with the public API, garbage collection callbacks can be customized.
    /// For users who want a simple implementation with minimal learning effort.
    /// </summary>
    public class NoaGCCollectCallbacks : INoaGCCollectCallbacks
    {
        /// <summary>
        /// See <see cref="INoaGCCollectCallbacks.IsAllowBaseGCCollect"/> for details.
        /// </summary>
        public virtual bool IsAllowBaseGCCollect => true;

        /// <summary>
        /// See <see cref="INoaGCCollectCallbacks.OnBeforeGCCollect"/> for details.
        /// </summary>
        public virtual void OnBeforeGCCollect() { }

        /// <summary>
        /// See <see cref="INoaGCCollectCallbacks.OnAfterGCCollect"/> for details.
        /// </summary>
        public virtual void OnAfterGCCollect() { }
    }
}
