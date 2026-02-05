using TMPro;
using UnityEngine;

namespace NoaDebugger
{
    static class ProfilerDrawerHelper
    {
        public static readonly string CurrentLabel = "Current";
        public static readonly string MaxLabel = "Max";
        public static readonly string MinLabel = "Min";
        public static readonly string AverageLabel = "Avg";

        public static readonly string DefaultMemoryChartLabel = "Memory";
        static readonly string DefaultTotalMemoryLabel = "Total";
        public static readonly string ReservedLabel = "Reserved";
        public static readonly string MaxReservedLabel = "Reserved (Max)";
        public static readonly string AllocatedLabel = "Allocated";
        public static readonly string MonoHeapLabel = "Mono Heap";
        public static readonly string MaxMonoHeapLabel = "Mono Heap (Max)";
        public static readonly string MonoUsedLabel = "Mono Used";

        public static readonly string SetPassCallsLabel = "SetPass Calls";
        public static readonly string DrawCallsLabel = "Draw Calls";
        public static readonly string BatchesLabel = "Batches";
        public static readonly string TrianglesLabel = "Triangles";
        public static readonly string VerticesLabel = "Vertices";

        public static readonly string FpsUnits = "fps";
        static readonly string ElapsedTimeUnits = "ms";
        public static readonly string MemoryUsageUnits = "MB";
        static readonly string DegreesCelsiusUnits = "deg C";

        public static void ShowHyphenValue(TextMeshProUGUI target)
        {
            target.text = NoaDebuggerDefine.HyphenValue;
            target.color = NoaDebuggerDefine.TextColors.Default;
        }

        public static void ShowMissingValue(TextMeshProUGUI target)
        {
            target.text = NoaDebuggerDefine.MISSING_VALUE;
            target.color = NoaDebuggerDefine.TextColors.Default;
        }

        public static string GetFpsText(int value)
        {
            return $"{value} {ProfilerDrawerHelper.FpsUnits}";
        }

        public static string GetElapsedTimeText(float value)
        {
            return $"{value} {ProfilerDrawerHelper.ElapsedTimeUnits}";
        }

        public static string GetTotalMemoryLabelText(NoaProfiler.MemoryProfilingType profilingType)
        {
            string label = profilingType switch
            {
                NoaProfiler.MemoryProfilingType.Unity => ProfilerDrawerHelper.MaxReservedLabel,
                NoaProfiler.MemoryProfilingType.Native => ProfilerDrawerHelper.DefaultTotalMemoryLabel,
                NoaProfiler.MemoryProfilingType.Mono => ProfilerDrawerHelper.MaxMonoHeapLabel,
                _ => ""
            };

            return $"{label}:";
        }

        public static string GetCurrentMemoryLabelText(NoaProfiler.MemoryProfilingType profilingType)
        {
            return profilingType switch
            {
                NoaProfiler.MemoryProfilingType.Unity => ProfilerDrawerHelper.AllocatedLabel,
                NoaProfiler.MemoryProfilingType.Native => ProfilerDrawerHelper.CurrentLabel,
                NoaProfiler.MemoryProfilingType.Mono => ProfilerDrawerHelper.MonoUsedLabel,
                _ => ""
            };
        }

        public static string GetMemoryMBText(float value)
        {
            return $"{value}{ProfilerDrawerHelper.MemoryUsageUnits}";
        }

        public static string GetRenderingValueText(long current, string max)
        {
            return $"{current} ({max})";
        }

        public static string GetTemperatureText(float value)
        {
            return $"{value} {ProfilerDrawerHelper.DegreesCelsiusUnits}";
        }

        public static string GetMaxMinText(string max, string min)
        {
            return $"{max} / {min}";
        }

        public static string GetMaxMinAvgText(string max, string min, string avg)
        {
            return $"{max} / {min} / {avg}";
        }

        public static string GetMemoryProfilingTypeText(object profilingType)
        {
            return $"{profilingType} Memory";
        }

        public static float ToRulerValue(float value)
        {
            float log = Mathf.Floor(Mathf.Log10(value));
            float rulerValue = Mathf.Pow(10, log);
            rulerValue *= Mathf.Floor(value / rulerValue);
            return Mathf.Floor(value / rulerValue) * rulerValue;
        }
    }
}
