using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class ReadOnlySettingsPanel : MonoBehaviour
    {
        [SerializeField]
        NoaDebuggerText _name;

        [SerializeField]
        NoaDebuggerText _text;

        public string Name
        {
            get => _name.text;
            set => _name.text = value;
        }

        public void Initialize(string labelText)
        {
            Assert.IsNotNull(_name);
            Assert.IsNotNull(_text);
            _text.text = labelText;
        }
    }
}
