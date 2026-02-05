using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace NoaDebugger
{
    sealed class MouseInputManager : IInputInternal
    {
        public void Initialize() { }

        public bool IsButtonUp()
        {
            return !UnityInput.GetMouseButton(0);
        }

        public bool IsButtonReleased()
        {
            return UnityInput.GetMouseButtonUp(0);
        }

        public Vector2 GetCursorPosition()
        {
            return UnityInput.mousePosition;
        }
    }
}
