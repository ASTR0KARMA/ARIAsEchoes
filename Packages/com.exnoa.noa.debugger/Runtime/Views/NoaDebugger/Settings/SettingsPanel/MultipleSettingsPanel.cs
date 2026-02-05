using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

namespace NoaDebugger
{
    abstract class MultipleSettingsPanel<T> : MonoBehaviour
    {
        [SerializeField]
        protected List<SettingsPanelBase<T>> _panels;

        protected List<SettingParameterBase<T>> _settings;

        public List<SettingParameterBase<T>> Settings => _settings;

        internal virtual void Initialize(List<SettingParameterBase<T>> settings)
        {
            _settings = settings;

            Assert.IsTrue(_panels != null && _panels.Count > 0, "パネルが設定されていません");
            Assert.AreEqual(_panels.Count, settings.Count, "パネル数と設定数が一致しません");

            for (int i = 0; i < _panels.Count; i++)
            {
                _panels[i].Initialize(settings[i]);
            }

            Refresh();
        }

        public virtual void Refresh()
        {
            foreach (var panel in _panels)
            {
                panel.Refresh();
            }
        }

        public virtual void SetEnabled(bool isEnabled)
        {
            foreach (var panel in _panels)
            {
                panel.SetEnabled(isEnabled);
            }
        }
    }
}
