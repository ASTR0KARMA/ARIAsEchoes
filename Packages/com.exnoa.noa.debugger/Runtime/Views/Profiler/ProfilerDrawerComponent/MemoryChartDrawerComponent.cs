using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class MemoryChartDrawerComponent : ChartDrawerComponentBase
    {
        static readonly Dictionary<float, string> MemoryRulerStringCache = new();

        NoaProfiler.MemoryProfilingType? _cachedProfilingType = null;

        protected override void OnInitialize()
        {
            _chart.SetUpdateRulersCallback(MemoryChartDrawerComponent.OnUpdateMemoryChartRulers);
        }

        public void OnShowMemoryChart(MemoryUnchangingInfo info)
        {
            _chart.SetValueHistoryBuffer(info.CurrentMemoryHistory);

            if (_cachedProfilingType != info.ProfilingType)
            {
                _SetStackDisplayAttributes(info.ProfilingType);
                _cachedProfilingType = info.ProfilingType;
            }
        }

        void _SetStackDisplayAttributes(NoaProfiler.MemoryProfilingType profilingType)
        {
            _chart.SetStackDisplayAttributesShows(0, profilingType != NoaProfiler.MemoryProfilingType.Native);

            switch (profilingType)
            {
                case NoaProfiler.MemoryProfilingType.Unity:
                    _chart.SetStackDisplayAttributesText(0, ProfilerDrawerHelper.ReservedLabel);
                    _chart.SetStackDisplayAttributesText(1, ProfilerDrawerHelper.AllocatedLabel);
                    break;
                case NoaProfiler.MemoryProfilingType.Native:
                    _chart.SetStackDisplayAttributesText(1, ProfilerDrawerHelper.DefaultMemoryChartLabel);
                    break;
                case NoaProfiler.MemoryProfilingType.Mono:
                    _chart.SetStackDisplayAttributesText(0, ProfilerDrawerHelper.MonoHeapLabel);
                    _chart.SetStackDisplayAttributesText(1, ProfilerDrawerHelper.MonoUsedLabel);
                    break;
            }
        }

        static void OnUpdateMemoryChartRulers(float maxValue, float[] positions, string[] labels)
        {
            Assert.AreEqual(positions.Length, 2, "Check 'Ruler Count' of StackedAreaChart.");
            Assert.AreEqual(labels.Length, 2, "Check 'Ruler Count' of StackedAreaChart.");

            positions[0] = ProfilerDrawerHelper.ToRulerValue(maxValue);
            labels[0] = toRulerText(positions[0]);
            positions[1] = positions[0] / 2;
            labels[1] = toRulerText(positions[1]);
            return;

            string toRulerText(float value)
            {
                if (MemoryChartDrawerComponent.MemoryRulerStringCache.TryGetValue(value, out string rulerText))
                {
                    return rulerText;
                }
                rulerText = $"{value.ToString(CultureInfo.InvariantCulture)}{ProfilerDrawerHelper.MemoryUsageUnits}";
                MemoryChartDrawerComponent.MemoryRulerStringCache[value] = rulerText;
                return rulerText;
            }
        }
    }
}
