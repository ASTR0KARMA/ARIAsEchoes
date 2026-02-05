using System;
using UnityEngine;

namespace NoaDebugger
{
    [Serializable]
    abstract class SettingParameterBase<T> : IMutableParameter<T>
    {
        [SerializeField]
        protected T _value;

        [SerializeField]
        protected bool _isSaved;

        bool _isChanged = false;

        bool _isShowOverrideMarker = false;

        public T Value => _value;

        public bool IsSaved => _isSaved;

        protected readonly T _defaultValue;

        protected T _savedValue;

        public bool IsChanged => _isChanged;

        public Action _onChangeValue;

        public bool IsShowOverrideMarker => _isShowOverrideMarker;

        protected SettingParameterBase(T defaultValue)
        {
            _defaultValue = defaultValue;
            _savedValue = defaultValue;
        }

        protected void _RefreshFlags(T newValue)
        {
            _isSaved = !newValue.Equals(_defaultValue);

            _isShowOverrideMarker = !newValue.Equals(_defaultValue);

            _isChanged = !newValue.Equals(_savedValue);
        }

        void _SetValue(T newValue)
        {
            _value = newValue;
        }

        void _SetValueWithFlags(T newValue)
        {
            _RefreshFlags(newValue);

            _SetValue(newValue);
        }

        public virtual void ApplySavedSettings(SettingParameterBase<T> savedSetting)
        {
            if (savedSetting == null || !savedSetting.IsSaved)
            {
                SetDefaultValue();
                return;
            }

            _value = savedSetting.Value;
            _savedValue = savedSetting.Value;

            _RefreshFlags(savedSetting.Value);
        }

        public virtual void SetDefaultValue()
        {
            _SetValue(_defaultValue);
        }

        public virtual void ResetSettings()
        {
            _SetValueWithFlags(_defaultValue);
        }

        public virtual void ChangeValue(T newValue)
        {
            _SetValueWithFlags(newValue);
            _onChangeValue?.Invoke();
            SettingsEventModel.OnSettingsValueChanged?.Invoke();
        }

        public void OnSaved()
        {
            _isChanged = false;
            _savedValue = _value;
        }
    }
}
