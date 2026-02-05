using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Profiling;

namespace NoaDebugger
{
    sealed partial class MemoryModel : ModelBase
    {
        public static readonly int UpdateIntervalFrames = 20;

        static readonly string MemoryModelOnUpdate = "MemoryModelOnUpdate";

        public static partial long? _GetCurrentNativeMemoryByte();

        int _framesSinceLastUpdate = MemoryModel.UpdateIntervalFrames;
        long _totalUnityMemoryUsageMB;
        long _totalNativeMemoryUsageMB;
        long _totalMonoMemoryUsageMB;
        int _unityMemoryCheckCount;
        int _nativeMemoryCheckCount;
        int _monoMemoryCheckCount;

        public MemoryInfo MemoryInfo { get; private set; }

        public UnityAction OnMemoryInfoChanged { get; set; }

        public MemoryModel()
        {
            MemoryInfo = new MemoryInfo();
            KeyValuePair<string, bool> isProfilingInfo = NoaDebuggerPrefsDefine.IsMemoryProfilingKeyValue;
            KeyValuePair<string, bool> isGraphShowingInfo = NoaDebuggerPrefsDefine.IsMemoryGraphShowingKeyValue;
            KeyValuePair<string, NoaProfiler.MemoryProfilingType> memoryProfilingTypeInfo = NoaDebuggerPrefsDefine.MemoryProfilingTypeKeyValue;
            bool isProfiling = NoaDebuggerPrefs.GetBoolean(isProfilingInfo.Key, isProfilingInfo.Value);
            bool isGraphShowing = NoaDebuggerPrefs.GetBoolean(isGraphShowingInfo.Key, isGraphShowingInfo.Value);
            NoaProfiler.MemoryProfilingType memoryProfilingType = (NoaProfiler.MemoryProfilingType)NoaDebuggerPrefs.GetInt(memoryProfilingTypeInfo.Key, (int)memoryProfilingTypeInfo.Value);
            MemoryInfo.ToggleProfiling(isProfiling);
            MemoryInfo.ToggleGraphShowing(isGraphShowing);
            MemoryInfo.ChangeProfilingType(memoryProfilingType);
            _HandleOnUpdate(isProfiling);
        }

        public void Dispose()
        {
            UpdateManager.DeleteAction(MemoryModelOnUpdate);
        }

        void _OnUpdate()
        {
            if (!MemoryInfo.IsProfiling)
            {
                return;
            }

            if (++_framesSinceLastUpdate < MemoryModel.UpdateIntervalFrames)
            {
                return;
            }

            MemoryInfo.StartProfiling();

            _framesSinceLastUpdate = 0;

            _SetUnityMemoryInfo();
            _SetNativeMemoryInfo();
            _SetMonoMemoryInfo();

            OnMemoryInfoChanged?.Invoke();
        }

        void _SetUnityMemoryInfo()
        {
            float currentReservedMemMB = MemoryModel.GetRoundedMemoryMB(Profiler.GetTotalReservedMemoryLong());
            float currentAllocatedMemMB = MemoryModel.GetRoundedMemoryMB(Profiler.GetTotalAllocatedMemoryLong());

            int addMemoryUsage = Mathf.FloorToInt(currentAllocatedMemMB * 100);
            _totalUnityMemoryUsageMB += addMemoryUsage;
            _unityMemoryCheckCount++;

            float averageMemMB = (float)_totalUnityMemoryUsageMB / _unityMemoryCheckCount;
            averageMemMB /= 100;
            averageMemMB = (float)Math.Round(averageMemMB, 2);

            MemoryInfo.RefreshUnityMemory(currentReservedMemMB, currentAllocatedMemMB, averageMemMB);
        }

        void _SetNativeMemoryInfo()
        {
            long? currentMemByte = MemoryModel._GetCurrentNativeMemoryByte();
            if (null == currentMemByte)
            {
                return;
            }
            float currentMemMB = MemoryModel.GetRoundedMemoryMB(currentMemByte.Value);

            int addMemoryUsage = Mathf.FloorToInt(currentMemMB * 100);
            _totalNativeMemoryUsageMB += addMemoryUsage;
            _nativeMemoryCheckCount++;

            if (currentMemByte == -1)
            {
                currentMemMB = -1;
            }

            float averageMemMB = (float)_totalNativeMemoryUsageMB / _nativeMemoryCheckCount;
            averageMemMB /= 100;
            averageMemMB = (float)Math.Round(averageMemMB, 2);

            MemoryInfo.RefreshNativeMemory(currentMemMB, averageMemMB);
        }

        void _SetMonoMemoryInfo()
        {
            float currentMonoHeapMB = MemoryModel.GetRoundedMemoryMB(Profiler.GetMonoHeapSizeLong());
            float currentMonoUsedMB = MemoryModel.GetRoundedMemoryMB(Profiler.GetMonoUsedSizeLong());

            int addMemoryUsage = Mathf.FloorToInt(currentMonoUsedMB * 100);
            _totalMonoMemoryUsageMB += addMemoryUsage;
            _monoMemoryCheckCount++;

            float averageMemMB = (float)_totalMonoMemoryUsageMB / _monoMemoryCheckCount;
            averageMemMB /= 100;
            averageMemMB = (float)Math.Round(averageMemMB, 2);

            MemoryInfo.RefreshMonoMemory(currentMonoHeapMB, currentMonoUsedMB, averageMemMB);
        }

        void _ResetMemoryInfo()
        {
            MemoryInfo.ResetProfiledValue();
            _framesSinceLastUpdate = MemoryModel.UpdateIntervalFrames;
            _totalUnityMemoryUsageMB = 0;
            _totalNativeMemoryUsageMB = 0;
            _totalMonoMemoryUsageMB = 0;
            _unityMemoryCheckCount = 0;
            _nativeMemoryCheckCount = 0;
            _monoMemoryCheckCount = 0;
        }

        public void ChangeProfilingState(bool isProfiling)
        {
            MemoryInfo.ToggleProfiling(isProfiling);

            NoaDebuggerPrefs.SetBoolean(NoaDebuggerPrefsDefine.IsMemoryProfilingKeyValue.Key, isProfiling);
            _HandleOnUpdate(isProfiling);
        }

        public void ChangeGraphShowingState(bool isGraphShowing)
        {
            MemoryInfo.ToggleGraphShowing(isGraphShowing);

            NoaDebuggerPrefs.SetBoolean(NoaDebuggerPrefsDefine.IsMemoryGraphShowingKeyValue.Key, isGraphShowing);
        }

        public void ChangeProfilingType(NoaProfiler.MemoryProfilingType profilingType)
        {
            MemoryInfo.ChangeProfilingType(profilingType);

            NoaDebuggerPrefs.SetInt(NoaDebuggerPrefsDefine.MemoryProfilingTypeKeyValue.Key, (int)profilingType);
        }

        void _HandleOnUpdate(bool isProfiling)
        {
            string key = MemoryModel.MemoryModelOnUpdate;

            if (isProfiling)
            {
                if (UpdateManager.ContainsKey(key))
                {
                    return;
                }

                _ResetMemoryInfo();
                UpdateManager.SetAction(key, _OnUpdate);
            }
            else
            {
                UpdateManager.DeleteAction(key);
            }
        }

        static float GetRoundedMemoryMB(long memByte)
        {
            return (float)Math.Round(DataUnitConverterModel.ByteToMB(memByte), 2);
        }
    }
}
