using UnityEngine;
using System.Collections.Generic;

namespace NoaDebugger
{
    sealed class MenuRuntimeSettings : FeatureToolSettingsBase
    {
        protected override string PlayerPrefsKeyPrefix => "Menu";

        [SerializeField]
        List<MenuInfo> _menuList;

        public List<MenuInfo> MenuList => _menuList;

        protected override void _InitializeSettings()
        {
            _menuList = _noaDebuggerSettings.MenuList;
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
