using System.Collections.Generic;
using UnityEngine;

namespace NoaDebugger
{
    sealed class ShortcutSettingsGroupView : MutableSettingsViewBase
    {
        [SerializeField] BoolSettingsPanel _enableAllShortcut;
        [SerializeField] ShortcutSettingsPanel _shortcutPanelPrefab;
        [SerializeField] GameObject _shortcutGroupRoot;

        List<ShortcutSettingsPanel> _instancedShortcutPanels;

        protected override SettingsResetButtonType ResetButtonType
        {
            get { return SettingsResetButtonType.Shortcut; }
        }

        protected override void Awake()
        {
            base.Awake();
            _instancedShortcutPanels = new List<ShortcutSettingsPanel>();
        }

        public override void Initialize(SettingsViewLinker linker)
        {
            var settings = linker._shortcutSettings;
            _enableAllShortcut.Initialize(settings.EnableAllShortcuts);

            foreach (var action in settings.Shortcuts)
            {
                ShortcutSettingsPanel panel;
                if (_instancedShortcutPanels.Exists(p => p.name == action.Name))
                {
                    panel = _instancedShortcutPanels.Find(p => p.name == action.Name);
                }
                else
                {
                    panel = ShortcutSettingsPanel.Instantiate(
                        _shortcutPanelPrefab,
                        _shortcutGroupRoot.transform,
                        action);
                    panel.name = action.Name;
                    _instancedShortcutPanels.Add(panel);
                }
                panel.Initialize(action.Enabled);
            }
        }

        void OnDestroy()
        {
            foreach (var shortcutPanel in _instancedShortcutPanels)
            {
                Destroy(shortcutPanel);
            }

            _instancedShortcutPanels = default;
        }
    }
}
