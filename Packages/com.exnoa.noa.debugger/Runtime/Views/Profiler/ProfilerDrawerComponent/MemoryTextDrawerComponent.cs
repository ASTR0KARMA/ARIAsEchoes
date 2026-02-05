using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class MemoryTextDrawerComponent : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI _memoryCapacityLabel;

        [SerializeField]
        TextMeshProUGUI _memoryCapacity;

        [SerializeField]
        TextMeshProUGUI _currentMemoryLabel;

        [SerializeField]
        TextMeshProUGUI _currentMemory;

        [SerializeField]
        TextMeshProUGUI _maxAndMinAndAverageMemory;

        void Awake()
        {
            Assert.IsNotNull(_memoryCapacityLabel);
            Assert.IsNotNull(_memoryCapacity);
            Assert.IsNotNull(_currentMemoryLabel);
            Assert.IsNotNull(_currentMemory);
            Assert.IsNotNull(_maxAndMinAndAverageMemory);
        }

        public void OnShowMemoryText(MemoryUnchangingInfo info, Color? enableTextColor = null)
        {
            Color enableColor = enableTextColor ?? NoaDebuggerDefine.TextColors.Dynamic;

            _memoryCapacityLabel.text = ProfilerDrawerHelper.GetTotalMemoryLabelText(info.ProfilingType);
            _currentMemoryLabel.text = ProfilerDrawerHelper.GetCurrentMemoryLabelText(info.ProfilingType);

            if (info.IsViewHyphen)
            {
                ProfilerDrawerHelper.ShowHyphenValue(_memoryCapacity);
                ProfilerDrawerHelper.ShowHyphenValue(_currentMemory);
                ProfilerDrawerHelper.ShowHyphenValue(_maxAndMinAndAverageMemory);
            }

            else if (!info.IsValid)
            {
                ProfilerDrawerHelper.ShowMissingValue(_memoryCapacity);
                ProfilerDrawerHelper.ShowMissingValue(_currentMemory);
                ProfilerDrawerHelper.ShowMissingValue(_maxAndMinAndAverageMemory);
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
                _maxAndMinAndAverageMemory.color = enableColor;
            }
        }

        void _ShowUnityMemory(MemoryUnchangingInfo info)
        {
            string currentReservedMemoryText = ProfilerDrawerHelper.GetMemoryMBText(info.CurrentReservedMemoryMB);
            string maxReservedMemoryText = ProfilerDrawerHelper.GetMemoryMBText(info.MaxReservedMemoryMB);
            _memoryCapacity.text = $"{currentReservedMemoryText} ({maxReservedMemoryText})";

            string maxMemoryText = ProfilerDrawerHelper.GetMemoryMBText(info.MaxAllocatedMemoryMB);
            string minMemoryText = ProfilerDrawerHelper.GetMemoryMBText(info.MinAllocatedMemoryMB);
            string avgMemoryText = ProfilerDrawerHelper.GetMemoryMBText(info.AverageAllocatedMemoryMB);

            _currentMemory.text = ProfilerDrawerHelper.GetMemoryMBText(info.CurrentAllocatedMemoryMB);

            _maxAndMinAndAverageMemory.text =
                $"{ProfilerDrawerHelper.GetMaxMinText(maxMemoryText, minMemoryText)} / {avgMemoryText}";
        }

        void _ShowNativeMemory(MemoryUnchangingInfo info)
        {
            var totalMemory = (long)DataUnitConverterModel.MBToByte(info.TotalNativeMemoryMB);
            _memoryCapacity.text = DataUnitConverterModel.ToHumanReadableBytes(totalMemory);

            string maxMemoryText = ProfilerDrawerHelper.GetMemoryMBText(info.MaxNativeMemoryMB);
            string minMemoryText = ProfilerDrawerHelper.GetMemoryMBText(info.MinNativeMemoryMB);
            string avgMemoryText = ProfilerDrawerHelper.GetMemoryMBText(info.AverageNativeMemoryMB);

            _currentMemory.text = ProfilerDrawerHelper.GetMemoryMBText(info.CurrentNativeMemoryMB);

            _maxAndMinAndAverageMemory.text =
                ProfilerDrawerHelper.GetMaxMinAvgText(maxMemoryText, minMemoryText, avgMemoryText);
        }

        void _ShowMonoMemory(MemoryUnchangingInfo info)
        {
            string currentMonoHeapSizeText = ProfilerDrawerHelper.GetMemoryMBText(info.CurrentMonoHeapSizeMB);
            string maxMonoHeapSizeText = ProfilerDrawerHelper.GetMemoryMBText(info.MaxMonoHeapSizeMB);
            _memoryCapacity.text = $"{currentMonoHeapSizeText} ({maxMonoHeapSizeText})";

            string maxMemoryText = ProfilerDrawerHelper.GetMemoryMBText(info.MaxMonoUsedSizeMB);
            string minMemoryText = ProfilerDrawerHelper.GetMemoryMBText(info.MinMonoUsedSizeMB);
            string avgMemoryText = ProfilerDrawerHelper.GetMemoryMBText(info.AverageMonoUsedSizeMB);

            _currentMemory.text = ProfilerDrawerHelper.GetMemoryMBText(info.CurrentMonoUsedSizeMB);

            _maxAndMinAndAverageMemory.text =
                $"{ProfilerDrawerHelper.GetMaxMinText(maxMemoryText, minMemoryText)} / {avgMemoryText}";
        }

        void OnDestroy()
        {
            _memoryCapacityLabel = default;
            _memoryCapacity = default;
            _currentMemoryLabel = default;
            _currentMemory = default;
            _maxAndMinAndAverageMemory = default;
        }
    }
}
