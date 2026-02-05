using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class MemoryOverlayView : ProfilerOverlayFeatureViewBase<MemoryUnchangingInfo, MemoryChartDrawerComponent>
    {
        protected override Color TextColor => NoaDebuggerDefine.TextColors.ProfilerMemory;

        [SerializeField]
        TextMeshProUGUI _memoryLabelsText;

        [SerializeField]
        TextMeshProUGUI _memoryValuesText;

        [SerializeField]
        TextMeshProUGUI _graphLabelText;

        protected override void OnInitialize()
        {
            Assert.IsNotNull(_memoryLabelsText);
            Assert.IsNotNull(_memoryValuesText);
            Assert.IsNotNull(_graphLabelText);
        }

        public override void UpdateView(MemoryUnchangingInfo data)
        {
            if (!_rootObject.activeInHierarchy || data == null)
            {
                return;
            }

            _UpdateText(data);
            _UpdateGraph(data);
        }

        protected override bool _IsShowGraph()
        {
            KeyValuePair<string, bool> isGraphShowingInfo = NoaDebuggerPrefsDefine.IsMemoryGraphShowingKeyValue;
            bool isGraphShowing = NoaDebuggerPrefs.GetBoolean(isGraphShowingInfo.Key, isGraphShowingInfo.Value);
            return isGraphShowing && _isShowGraphOnOverlay;
        }

        void _UpdateText(MemoryUnchangingInfo data)
        {
            var textInfoList = new List<(string, float)>();

            switch (data.ProfilingType)
            {
                case NoaProfiler.MemoryProfilingType.Unity:
                    if (_textType == NoaProfiler.OverlayTextType.Simple)
                    {
                        textInfoList.Add((ProfilerDrawerHelper.AllocatedLabel, data.CurrentAllocatedMemoryMB));
                    }
                    else
                    {
                        textInfoList.Add((ProfilerDrawerHelper.AllocatedLabel, data.CurrentAllocatedMemoryMB));
                        textInfoList.Add((ProfilerDrawerHelper.ReservedLabel, data.CurrentReservedMemoryMB));
                        textInfoList.Add((ProfilerDrawerHelper.MaxReservedLabel, data.MaxReservedMemoryMB));
                    }

                    break;

                case NoaProfiler.MemoryProfilingType.Native:
                    if (_textType == NoaProfiler.OverlayTextType.Simple)
                    {
                        textInfoList.Add((ProfilerDrawerHelper.CurrentLabel, data.CurrentNativeMemoryMB));
                    }
                    else
                    {
                        textInfoList.Add((ProfilerDrawerHelper.CurrentLabel, data.CurrentNativeMemoryMB));
                        textInfoList.Add((ProfilerDrawerHelper.MaxLabel, data.MaxNativeMemoryMB));
                        textInfoList.Add((ProfilerDrawerHelper.MinLabel, data.MinNativeMemoryMB));
                        textInfoList.Add((ProfilerDrawerHelper.AverageLabel, data.AverageNativeMemoryMB));
                    }

                    break;

                case NoaProfiler.MemoryProfilingType.Mono:
                    if (_textType == NoaProfiler.OverlayTextType.Simple)
                    {
                        textInfoList.Add((ProfilerDrawerHelper.MonoUsedLabel, data.CurrentMonoUsedSizeMB));
                    }
                    else
                    {
                        textInfoList.Add((ProfilerDrawerHelper.MonoUsedLabel, data.CurrentMonoUsedSizeMB));
                        textInfoList.Add((ProfilerDrawerHelper.MonoHeapLabel, data.CurrentMonoHeapSizeMB));
                        textInfoList.Add((ProfilerDrawerHelper.MaxMonoHeapLabel, data.MaxMonoHeapSizeMB));
                    }

                    break;
            }

            string[] labels = textInfoList.Select(x => _GetLabelText(x.Item1)).ToArray();
            string[] values = textInfoList.Select(x => _GetValueText(x.Item2, data)).ToArray();

            _memoryLabelsText.text = string.Join("\n", labels);
            _memoryValuesText.text = string.Join("\n", values);
        }

        void _UpdateGraph(MemoryUnchangingInfo data)
        {
            if (!_IsShowGraph())
            {
                return;
            }

            _graph.OnShowMemoryChart(data);

            _graphLabelText.text = ProfilerDrawerHelper.GetMemoryProfilingTypeText(data.ProfilingType);
        }

        string _GetValueText(float value, MemoryUnchangingInfo data)
        {
            bool isViewHyphen = data.IsViewHyphen;
            bool isValid = data.IsValid;

            if (isViewHyphen || !isValid)
            {
                return _GetValueText(value, isViewHyphen, isValid);
            }

            return $"{_GetValueText(value)} {ProfilerDrawerHelper.MemoryUsageUnits}";
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _memoryLabelsText = default;
            _memoryValuesText = default;
            _graphLabelText = default;
        }
    }
}
