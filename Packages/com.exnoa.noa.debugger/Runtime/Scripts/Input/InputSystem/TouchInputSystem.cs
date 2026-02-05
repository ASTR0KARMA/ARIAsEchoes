#if INPUT_SYSTEM_PACKAGE_AVAILABLE
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace NoaDebugger
{
    sealed class TouchInputSystem : IInputInternal
    {
        public void Initialize()
        {
            EnhancedTouchSupport.Enable();
        }

        public bool IsButtonUp()
        {
            return Touch.activeTouches.Count <= 0;
        }

        public bool IsButtonReleased()
        {
            return Touch.activeTouches.Count > 0
                   && Touch.activeTouches[0].phase == UnityEngine.InputSystem.TouchPhase.Ended;
        }

        public Vector2 GetCursorPosition()
        {
            return Touchscreen.current.primaryTouch.position.ReadValue();
        }
    }
}
#endif
