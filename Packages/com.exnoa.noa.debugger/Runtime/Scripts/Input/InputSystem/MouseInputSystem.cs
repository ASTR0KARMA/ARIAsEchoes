#if INPUT_SYSTEM_PACKAGE_AVAILABLE
using UnityEngine;
using UnityEngine.InputSystem;

namespace NoaDebugger
{
    sealed class MouseInputSystem : IInputInternal
    {
        public void Initialize() { }

        public bool IsButtonUp()
        {
            return !Mouse.current.leftButton.isPressed;
        }

        public bool IsButtonReleased()
        {
            return Mouse.current.leftButton.wasReleasedThisFrame;
        }

        public Vector2 GetCursorPosition()
        {
            return Mouse.current.position.ReadValue();
        }
    }
}
#endif
