using UnityEngine;

namespace NoaDebugger
{
    sealed class MenuSettingsComponent : MonoBehaviour
    {
        [SerializeField]
        NoaDebuggerText _menuName;

        public void SetMenuName(string name)
        {
            _menuName.text = name;
        }
    }
}
