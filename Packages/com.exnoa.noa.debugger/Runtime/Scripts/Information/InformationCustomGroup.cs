using System.Collections.Generic;
using System.Linq;

namespace NoaDebugger
{
    sealed class InformationCustomGroup
    {
        public string Name { get; }
        public string DisplayName { get; }
        public List<InformationCustomKeyValue> KeyValues { get; private set; } = new List<InformationCustomKeyValue>();
        public int Order { get; }

        public InformationCustomGroup(string name, string displayName, int order)
        {
            Name = name;
            DisplayName = displayName;
            Order = order;
        }

        public void AddKeyValue(InformationCustomKeyValue keyValue)
        {
            KeyValues.Add(keyValue);
            KeyValues = KeyValues.OrderBy(kv => kv.Order).ToList();
        }

        public Dictionary<string, string> GetKeyValuesAsDictionary()
        {
            var keyValues = new Dictionary<string, string>();
            KeyValues.ForEach(kv => keyValues[kv.Key] = kv.Parameter.GetStringValue());

            return keyValues;
        }
    }
}
