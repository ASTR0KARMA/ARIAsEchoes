using UnityEngine;
using UnityEngine.UI;
using System;

namespace NoaDebugger
{
    public class SelectPanel : MonoBehaviour
    {
        [SerializeField]
        GameObject _line;

        [SerializeField]
        Button _button;
        bool _selectable;

        public Action<bool> OnSelect;

        public bool IsSelected
        {
            get { return _line.activeSelf; }
        }

        void Awake()
        {
            _button.onClick.AddListener(_OnSelect);
        }

        void _OnSelect()
        {
            if (!_selectable)
            {
                return;
            }

            if (_line.activeSelf)
            {
                Deselect();
            }
            else
            {
                Select();
            }
        }

        public void SetSelectionState(bool isSelect, bool isSelection)
        {
            _selectable = isSelection;
            _line.SetActive(isSelect);
        }

        public void Select()
        {
            if (!_selectable)
            {
                return;
            }

            _line.SetActive(true);
            OnSelect?.Invoke(true);
        }

        public void Deselect()
        {
            _line.SetActive(false);
            OnSelect?.Invoke(false);
        }

        public void SetSelectable(bool selectable)
        {
            _selectable = selectable;
        }
    }
}
