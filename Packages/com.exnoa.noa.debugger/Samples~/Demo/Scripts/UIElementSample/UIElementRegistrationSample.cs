#if NOA_DEBUGGER
using System;
using NoaDebugger;
using UnityEngine;
using UnityEngine.Profiling;

namespace NoaDebuggerDemo
{
    public class UIElementRegistrationSample
    {
        const int UI_ELEMENT_FONT_SIZE_SCALING_RATIO = 75;
        const AnchorType PORTRAIT_ANCHOR_TYPE = AnchorType.LowerRight;
        const AnchorType LANDSCAPE_ANCHOR_TYPE = AnchorType.LowerCenter;

        public AnchorType CurrentAnchorType { get; private set; }

        public UIElementRegistrationSample(bool isPortrait)
        {
            SetAlignment();
            RegisterUIElement(isPortrait);
        }

        void SetAlignment()
        {
            NoaUIElement.SetVerticalAlignment(PORTRAIT_ANCHOR_TYPE);
            NoaUIElement.SetHorizontalAlignment(LANDSCAPE_ANCHOR_TYPE);
        }

        public void RegisterUIElement(bool isPortrait)
        {
            CurrentAnchorType = isPortrait ? PORTRAIT_ANCHOR_TYPE : LANDSCAPE_ANCHOR_TYPE;

            string defaultSpace = "   ";
            string dataToPerformanceSpace = "    ";
            if (isPortrait)
            {
                defaultSpace = "";
                dataToPerformanceSpace = "";
            }

            const string fpsValueColor = "#00FB57";
            const string memoryValueColor = "#47BCFF";

            NoaUIElement.RegisterUIElement(
                NoaUITextElement.Create(
                    "NoaDebuggerBuildNumberKey",
                    $"<size={UI_ELEMENT_FONT_SIZE_SCALING_RATIO}%>Build : 9999",
                    CurrentAnchorType));

            NoaUIElement.RegisterUIElement(
                NoaUITextElement.Create(
                    "NoaDebuggerUserIdKey",
                    $"<size={UI_ELEMENT_FONT_SIZE_SCALING_RATIO}%>{defaultSpace}UserID : 12345",
                    CurrentAnchorType));

            NoaUIElement.RegisterUIElement(
                NoaUITextElement.Create(
                    "NoaDebuggerServerTimeKey",
                    () => $"<size={UI_ELEMENT_FONT_SIZE_SCALING_RATIO}%>{defaultSpace}ServerTime : {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}",
                    CurrentAnchorType));

            NoaUIElement.RegisterUIElement(
                NoaUITextElement.Create(
                    "NoaDebuggerCurrentFpsKey",
                    () =>
                    {
                        float currentFps = 0;
                        string elapsedTime = "0";

                        if (NoaProfiler.ProfilerInfo != null)
                        {
                            var fpsInfo = NoaProfiler.ProfilerInfo.FpsInfo;
                            float time = Mathf.Ceil(NoaProfiler.ProfilerInfo.FpsInfo.ElapsedTime * 10000) / 10;

                            if (time > 10)
                            {
                                elapsedTime = time.ToString("0.0");
                            }
                            else
                            {
                                elapsedTime = " " + time.ToString("0.0");
                            }

                            currentFps = fpsInfo.CurrentFps;
                        }

                        return $"<size={UI_ELEMENT_FONT_SIZE_SCALING_RATIO}%>{dataToPerformanceSpace}FPS : <color={fpsValueColor}>{currentFps} ({elapsedTime})</color>";
                    },
                    CurrentAnchorType));

            NoaUIElement.RegisterUIElement(
                NoaUITextElement.Create(
                    "NoaDebuggerMemorySizeKey",
                    () =>
                    {
                        var totalAllocatedMemory = Profiler.GetTotalAllocatedMemoryLong();
                        string allocatedMemory = $"{((double)totalAllocatedMemory / 1024f / 1024f):0.0}";
                        var totalReservedMemory = Profiler.GetTotalReservedMemoryLong();
                        string reservedMemory = $"{((double)totalReservedMemory / 1024f / 1024f):0.0}";

                        return $"<size={UI_ELEMENT_FONT_SIZE_SCALING_RATIO}%>{defaultSpace}MEM : <color={memoryValueColor}>{allocatedMemory} / {reservedMemory}</color>";
                    },
                    CurrentAnchorType));
        }

        public void ReregistrationForOrientation(bool isPortrait)
        {
            UnregisterAllUIElements();
            RegisterUIElement(isPortrait);
        }

        void UnregisterAllUIElements()
        {
            NoaUIElement.UnregisterAllUIElements();
        }
    }
}
#endif
