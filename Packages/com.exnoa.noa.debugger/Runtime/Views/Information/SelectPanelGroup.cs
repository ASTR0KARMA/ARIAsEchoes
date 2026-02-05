using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

namespace NoaDebugger
{
    public class SelectPanelGroup : MonoBehaviour
    {
        [SerializeField]
        GameObject _line;

        [SerializeField]
        Button _button;

        Dictionary<string, SelectPanel> _dynamicPanels = new Dictionary<string, SelectPanel>();

        public Action<bool> OnSelect { get; set; }

        public Action<string, bool> OnSelectChild { get; set; }

        bool _selectable;

        void Start()
        {
            _button.onClick.AddListener(_OnSelect);
        }

        void _OnSelect()
        {
            if (!_selectable)
            {
                return;
            }

            bool isActive = _line.activeSelf;
            _line.SetActive(!isActive);

            if (_line.activeSelf)
            {
                SelectAll();
            }
            else
            {
                DeselectAll();
            }

            OnSelect?.Invoke(!isActive);
        }

        void _OnChildPanelSelected(string key, bool isSelected)
        {
            bool anySelected = false;
            if (isSelected)
            {
                anySelected = true;
            }
            else
            {
                foreach (var panel in _dynamicPanels.Values)
                {
                    if (panel.IsSelected)
                    {
                        anySelected = true;
                        break;
                    }
                }
            }

            bool isPanelGroupSelected = _line.activeSelf;
            _line.SetActive(anySelected);

            if (isPanelGroupSelected != anySelected)
            {
                OnSelect?.Invoke(anySelected);
            }

            OnSelectChild?.Invoke(key, isSelected);
        }

        public void SetSelectionState(bool isSelect, bool isSelection)
        {
            _selectable = isSelection;
            _line.SetActive(isSelect);
        }

        public void AddPanel(string key, SelectPanel panel)
        {
            if (panel == null || string.IsNullOrEmpty(key))
            {
                return;
            }

            if (_dynamicPanels.ContainsKey(key))
            {
                LogModel.DebugLogWarning($"Panel with key '{key}' already exists.");
                return;
            }

            _dynamicPanels.Add(key, panel);
            panel.OnSelect += (isSelected) => _OnChildPanelSelected(key, isSelected);
        }

        public void SetChildSelectionState(string key, bool isSelect, bool isSelection)
        {
            if (_dynamicPanels.TryGetValue(key, out SelectPanel panel))
            {
                panel.SetSelectionState(isSelect, isSelection);
            }
        }

        public void Select(string key)
        {
            if (!_selectable)
            {
                return;
            }

            if (_dynamicPanels.TryGetValue(key, out SelectPanel panel))
            {
                panel.Select();
            }
        }

        void SelectAll()
        {
            if (!_selectable)
            {
                return;
            }

            foreach (var panel in _dynamicPanels.Values)
            {
                panel.Select();
            }
        }

        void DeselectAll()
        {
            foreach (var panel in _dynamicPanels.Values)
            {
                panel.Deselect();
            }
        }

        public void SetSelectable(bool selectable)
        {
            _selectable = selectable;
            foreach (var panel in _dynamicPanels.Values)
            {
                panel.SetSelectable(selectable);
            }
        }
    }
}
