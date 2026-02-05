using System;
using UnityEngine;

namespace NoaDebugger
{
    [Serializable]
    abstract class ToolSettingsBase
    {
        public event Action OnUpdateSettings;

        protected NoaDebuggerSettings _noaDebuggerSettings;

        string PlayerPrefsKey => PlayerPrefsKeyPrefix + PlayerPrefsSuffix;
        protected abstract string PlayerPrefsKeyPrefix { get; }
        protected abstract string PlayerPrefsSuffix { get; }

        public void Initialize()
        {
            _noaDebuggerSettings = NoaDebuggerSettingsManager.GetOriginalNoaDebuggerSettings();

            _InitializeSettings();

            string prefsJson = NoaDebuggerPrefs.GetString(PlayerPrefsKey, "");

            if (!String.IsNullOrEmpty(prefsJson))
            {
                _LoadSettings(prefsJson);
            }
            else
            {
                _SetDefaultValue();
                _Save();
            }
        }

        protected abstract void _InitializeSettings();

        protected abstract void _LoadSettings(string prefsJson);

        public void Reset()
        {
            _ResetSettings();
        }

        public void Save()
        {
            _Save();
        }

        public void InvokeOnUpdateSettings()
        {
            OnUpdateSettings?.Invoke();
        }

        protected abstract void _SetDefaultValue();

        protected abstract void _ResetSettings();

        protected virtual void _Save()
        {
            SaveCore();
            NoaDebuggerSettingsManager.UpdateRuntimeNoaDebuggerSettings();
            OnUpdateSettings?.Invoke();
        }

        public virtual void SaveCore()
        {
            NoaDebuggerPrefs.SetString(PlayerPrefsKey, JsonUtility.ToJson(this));
        }

        public abstract void ApplyCache(NoaDebuggerSettings originalSettings);

        public abstract bool IsValueChanged();
    }
}
