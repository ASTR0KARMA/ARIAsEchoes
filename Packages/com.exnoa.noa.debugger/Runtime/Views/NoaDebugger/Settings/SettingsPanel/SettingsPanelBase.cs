using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    abstract class SettingsPanelBase<T> : MonoBehaviour
    {
        [SerializeField]
        NoaDebuggerText _name;

        [SerializeField]
        GameObject _overrideLine;

        [SerializeField]
        GameObject _disableObject;

        protected IMutableParameter<T> _parameter;

        public string Name
        {
            get => _name.text;
            set => _name.text = value;
        }

        public virtual void Initialize(IMutableParameter<T> settings)
        {
            Assert.IsNotNull(_name);
            Assert.IsNotNull(_overrideLine);
            Assert.IsNotNull(_disableObject);
            _parameter = settings;
        }

        public void Refresh()
        {
            _Refresh();
            _overrideLine.SetActive(_parameter.IsShowOverrideMarker);
        }

        protected abstract void _Refresh();

        public virtual void SetEnabled(bool isEnabled)
        {
            _disableObject.SetActive(!isEnabled);
        }
    }
}
