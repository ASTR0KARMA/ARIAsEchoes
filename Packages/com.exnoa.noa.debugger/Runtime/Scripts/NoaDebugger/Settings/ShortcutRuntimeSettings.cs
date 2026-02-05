using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace NoaDebugger
{
    [Serializable]
    sealed class ShortcutRuntimeSettings : FeatureToolSettingsBase
    {
        protected override string PlayerPrefsKeyPrefix => "Shortcut";

        [SerializeField]
        BoolSettingParameter _enableAllShortcuts;

        [SerializeField]
        List<ShortcutActionInfo> _shortcuts;

        public BoolSettingParameter EnableAllShortcuts => _enableAllShortcuts;

        public List<ShortcutActionInfo> Shortcuts => _shortcuts;

        protected override void _InitializeSettings()
        {
            _enableAllShortcuts = new BoolSettingParameter(defaultValue: _noaDebuggerSettings.EnableAllShortcuts);

            _shortcuts = new List<ShortcutActionInfo>();
            foreach (var action in _noaDebuggerSettings.EnabledShortcutActions)
            {
                _shortcuts.Add(_CreateShortcutSettings(action));
            }
        }

        ShortcutActionInfo _CreateShortcutSettings(ShortcutAction shortcutAction)
        {
            var shortcutEnabled = new BoolSettingParameter(defaultValue: shortcutAction.combination.isEnabled);

            return new ShortcutActionInfo(shortcutEnabled, shortcutAction);
        }

        protected override void _LoadSettings(string prefsJson)
        {
            var loadInfo = JsonUtility.FromJson<ShortcutRuntimeSettings>(prefsJson);

            if (loadInfo == null)
            {
                LogModel.LogWarning("Failed to parse JSON for ShortcutRuntimeSettings. Using default values.");
                _SetDefaultValue();
                return;
            }

            _enableAllShortcuts.ApplySavedSettings(loadInfo._enableAllShortcuts);

            foreach (var shortcut in _shortcuts)
            {
                ShortcutActionInfo savedShortcut = loadInfo._shortcuts
                    .FirstOrDefault(x => x.SavedType == shortcut.SavedType);

                shortcut.Enabled.ApplySavedSettings(savedShortcut?.Enabled);
            }
        }

        protected override void _SetDefaultValue()
        {
            _enableAllShortcuts.SetDefaultValue();
            foreach (var shortcut in _shortcuts)
            {
                shortcut.Enabled.SetDefaultValue();
            }
        }

        protected override void _ResetSettings()
        {
            _enableAllShortcuts.ResetSettings();
            foreach (var shortcut in _shortcuts)
            {
                shortcut.Enabled.ResetSettings();
            }
        }

        public override void SaveCore()
        {
            base.SaveCore();
            _enableAllShortcuts.OnSaved();
            if (_shortcuts != null)
            {
                foreach (var shortcut in _shortcuts)
                {
                    shortcut.Enabled.OnSaved();
                }
            }
        }

        public override void ApplyCache(NoaDebuggerSettings originalSettings)
        {
            originalSettings.EnableAllShortcuts = _enableAllShortcuts.Value;
            for (int i = 0; i < _shortcuts.Count && i < originalSettings.EnabledShortcutActions.Count; i++)
            {
                var actionEnabled = _shortcuts[i].Enabled.Value;
                if (UnityInputUtils.IsEnableInputSystem)
                {
                    originalSettings.ShortcutActions[i].combination.isEnabled = actionEnabled;
                }
                else
                {
                    originalSettings.ShortcutActionsOnInputManager[i].combination.isEnabled = actionEnabled;
                }
            }
        }

        public override bool IsValueChanged()
        {
            bool result = false;
            result |= _enableAllShortcuts.IsChanged;
            if (_shortcuts != null)
            {
                foreach (var shortcut in _shortcuts)
                {
                    result |= shortcut.Enabled.IsChanged;
                }
            }
            return result;
        }
    }
}
