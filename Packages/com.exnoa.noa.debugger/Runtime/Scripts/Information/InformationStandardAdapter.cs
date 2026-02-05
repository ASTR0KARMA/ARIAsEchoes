using System.Collections.Generic;
using System.Linq;

namespace NoaDebugger
{
    class StandardGroupProviderAdapter : ISelectableGroupProvider
    {
        readonly List<SelectableKeyValueBase> _groups;

        public StandardGroupProviderAdapter(List<SelectableKeyValueBase> groups)
        {
            _groups = groups ?? new List<SelectableKeyValueBase>();
        }

        public IEnumerable<string> GetGroupNames()
        {
            return _groups
                   .OfType<ISelectableGroupBase>()
                   .Select(g => g.GroupName);
        }

        public bool ContainsGroup(string groupName)
        {
            return _groups
                   .OfType<ISelectableGroupBase>()
                   .Any(g => g.GroupName == groupName);
        }

        public IEnumerable<ISelectableItem> GetSelectableItems(string groupName)
        {
            var group = _groups
                        .OfType<ISelectableGroupBase>()
                        .FirstOrDefault(g => g.GroupName == groupName) as SelectableKeyValueBase;

            if (group == null)
            {
                return Enumerable.Empty<ISelectableItem>();
            }

            return group.Items.Select(item => new SelectableItemAdapter(item));
        }

        public IEnumerable<ISelectableItem> GetSelectedItems(string groupName)
        {
            var group = _groups
                        .OfType<ISelectableGroupBase>()
                        .FirstOrDefault(g => g.GroupName == groupName) as SelectableKeyValueBase;

            if (group == null)
            {
                return Enumerable.Empty<ISelectableItem>();
            }

            return group.GetSelectedItems().Select(item => new SelectableItemAdapter(item));
        }

        public void SetItemSelection(string groupName, string key, bool isSelected)
        {
            var group = _groups
                        .OfType<ISelectableGroupBase>()
                        .FirstOrDefault(g => g.GroupName == groupName) as SelectableKeyValueBase;

            if (group != null)
            {
                group.SetSelection(key, isSelected);
            }
        }

        public void SelectAllInGroup(string groupName)
        {
            var group = _groups
                        .OfType<ISelectableGroupBase>()
                        .FirstOrDefault(g => g.GroupName == groupName) as SelectableKeyValueBase;

            if (group != null)
            {
                group.SelectAll();
            }
        }

        public void DeselectAllInGroup(string groupName)
        {
            var group = _groups
                        .OfType<ISelectableGroupBase>()
                        .FirstOrDefault(g => g.GroupName == groupName) as SelectableKeyValueBase;

            if (group != null)
            {
                group.DeselectAll();
            }
        }

        class SelectableItemAdapter : ISelectableItem
        {
            readonly SelectableKeyValue _original;

            public SelectableItemAdapter(SelectableKeyValue original)
            {
                _original = original;
            }

            public string Key => _original.KeyValuePair.Key;
            public object Value => _original.KeyValuePair.Value;
        }
    }
}
