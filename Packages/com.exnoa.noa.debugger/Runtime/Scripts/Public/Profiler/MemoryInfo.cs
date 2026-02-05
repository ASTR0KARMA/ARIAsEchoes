using System;
using System.Collections.Generic;
using UnityEngine;

namespace NoaDebugger
{
    /// <summary>
    /// Memory information.
    /// </summary>
    public sealed class MemoryInfo
    {
        /// <summary>
        /// Recent reserved memory.
        /// </summary>
        public float CurrentReservedMemoryMB { private set; get; }

        /// <summary>
        /// Maximum measured reserved memory.
        /// </summary>
        public float MaxReservedMemoryMB { private set; get; }

        /// <summary>
        /// Recent allocated memory.
        /// </summary>
        public float CurrentAllocatedMemoryMB { private set; get; } = -1;

        /// <summary>
        /// Maximum measured allocated memory.
        /// </summary>
        public float MaxAllocatedMemoryMB { private set; get; }

        /// <summary>
        /// Minimum measured allocated memory.
        /// </summary>
        public float MinAllocatedMemoryMB { private set; get; } = float.MaxValue;

        /// <summary>
        /// Average allocated memory.
        /// </summary>
        public float AverageAllocatedMemoryMB { private set; get; }

        /// <summary>
        /// Default total memory capacity (RAM capacity of device).
        /// </summary>
        internal static readonly float DefaultTotalNativeMemoryMB = SystemInfo.systemMemorySize;

        /// <summary>
        /// Total memory capacity (default is the RAM capacity of device).
        /// </summary>
        public float TotalNativeMemoryMB { private set; get; } = MemoryInfo.DefaultTotalNativeMemoryMB;

        /// <summary>
        /// Total memory capacity (default is the RAM capacity of device).
        /// </summary>
        [Obsolete("Use MemoryInfo.TotalNativeMemoryMB instead.")]
        public float TotalMemoryMB => TotalNativeMemoryMB;

        /// <summary>
        /// Recent native memory usage.
        /// </summary>
        public float CurrentNativeMemoryMB { private set; get; } = -1;

        /// <summary>
        /// Recent native memory usage.
        /// </summary>
        [Obsolete("Use MemoryInfo.CurrentNativeMemoryMB instead.")]
        public float CurrentMemoryMB => CurrentNativeMemoryMB;

        /// <summary>
        /// Maximum measured native memory usage.
        /// </summary>
        public float MaxNativeMemoryMB { private set; get; }

        /// <summary>
        /// Maximum measured native memory usage.
        /// </summary>
        [Obsolete("Use MemoryInfo.MaxNativeMemoryMB instead.")]
        public float MaxMemoryMB => MaxNativeMemoryMB;

        /// <summary>
        /// Minimum measured native memory usage.
        /// </summary>
        public float MinNativeMemoryMB { private set; get; } = float.MaxValue;

        /// <summary>
        /// Minimum measured native memory usage.
        /// </summary>
        [Obsolete("Use MemoryInfo.MinNativeMemoryMB instead.")]
        public float MinMemoryMB => MinNativeMemoryMB;

        /// <summary>
        /// Average native memory usage.
        /// </summary>
        public float AverageNativeMemoryMB { private set; get; }

        /// <summary>
        /// Average native memory usage.
        /// </summary>
        [Obsolete("Use MemoryInfo.AverageNativeMemoryMB instead.")]
        public float AverageMemoryMB => AverageNativeMemoryMB;

        /// <summary>
        /// Recent mono heap size.
        /// </summary>
        public float CurrentMonoHeapSizeMB { private set; get; }

        /// <summary>
        /// Maximum measured mono heap size.
        /// </summary>
        public float MaxMonoHeapSizeMB { private set; get; }

        /// <summary>
        /// Recent mono used size.
        /// </summary>
        public float CurrentMonoUsedSizeMB { private set; get; } = -1;

        /// <summary>
        /// Maximum measured mono used size.
        /// </summary>
        public float MaxMonoUsedSizeMB { private set; get; }

        /// <summary>
        /// Minimum measured mono used size.
        /// </summary>
        public float MinMonoUsedSizeMB { private set; get; } = float.MaxValue;

        /// <summary>
        /// Average mono used size.
        /// </summary>
        public float AverageMonoUsedSizeMB { private set; get; }


        /// <summary>
        /// Whether it is being measured or not.
        /// </summary>
        public bool IsProfiling { private set; get; }

