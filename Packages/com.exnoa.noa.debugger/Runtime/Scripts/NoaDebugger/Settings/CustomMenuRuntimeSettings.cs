using System;
using UnityEngine;
using System.Collections.Generic;

namespace NoaDebugger
{
    sealed class CustomMenuRuntimeSettings : FeatureToolSettingsBase
    {
        protected override string PlayerPrefsKeyPrefix => "CustomMenu";

        string _selectedPath;
        List<string> _scriptNames;

        public string SelectedPath => _selectedPath;

        public List<string> ScriptNames => _scriptNames;

        protected override void _InitializeSettings()
        {
            _selectedPath = _noaDebuggerSettings.CustomMenuFolderPath;

            _scriptNames = new List<string>();
            foreach (var customMenu in _noaDebuggerSettings.CustomMenuList)
            {
                var viewName = customMenu.GetViewName();
                _scriptNames.Add(viewName);
            }
        }

        protected override void _LoadSettings(string prefsJson)
        {
        }

        protected override void _SetDefaultValue()
        {
        }

        protected override void _ResetSettings()
        {
        }

        public override void ApplyCache(NoaDebuggerSettings originalSettings)
        {
        }

        public override bool IsValueChanged()
        {
            return false;
        }
    }
}
