using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace NoaDebugger
{
    /// <summary>
    /// You can get the information of the Information function through this class
    /// </summary>
    public class NoaInformation
    {
        /// <summary>
        /// Returns the application information it holds
        /// </summary>
        public static ApplicationInformation ApplicationInformation
        {
            get
            {
                var presenter = NoaDebugger.GetPresenter<InformationPresenter>();

                return presenter == null ? null : presenter.CreateApplicationInformation();
            }
        }

        /// <summary>
        /// Returns the device information it holds
        /// </summary>
        public static DeviceInformation DeviceInformation
        {
            get
            {
                var presenter = NoaDebugger.GetPresenter<InformationPresenter>();

                return presenter == null ? null : presenter.CreateDeviceInformation();
            }
        }

        /// <summary>
        /// Returns the system information it holds
        /// </summary>
        [Obsolete("Use the 'NoaDebugger.ApplicationInformation' or 'NoaDebugger.DeviceInformation' classes instead.")]
        public static SystemInformation SystemInformation
        {
            get
            {
                var presenter = NoaDebugger.GetPresenter<InformationPresenter>();

                return presenter == null ? null : presenter.CreateSystemInformation();
            }
        }

        /// <summary>
        /// Returns the Unity information it holds
        /// </summary>
        [Obsolete("Use the 'NoaDebugger.ApplicationInformation' or 'NoaDebugger.DeviceInformation' classes instead.")]
        public static UnityInformation UnityInformation
        {
            get
            {
                var presenter = NoaDebugger.GetPresenter<InformationPresenter>();

                return presenter == null ? null : presenter.CreateUnityInformation();
            }
        }

        /// <summary>
        /// Registers a custom download callback.
        /// If the argument is null, unregisters the corresponding callback.
        /// </summary>
        /// <param name="commonCallbacks">A class implementing INoaDownloadCallbacks or derived from NoaDownloadCallbacks</param>
        /// <param name="informationCallbacks">A class implementing INoaInformationDownloadCallbacks or derived from NoaInformationDownloadCallbacks</param>
        public static void SetDownloadCallbacks(INoaDownloadCallbacks commonCallbacks, INoaInformationDownloadCallbacks informationCallbacks)
        {
            DownloadCallbacks = commonCallbacks;
            InformationDownloadCallbacks = informationCallbacks;
        }

        internal static INoaDownloadCallbacks DownloadCallbacks { get; private set; } = null;

        internal static INoaInformationDownloadCallbacks InformationDownloadCallbacks { get; private set; } = null;

        /// <summary>
        /// Event triggered when logs are downloaded.
        /// Users can register their custom actions to be executed upon log download.
        /// The event provides the filename and the JSON data as strings.
        /// If the event handler returns true, the logs will be downloaded locally.
        /// If the event handler returns false, the logs will not be downloaded locally.
        /// </summary>
        [Obsolete("Inherit from NoaDownloadCallbacks and override OnBeforeDownload instead.")]
        public static Func<string, string, bool> OnDownload
        {
            get
            {
                var presenter = NoaDebugger.GetPresenter<InformationPresenter>();

                return presenter == null ? null : presenter._onDownload;
            }
            set
            {
                var presenter = NoaDebugger.GetPresenter<InformationPresenter>();

                if (presenter == null)
                {
                    return;
                }

                if (value == null)
                {
                    presenter._onDownload = null;
                }
                else
                {
                    presenter._onDownload += value;
                }
            }
        }

        /// <summary>
        /// Users can register custom actions to be executed when the information copy feature is initiated.
        /// The event provides a dictionary of selected information groups organized by tab name.
        /// Dictionary key: Tab name (e.g., "App", "Device", "Custom")
        /// Dictionary value: List of selected information groups
        /// </summary>
        public static UnityAction<Dictionary<string,List<InformationGroup>>,string> OnCopied
        {
            get
            {
                var presenter = NoaDebugger.GetPresenter<InformationPresenter>();
                return presenter != null ? presenter._onCopied : null;
            }
            set
            {
                var presenter = NoaDebugger.GetPresenter<InformationPresenter>();

                if (presenter == null)
                {
                    return;
                }

                if (value == null)
                {
                    presenter._onCopied = null;
                }
                else
                {
                    presenter._onCopied += value;
                }
            }
        }

        /// <summary>
        /// Users can register custom actions to be executed when the information send feature is initiated.
        /// The event provides a dictionary of selected information groups organized by tab name.
        /// Dictionary key: Tab name (e.g., "App", "Device", "Custom")
        /// Dictionary value: List of selected information groups
        /// </summary>
        public static UnityAction<Dictionary<string,List<InformationGroup>>> OnSend
        {
            get
            {
                var presenter = NoaDebugger.GetPresenter<InformationPresenter>();
                return presenter != null ? presenter._onSend : null;
            }
            set
            {
                var presenter = NoaDebugger.GetPresenter<InformationPresenter>();

                if (presenter == null)
                {
                    return;
                }

                if (value == null)
                {
                    presenter._onSend = null;
                }
                else
                {
                    presenter._onSend += value;
                }
            }
        }
    }
}
