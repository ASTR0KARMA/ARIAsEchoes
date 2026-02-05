namespace NoaDebugger
{
    /// <summary>
    /// Post-download status.
    /// </summary>
    public enum NoaDownloadStatus
    {
        Succeeded,
        Failed,

        /// <summary>
        /// Not executed NOA Debugger’s download process.
        /// </summary>
        NotExecuted,
    }

    /// <summary>
    /// Editable download-related information.
    /// </summary>
    public struct NoaDownloadInfo
    {
        public string FileName;
        public string Json;
    }

    /// <summary>
    /// By implementing this interface and registering it with the public API, download callbacks can be customized.
    /// For users who want a highly flexible implementation with detailed customization.
    /// </summary>
    public interface INoaDownloadCallbacks
    {
        /// <summary>
        /// Whether to execute NOA Debugger’s download process.
        /// </summary>
        public bool IsAllowBaseDownload { get; }

        /// <summary>
        /// Called before executing NOA Debugger’s download.
        /// The returned information is applied to the download, so you can edit it here.
        /// </summary>
        /// <param name="info">Input download-related information.</param>
        /// <returns>Output download-related information.</returns>
        public NoaDownloadInfo OnBeforeDownload(NoaDownloadInfo info);

        /// <summary>
        /// Called after the download process.
        /// This method is executed even if NOA Debugger’s download process was not executed.
        /// </summary>
        /// <param name="info">Download-related information.</param>
        /// <param name="status">Post-download status.</param>
        public void OnAfterDownload(NoaDownloadInfo info, NoaDownloadStatus status);
    }

    /// <summary>
    /// By inheriting this class and registering it with the public API, download callbacks can be customized.
    /// For users who want a simple implementation with minimal learning effort.
    /// </summary>
    public class NoaDownloadCallbacks : INoaDownloadCallbacks
    {

        /// <summary>
        /// See <see cref="INoaDownloadCallbacks.IsAllowBaseDownload"/> for details.
        /// </summary>
        public virtual bool IsAllowBaseDownload => true;

        /// <summary>
        /// See <see cref="INoaDownloadCallbacks.OnBeforeDownload"/> for details.
        /// </summary>
        public virtual NoaDownloadInfo OnBeforeDownload(NoaDownloadInfo info)
        {
            return info;
        }

        /// <summary>
        /// See <see cref="INoaDownloadCallbacks.OnAfterDownload"/> for details.
        /// </summary>
        public virtual void OnAfterDownload(NoaDownloadInfo info, NoaDownloadStatus status) { }
    }
}
