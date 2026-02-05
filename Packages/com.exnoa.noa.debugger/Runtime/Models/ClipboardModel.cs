using UnityEngine;

namespace NoaDebugger
{
    sealed class ClipboardModel : ModelBase
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void NoaDebuggerCopyClipboard(string input);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void NoaDebuggerRegisterTouchFocusEvent();

        public static void Initialize()
        {
            Debug.Log("Registered an event listener in JavaScript to gain focus on touch.");
            Debug.Log("The canvas element's tabindex was set to -1.");

            if(UserAgentModel.IsWebGLandiOSorMacSafari)
            {
                Debug.Log("Long-press to clipboard copy functionality may not work in Safari (WebKit).");
            }

            NoaDebuggerRegisterTouchFocusEvent();
        }

        static void _Copy(string input)
        {
            NoaDebuggerCopyClipboard(input);
        }
#else
        public static void Initialize() {}

        static void _Copy(string input)
        {
            GUIUtility.systemCopyBuffer = input;

            if (!string.IsNullOrEmpty(input) && string.IsNullOrEmpty(GUIUtility.systemCopyBuffer))
            {
                LogModel.LogError(NoaDebuggerDefine.ClipboardCopyFailedText);
            }
        }
#endif

        public static void Copy(string input)
        {
            _Copy(input);
        }
    }
}
