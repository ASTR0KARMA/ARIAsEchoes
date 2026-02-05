using System.Collections.Generic;

namespace NoaDebugger
{
    /// <summary>
    /// Custom information group
    /// </summary>
    public class NoaCustomInformationGroup
    {
        /// <summary>
        /// group name
        /// </summary>
        public string Name { get;}

        /// <summary>
        /// int key values
        /// </summary>
        public Dictionary<string, NoaCustomInformationIntValue> IntKeyValues { get; }

        /// <summary>
        /// float key values
        /// </summary>
        public Dictionary<string, NoaCustomInformationFloatValue> FloatKeyValues { get; }

        /// <summary>
        /// string key values
        /// </summary>
        public Dictionary<string, NoaCustomInformationStringValue> StringKeyValues { get; }

        /// <summary>
        /// bool key values
        /// </summary>
        public Dictionary<string, NoaCustomInformationBoolValue> BoolKeyValues { get; }

        /// <summary>
        /// Enum key values
        /// </summary>
        public Dictionary<string, NoaCustomInformationEnumValue> EnumKeyValues { get; }

        /// <summary>
        /// key values
        /// </summary>
        public Dictionary<string, string> KeyValues { get;}

        /// <summary>
        /// constructors
        /// </summary>
        internal NoaCustomInformationGroup(string groupName, InformationCustomGroup group)
        {
            Name = groupName;
            KeyValues = group.GetKeyValuesAsDictionary();
            IntKeyValues = new Dictionary<string, NoaCustomInformationIntValue>();
            FloatKeyValues = new Dictionary<string, NoaCustomInformationFloatValue>();
            StringKeyValues = new Dictionary<string, NoaCustomInformationStringValue>();
            BoolKeyValues = new Dictionary<string, NoaCustomInformationBoolValue>();
            EnumKeyValues = new Dictionary<string, NoaCustomInformationEnumValue>();

            foreach (var keyValue in group.KeyValues)
            {
                var visitor = new NoaCustomInformationGroupVisitor(
                    IntKeyValues,
                    FloatKeyValues,
                    StringKeyValues,
                    BoolKeyValues,
                    EnumKeyValues,
                    keyValue.Key
                );
                keyValue.Parameter.Accept(visitor);
            }
        }
    }
}