        /// <summary>
        /// Memory measurement type.
        /// </summary>
        public NoaProfiler.MemoryProfilingType ProfilingType { private set; get; }

        /// <summary>
        /// Memory consumption history (for graph display).
        /// </summary>
        internal RingBuffer<float[]> CurrentMemoryHistory { private set; get; }

        List<float[]> _currentMemoryHistoryValueBuffer = null;

        /// <summary>
        /// Whether to display the graph or not.
        /// </summary>
        internal bool IsGraphShowing { private set; get; }

        /// <summary>
        /// Whether profiling has started at least once.
        /// </summary>
        internal bool IsStartProfiling { private set; get; }

        /// <summary>
        /// Generates MemoryInfo.
        /// </summary>
        internal MemoryInfo()
        {
            _InitializeHistoryBuffer();
        }

        /// <summary>
        /// Specifies the total memory capacity.
        /// </summary>
        /// <param name="totalMemoryMB">Specifies the total memory capacity. If omitted, it will be the RAM capacity of the device.</param>
        internal void SetTotalNativeMemoryMB(float totalMemoryMB = -1)
        {
            TotalNativeMemoryMB = (totalMemoryMB < 0)
                ? MemoryInfo.DefaultTotalNativeMemoryMB
                : totalMemoryMB;
        }

        /// <summary>
        /// Resets the measured values.
        /// </summary>
        internal void ResetProfiledValue()
        {
            CurrentReservedMemoryMB = 0;
            MaxReservedMemoryMB = 0;
            CurrentAllocatedMemoryMB = -1;
            MaxAllocatedMemoryMB = 0;
            MinAllocatedMemoryMB = float.MaxValue;
            AverageAllocatedMemoryMB = 0;
            CurrentNativeMemoryMB = -1;
            MaxNativeMemoryMB = 0;
            MinNativeMemoryMB = float.MaxValue;
            AverageNativeMemoryMB = 0;
            CurrentMonoHeapSizeMB = 0;
            MaxMonoHeapSizeMB = 0;
            CurrentMonoUsedSizeMB = -1;
            MaxMonoUsedSizeMB = 0;
            MinMonoUsedSizeMB = float.MaxValue;
            AverageMonoUsedSizeMB = 0;
        }

        /// <summary>
        /// Updates the unity memory information.
        /// </summary>
        /// <param name="currentReservedMemoryMB">Specifies the current reserved memory.</param>
        /// <param name="currentAllocatedMemoryMB">Specifies the current allocated memory.</param>
        /// <param name="averageAllocatedMemoryMB">Specifies the average allocated memory.</param>
        internal void RefreshUnityMemory(float currentReservedMemoryMB, float currentAllocatedMemoryMB, float averageAllocatedMemoryMB)
        {
            CurrentReservedMemoryMB = currentReservedMemoryMB;
            MaxReservedMemoryMB = Mathf.Max(currentReservedMemoryMB, MaxReservedMemoryMB);
            CurrentAllocatedMemoryMB = currentAllocatedMemoryMB;
            MaxAllocatedMemoryMB = Mathf.Max(currentAllocatedMemoryMB, MaxAllocatedMemoryMB);
            MinAllocatedMemoryMB = Mathf.Min(currentAllocatedMemoryMB, MinAllocatedMemoryMB);
            AverageAllocatedMemoryMB = averageAllocatedMemoryMB;

            if (ProfilingType == NoaProfiler.MemoryProfilingType.Unity)
            {
                _AddHistory(currentReservedMemoryMB - currentAllocatedMemoryMB, currentAllocatedMemoryMB);
            }
        }

        /// <summary>
        /// Updates the native memory information.
        /// </summary>
        /// <param name="currentMemoryMB">Specifies the current memory usage.</param>
        /// <param name="averageMemoryMB">Specifies the average memory usage.</param>
        internal void RefreshNativeMemory(float currentMemoryMB, float averageMemoryMB)
        {
            CurrentNativeMemoryMB = currentMemoryMB;
            MaxNativeMemoryMB = Mathf.Max(currentMemoryMB, MaxNativeMemoryMB);
            MinNativeMemoryMB = Mathf.Min(currentMemoryMB, MinNativeMemoryMB);
            AverageNativeMemoryMB = averageMemoryMB;

            if (ProfilingType == NoaProfiler.MemoryProfilingType.Native)
            {
                _AddHistory(0, currentMemoryMB);
            }
        }

