using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NoaDebugger.DebugCommand
{
    sealed class CommandCollapsedStatusSaveData
    {
        sealed class CollapsedStatusCategoryArray
        {
            public string[] _collapsedStatusJson;
        }

        sealed class CollapsedStatusCategory
        {
            public string _categoryName;

            public string[] _collapseChangedGroupsJson;
        }

        sealed class CollapsedStatus
        {
            public string _groupName;

            public bool _isCollapsed;
        }

        public string _prefsKey;
        public Dictionary<string, Dictionary<string, bool>> _status = new();

        public CommandCollapsedStatusSaveData(string prefsKey)
        {
            _prefsKey = prefsKey;

            _Load();
        }

        void _Load()
        {
            string json = NoaDebuggerPrefs.GetString(_prefsKey, "");
            CollapsedStatusCategoryArray categories = JsonUtility.FromJson<CollapsedStatusCategoryArray>(json);
            if (categories == null)
            {
                return;
            }

            _status = new Dictionary<string, Dictionary<string, bool>>();
            foreach (string categoryJson in categories._collapsedStatusJson)
            {
                CollapsedStatusCategory category = JsonUtility.FromJson<CollapsedStatusCategory>(categoryJson);

                var groupDic = new Dictionary<string, bool>();
                foreach (string groupJson in category._collapseChangedGroupsJson)
                {
                    CollapsedStatus status = JsonUtility.FromJson<CollapsedStatus>(groupJson);
                    groupDic.Add(status._groupName, status._isCollapsed);
                }
                _status.Add(category._categoryName, groupDic);
            }
        }

        public void Save()
        {
            CollapsedStatusCategoryArray categories = new CollapsedStatusCategoryArray();
            categories._collapsedStatusJson = _status
                .Where(target => target.Value.Count > 0)
                .Select(target =>
                {
                    string[] groups = target.Value
                        .Select(group =>
                        {
                            var status = new CollapsedStatus()
                            {
                                _groupName = group.Key,
                                _isCollapsed = group.Value
                            };

                            return JsonUtility.ToJson(status);
                        })
                        .ToArray();

                    var category = new CollapsedStatusCategory()
                    {
                        _categoryName = target.Key,
                        _collapseChangedGroupsJson = groups
                    };

                    return JsonUtility.ToJson(category);
                })
                .ToArray();

            string json = JsonUtility.ToJson(categories);
            NoaDebuggerPrefs.SetString(_prefsKey, json);
        }

        public void UpdateStatus(string categoryName, string groupName, bool isCollapsed)
        {
            if (!_status.ContainsKey(categoryName))
            {
                var groupDic = new Dictionary<string, bool>()
                {
                    { groupName, isCollapsed }
                };
                _status.Add(categoryName, groupDic);
            }
            else if (!_status[categoryName].ContainsKey(groupName))
            {
                _status[categoryName].Add(groupName, isCollapsed);
            }
            else
            {
                _status[categoryName][groupName] = isCollapsed;
            }
        }

        public bool GetIsCollapsed(string categoryName, string groupName, bool isCollapsedDefault)
        {
            if (!_status.ContainsKey(categoryName) || !_status[categoryName].ContainsKey(groupName))
            {
                return isCollapsedDefault;
            }

            if (isCollapsedDefault == _status[categoryName][groupName])
            {
                _status[categoryName].Remove(groupName);
                return isCollapsedDefault;
            }

            return _status[categoryName][groupName];
        }
    }
}
