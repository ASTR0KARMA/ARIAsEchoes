#if NOA_DEBUGGER
using System;
using NoaDebugger;
using UnityEngine;

namespace NoaDebuggerDemo
{
    public static class ControllerSetCustomActionSample
    {
        const int CONTROLLER_F1_ACTION_INDEX = 0;
        const int CONTROLLER_F2_ACTION_INDEX = 1;
        const int CONTROLLER_F3_ACTION_INDEX = 2;
        const int CONTROLLER_F4_ACTION_INDEX = 3;
        const int CONTROLLER_F5_ACTION_INDEX = 4;

        static readonly System.Random RandomGenerator = new(Environment.TickCount);

        public static void SetCustomAction()
        {
            NoaController.SetCustomTapAction(
                CONTROLLER_F1_ACTION_INDEX,
                () =>
                {
                    int randomNumber = RandomGenerator.Next(1, 4);
                    string message = "F1 key was pressed.";

                    switch (randomNumber)
                    {
                        case 1:
                            Debug.Log($"[Log] {message}");

                            break;

                        case 2:
                            Debug.LogWarning($"[Warning] {message}");

                            break;

                        case 3:
                            Debug.LogError($"[Error] {message}");

                            break;
                    }
                },
                () => "Logging message");

            NoaController.SetCustomLongPressAction(
                CONTROLLER_F1_ACTION_INDEX,
                () =>
                {
                    NoaConsoleLog.Clear();
                    Debug.Log("F1 key was held down.");
                },
                () => "All logs cleared");

            NoaController.SetCustomActionType(CONTROLLER_F2_ACTION_INDEX, NoaController.CustomActionType.ToggleButton);

            NoaController.SetCustomToggleAction(
                CONTROLLER_F2_ACTION_INDEX,
                (isToggleOn) =>
                {
                    Debug.Log($"F2 toggle changed : {isToggleOn}");

                    if (isToggleOn)
                    {
                        NoaDebug.SetOverlayEnabled(NoaDebug.OverlayFeatures.ConsoleLog, true);
                    }
                    else
                    {
                        NoaDebug.SetOverlayEnabled(NoaDebug.OverlayFeatures.ConsoleLog, false);
                    }
                },
                (_) => "Console-log overlay toggled",
                initialState: NoaDebug.GetOverlayEnabled(NoaDebug.OverlayFeatures.ConsoleLog));

            NoaController.SetCustomActionType(CONTROLLER_F3_ACTION_INDEX, NoaController.CustomActionType.ToggleButton);

            NoaController.SetCustomToggleAction(
                CONTROLLER_F3_ACTION_INDEX,
                (isToggleOn) =>
                {
                    Debug.Log($"F3 toggle changed : {isToggleOn}");

                    if (isToggleOn)
                    {
                        NoaDebug.SetOverlayEnabled(NoaDebug.OverlayFeatures.Profiler, true);
                    }
                    else
                    {
                        NoaDebug.SetOverlayEnabled(NoaDebug.OverlayFeatures.Profiler, false);
                    }
                },
                (_) => "Profiler overlay toggled",
                initialState: NoaDebug.GetOverlayEnabled(NoaDebug.OverlayFeatures.Profiler));

            const bool isUiElementInitialVisible = true;
            NoaController.SetCustomActionType(CONTROLLER_F4_ACTION_INDEX, NoaController.CustomActionType.ToggleButton);
            NoaController.SetCustomToggleAction(
                CONTROLLER_F4_ACTION_INDEX,
                (isToggleOn) =>
                {
                    Debug.Log($"F4 toggle changed : {isToggleOn}");

                    NoaUIElement.SetAllUIElementsVisibility(isToggleOn);
                },
                (_) => "UIElement visible toggled",
                initialState:  isUiElementInitialVisible);
            NoaUIElement.SetAllUIElementsVisibility(isUiElementInitialVisible);

            NoaController.SetCustomTapAction(
                CONTROLLER_F5_ACTION_INDEX,
                () =>
                {
                    Debug.Log("F5 key was pressed.");
                    NoaDebug.Show(NoaDebug.MenuType.Command);
                },
                () => "Open command");

            NoaController.SetScreenshotCallbacks(new CustomScreenshotCallbacks());
        }
    }
}
#endif
