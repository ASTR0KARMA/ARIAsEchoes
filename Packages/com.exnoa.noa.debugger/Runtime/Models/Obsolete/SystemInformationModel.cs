using System;
using UnityEngine;

namespace NoaDebugger
{
    [Obsolete("Use the 'NoaDebugger.*InformationModel' classes without obsolete attribute instead.")]
    sealed class SystemInformationModel : ModelBase
    {
        public ApplicationInfo ApplicationInfo { private set; get; }
        public DeviceInfo DeviceInfo { private set; get; }
        public CpuInfo CpuInfo { private set; get; }
        public GpuInfo GpuInfo { private set; get; }
        public SystemMemoryInfo SystemMemoryInfo { private set; get; }
        public DisplayInfo DisplayInfo { private set; get; }

        public SystemInformationModel()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            string deviceType = NoaDebuggerDefine.MISSING_VALUE;

            float memorySize = -1;
#else
            string deviceType = SystemInfo.deviceType.ToString();
            float memorySize = SystemInfo.systemMemorySize;
#endif
            ApplicationInfo = new ApplicationInfo();

            DeviceInfo = new DeviceInfo(deviceType);

            CpuInfo = new CpuInfo();

            GpuInfo = new GpuInfo();

            SystemMemoryInfo = new SystemMemoryInfo(memorySize);

            DisplayInfo = new DisplayInfo();
        }

        public void OnUpdate()
        {
            DisplayInfo.Refresh();
        }

        public IKeyValueParser[] CreateExportData()
        {
            return new IKeyValueParser[]
            {
                KeyObjectParser.CreateFromClass(ApplicationInfo, "Application"),
                KeyObjectParser.CreateFromClass(DeviceInfo, "Device"),
                KeyObjectParser.CreateFromClass(CpuInfo, "CPU Spec"),
                KeyObjectParser.CreateFromClass(GpuInfo, "GPU Spec"),
                KeyObjectParser.CreateFromClass(SystemMemoryInfo, "Memory Spec"),
                KeyObjectParser.CreateFromClass(DisplayInfo, "Display"),
            };
        }
    }
}
