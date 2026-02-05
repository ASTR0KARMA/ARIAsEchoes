using System;
using System.Collections.Generic;
using UnityEngine;

namespace NoaDebugger
{
    sealed class MenuSettingsGroupView : SettingsViewBase
    {
        [SerializeField] GameObject _menuGroupRoot;
        [SerializeField] ReadOnlySettingsPanel _menuPanelPrefab;

        List<ReadOnlySettingsPanel> _instancedMenuPanels;

        void Awake()
        {
            _instancedMenuPanels = new List<ReadOnlySettingsPanel>();
        }

        public override void Initialize(SettingsViewLinker linker)
        {
            foreach (var menu in linker._menuSettings.MenuList)
            {
                ReadOnlySettingsPanel menuPanel;
                if (_instancedMenuPanels.Exists(panel => panel.name == menu.Name))
                {
                    menuPanel = _instancedMenuPanels.Find(panel => panel.name == menu.Name);
                }
                else
                {
                    menuPanel = GameObject.Instantiate(_menuPanelPrefab, _menuGroupRoot.transform);
                    menuPanel.name = menu.Name;
                    menuPanel.GetComponent<MenuSettingsComponent>().SetMenuName(menu.Name);
                    _instancedMenuPanels.Add(menuPanel);
                }

                string value = menu.Enabled ? NoaDebuggerDefine.SETTINGS_ENABLED_VALUE : NoaDebuggerDefine.SETTINGS_DISABLED_VALUE;
                menuPanel.Initialize(value);
            }
        }

        void OnDestroy()
        {
            foreach (var menuPanel in _instancedMenuPanels)
            {
                Destroy(menuPanel);
            }
            _instancedMenuPanels = default;
        }
    }
}
