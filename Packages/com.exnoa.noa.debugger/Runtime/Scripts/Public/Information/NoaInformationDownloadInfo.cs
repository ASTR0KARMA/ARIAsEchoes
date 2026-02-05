using System.Collections.Generic;

namespace NoaDebugger
{
    /// <summary>
    /// Represents information for Information download.
    /// Holds tab types (Application/Device/Custom) as keys and lists of groups contained in each tab as values.
    /// </summary>
    public struct NoaInformationDownloadInfo
    {
        /// <summary>
        /// Read-only dictionary that holds pairs of tab types and lists of InformationGroups.
        /// Key: Tab type (App, Device, Custom)
        /// Value: List of information groups belonging to that tab
        /// </summary>
        public IReadOnlyDictionary<InformationTabType, IReadOnlyList<InformationGroup>> InformationGroups;

        /// <summary>
        /// Constructor that creates an instance from a mutable dictionary
        /// </summary>
        /// <param name="informationGroups">Dictionary of tab types and their corresponding group lists</param>
        public NoaInformationDownloadInfo(Dictionary<InformationTabType, IReadOnlyList<InformationGroup>> informationGroups)
        {
            InformationGroups = informationGroups ?? new Dictionary<InformationTabType, IReadOnlyList<InformationGroup>>();
        }
    }
}
