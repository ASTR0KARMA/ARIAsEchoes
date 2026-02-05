using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NoaDebugger
{
    sealed class CustomInformationView : InformationCategoryView<CustomInformationViewLinker>
    {
        [SerializeField]
        GameObject _noDataLabel;

        protected override void _OnShow(CustomInformationViewLinker linker)
        {
            base._OnShow(linker);

            gameObject.SetActive(true);

            if(linker._isBuild)
            {
                foreach (var (key, group) in _groupViews)
                {
                    GameObject.Destroy(group.gameObject);
                }

                _groupViews.Clear();
            }

            foreach (var group in linker._groups)
            {
                SetCustomGroup(group, linker._isSelection);
            }

            bool isEmptyGroups = _groupViews.Count == 0;
            _noDataLabel.SetActive(isEmptyGroups);
        }

        void SetCustomGroup(InformationCustomGroup group, bool isSelection)
        {
            foreach (var keyValue in group.KeyValues)
            {
                var creator = new CustomInformationParamCreator(this, group.Name, keyValue.DisplayName);
                keyValue.Parameter.Accept(creator);
                
                var isSelect = isSelection && keyValue.IsSelect;
                InitChildPanel(group.Name, keyValue.Key, isSelect, isSelection);

                bool isChildSelected = isSelection && group.KeyValues.Any(item => item.IsSelect);
                InitGroupPanel(group.Name, isChildSelected, isSelection);
            }
        }

        protected override void _OnHide()
        {
            gameObject.SetActive(false);
        }


        class CustomInformationParamCreator : ICustomParameterVisitor
        {
            readonly CustomInformationView _view;
            readonly string _groupName;
            readonly string _displayName;

            public CustomInformationParamCreator(CustomInformationView view, string groupName, string displayName)
            {
                _view = view;
                _groupName = groupName;
                _displayName = displayName;
            }

            public void Visit(IntImmutableCustomInformationParameter parameter)
            {
                _view.SetReadOnly(
                    _groupName,
                    _displayName,
                    parameter.GetStringValue());
            }

            public void Visit(FloatImmutableCustomInformationParameter parameter)
            {
                _view.SetReadOnly(
                    _groupName,
                    _displayName,
                    parameter.GetStringValue());
            }

            public void Visit(BoolImmutableCustomInformationParameter parameter)
            {
                _view.SetReadOnly(
                    _groupName,
                    _displayName,
                    parameter.GetStringValue());
            }

            public void Visit(StringImmutableCustomInformationParameter parameter)
            {
                _view.SetReadOnly(
                    _groupName,
                    _displayName,
                    parameter.GetStringValue());
            }

            public void Visit(EnumImmutableCustomInformationParameter parameter)
            {
                _view.SetReadOnly(
                    _groupName,
                    _displayName,
                    parameter.GetStringValue());
            }

            public void Visit(IntCustomInformationParameter parameter)
            {
                _view.SetInt(_groupName, _displayName, parameter);
            }

            public void Visit(FloatCustomInformationParameter parameter)
            {
                _view.SetFloat(_groupName, _displayName, parameter);
            }

            public void Visit(BoolCustomInformationParameter parameter)
            {
                _view.SetBool(_groupName, _displayName, parameter);
            }

            public void Visit(StringCustomInformationParameter parameter)
            {
                _view.SetString(_groupName, _displayName, parameter);
            }

            public void Visit(EnumCustomInformationParameter parameter)
            {
                _view.SetEnum(_groupName, _displayName, parameter);
            }
        }
    }

    sealed class CustomInformationViewLinker : InformationCommonViewLinker
    {
        public List<InformationCustomGroup> _groups;

        public bool _isBuild = true;
    }
}
