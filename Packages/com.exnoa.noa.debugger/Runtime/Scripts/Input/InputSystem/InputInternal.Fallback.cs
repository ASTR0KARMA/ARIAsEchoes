#if !INPUT_SYSTEM_PACKAGE_AVAILABLE
using UnityEngine;

namespace NoaDebugger
{
    sealed class InputInternal : IInputInternal
    {
        public void Initialize() { }

        public bool IsButtonUp() => true;

        public bool IsButtonReleased() => false;

        public Vector2 GetCursorPosition() => default;

        public IInputInternal GetInputInternal() => this;
    }
}
#endif
