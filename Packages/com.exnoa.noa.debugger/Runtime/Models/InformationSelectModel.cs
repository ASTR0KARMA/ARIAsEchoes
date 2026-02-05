using System.Collections.Generic;
using System;
using System.Linq;

namespace NoaDebugger
{
    sealed class InformationSelectModel : ModelBase
    {
        public Action<string, string, bool> OnSelectChild;

        InformationView.ToggleTabType _currentTabType;
        InformationBaseSelectModel[] _allSelectModels;

        public InformationSelectModel()
        {
            int tabCount = Enum.GetNames(typeof(InformationView.ToggleTabType)).Length;
            _allSelectModels = new InformationBaseSelectModel[tabCount];
            _allSelectModels[(int)InformationView.ToggleTabType.Application] = new InformationApplicationSelectModel();
            _allSelectModels[(int)InformationView.ToggleTabType.Device] = new InformationDeviceSelectModel();
            _allSelectModels[(int)InformationView.ToggleTabType.Custom] = new InformationCustomSelectModel();
            OnSelectChild += _OnSelectChild;
        }

        InformationBaseSelectModel _GetCurrentModel()
        {
            return _allSelectModels[(int)_currentTabType];
        }

        void _OnSelectChild(string groupName, string key, bool isSelect)
        {
            var currentModel = _GetCurrentModel();
            currentModel.OnSelect(groupName, key, isSelect);
        }

        KeyValueSerializer _CreateInformationKeyValueSerializer()
        {
            IKeyValueParser[] tabParser = ExportToAllTabParsers();
            return new KeyValueSerializer("Information", tabParser);
        }

        IKeyValueParser[] ExportToAllTabParsers()
        {
            var allTabInformationGroups = new Dictionary<InformationTabType, IReadOnlyList<InformationGroup>>();
            foreach (var selectModel in _allSelectModels)
            {
                var groups = selectModel.ExportToInformationGroups();
                if (groups.Count > 0)
                {
                    allTabInformationGroups[selectModel.TabType] = groups;
                }
                else
                {
                    allTabInformationGroups[selectModel.TabType] = new List<InformationGroup>();
                }
            }

            if (NoaInformation.InformationDownloadCallbacks != null)
            {
                var downloadInfo = new NoaInformationDownloadInfo(allTabInformationGroups);
                var filteredDownloadInfo = NoaInformation.InformationDownloadCallbacks.OnBeforeConversion(downloadInfo);

                allTabInformationGroups = new Dictionary<InformationTabType, IReadOnlyList<InformationGroup>>(
                    filteredDownloadInfo.InformationGroups);
            }

            var allTabParsers = new List<IKeyValueParser>();
            foreach (var tabEntry in allTabInformationGroups)
            {
                var tabType = tabEntry.Key;
                var groups = tabEntry.Value;

                var selectModel = _allSelectModels.FirstOrDefault(s => s.TabType == tabType);
                if (selectModel == null)
                {
                    continue;
                }

                var tabParser = CreateTabParser(selectModel.TabName, groups);
                if (tabParser != null)
                {
                    allTabParsers.Add(tabParser);
                }
            }

            return allTabParsers.ToArray();
        }

        IKeyValueParser CreateTabParser(string tabName, IReadOnlyList<InformationGroup> groups)
        {
            if (groups.Count == 0)
            {
                return new KeyObjectParser(tabName, Array.Empty<IKeyValueParser>());
            }

            var groupParsers = new List<IKeyValueParser>();

            foreach (var group in groups)
            {
                var itemParsers = new List<KeyValueParser>();

                foreach (var kvp in group.KeyValues)
                {
                    itemParsers.Add(new KeyValueParser(kvp.Key, kvp.Value?.ToString() ?? string.Empty));
                }

                groupParsers.Add(new KeyObjectParser(group.Name, itemParsers.ToArray()));
            }

            return new KeyObjectParser(tabName, groupParsers.ToArray());
        }

        public T GetSelectableModel<T>() where T : InformationBaseSelectModel
        {
            foreach (var model in _allSelectModels)
            {
                if (model is T typedModel)
                {
                    return typedModel;
                }
            }

            return null;
        }

        public void SelectTab(InformationView.ToggleTabType tabType)
        {
            _currentTabType = tabType;
        }

        public void Select(string groupName, string key)
        {
            var currentModel = _GetCurrentModel();
            currentModel.Select(groupName, key);
        }

        public void Deselect(string groupName, string key)
        {
            var currentModel = _GetCurrentModel();
            currentModel.Deselect(groupName, key);
        }

        public void SelectAll()
        {
            var currentModel = _GetCurrentModel();
            currentModel.SelectAll();
        }

        public void DeselectAll()
        {
            var currentModel = _GetCurrentModel();
            currentModel.DeselectAll();
        }

        public string CreateExportJsonString(string label = "")
        {
            List<KeyValueSerializer> exportData = new List<KeyValueSerializer>();
            KeyValueSerializer serializeData = _CreateInformationKeyValueSerializer();
            exportData.Add(serializeData);

            if (!string.IsNullOrEmpty(label))
            {
                exportData.Add(KeyValueSerializer.CreateSubData(label));
            }

            return KeyValueSerializer.SerializeToJson(exportData.ToArray());
        }

        public int TotalSelectCount()
        {
            int count = 0;
            foreach (var selectModel in _allSelectModels)
            {
                count += selectModel.SelectCount();
            }

            return count;
        }

        public Dictionary<string, List<InformationGroup>> CreateExportAllSelectedInformationGroup()
        {
            var allInformationGroups = new Dictionary<string, List<InformationGroup>>();
            foreach (var selectModel in _allSelectModels)
            {
                allInformationGroups[selectModel.TabName] = selectModel.ExportToInformationGroups();
            }

            return allInformationGroups;
        }
    }
}
