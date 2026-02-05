using UnityEngine;

namespace NoaDebugger
{
    static class Input
    {
        static readonly InputInternal _internal = new InputInternal();
        static readonly IInputInternal _input = _internal.GetInputInternal();

        public static void Initialize() => _input.Initialize();

        public static bool IsButtonUp() => _input.IsButtonUp();

        public static bool IsButtonReleased() => _input.IsButtonReleased();

        public static Vector2 GetCursorPosition() => _input.GetCursorPosition();
    }
}
