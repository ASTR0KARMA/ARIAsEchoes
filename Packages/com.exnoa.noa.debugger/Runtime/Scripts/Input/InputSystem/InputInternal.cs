#if INPUT_SYSTEM_PACKAGE_AVAILABLE
namespace NoaDebugger
{
    sealed class InputInternal
    {
        readonly IInputInternal _input;

        public InputInternal()
        {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            _input = new TouchInputSystem();
#else
            if (UserAgentModel.IsMobileBrowser)
            {
                _input = new TouchInputSystem();
            }
            else
            {
                _input = new MouseInputSystem();
            }
#endif
        }

        public IInputInternal GetInputInternal()
        {
            return _input;
        }
    }
}
#endif
