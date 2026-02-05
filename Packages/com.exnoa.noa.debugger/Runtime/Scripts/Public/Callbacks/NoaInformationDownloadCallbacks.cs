namespace NoaDebugger
{
    /// <summary>
    /// By implementing this interface and registering it with the public API, Information download callbacks can be customized.
    /// For users who want a highly flexible implementation with detailed customization.
    /// </summary>
    public interface INoaInformationDownloadCallbacks
    {
        /// <summary>
        /// Called before converting the download target data to a string.
        /// The conversion uses the returned NoaInformationDownloadInfo, so you can apply custom filtering here.
        /// </summary>
        /// <param name="downloadInfo">Input Information data to be downloaded.</param>
        /// <returns>Output Information data to be downloaded.</returns>
        NoaInformationDownloadInfo OnBeforeConversion(NoaInformationDownloadInfo downloadInfo);
    }

    /// <summary>
    /// By inheriting this class and registering it with the public API, Information download callbacks can be customized.
    /// For users who want a simple implementation with minimal learning effort.
    /// </summary>
    public class NoaInformationDownloadCallbacks : INoaDownloadCallbacks, INoaInformationDownloadCallbacks
    {
        /// <summary>
        /// See <see cref="INoaDownloadCallbacks.IsAllowBaseDownload"/> for details.
        /// </summary>
        public virtual bool IsAllowBaseDownload => true;

        /// <summary>
        /// See <see cref="INoaInformationDownloadCallbacks.OnBeforeConversion"/> for details.
        /// </summary>
        public virtual NoaInformationDownloadInfo OnBeforeConversion(NoaInformationDownloadInfo downloadInfo)
        {
            return downloadInfo;
        }

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
