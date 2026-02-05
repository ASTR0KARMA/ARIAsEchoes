using UnityEngine;

namespace NoaDebugger
{
    abstract class SettingsViewBase : MonoBehaviour
    {
        public abstract void Initialize(SettingsViewLinker linker);
    }
}
