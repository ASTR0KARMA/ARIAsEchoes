using System;
using UnityEngine;

namespace NoaDebugger
{
    /// <summary>
    /// Represents a collection of the other information for application.
    /// </summary>
    public sealed class ApplicationOtherInformationGroup
    {
        /// <summary>
        /// Gets the path to a persistent data directory.
        /// </summary>
        public string PersistentDataPath { get; }

        /// <summary>
        /// Get the path to the StreamingAssets folder.
        /// </summary>
        public string SteamingAssetsPath { get; }

        /// <summary>
        /// Gets the path to a temporary data / cache directory.
        /// </summary>
        public string TemporaryCachePath { get; }

        /// <summary>
        /// Gets the path to the game data folder on the target device.
        /// </summary>
        public string DataPath { get; }

        /// <summary>
        /// Gets the name of the store or package that installed the application.
        /// </summary>
        public string InstallerName { get; }

        /// <summary>
        /// Gets application install mode.
        /// </summary>
        public ApplicationInstallMode InstallMode { get; }

        /// <summary>
        /// Gets the string array containing the command-line arguments for the current process.
        /// </summary>
        public string[] CommandLineArgs { get; }

        /// <summary>
        /// Gets the URL of the document. For WebGL, this a web URL.
        /// For Android, iOS, or Universal Windows Platform (UWP) this is a deep link URL.
        /// </summary>
        public string AbsoluteUrl => Application.absoluteURL;

        /// <summary>
        /// Gets the GUID for this build.
        /// </summary>
        public string BuildGuid { get; }

        /// <summary>
        /// Gets the unique cloud project identifier. It is unique for every project.
        /// </summary>
        public string CloudProjectId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationOtherInformationGroup"/>.
        /// </summary>
        internal ApplicationOtherInformationGroup()
        {
            PersistentDataPath = Application.persistentDataPath;
            SteamingAssetsPath = Application.streamingAssetsPath;
            TemporaryCachePath = Application.temporaryCachePath;
            DataPath = Application.dataPath;
            InstallerName = Application.installerName;
            InstallMode = Application.installMode;
            CommandLineArgs = Environment.GetCommandLineArgs();
            BuildGuid = Application.buildGUID;
            CloudProjectId = Application.cloudProjectId;
        }
    }
}
