namespace NoaDebugger
{
    /// <summary>
    /// By implementing this interface and registering it with the public API, asset unloading callbacks can be customized.
    /// For users who want a highly flexible implementation with detailed customization.
    /// </summary>
    public interface INoaUnloadAssetsCallbacks
    {
        /// <summary>
        /// Whether to execute NOA Debugger’s asset unloading process.
        /// </summary>
        public bool IsAllowBaseUnloadAssets { get; }

        /// <summary>
        /// Called before executing NOA Debugger’s asset unloading.
        /// </summary>
        public void OnBeforeUnloadAssets();

        /// <summary>
        /// Called after the asset unloading process.
        /// This method is executed even if NOA Debugger’s asset unloading process was not executed.
        /// </summary>
        public void OnAfterUnloadAssets();
    }

    /// <summary>
    /// By inheriting this class and registering it with the public API, asset unloading callbacks can be customized.
    /// For users who want a simple implementation with minimal learning effort.
    /// </summary>
    public class NoaUnloadAssetsCallbacks : INoaUnloadAssetsCallbacks
    {
        /// <summary>
        /// See <see cref="INoaUnloadAssetsCallbacks.IsAllowBaseUnloadAssets"/> for details.
        /// </summary>
        public virtual bool IsAllowBaseUnloadAssets => true;

        /// <summary>
        /// See <see cref="INoaUnloadAssetsCallbacks.OnBeforeUnloadAssets"/> for details.
        /// </summary>
        public virtual void OnBeforeUnloadAssets() { }

        /// <summary>
        /// See <see cref="INoaUnloadAssetsCallbacks.OnAfterUnloadAssets"/> for details.
        /// </summary>
        public virtual void OnAfterUnloadAssets() { }
    }
}
