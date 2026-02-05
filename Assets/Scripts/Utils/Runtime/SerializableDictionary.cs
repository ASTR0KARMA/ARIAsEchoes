using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector]
        private List<TKey> _keyData = new();

        [SerializeField, HideInInspector]
        private List<TValue> _valueData = new();

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Clear();
            for (int i = 0; i < _keyData.Count && i < _valueData.Count; i++)
            {
                this[_keyData[i]] = _valueData[i];
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            _keyData.Clear();
            _valueData.Clear();
            
            _keyData.AddRange(Keys);
            _valueData.AddRange(Values);
        }
    }
}

