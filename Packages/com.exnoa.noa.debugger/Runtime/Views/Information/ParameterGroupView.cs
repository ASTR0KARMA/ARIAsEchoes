using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class ParameterGroupView : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI _header;

        [SerializeField]
        Transform _groupRoot;

        [SerializeField]
        Transform _line;

        [SerializeField]
        SelectPanelGroup _selectPanelGroup;

        readonly Dictionary<string, ReadOnlySettingsPanel> _readOnlySettings = new();
        readonly Dictionary<string, BoolSettingsPanel> _boolSettings = new();
        readonly Dictionary<string, IntSettingsPanel> _intSettings = new();
        readonly Dictionary<string, FloatSettingsPanel> _floatSettings = new();
        readonly Dictionary<string, EnumSettingsPanel> _enumSettings = new();
        readonly Dictionary<string, StringSettingsPanel> _stringSettings = new();
        readonly HashSet<string> _propertyNames = new();

        void OnValidate()
        {
            Assert.IsNotNull(_header);
            Assert.IsNotNull(_groupRoot);
            Assert.IsNotNull(_line);
            Assert.IsNotNull(_selectPanelGroup);
        }

        public string Header
        {
            get => _header.text;
            set => _header.text = value;
        }

        void RegisterSelectPanel(string propertyName, GameObject panelObject)
        {
            var selectPanel = panelObject.GetComponent<SelectPanel>();
            if (selectPanel != null)
            {
                _selectPanelGroup.AddPanel(propertyName, selectPanel);
            }
        }

        public void SetReadOnly(string propertyName, object value, ReadOnlySettingsPanel original)
        {
            ReadOnlySettingsPanel panel;

            if (!_propertyNames.Contains(propertyName))
            {
                GameObject panelObject = GameObject.Instantiate(original.gameObject, _groupRoot);
                panel = panelObject.GetComponent<ReadOnlySettingsPanel>();
                panel.Name = propertyName;
                _readOnlySettings[propertyName] = panel;
                _propertyNames.Add(propertyName);

                RegisterSelectPanel(propertyName, panelObject);
            }
            else if (!_readOnlySettings.TryGetValue(propertyName, out panel))
            {
                throw new InvalidOperationException($"Property '{propertyName}' already exists as another type.");
            }

            panel.Initialize(value != null ? value.ToString() : string.Empty);
        }

        public void SetBool(string propertyName, IMutableParameter<bool> value, BoolSettingsPanel original)
        {
            BoolSettingsPanel panel;

            if (!_propertyNames.Contains(propertyName))
            {
                GameObject panelObject = GameObject.Instantiate(original.gameObject, _groupRoot);
                panel = panelObject.GetComponent<BoolSettingsPanel>();
                panel.Name = propertyName;
                _boolSettings[propertyName] = panel;
                _propertyNames.Add(propertyName);

                RegisterSelectPanel(propertyName, panelObject);
            }
            else if (!_boolSettings.TryGetValue(propertyName, out panel))
            {
                throw new InvalidOperationException($"Property '{propertyName}' already exists as another type.");
            }

            panel.Initialize(value);
        }

        public void SetInt(string propertyName, IMutableParameter<int> value, IntSettingsPanel original)
        {
            IntSettingsPanel panel;

            if (!_propertyNames.Contains(propertyName))
            {
                GameObject panelObject = GameObject.Instantiate(original.gameObject, _groupRoot);
                panel = panelObject.GetComponent<IntSettingsPanel>();
                panel.Name = propertyName;
                _intSettings[propertyName] = panel;
                _propertyNames.Add(propertyName);

                RegisterSelectPanel(propertyName, panelObject);
            }
            else if (!_intSettings.TryGetValue(propertyName, out panel))
            {
                throw new InvalidOperationException($"Property '{propertyName}' already exists as another type.");
            }

            panel.Initialize(value);
        }

        public void SetFloat(string propertyName, IMutableParameter<float> value, FloatSettingsPanel original)
        {
            FloatSettingsPanel panel;

            if (!_propertyNames.Contains(propertyName))
            {
                GameObject panelObject = GameObject.Instantiate(original.gameObject, _groupRoot);
                panel = panelObject.GetComponent<FloatSettingsPanel>();
                panel.Name = propertyName;
                _floatSettings[propertyName] = panel;
                _propertyNames.Add(propertyName);

                RegisterSelectPanel(propertyName, panelObject);
            }
            else if (!_floatSettings.TryGetValue(propertyName, out panel))
            {
                throw new InvalidOperationException($"Property '{propertyName}' already exists as another type.");
            }

            panel.Initialize(value);
        }

        public void SetEnum(string propertyName, IMutableParameter<Enum> value, EnumSettingsPanel original)
        {
            EnumSettingsPanel panel;

            if (!_propertyNames.Contains(propertyName))
            {
                GameObject panelObject = GameObject.Instantiate(original.gameObject, _groupRoot);
                panel = panelObject.GetComponent<EnumSettingsPanel>();
                panel.Name = propertyName;
                _enumSettings[propertyName] = panel;
                _propertyNames.Add(propertyName);

                RegisterSelectPanel(propertyName, panelObject);
            }
            else if (!_enumSettings.TryGetValue(propertyName, out panel))
            {
                throw new InvalidOperationException($"Property '{propertyName}' already exists as another type.");
            }

            panel.Initialize(value);
        }

        public void SetString(string propertyName, IMutableParameter<string> value, StringSettingsPanel original)
        {
            StringSettingsPanel panel;

            if (!_propertyNames.Contains(propertyName))
            {
                GameObject panelObject = GameObject.Instantiate(original.gameObject, _groupRoot);
                panel = panelObject.GetComponent<StringSettingsPanel>();
                panel.Name = propertyName;
                _stringSettings[propertyName] = panel;
                _propertyNames.Add(propertyName);

                RegisterSelectPanel(propertyName, panelObject);
            }
            else if (!_stringSettings.TryGetValue(propertyName, out panel))
            {
                throw new InvalidOperationException($"Property '{propertyName}' already exists as another type.");
            }

            panel.Initialize(value);
        }

        public void SetPanelSelectable(bool selectable)
        {
            _selectPanelGroup.SetSelectable(selectable);
        }

        public void InitGroupPanel(bool isSelect, bool isSelection)
        {
            _selectPanelGroup.SetSelectionState(isSelect, isSelection);
        }

        public void InitChildPanel(string propertyName, bool isSelect, bool isSelection)
        {
            _selectPanelGroup.SetChildSelectionState(propertyName, isSelect, isSelection);
        }

        public void ShowRuler(bool show)
        {
            _line.gameObject.SetActive(show);
        }
    }
}
