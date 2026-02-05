using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace NoaDebugger
{
    sealed class InputInternal
    {
        readonly IInputInternal _input;

        public InputInternal()
        {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            _input = new TouchInputManager();
#else
            if (UserAgentModel.IsMobileBrowser)
            {
                _input = new TouchInputManager();
            }
            else
            {
                _input = new MouseInputManager();
            }
#endif
        }

        public IInputInternal GetInputInternal()
        {
            return _input;
        }
    }
}
