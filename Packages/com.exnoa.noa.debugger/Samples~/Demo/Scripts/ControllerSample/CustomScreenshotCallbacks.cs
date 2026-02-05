#if NOA_DEBUGGER
using System.IO;
using NoaDebugger;
using UnityEngine;

namespace NoaDebuggerDemo
{
    sealed class CustomScreenshotCallbacks : NoaScreenshotCallbacks
    {
        static string ScreenshotFileName => "NoaDebuggerDemoScreenshot.png";

        public override void OnBeforePrepareScreenshot()
        {
            if (NoaController.IsGamePlaying)
            {
                NoaController.TogglePauseResume();
            }
        }

        public override void OnAfterScreenshot()
        {
            byte[] data = NoaController.GetCapturedScreenshot();

            if (data != null)
            {
                SaveScreenshot(data);
                NoaController.ClearCapturedScreenshot();
            }
            else
            {
                Debug.LogError("No screenshot data.");
            }

            NoaUIElement.SetAllUIElementsVisibility(true);

            if (!NoaController.IsGamePlaying)
            {
                NoaController.TogglePauseResume();
            }
        }

        static void SaveScreenshot(byte[] data)
        {
#if UNITY_EDITOR
            string directory = Path.Combine(Application.dataPath, "..", "Temp");
#else
            string directory = Application.persistentDataPath;
#endif
            string filePath = Path.Combine(directory, ScreenshotFileName);
            File.WriteAllBytes(filePath, data);
            Debug.Log($"Screenshot saved to: {filePath}");
        }
    }
}
#endif
