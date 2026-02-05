using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

namespace NoaDebugger
{
    abstract class InformationBaseSelectModel : ModelBase, IInformationSelectable
    {
        protected abstract ISelectableGroupProvider GroupProvider { get; }

        public abstract string TabName { get; }

        public InformationTabType TabType { get; }

        protected InformationBaseSelectModel(InformationTabType tabType)
        {
            TabType = tabType;
        }

        string ConvertToString(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var type = value.GetType();
            var interfaces = type.GetInterfaces();

            foreach (var interfaceType in interfaces)
            {
                if (interfaceType.IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() == typeof(IMutableParameter<>))
                {
                    var valueProperty = type.GetProperty("Value");
                    if (valueProperty != null)
                    {
                        var actualValue = valueProperty.GetValue(value);
                        return actualValue?.ToString() ?? string.Empty;
                    }
                }
            }

            return value.ToString() ?? string.Empty;
        }

        public void OnSelect(string groupName, string key, bool isSelect)
        {
            if (isSelect)
            {
                Select(groupName, key);
            }
            else
            {
                Deselect(groupName, key);
            }
        }

        public void Select(string groupName, string key)
        {
            if (GroupProvider.ContainsGroup(groupName))
            {
                GroupProvider.SetItemSelection(groupName, key, true);
            }
        }

        public void Deselect(string groupName, string key)
        {
            if (GroupProvider.ContainsGroup(groupName))
            {
                GroupProvider.SetItemSelection(groupName, key, false);
            }
        }

        public int SelectCount()
        {
            int count = 0;

            foreach (var groupName in GroupProvider.GetGroupNames())
            {
                count += GroupProvider.GetSelectedItems(groupName).Count();
            }

            return count;
        }

        public void SelectAll()
        {
            foreach (var groupName in GroupProvider.GetGroupNames())
            {
                GroupProvider.SelectAllInGroup(groupName);
            }
        }

        public void DeselectAll()
        {
            foreach (var groupName in GroupProvider.GetGroupNames())
            {
                GroupProvider.DeselectAllInGroup(groupName);
            }
        }

        public List<InformationGroup> ExportToInformationGroups()
        {
            var informationGroups = new List<InformationGroup>();

            foreach (var groupName in GroupProvider.GetGroupNames())
            {
                var selectedItems = GroupProvider.GetSelectedItems(groupName);

                if (selectedItems.Any())
                {
                    var keyValues = new Dictionary<string, object>();

                    foreach (var item in selectedItems)
                    {
                        keyValues[item.Key] = ConvertToString(item.Value);
                    }

                    informationGroups.Add(new InformationGroup(groupName, keyValues));
                }
            }

            return informationGroups;
        }
    }

    interface IInformationSelectable
    {
        void OnSelect(string groupName, string key, bool isSelect);

        void Select(string groupName, string key);

        void Deselect(string groupName, string key);

        int SelectCount();

        void SelectAll();

        void DeselectAll();
    }

    interface ISelectableGroupProvider
    {
        IEnumerable<string> GetGroupNames();

        bool ContainsGroup(string groupName);

        IEnumerable<ISelectableItem> GetSelectedItems(string groupName);

        void SetItemSelection(string groupName, string key, bool isSelected);

        void SelectAllInGroup(string groupName);

        void DeselectAllInGroup(string groupName);
    }

    interface ISelectableItem
    {
        string Key { get; }
        object Value { get; }
    }
}
