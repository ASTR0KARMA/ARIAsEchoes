using System;

namespace NoaDebugger
{
    /// <summary>
    /// Through this class you can retrieve various values of the Profiler function.
    /// </summary>
    public class NoaProfiler
    {
        /// <summary>
        /// Types of features in the Profiler.
        /// </summary>
        public enum FeatureType
        {
            Fps,
            Memory,
            Rendering
        }

        /// <summary>
        /// Types of memory measurement.
        /// </summary>
        public enum MemoryProfilingType
        {
            Unity,
            Native,
            Mono
        }

        /// <summary>
        /// Orientation for placing multiple feature overlays in the Profiler.
        /// </summary>
        public enum OverlayAxis
        {
            Horizontal,
            Vertical
        }

        /// <summary>
        /// Text display type for each feature overlay in the Profiler.
        /// </summary>
        public enum OverlayTextType
        {
            Simple,
            Full
        }

        /// <summary>
        /// Specifies the Memory measurement type.
        /// </summary>
        /// <param name="profilingType">Specifies the Memory measurement type you want to change to.</param>
        public static void SetMemoryProfilingType(MemoryProfilingType profilingType)
        {
            ProfilerPresenter presenter = NoaDebugger.GetPresenter<ProfilerPresenter>();

            if (presenter == null)
            {
                return;
            }

            presenter.ChangeMemoryProfilingType(profilingType);
        }

        /// <summary>
        /// Returns the Profiler information being held.
        /// </summary>
        public static ProfilerInfo ProfilerInfo => NoaProfiler._GetProfilerInfo();

        /// <summary>
        /// Returns the most recent FPS information.
        /// </summary>
        public static FpsInfo LatestFpsInfo => _GetLatestFpsInfo();

        /// <summary>
        /// Returns the most recent Memory information.
        /// </summary>
        public static MemoryInfo LatestMemoryInfo => _GetLatestMemoryInfo();

        /// <summary>
        /// Returns the most recent Rendering information.
        /// </summary>
        public static RenderingInfo LatestRenderingInfo => _GetLatestRenderingInfo();

        /// <summary>
        /// Returns the most recent Battery information.
        /// </summary>
        [Obsolete]
        public static BatteryInfo LatestBatteryInfo => _GetLatestBatteryInfo();

        /// <summary>
        /// Returns the most recent Thermal information.
        /// </summary>
        [Obsolete]
        public static ThermalInfo LatestThermalInfo => _GetLatestThermalInfo();

        /// <summary>
        /// The FPS measurement status.
        /// </summary>
        public static bool IsFpsProfiling
        {
            get => _IsFpsProfiling();
            set => _ChangeFpsProfiling(value);
        }

        /// <summary>
        /// The measurement state of Memory.
        /// </summary>
        public static bool IsMemoryProfiling
        {
            get => _IsMemoryProfiling();
            set => _ChangeMemoryProfiling(value);
        }

        /// <summary>
        /// The total memory capacity in MB units. If a negative value is specified, it will be the RAM capacity of the device.
        /// </summary>
        public static float TotalNativeMemoryMB
        {
            get => _GetTotalNativeMemoryMB();
            set => _SetTotalNativeMemoryMB(value);
        }

        /// <summary>
        /// The total memory capacity in MB units. If a negative value is specified, it will be the RAM capacity of the device.
        /// </summary>
        [Obsolete("Use NoaProfiler.TotalNativeMemoryMB instead.")]
        public static float TotalMemoryMB
        {
            get => TotalNativeMemoryMB;
            set => TotalNativeMemoryMB = value;
        }

        /// <summary>
        /// The measurement state of Rendering.
        /// </summary>
        public static bool IsRenderingProfiling
        {
            get => _IsRenderingProfiling();
            set => _ChangeRenderingProfiling(value);
        }

        /// <summary>
        /// The measurement state of Battery.
        /// </summary>
        [Obsolete]
        public static bool IsBatteryProfiling
        {
            get => _IsBatteryProfiling();
            set => _ChangeBatteryProfiling(value);
        }

        /// <summary>
        /// The measurement state of Thermal.
        /// </summary>
        [Obsolete]
        public static bool IsThermalProfiling
        {
            get => _IsThermalProfiling();
            set => _ChangeThermalProfiling(value);
        }

        /// <summary>
        /// Registers a custom garbage collection callback.
        /// If the argument is null, unregisters the corresponding callback.
        /// </summary>
        /// <param name="callbacks">A class implementing INoaGCCollectCallbacks or derived from NoaGCCollectCallbacks</param>
        public static void SetGCCollectCallbacks(INoaGCCollectCallbacks callbacks)
        {
            GCCollectCallbacks = callbacks;
        }

        internal static INoaGCCollectCallbacks GCCollectCallbacks { get; private set; } = null;

