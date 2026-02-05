using System.Collections.Generic;
using System.Linq;

namespace NoaDebugger
{
    class InformationCustomProviderAdapter : ISelectableGroupProvider
    {
        readonly List<InformationCustomGroup> _groups;

        public InformationCustomProviderAdapter(List<InformationCustomGroup> groups)
        {
            _groups = groups ?? new List<InformationCustomGroup>();
        }

        public IEnumerable<string> GetGroupNames()
        {
            return _groups.Select(g => g.Name);
        }

        public bool ContainsGroup(string groupName)
        {
            return _groups.Any(g => g.Name == groupName);
        }

        public IEnumerable<ISelectableItem> GetSelectedItems(string groupName)
        {
            var group = _groups.FirstOrDefault(g => g.Name == groupName);
            if (group == null)
            {
                return Enumerable.Empty<ISelectableItem>();
            }

            return group.KeyValues
                .Where(kv => kv.IsSelect)
                .Select(kv => new CustomSelectableItemAdapter(kv));
        }

        public void SetItemSelection(string groupName, string key, bool isSelected)
        {
            var group = _groups.FirstOrDefault(g => g.Name == groupName);
            if (group != null)
            {
                var item = group.KeyValues.FirstOrDefault(kv => kv.Key == key);
                if (item != null)
                {
                    item.IsSelect = isSelected;
                }
            }
        }

        public void SelectAllInGroup(string groupName)
        {
            var group = _groups.FirstOrDefault(g => g.Name == groupName);
            if (group != null)
            {
                foreach (var item in group.KeyValues)
                {
                    item.IsSelect = true;
                }
            }
        }

        public void DeselectAllInGroup(string groupName)
        {
            var group = _groups.FirstOrDefault(g => g.Name == groupName);
            if (group != null)
            {
                foreach (var item in group.KeyValues)
                {
                    item.IsSelect = false;
                }
            }
        }

        class CustomSelectableItemAdapter : ISelectableItem
        {
            readonly InformationCustomKeyValue _original;

            public CustomSelectableItemAdapter(InformationCustomKeyValue original)
            {
                _original = original;
            }

            public string Key => _original.Key;
            public object Value => _original.Parameter?.GetStringValue() ?? string.Empty;
        }
    }
}
