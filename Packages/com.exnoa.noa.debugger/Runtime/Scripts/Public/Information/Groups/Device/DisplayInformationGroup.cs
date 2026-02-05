using UnityEngine;

namespace NoaDebugger
{
    /// <summary>
    /// Represents a collection of the display information.
    /// </summary>
    public sealed class DisplayInformationGroup
    {
        /// <summary>
        /// Gets the current screen resolution.
        /// </summary>
        public Resolution Resolution => Screen.currentResolution;

        /// <summary>
        /// Gets the screen aspect ratio.
        /// </summary>
        public float Aspect => (float)Screen.width / Screen.height;

        /// <summary>
        /// Gets the resolution's vertical refresh rate in Hz.
        /// </summary>
        public double RefreshRate
        {
            get
            {
#if UNITY_2022_2_OR_NEWER
                return Screen.currentResolution.refreshRateRatio.value;
#else
                return Screen.currentResolution.refreshRate;
#endif
            }
        }

        /// <summary>
        /// Gets the current DPI of the screen / device.
        /// </summary>
        public float Dpi => Screen.dpi;

        /// <summary>
        /// Gets the safe area of the screen in pixels.
        /// </summary>
        public Rect SafeArea => Screen.safeArea;

        /// <summary>
        /// Gets the bitwise combination of HDRDisplaySupportFlags describing the support for HDR displays on the system.
        /// </summary>
        public HDRDisplaySupportFlags Hdr => SystemInfo.hdrDisplaySupportFlags;
    }
}