        /// <summary>
        /// Event triggered when the Force GC Collect button is pressed.
        /// Users can register their custom actions to be executed upon pressing the Force GC Collect button.
        /// If the event handler returns true, garbage collection will be executed.
        /// If the event handler returns false, garbage collection will not be executed.
        /// </summary>
        [Obsolete("Inherit from NoaGCCollectCallbacks and override OnBeforeGCCollect instead.")]
        public static Func<bool> OnGcCollect
        {
            get
            {
                var presenter = NoaDebugger.GetPresenter<ProfilerPresenter>();

                return presenter != null ? presenter._onGCCollect : null;
            }
            set
            {
                var presenter = NoaDebugger.GetPresenter<ProfilerPresenter>();

                if (presenter == null)
                {
                    return;
                }

                if (value == null)
                {
                    presenter._onGCCollect = null;
                }
                else
                {
                    presenter._onGCCollect += value;
                }
            }
        }

        /// <summary>
        /// Registers a custom asset unloading callback.
        /// If the argument is null, unregisters the corresponding callback.
        /// </summary>
        /// <param name="callbacks">A class implementing INoaUnloadAssetsCallbacks or derived from NoaUnloadAssetsCallbacks</param>
        public static void SetUnloadAssetsCallbacks(INoaUnloadAssetsCallbacks callbacks)
        {
            UnloadAssetsCallbacks = callbacks;
        }

        internal static INoaUnloadAssetsCallbacks UnloadAssetsCallbacks { get; private set; } = null;

        /// <summary>
        /// Event triggered when the Unload Unused Assets button is pressed.
        /// Users can register their custom actions to be executed upon pressing the Unload Unused Assets button.
        /// If the event handler returns true, asset unloading will be executed.
        /// If the event handler returns false, asset unloading will not be executed.
        /// </summary>
        [Obsolete("Inherit from NoaUnloadAssetsCallbacks and override OnBeforeUnloadAssets instead.")]
        public static Func<bool> OnUnloadAssets
        {
            get
            {
                var presenter = NoaDebugger.GetPresenter<ProfilerPresenter>();

                return presenter != null ? presenter._onUnloadAsset : null;
            }
            set
            {
                var presenter = NoaDebugger.GetPresenter<ProfilerPresenter>();

                if (presenter == null)
                {
                    return;
                }

                if (value == null)
                {
                    presenter._onUnloadAsset = null;
                }
                else
                {
                    presenter._onUnloadAsset += value;
                }
            }
        }

        /// <summary>
        /// Returns the Profiler information being held.
        /// </summary>
        /// <returns>Returns the Profiler information being held.</returns>
        static ProfilerInfo _GetProfilerInfo()
        {
            ProfilerPresenter presenter = NoaDebugger.GetPresenter<ProfilerPresenter>();

            if (presenter == null)
            {
                return null;
            }

            return new ProfilerInfo(presenter);
        }


        /// <summary>
        /// Returns the most recent Fps information.
        /// </summary>
        /// <returns>Returns the most recent Fps information.</returns>
        static FpsInfo _GetLatestFpsInfo()
        {
            ProfilerPresenter presenter = NoaDebugger.GetPresenter<ProfilerPresenter>();

            if (presenter == null)
            {
                return null;
            }

            return presenter.GetFpsInfo();
        }

        /// <summary>
        /// Returns the FPS measurement state.
        /// </summary>
        /// <returns>Measurement state.</returns>
        static bool _IsFpsProfiling()
        {
            ProfilerPresenter presenter = NoaDebugger.GetPresenter<ProfilerPresenter>();

            if (presenter == null)
            {
                return false;
            }

            return presenter.IsFpsProfiling();
        }

        /// <summary>
        /// Changes the FPS measurement state.
        /// </summary>
        /// <param name="isProfiling">Specifies the measurement state you want to change to.</param>
        static void _ChangeFpsProfiling(bool isProfiling)
        {
            ProfilerPresenter presenter = NoaDebugger.GetPresenter<ProfilerPresenter>();

            if (presenter == null)
            {
                return;
            }

            presenter.ChangeFpsProfiling(isProfiling);
        }


        /// <summary>
        /// Returns the most recent Memory information.
        /// </summary>
        /// <returns>Returns the most recent Memory information.</returns>
        static MemoryInfo _GetLatestMemoryInfo()
        {
            ProfilerPresenter presenter = NoaDebugger.GetPresenter<ProfilerPresenter>();

            if (presenter == null)
            {
                return null;
            }

            return presenter.GetMemoryInfo();
        }

        /// <summary>
        /// Returns the Memory measurement state.
        /// </summary>
        /// <returns>Measurement state.</returns>
        static bool _IsMemoryProfiling()
        {
            ProfilerPresenter presenter = NoaDebugger.GetPresenter<ProfilerPresenter>();

            if (presenter == null)
            {
                return false;
            }

            return presenter.IsMemoryProfiling();
        }

        /// <summary>
        /// Changes the Memory measurement state.
        /// </summary>
        /// <param name="isProfiling">Specifies the measurement state you want to change to.</param>
        static void _ChangeMemoryProfiling(bool isProfiling)
        {
            ProfilerPresenter presenter = NoaDebugger.GetPresenter<ProfilerPresenter>();

            if (presenter == null)
            {
                return;
            }

            presenter.ChangeMemoryProfiling(isProfiling);
        }

