using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace NoaDebugger
{
    sealed class TouchInputManager : IInputInternal
    {
        public void Initialize() { }

        public bool IsButtonUp()
        {
            return UnityInput.touchCount <= 0;
        }

        public bool IsButtonReleased()
        {
            return UnityInput.touchCount > 0 && UnityInput.GetTouch(0).phase == TouchPhase.Ended;
        }

        public Vector2 GetCursorPosition()
        {
            if (UnityInput.touchCount > 0)
            {
                return UnityInput.GetTouch(0).position;
            }
            return Vector2.zero;
        }
    }
}
