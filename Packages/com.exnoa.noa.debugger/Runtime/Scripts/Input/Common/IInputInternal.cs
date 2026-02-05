using UnityEngine;

namespace NoaDebugger
{
    interface IInputInternal
    {
        public void Initialize();

        public bool IsButtonUp();

        public bool IsButtonReleased();

        public Vector2 GetCursorPosition();
    }
}