        /// <summary>
        /// Returns total memory capacity.
        /// </summary>
        /// <returns>Total memory capacity (in MB). -1 if not available.</returns>
        static float _GetTotalNativeMemoryMB()
        {
            ProfilerPresenter presenter = NoaDebugger.GetPresenter<ProfilerPresenter>();

            if (presenter == null)
            {
                return -1;
            }

            return presenter.GetTotalNativeMemoryMB();
        }

        /// <summary>
        /// Sets total memory capacity.
        /// </summary>
        /// <param name="totalMemoryMB">Set the total memory capacity in MB units. If omitted, it becomes the RAM capacity of the device.</param>
        static void _SetTotalNativeMemoryMB(float totalMemoryMB = -1)
        {
            ProfilerPresenter presenter = NoaDebugger.GetPresenter<ProfilerPresenter>();

            if (presenter == null)
            {
                return;
            }

            presenter.SetTotalNativeMemoryMB(totalMemoryMB);
        }


        /// <summary>
        /// Returns the most recent RenderingInfo.
        /// </summary>
        /// <returns>Most recent RenderingInfo.</returns>
        static RenderingInfo _GetLatestRenderingInfo()
        {
            ProfilerPresenter presenter = NoaDebugger.GetPresenter<ProfilerPresenter>();

            if (presenter == null)
            {
                return null;
            }

            return presenter.GetRenderingInfo();
        }

        /// <summary>
        /// Returns the measurement state of Rendering.
        /// </summary>
        /// <returns>Measurement state.</returns>
        static bool _IsRenderingProfiling()
        {
            ProfilerPresenter presenter = NoaDebugger.GetPresenter<ProfilerPresenter>();

            if (presenter == null)
            {
                return false;
            }

            return presenter.IsRenderingProfiling();
        }

        /// <summary>
        /// Changes the measurement state of Rendering.
        /// </summary>
        /// <param name="isProfiling">Specifies the measurement state you want to change to.</param>
        static void _ChangeRenderingProfiling(bool isProfiling)
        {
            ProfilerPresenter presenter = NoaDebugger.GetPresenter<ProfilerPresenter>();

            if (presenter == null)
            {
                return;
            }

            presenter.ChangeRenderingProfiling(isProfiling);
        }


        /// <summary>
        /// Returns the most recent BatteryInfo.
        /// </summary>
        /// <returns>Most recent BatteryInfo.</returns>
        static BatteryInfo _GetLatestBatteryInfo()
        {
            ProfilerPresenter presenter = NoaDebugger.GetPresenter<ProfilerPresenter>();

            if (presenter == null)
            {
                return null;
            }

            return presenter.GetBatteryInfo();
        }

        /// <summary>
        /// Returns the measurement state of Battery.
        /// </summary>
        /// <returns>Measurement state.</returns>
        static bool _IsBatteryProfiling()
        {
            ProfilerPresenter presenter = NoaDebugger.GetPresenter<ProfilerPresenter>();

            if (presenter == null)
            {
                return false;
            }

            return presenter.IsBatteryProfiling();
        }

        /// <summary>
        /// Changes the measurement state of Battery.
        /// </summary>
        /// <param name="isProfiling">Specifies the measurement state you want to change to.</param>
        static void _ChangeBatteryProfiling(bool isProfiling)
        {
            ProfilerPresenter presenter = NoaDebugger.GetPresenter<ProfilerPresenter>();

            if (presenter == null)
            {
                return;
            }

            presenter.ChangeBatteryProfiling(isProfiling);
        }


        /// <summary>
        /// Returns the most recent ThermalInfo.
        /// </summary>
        /// <returns>Most recent ThermalInfo.</returns>
        static ThermalInfo _GetLatestThermalInfo()
        {
            ProfilerPresenter presenter = NoaDebugger.GetPresenter<ProfilerPresenter>();

            if (presenter == null)
            {
                return null;
            }

            return presenter.GetThermalInfo();
        }

        /// <summary>
        /// Returns the measurement state of Thermal.
        /// </summary>
        /// <returns>Measurement state.</returns>
        static bool _IsThermalProfiling()
        {
            ProfilerPresenter presenter = NoaDebugger.GetPresenter<ProfilerPresenter>();

            if (presenter == null)
            {
                return false;
            }

            return presenter.IsThermalProfiling();
        }

        /// <summary>
        /// Changes the measurement state of Thermal.
        /// </summary>
        /// <param name="isProfiling">Specifies the measurement state you want to change to.</param>
        static void _ChangeThermalProfiling(bool isProfiling)
        {
            ProfilerPresenter presenter = NoaDebugger.GetPresenter<ProfilerPresenter>();

            if (presenter == null)
            {
                return;
            }

            presenter.ChangeThermalProfiling(isProfiling);
        }
    }
}
