using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class RenderingChartDrawerComponent : ChartDrawerComponentBase
    {
        static readonly Dictionary<float, string> IntegerValueRulerStringCache = new();

        protected override void OnInitialize()
        {
            _chart.SetUpdateRulersCallback(RenderingChartDrawerComponent.OnUpdateRenderingChartRulers);
        }

        public void OnShowRenderingChart(RenderingUnchangingInfo info)
        {
            _chart.SetValueHistoryBuffer(info.CurrentValueHistory);
        }

        static void OnUpdateRenderingChartRulers(float maxValue, float[] positions, string[] labels)
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
                if (RenderingChartDrawerComponent.IntegerValueRulerStringCache.TryGetValue(value, out string rulerText))
                {
                    return rulerText;
                }
                rulerText = value.ToString(CultureInfo.InvariantCulture);
                RenderingChartDrawerComponent.IntegerValueRulerStringCache[value] = rulerText;
                return rulerText;
            }
        }
    }
}
