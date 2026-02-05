using System;
using System.Linq;
using System.Collections.Generic;

namespace NoaDebugger
{
    sealed class CustomInformationModel : ModelBase
    {
        List<InformationCustomGroup> _allGroups;

        public CustomInformationModel()
        {
            _allGroups = new List<InformationCustomGroup>();
        }

        public void AddGroup(string groupName, string displayName, int order = Int32.MaxValue)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new ArgumentException($"groupName is null");
            }

            if (_ExistsGroup(groupName))
            {
                throw new Exception($"Duplicate Key Exception groupName: {groupName}");
            }

            _AddGroup(groupName, displayName, order);
        }

        bool _ExistsGroup(string name)
        {
            return _allGroups.Exists(g => g.Name == name);
        }

        void _AddGroup(string name, string displayName, int order = Int32.MaxValue)
        {
            if (string.IsNullOrEmpty(name) || _ExistsGroup(name))
            {
                return;
            }

            var groupInfo = new InformationCustomGroup(
                name,
                string.IsNullOrEmpty(displayName) ? name : displayName,
                order);

            _allGroups.Add(groupInfo);
            _allGroups = _allGroups.OrderBy(g => g.Order).ToList();
        }

        bool _ExistsKey(string groupName, string keyName)
        {
            var groupInfo = GetGroup(groupName);
            if (groupInfo == null)
            {
                return false;
            }

            var keyValues = groupInfo.KeyValues;
            var keyValue = keyValues.FirstOrDefault(kv => kv.Key == keyName);
            return keyValue != null;
        }

        public NoaCustomInformationIntValue GetIntValue(string groupName, string keyName)
        {
            var groupInfo = GetGroup(groupName);
            if (groupInfo == null)
            {
                return null;
            }

            var keyValues = groupInfo.KeyValues;
            var keyValue = keyValues.FirstOrDefault(kv => kv.Key == keyName);
            if (keyValue != null)
            {
                if (keyValue.Parameter is IntCustomInformationParameter parameter)
                {
                    return new NoaCustomInformationIntValue(
                        () => parameter.Value,
                        (value) => parameter.ChangeValue(value)
                    );
                }
                if (keyValue.Parameter is IntImmutableCustomInformationParameter immutableParameter)
                {
                    return new NoaCustomInformationIntValue(
                        () => immutableParameter.Value,
                        null
                    );
                }
            }

            return null;
        }

        public NoaCustomInformationFloatValue GetFloatValue(string groupName, string keyName)
        {
            var groupInfo = GetGroup(groupName);
            if (groupInfo == null)
            {
                return null;
            }

            var keyValues = groupInfo.KeyValues;
            var keyValue = keyValues.FirstOrDefault(kv => kv.Key == keyName);
            if (keyValue != null)
            {
                if (keyValue.Parameter is FloatCustomInformationParameter floatParameter)
                {
                    return new NoaCustomInformationFloatValue(
                        () => floatParameter.Value,
                        (value) => floatParameter.ChangeValue(value)
                    );
                }
                if (keyValue.Parameter is FloatImmutableCustomInformationParameter immutableParameter)
                {
                    return new NoaCustomInformationFloatValue(
                        () => immutableParameter.Value,
                        null
                    );
                }
            }

            return null;
        }

        public NoaCustomInformationBoolValue GetBoolValue(string groupName, string keyName)
        {
            var groupInfo = GetGroup(groupName);
            if (groupInfo == null)
            {
                return null;
            }

            var keyValues = groupInfo.KeyValues;
            var keyValue = keyValues.FirstOrDefault(kv => kv.Key == keyName);
            if (keyValue != null)
            {
                if (keyValue.Parameter is BoolCustomInformationParameter boolParameter)
                {
                    return new NoaCustomInformationBoolValue(
                        () => boolParameter.Value,
                        (value) => boolParameter.ChangeValue(value)
                    );
                }
                if (keyValue.Parameter is BoolImmutableCustomInformationParameter immutableParameter)
                {
                    return new NoaCustomInformationBoolValue(
                        () => immutableParameter.Value,
                        null
                    );
                }
            }

            return null;
        }

        public NoaCustomInformationStringValue GetStringValue(string groupName, string keyName)
        {
            var groupInfo = GetGroup(groupName);
            if (groupInfo == null)
            {
                return null;
            }

            var keyValues = groupInfo.KeyValues;
            var keyValue = keyValues.FirstOrDefault(kv => kv.Key == keyName);
            if (keyValue != null)
            {
                if (keyValue.Parameter is StringCustomInformationParameter stringParameter)
                {
                    return new NoaCustomInformationStringValue(
                        () => stringParameter.Value,
                        (value) => stringParameter.ChangeValue(value)
                    );
                }
                if (keyValue.Parameter is StringImmutableCustomInformationParameter immutableParameter)
                {
                    return new NoaCustomInformationStringValue(
                        () => immutableParameter.Value,
                        null
                    );
                }
            }

            return null;
        }

        public NoaCustomInformationEnumValue GetEnumValue(string groupName, string keyName)
        {
            var groupInfo = GetGroup(groupName);
            if (groupInfo == null)
            {
                return null;
            }

            var keyValues = groupInfo.KeyValues;
            var keyValue = keyValues.FirstOrDefault(kv => kv.Key == keyName);
            if (keyValue != null)
            {
                if (keyValue.Parameter is EnumCustomInformationParameter enumParameter)
                {
                    return new NoaCustomInformationEnumValue(
                        () => enumParameter.Value,
                        (value) => enumParameter.ChangeValue(value)
                    );
                }
                if (keyValue.Parameter is EnumImmutableCustomInformationParameter immutableParameter)
                {
                    return new NoaCustomInformationEnumValue(
                        () => immutableParameter.Value,
                        null
                    );
                }
            }

            return null;
        }

        public void AddKeyValues(string groupName, InformationCustomKeyValue keyValue)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new ArgumentException("groupName is null");
            }

            if (string.IsNullOrEmpty(keyValue.Key))
            {
                throw new ArgumentException("keyName is null");
            }

            if (_ExistsKey(groupName, keyValue.Key))
            {
                throw new Exception($"Duplicate Key Exception keyName: {keyValue.Key}");
            }

            InformationCustomGroup groupInfo = _GetOrCreateGroup(groupName, groupName);
            _AddKeyValues(groupInfo, keyValue);
        }

        void _AddKeyValues(InformationCustomGroup groupInfo, InformationCustomKeyValue keyValue)
        {
            if (_ExistsKey(groupInfo.Name, keyValue.Key))
            {
                return;
            }

            groupInfo.AddKeyValue(keyValue);
        }

        InformationCustomGroup _GetOrCreateGroup(string name, string displayName = "", int order = Int32.MaxValue)
        {
            if (!_ExistsGroup(name))
            {
                _AddGroup(name, displayName, order);
            }

            return GetGroup(name);
        }

        public InformationCustomGroup GetGroup(string name)
        {
            return _allGroups.FirstOrDefault(g => g.Name == name);
        }

        public List<InformationCustomGroup> GetAllGroups()
        {
            return _allGroups;
        }

        public void RemoveKeyValue(string groupName, string keyName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new ArgumentException("groupName is null");
            }

            if (string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentException("keyName is null");
            }

            var groupInfo = GetGroup(groupName);
            if (groupInfo == null)
            {
                return;
            }

            groupInfo.KeyValues.RemoveAll(kv => kv.Key == keyName);
        }

        public void RemoveGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new ArgumentException("groupName is null");
            }

            _allGroups.RemoveAll(g => g.Name == groupName);
        }

        public void RemoveAll()
        {
            _allGroups.Clear();
        }

        public IKeyValueParser[] CreateExportData()
        {
            List<IKeyValueParser> parsers = new List<IKeyValueParser>();

            foreach (var groupInfo in _allGroups.OrderBy(g => g.Order))
            {
                var keyValueParsers = new List<KeyValueParser>();

                foreach (var keyValue in groupInfo.KeyValues.OrderBy(kv => kv.Order))
                {
                    keyValueParsers.Add(new KeyValueParser(keyValue.Key, keyValue.Parameter.GetStringValue()));
                }

                parsers.Add(new KeyObjectParser(groupInfo.Name, keyValueParsers.ToArray()));
            }

            return parsers.ToArray();
        }
    }
}
