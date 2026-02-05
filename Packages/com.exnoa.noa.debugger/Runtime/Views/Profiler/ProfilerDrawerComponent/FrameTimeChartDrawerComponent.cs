using System.Collections.Generic;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class FrameTimeChartDrawerComponent : ChartDrawerComponentBase
    {
        static readonly float[] FrameTimeChartFpsRulerValues =
        {
            1 / 64f,
            1 / 32f,
            1 / 16f,
            1 / 8f,
            1 / 4f,
            1 / 2f,
            1f,
            2f,
            4f,
            8f,
            15f,
            30f,
            60f,
            120f,
            240f,
            500f,
            1000f
        };

        static readonly Dictionary<float, string> FrameTimeRulerStringCache = new();

        protected override void OnInitialize()
        {
            FrameTimeChartDrawerComponent.CreateFrameTimeRulerStringCache();
            _chart.SetUpdateRulersCallback(FrameTimeChartDrawerComponent.OnUpdateFrameTimeChartRulers);
        }

        public void OnShowFrameTime(ProfilerFrameTimeViewInformation info)
        {
            _chart.SetValueHistoryBuffer(info._histories);
        }

        static void CreateFrameTimeRulerStringCache()
        {
            foreach (float fps in FrameTimeChartDrawerComponent.FrameTimeChartFpsRulerValues)
            {
                if (FrameTimeChartDrawerComponent.FrameTimeRulerStringCache.ContainsKey(fps))
                {
                    continue;
                }

                float frameTime = FrameTimeChartDrawerComponent.FpsToFrameTimeMilliseconds(fps);
                FrameTimeChartDrawerComponent.FrameTimeRulerStringCache[fps] = $"{(int)frameTime}ms ({fps}FPS)";
            }
        }

        static void OnUpdateFrameTimeChartRulers(float maxValue, float[] positions, string[] labels)
        {
            Assert.AreEqual(positions.Length, 2, "Check 'Ruler Count' of StackedAreaChart.");
            Assert.AreEqual(labels.Length, 2, "Check 'Ruler Count' of StackedAreaChart.");

            for (var i = 0; i < FrameTimeChartDrawerComponent.FrameTimeChartFpsRulerValues.Length; ++i)
            {
                float maxFps = FrameTimeChartDrawerComponent.FrameTimeChartFpsRulerValues[i];
                float maxFrameTime = FrameTimeChartDrawerComponent.FpsToFrameTimeMilliseconds(maxFps);

                if (maxValue > maxFrameTime)
                {
                    labels[0] = FrameTimeChartDrawerComponent.FrameTimeRulerStringCache[maxFps];
                    positions[0] = maxFrameTime;

                    if (i < FrameTimeChartDrawerComponent.FrameTimeChartFpsRulerValues.Length - 1)
                    {
                        float altFps = FrameTimeChartDrawerComponent.FrameTimeChartFpsRulerValues[i + 1];
                        labels[1] = FrameTimeChartDrawerComponent.FrameTimeRulerStringCache[altFps];
                        positions[1] = FrameTimeChartDrawerComponent.FpsToFrameTimeMilliseconds(altFps);
                    }

                    break;
                }
            }
        }

        static float FpsToFrameTimeMilliseconds(float fps) => (1 / fps) * 1000f;
    }
}
