using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NoaDebugger
{
    abstract class InformationCategoryView<TLinker> : ViewBase<TLinker> where TLinker : InformationCommonViewLinker
    {
        [SerializeField]
        ParameterGroupViewFactory _viewFactory;

        protected readonly Dictionary<string, ParameterGroupView> _groupViews = new();
        ParameterGroupView _lastAddedGroup = null;

        protected Action<string,bool> _onSelectGroup;
        protected Action<string,string,bool> _onSelectChild;

        protected override void _OnShow(TLinker linker)
        {
            _onSelectGroup = linker._onSelectGroup;
            _onSelectChild = linker._onSelectChild;
            foreach (var groupView in _groupViews)
            {
                groupView.Value.SetPanelSelectable(linker._isSelection);
            }
        }

        protected void SetReadOnly(string groupName, string propertyName, object value)
        {
            ParameterGroupView groupView = GetGroupView(groupName);
            groupView.SetReadOnly(propertyName, value, _viewFactory.ReadOnlySettingsPanel);
        }

        protected void SetBool(string groupName, string propertyName, IMutableParameter<bool> value)
        {
            ParameterGroupView groupView = GetGroupView(groupName);
            groupView.SetBool(propertyName, value, _viewFactory.BoolSettingsPanel);
        }

        protected void SetInt(string groupName, string propertyName, IMutableParameter<int> value)
        {
            ParameterGroupView groupView = GetGroupView(groupName);
            groupView.SetInt(propertyName, value, _viewFactory.IntSettingsPanel);
        }

        protected void SetFloat(string groupName, string propertyName, IMutableParameter<float> value)
        {
            ParameterGroupView groupView = GetGroupView(groupName);
            groupView.SetFloat(propertyName, value, _viewFactory.FloatSettingsPanel);
        }

        protected void SetEnum(string groupName, string propertyName, IMutableParameter<Enum> value)
        {
            ParameterGroupView groupView = GetGroupView(groupName);
            groupView.SetEnum(propertyName, value, _viewFactory.EnumSettingsPanel);
        }

        protected void SetString(string groupName, string propertyName, IMutableParameter<string> value)
        {
            ParameterGroupView groupView = GetGroupView(groupName);
            groupView.SetString(propertyName, value, _viewFactory.StringSettingsPanel);
        }

        protected void SetValueByType(string groupName, string propertyName, object value)
        {
            if (value == null)
            {
                SetReadOnly(groupName, propertyName, "null");
                return;
            }

            if (value is IMutableParameter<float> floatParam)
            {
                SetFloat(groupName, propertyName, floatParam);
            }
            else if (value is IMutableParameter<int> intParam)
            {
                SetInt(groupName, propertyName, intParam);
            }
            else if (value is IMutableParameter<bool> boolParam)
            {
                SetBool(groupName, propertyName, boolParam);
            }
            else if (value is IMutableParameter<Enum> enumParam)
            {
                SetEnum(groupName, propertyName, enumParam);
            }
            else if (value is string || value is int || value is float || value is bool || value is Enum)
            {
                SetReadOnly(groupName, propertyName, value);
            }
            else
            {
                SetReadOnly(groupName, propertyName, value.ToString());
            }
        }

        protected void InitGroupPanel(string groupName, bool isSelect, bool isSelection)
        {
            ParameterGroupView groupView = GetGroupView(groupName);
            groupView.InitGroupPanel(isSelect, isSelection);
        }

        protected void InitChildPanel(string groupName, string propertyName, bool isSelect, bool isSelection)
        {
            ParameterGroupView groupView = GetGroupView(groupName);
            groupView.InitChildPanel(propertyName, isSelect, isSelection);
        }

        protected void SetGroupCommon<T>(string groupName, T selectableData, bool isSelection)
            where T : SelectableKeyValueBase
        {
            foreach (var info in selectableData.Items)
            {
                var keyValue = info.KeyValuePair;
                SetValueByType(groupName, keyValue.Key, keyValue.Value);

                var isSelect = isSelection && info.IsSelect;
                InitChildPanel(groupName, keyValue.Key, isSelect, isSelection);
            }

            bool isChildSelected = isSelection && selectableData.Items.Any(item => item.IsSelect);
            InitGroupPanel(groupName, isChildSelected, isSelection);
        }

        protected override void _OnHide()
        {
            gameObject.SetActive(false);
        }

        ParameterGroupView GetGroupView(string groupName)
        {
            if (_groupViews.TryGetValue(groupName, out ParameterGroupView groupView))
            {
                return groupView;
            }

            if (_lastAddedGroup != null)
            {
                _lastAddedGroup.ShowRuler(true);
            }

            groupView = _viewFactory.CreateGroupView(groupName);
            groupView.ShowRuler(false);
            _groupViews[groupName] = groupView;
            _lastAddedGroup = groupView;

            var panelGroup = groupView.GetComponent<SelectPanelGroup>();
            if (panelGroup != null)
            {
                panelGroup.OnSelect = (isSelect) => _OnSelectGroup(groupName, isSelect);
                panelGroup.OnSelectChild = (key, isSelect) =>  _OnSelectChild(groupName, key, isSelect);
            }

            return groupView;
        }

        void _OnSelectGroup(string groupName, bool isSelect)
        {
            _onSelectGroup?.Invoke(groupName, isSelect);
        }

        void _OnSelectChild(string groupName, string key, bool isSelect)
        {
            _onSelectChild?.Invoke(groupName, key, isSelect);
        }
    }

    abstract class InformationCommonViewLinker : ViewLinkerBase
    {
        public Action<string,bool> _onSelectGroup;
        public Action<string, string, bool> _onSelectChild;
        public bool _isSelection;
    }
}