        /// <summary>
        /// Updates the mono memory information.
        /// </summary>
        /// <param name="currentMonoHeapSizeMB">Specifies the current mono heap size.</param>
        /// <param name="currentMonoUsedSizeMB">Specifies the current mono used size.</param>
        /// <param name="averageMonoUsedSizeMB">Specifies the average mono used size.</param>
        internal void RefreshMonoMemory(float currentMonoHeapSizeMB, float currentMonoUsedSizeMB, float averageMonoUsedSizeMB)
        {
            CurrentMonoHeapSizeMB = currentMonoHeapSizeMB;
            MaxMonoHeapSizeMB = Mathf.Max(currentMonoHeapSizeMB, MaxMonoHeapSizeMB);
            CurrentMonoUsedSizeMB = currentMonoUsedSizeMB;
            MaxMonoUsedSizeMB = Mathf.Max(currentMonoUsedSizeMB, MaxMonoUsedSizeMB);
            MinMonoUsedSizeMB = Mathf.Min(currentMonoUsedSizeMB, MinMonoUsedSizeMB);
            AverageMonoUsedSizeMB = averageMonoUsedSizeMB;

            if (ProfilingType == NoaProfiler.MemoryProfilingType.Mono)
            {
                _AddHistory(currentMonoHeapSizeMB - currentMonoUsedSizeMB, currentMonoUsedSizeMB);
            }
        }

        /// <summary>
        /// Starts measuring.
        /// </summary>
        internal void StartProfiling()
        {
            IsStartProfiling = true;
        }

        /// <summary>
        /// Changes the measurement state.
        /// </summary>
        /// <param name="isProfiling">Specifies the measurement state.</param>
        internal void ToggleProfiling(bool isProfiling)
        {
            IsProfiling = isProfiling;
        }

        /// <summary>
        /// Changes the graph display state.
        /// </summary>
        /// <param name="isShowing">Specifies the graph display state.</param>
        internal void ToggleGraphShowing(bool isShowing)
        {
            IsGraphShowing = isShowing;

            if (!isShowing)
            {
                CurrentMemoryHistory.Clear();
            }
        }

        /// <summary>
        /// Changes the measurement type.
        /// </summary>
        /// <param name="profilingType">Specifies the measurement type.</param>
        internal void ChangeProfilingType(NoaProfiler.MemoryProfilingType profilingType)
        {
            if (profilingType != ProfilingType)
            {
                _InitializeHistoryBuffer();
                ProfilingType = profilingType;
            }
        }

        /// <summary>
        /// Whether current measurement value is valid.
        /// </summary>
        internal bool IsValid()
        {
            float currentMemory = ProfilingType switch
            {
                NoaProfiler.MemoryProfilingType.Unity => CurrentAllocatedMemoryMB,
                NoaProfiler.MemoryProfilingType.Native => CurrentNativeMemoryMB,
                NoaProfiler.MemoryProfilingType.Mono => CurrentMonoUsedSizeMB,
                _ => CurrentNativeMemoryMB
            };

            return currentMemory >= 0;
        }

        /// <summary>
        /// Initializes the history buffer.
        /// </summary>
        void _InitializeHistoryBuffer()
        {
            int memoryHistoryCapacity = NoaDebuggerDefine.ProfilerChartHistoryCount / MemoryModel.UpdateIntervalFrames;
            CurrentMemoryHistory = new RingBuffer<float[]>(memoryHistoryCapacity);
            _currentMemoryHistoryValueBuffer = new List<float[]>(memoryHistoryCapacity);

            for (var i = 0; i < _currentMemoryHistoryValueBuffer.Capacity; ++i)
            {
                _currentMemoryHistoryValueBuffer.Add(new float[2]);
            }
        }

        /// <summary>
        /// Add history for graph display.
        /// </summary>
        /// <param name="currentMemoryCapacityMB">Specifies the current memory capacity for graph display.</param>
        /// <param name="currentMemoryUsageMB">Specifies the current memory usage for graph display.</param>
        void _AddHistory(float currentMemoryCapacityMB, float currentMemoryUsageMB)
        {
            if (IsGraphShowing)
            {
                int valueIndex = CurrentMemoryHistory.Tail;
                _currentMemoryHistoryValueBuffer[valueIndex][0] = currentMemoryCapacityMB;
                _currentMemoryHistoryValueBuffer[valueIndex][1] = currentMemoryUsageMB;
                CurrentMemoryHistory.Append(_currentMemoryHistoryValueBuffer[valueIndex]);
            }
        }
    }
}
