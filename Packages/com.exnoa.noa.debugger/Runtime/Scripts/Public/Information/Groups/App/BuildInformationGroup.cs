using UnityEngine;

namespace NoaDebugger
{
    /// <summary>
    /// Represents a collection of the build information.
    /// </summary>
    public sealed class BuildInformationGroup
    {
        /// <summary>
        /// Gets application company name.
        /// </summary>
        public string CompanyName { get; }

        /// <summary>
        /// Gets application product name.
        /// </summary>
        public string ProductName { get; }

        /// <summary>
        /// Gets application identifier.
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Gets application version number.
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Gets the version of the Unity runtime used to play the content.
        /// </summary>
        public string UnityVersion { get; }

        /// <summary>
        /// Gets the scripting backend.
        /// </summary>
        public string ScriptingBackend { get; }

        /// <summary>
        /// True if the application is built as a development build; otherwise false.
        /// </summary>
        public bool IsDebugBuild { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildInformationGroup"/>.
        /// </summary>
        internal BuildInformationGroup()
        {
            CompanyName = Application.companyName;
            ProductName = Application.productName;
            Identifier = Application.identifier;
            Version = Application.version;
            UnityVersion = Application.unityVersion;
#if ENABLE_IL2CPP
            ScriptingBackend = "IL2CPP";
#elif ENABLE_MONO
            ScriptingBackend = "Mono";
#else
            ScriptingBackend = "Unknown";
#endif
            IsDebugBuild = Debug.isDebugBuild;
        }
    }
}
