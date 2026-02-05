using System.Collections.Generic;

namespace NoaDebugger
{
    /// <summary>
    /// By implementing this interface and registering it with the public API, download callbacks can be customized.
    /// For users who want a highly flexible implementation with detailed customization.
    /// </summary>
    public interface INoaConsoleLogDownloadCallbacks
    {
        /// <summary>
        /// Called before converting the target log list to JSON text.
        /// The conversion uses the returned list, so you can apply custom filtering here.
        /// </summary>
        /// <param name="logEntries">Input logs to be downloaded.</param>
        /// <returns>Output logs to be downloaded.</returns>
        IReadOnlyList<ConsoleLogEntry> OnBeforeConversion(IReadOnlyList<ConsoleLogEntry> logEntries);
    }

    /// <summary>
    /// By inheriting this class and registering it with the public API, download callbacks can be customized.
    /// For users who want a simple implementation with minimal learning effort.
    /// </summary>
    public class NoaConsoleLogDownloadCallbacks : INoaDownloadCallbacks, INoaConsoleLogDownloadCallbacks
    {
        /// <summary>
        /// See <see cref="INoaDownloadCallbacks.IsAllowBaseDownload"/> for details.
        /// </summary>
        public virtual bool IsAllowBaseDownload => true;

        /// <summary>
        /// See <see cref="INoaConsoleLogDownloadCallbacks.OnBeforeConversion"/> for details.
        /// </summary>
        public virtual IReadOnlyList<ConsoleLogEntry> OnBeforeConversion(IReadOnlyList<ConsoleLogEntry> logEntries)
        {
            return logEntries;
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
