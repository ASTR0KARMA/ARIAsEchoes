using UnityEngine;

namespace NoaDebugger
{
    /// <summary>
    /// Represents a collection of the network information.
    /// </summary>
    public sealed class NetworkInformationGroup
    {
        /// <summary>
        /// Gets the type of internet reachability currently possible on the device.
        /// </summary>
        public NetworkReachability Reachability => Application.internetReachability;
    }
}
