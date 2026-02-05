using System.Collections.Generic;
using System.Linq;

namespace NoaDebugger
{
    abstract class SelectableKeyValueBase
    {
        List<SelectableKeyValue> _selectableKeyValues;

        public List<SelectableKeyValue> Items => _selectableKeyValues;

        protected SelectableKeyValueBase()
        {
            _selectableKeyValues = new List<SelectableKeyValue>();
        }

        public void SelectAll()
        {
            foreach (var item in _selectableKeyValues)
            {
                item.IsSelect = true;
            }
        }

        public void DeselectAll()
        {
            foreach (var item in _selectableKeyValues)
            {
                item.IsSelect = false;
            }
        }

        public int GetSelectedCount()
        {
            return _selectableKeyValues.Count(item => item.IsSelect);
        }

        public List<SelectableKeyValue> GetSelectedItems()
        {
            return _selectableKeyValues.Where(item => item.IsSelect).ToList();
        }

        public void SetSelection(string key, bool isSelect)
        {
            var item = _selectableKeyValues.FirstOrDefault(i => i.KeyValuePair.Key == key);
            if (item != null)
            {
                item.IsSelect = isSelect;
            }
        }

        protected void AddItem(string key, object value)
        {
            _selectableKeyValues.Add(new SelectableKeyValue(key, value));
        }

        protected void UpdateItem(string key, object value)
        {
            var item = _selectableKeyValues.FirstOrDefault(i => i.KeyValuePair.Key == key);
            if (item != null)
            {
                item.UpdateValue(value);
            }
        }
    }

    sealed class SelectableKeyValue
    {
        public KeyValuePair<string, object> KeyValuePair { get; private set; }

        public bool IsSelect { get; set; }

        public SelectableKeyValue(string key, object value, bool isSelect = false)
        {
            KeyValuePair = new KeyValuePair<string, object>(key, value);
            IsSelect = isSelect;
        }

        public void UpdateValue(object value)
        {
            KeyValuePair = new KeyValuePair<string, object>(KeyValuePair.Key, value);
        }
    }
}
