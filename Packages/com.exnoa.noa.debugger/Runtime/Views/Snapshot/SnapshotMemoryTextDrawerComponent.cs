using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class SnapshotMemoryTextDrawerComponent : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI _memoryCapacityLabel;

        [SerializeField]
        TextMeshProUGUI _memoryCapacity;

        [SerializeField]
        TextMeshProUGUI _currentMemory;

        [SerializeField]
        TextMeshProUGUI _maxAndMinMemory;

        [SerializeField]
        TextMeshProUGUI _averageMemory;

        void Awake()
        {
            Assert.IsNotNull(_memoryCapacityLabel);
            Assert.IsNotNull(_memoryCapacity);
            Assert.IsNotNull(_currentMemory);
            Assert.IsNotNull(_maxAndMinMemory);
            Assert.IsNotNull(_averageMemory);
        }

        public void OnShowMemoryText(MemoryUnchangingInfo info, Color? enableTextColor = null)
        {
            Color enableColor = enableTextColor ?? NoaDebuggerDefine.TextColors.Dynamic;

            _memoryCapacityLabel.text = ProfilerDrawerHelper.GetTotalMemoryLabelText(info.ProfilingType);

            if (info.IsViewHyphen)
            {
                ProfilerDrawerHelper.ShowHyphenValue(_memoryCapacity);
                ProfilerDrawerHelper.ShowHyphenValue(_currentMemory);
                ProfilerDrawerHelper.ShowHyphenValue(_maxAndMinMemory);
                ProfilerDrawerHelper.ShowHyphenValue(_averageMemory);
            }

            else if (!info.IsValid)
            {
                ProfilerDrawerHelper.ShowMissingValue(_memoryCapacity);
                ProfilerDrawerHelper.ShowMissingValue(_currentMemory);
                ProfilerDrawerHelper.ShowMissingValue(_maxAndMinMemory);
                ProfilerDrawerHelper.ShowMissingValue(_averageMemory);
            }

            else
            {
                switch (info.ProfilingType)
                {
                    case NoaProfiler.MemoryProfilingType.Unity:
                        _ShowUnityMemory(info);
                        break;

                    case NoaProfiler.MemoryProfilingType.Native:
                        _ShowNativeMemory(info);
                        break;

                    case NoaProfiler.MemoryProfilingType.Mono:
                        _ShowMonoMemory(info);
                        break;
                }

                _memoryCapacity.color = enableColor;
                _currentMemory.color = enableColor;
                _maxAndMinMemory.color = enableColor;
                _averageMemory.color = enableColor;
            }
        }

        void _ShowUnityMemory(MemoryUnchangingInfo info)
        {
            string currentReservedMemoryText = ProfilerDrawerHelper.GetMemoryMBText(info.CurrentReservedMemoryMB);
            string maxReservedMemoryText = ProfilerDrawerHelper.GetMemoryMBText(info.MaxReservedMemoryMB);
            _memoryCapacity.text = $"{currentReservedMemoryText} ({maxReservedMemoryText})";

            _currentMemory.text = ProfilerDrawerHelper.GetMemoryMBText(info.CurrentAllocatedMemoryMB);

            string maxMemoryText = ProfilerDrawerHelper.GetMemoryMBText(info.MaxAllocatedMemoryMB);
            string minMemoryText = ProfilerDrawerHelper.GetMemoryMBText(info.MinAllocatedMemoryMB);

            _maxAndMinMemory.text = ProfilerDrawerHelper.GetMaxMinText(maxMemoryText, minMemoryText);
            _averageMemory.text = ProfilerDrawerHelper.GetMemoryMBText(info.AverageAllocatedMemoryMB);
        }

        void _ShowNativeMemory(MemoryUnchangingInfo info)
        {
            var totalMemory = (long)DataUnitConverterModel.MBToByte(info.TotalNativeMemoryMB);
            _memoryCapacity.text = DataUnitConverterModel.ToHumanReadableBytes(totalMemory);

            _currentMemory.text = ProfilerDrawerHelper.GetMemoryMBText(info.CurrentNativeMemoryMB);

            string maxMemoryText = ProfilerDrawerHelper.GetMemoryMBText(info.MaxNativeMemoryMB);
            string minMemoryText = ProfilerDrawerHelper.GetMemoryMBText(info.MinNativeMemoryMB);

            _maxAndMinMemory.text = ProfilerDrawerHelper.GetMaxMinText(maxMemoryText, minMemoryText);
            _averageMemory.text = ProfilerDrawerHelper.GetMemoryMBText(info.AverageNativeMemoryMB);
        }

        void _ShowMonoMemory(MemoryUnchangingInfo info)
        {
            string currentMonoHeapSizeText = ProfilerDrawerHelper.GetMemoryMBText(info.CurrentMonoHeapSizeMB);
            string maxMonoHeapSizeText = ProfilerDrawerHelper.GetMemoryMBText(info.MaxMonoHeapSizeMB);
            _memoryCapacity.text = $"{currentMonoHeapSizeText} ({maxMonoHeapSizeText})";

            _currentMemory.text = ProfilerDrawerHelper.GetMemoryMBText(info.CurrentMonoUsedSizeMB);

            string maxMemoryText = ProfilerDrawerHelper.GetMemoryMBText(info.MaxMonoUsedSizeMB);
            string minMemoryText = ProfilerDrawerHelper.GetMemoryMBText(info.MinMonoUsedSizeMB);

            _maxAndMinMemory.text = ProfilerDrawerHelper.GetMaxMinText(maxMemoryText, minMemoryText);
            _averageMemory.text = ProfilerDrawerHelper.GetMemoryMBText(info.AverageMonoUsedSizeMB);
        }

        void OnDestroy()
        {
            _memoryCapacityLabel = default;
            _memoryCapacity = default;
            _currentMemory = default;
            _maxAndMinMemory = default;
            _averageMemory = default;
        }
    }
}
