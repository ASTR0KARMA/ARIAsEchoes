using System.Collections.Generic;
using UnityEngine;

namespace NoaDebugger
{
    sealed class CustomMenuSettingsGroupView : SettingsViewBase
    {
        [SerializeField] ReadOnlySettingsPanel _selectedPath;
        [SerializeField] GameObject _customMenuGroupRoot;
        [SerializeField] ReadOnlySettingsPanel _customMenuPanelPrefab;

        List<ReadOnlySettingsPanel> _instancedCustomMenuPanels;

        public override void Initialize(SettingsViewLinker linker)
        {
            _selectedPath.Initialize(linker._customMenuSettings.SelectedPath);

            if (_instancedCustomMenuPanels != null)
            {
                return;
            }

            _instancedCustomMenuPanels = new List<ReadOnlySettingsPanel>();
            foreach (var menuName in linker._customMenuSettings.ScriptNames)
            {
                var menuPanel = GameObject.Instantiate(_customMenuPanelPrefab, _customMenuGroupRoot.transform);
                menuPanel.Initialize("");
                menuPanel.GetComponent<MenuSettingsComponent>().SetMenuName(menuName);
                _instancedCustomMenuPanels.Add(menuPanel);
            }
        }

        void OnDestroy()
        {
            if (_instancedCustomMenuPanels != null)
            {
                foreach (var panel in _instancedCustomMenuPanels)
                {
                    Destroy(panel.gameObject);
                }
                _instancedCustomMenuPanels.Clear();
            }
        }
    }
}
