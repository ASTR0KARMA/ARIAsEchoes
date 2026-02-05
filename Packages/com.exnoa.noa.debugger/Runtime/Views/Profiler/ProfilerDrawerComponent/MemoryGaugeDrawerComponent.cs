using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class MemoryGaugeDrawerComponent : MonoBehaviour
    {
        [SerializeField]
        GaugeChart _memoryGauge;

        void Awake()
        {
            Assert.IsNotNull(_memoryGauge);
        }

        public void OnShowMemoryGauge(MemoryUnchangingInfo info)
        {
            if (info.IsViewHyphen || !info.IsValid)
            {
                _memoryGauge.MaxValue = 0;
                _memoryGauge.Value = 0;
                return;
            }

            switch (info.ProfilingType)
            {
                case NoaProfiler.MemoryProfilingType.Unity:
                    _memoryGauge.MaxValue = info.CurrentReservedMemoryMB;
                    _memoryGauge.Value = info.CurrentAllocatedMemoryMB;
                    break;

                case NoaProfiler.MemoryProfilingType.Native:
                    _memoryGauge.MaxValue = info.TotalNativeMemoryMB;
                    _memoryGauge.Value = info.CurrentNativeMemoryMB;
                    break;

                case NoaProfiler.MemoryProfilingType.Mono:
                    _memoryGauge.MaxValue = info.CurrentMonoHeapSizeMB;
                    _memoryGauge.Value = info.CurrentMonoUsedSizeMB;
                    break;
            }
        }

        void OnDestroy()
        {
            _memoryGauge = default;
        }
    }
}
