using System.Collections.Generic;

namespace NoaDebugger
{
    /// <summary>
    /// A class that represents a group of system information.
    /// This class is read-only; to modify it, you must create a new InformationGroup instance.
    /// </summary>
    public sealed class InformationGroup
    {
        /// <summary>
        /// The group name (read-only)
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// A read-only Dictionary that holds pairs of system item names and values
        /// </summary>
        public IReadOnlyDictionary<string, object> KeyValues { get; }

        /// <summary>
        /// Constructor that accepts a mutable Dictionary
        /// </summary>
        /// <param name="name">Group name</param>
        /// <param name="keyValues">Pairs of system item names and values</param>
        public InformationGroup(string name, Dictionary<string, object> keyValues)
        {
            Name = name;
            KeyValues = keyValues ?? new Dictionary<string, object>();
        }
    }
}
