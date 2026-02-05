using System;

namespace NoaDebugger
{
    /// <summary>
    /// Memory information that cannot be updated.
    /// </summary>
    public sealed class MemoryUnchangingInfo
    {
        /// <summary>
        /// Recent reserved memory.
        /// </summary>
        public float CurrentReservedMemoryMB { get; }

        /// <summary>
        /// Maximum measured reserved memory.
        /// </summary>
        public float MaxReservedMemoryMB { get; }

        /// <summary>
        /// Recent allocated memory.
        /// </summary>
        public float CurrentAllocatedMemoryMB { get; }

        /// <summary>
        /// Maximum measured allocated memory.
        /// </summary>
        public float MaxAllocatedMemoryMB { get; }

        /// <summary>
        /// Minimum measured allocated memory.
        /// </summary>
        public float MinAllocatedMemoryMB { get; }

        /// <summary>
        /// Average allocated memory.
        /// </summary>
        public float AverageAllocatedMemoryMB { get; }

        /// <summary>
        /// Total memory capacity.
        /// </summary>
        public float TotalNativeMemoryMB { get; }

        /// <summary>
        /// Total memory capacity.
        /// </summary>
        [Obsolete("Use MemoryUnchangingInfo.TotalNativeMemoryMB instead.")]
        public float TotalMemoryMB { get; }

        /// <summary>
        /// Recent native memory usage.
        /// </summary>
        public float CurrentNativeMemoryMB { get; }

        /// <summary>
        /// Recent native memory usage.
        /// </summary>
        [Obsolete("Use MemoryUnchangingInfo.CurrentNativeMemoryMB instead.")]
        public float CurrentMemoryMB { get; }

        /// <summary>
        /// Maximum measured native memory usage.
        /// </summary>
        public float MaxNativeMemoryMB { get; }

        /// <summary>
        /// Maximum measured native memory usage.
        /// </summary>
        [Obsolete("Use MemoryUnchangingInfo.MaxNativeMemoryMB instead.")]
        public float MaxMemoryMB { get; }

        /// <summary>
        /// Minimum measured native memory usage.
        /// </summary>
        public float MinNativeMemoryMB { get; }

        /// <summary>
        /// Minimum measured native memory usage.
        /// </summary>
        [Obsolete("Use MemoryUnchangingInfo.MinNativeMemoryMB instead.")]
        public float MinMemoryMB { get; }

        /// <summary>
        /// Average native memory usage.
        /// </summary>
        public float AverageNativeMemoryMB { get; }

        /// <summary>
        /// Average native memory usage.
        /// </summary>
        [Obsolete("Use MemoryUnchangingInfo.AverageNativeMemoryMB instead.")]
        public float AverageMemoryMB { get; }

        /// <summary>
        /// Recent mono heap size.
        /// </summary>
        public float CurrentMonoHeapSizeMB { get; }

        /// <summary>
        /// Maximum measured mono heap size.
        /// </summary>
        public float MaxMonoHeapSizeMB { get; }

        /// <summary>
        /// Recent mono used size.
        /// </summary>
        public float CurrentMonoUsedSizeMB { get; }

        /// <summary>
        /// Maximum measured mono used size.
        /// </summary>
        public float MaxMonoUsedSizeMB { get; }

        /// <summary>
        /// Minimum measured mono used size.
        /// </summary>
        public float MinMonoUsedSizeMB { get; }

        /// <summary>
        /// Average mono used size.
        /// </summary>
        public float AverageMonoUsedSizeMB { get; }


        /// <summary>
        /// Whether it is being measured or not.
        /// </summary>
        public bool IsProfiling { get; }

        /// <summary>
        /// Memory measurement type.
        /// </summary>
        public NoaProfiler.MemoryProfilingType ProfilingType { get; }

        /// <summary>
        /// Memory consumption history (for graph display).
        /// </summary>
        internal RingBuffer<float[]> CurrentMemoryHistory { get; }

        /// <summary>
        /// Whether to show the graph or not.
        /// </summary>
        internal bool IsGraphShowing { get; }

        /// <summary>
        /// Whether to display a hyphen.
        /// </summary>
        internal bool IsViewHyphen { get; }

        /// <summary>
        /// Whether current measurement value is valid.
        /// </summary>
        internal bool IsValid { get; }

        /// <summary>
        /// Generate MemoryUnchangingInfo.
        /// </summary>
        /// <param name="info">Specifies the information to be referred to.</param>
        internal MemoryUnchangingInfo(MemoryInfo info)
        {
            bool isViewHyphen = !info.IsProfiling;
            isViewHyphen &= !info.IsStartProfiling;
            CurrentReservedMemoryMB = info.CurrentReservedMemoryMB;
            MaxReservedMemoryMB = info.MaxReservedMemoryMB;
            CurrentAllocatedMemoryMB = info.CurrentAllocatedMemoryMB;
            MaxAllocatedMemoryMB = info.MaxAllocatedMemoryMB;
            MinAllocatedMemoryMB = info.MinAllocatedMemoryMB;
            AverageAllocatedMemoryMB = info.AverageAllocatedMemoryMB;
            TotalNativeMemoryMB = info.TotalNativeMemoryMB;
            CurrentNativeMemoryMB = info.CurrentNativeMemoryMB;
            MaxNativeMemoryMB = info.MaxNativeMemoryMB;
            MinNativeMemoryMB = info.MinNativeMemoryMB;
            AverageNativeMemoryMB = info.AverageNativeMemoryMB;
            CurrentMonoHeapSizeMB = info.CurrentMonoHeapSizeMB;
            MaxMonoHeapSizeMB = info.MaxMonoHeapSizeMB;
            CurrentMonoUsedSizeMB = info.CurrentMonoUsedSizeMB;
            MaxMonoUsedSizeMB = info.MaxMonoUsedSizeMB;
            MinMonoUsedSizeMB = info.MinMonoUsedSizeMB;
            AverageMonoUsedSizeMB = info.AverageMonoUsedSizeMB;
            IsProfiling = info.IsProfiling;
            ProfilingType = info.ProfilingType;
            CurrentMemoryHistory = info.CurrentMemoryHistory;
            IsGraphShowing = info.IsGraphShowing;
            IsViewHyphen = isViewHyphen;
            IsValid = info.IsValid();

#pragma warning disable 618
            TotalMemoryMB = info.TotalMemoryMB;
            CurrentMemoryMB = info.CurrentMemoryMB;
            MaxMemoryMB = info.MaxMemoryMB;
            MinMemoryMB = info.MinMemoryMB;
            AverageMemoryMB = info.AverageMemoryMB;
#pragma warning restore 618
        }
    }
}
