using UnityEngine;

namespace NoaDebugger
{
    /// <summary>
    /// Represents a collection of the texture format support information.
    /// </summary>
    public sealed class TextureFormatSupportInformationGroup
    {
        /// <summary>
        /// True if any of ASTC texture formats are supported on this device; otherwise false.
        /// </summary>
        public bool SupportsAstc { get; }

        /// <summary>
        /// True if ETC1 texture format is supported on this device; otherwise false.
        /// </summary>
        public bool SupportsEtc1 { get; }

        /// <summary>
        /// True if ETC2 texture format is supported on this device; otherwise false.
        /// </summary>
        public bool SupportsEtc2 { get; }

        /// <summary>
        /// True if DXT1 texture format is supported on this device; otherwise false.
        /// </summary>
        public bool SupportsDxt1 { get; }

        /// <summary>
        /// True if DXT5 texture format is supported on this device; otherwise false.
        /// </summary>
        public bool SupportsDxt5 { get; }

        /// <summary>
        /// True if BC4 texture format is supported on this device; otherwise false.
        /// </summary>
        public bool SupportsBc4 { get; }

        /// <summary>
        /// True if BC5 texture format is supported on this device; otherwise false.
        /// </summary>
        public bool SupportsBc5 { get; }

        /// <summary>
        /// True if BC6H texture format is supported on this device; otherwise false.
        /// </summary>
        public bool SupportsBc6H { get; }

        /// <summary>
        /// True if BC7 texture format is supported on this device; otherwise false.
        /// </summary>
        public bool SupportsBc7 { get; }

        /// <summary>
        /// True if any of PVRTC texture formats are supported on this device; otherwise false.
        /// </summary>
        /// <remarks>
        /// Returns false in Unity 6000.1 or newer, which does not support this format.
        /// </remarks>
        public bool SupportsPvrtc
        {
#if UNITY_6000_1_OR_NEWER
            get
            {
                LogModel.LogWarning("Value cannot be retrieved in Unity 6000.1 or newer.");
                return false;
            }
#else
            get;
#endif
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureFormatSupportInformationGroup"/>.
        /// </summary>
        internal TextureFormatSupportInformationGroup()
        {
            SupportsAstc =
                SystemInfo.SupportsTextureFormat(TextureFormat.ASTC_4x4)
                | SystemInfo.SupportsTextureFormat(TextureFormat.ASTC_5x5)
                | SystemInfo.SupportsTextureFormat(TextureFormat.ASTC_6x6)
                | SystemInfo.SupportsTextureFormat(TextureFormat.ASTC_8x8)
                | SystemInfo.SupportsTextureFormat(TextureFormat.ASTC_10x10)
                | SystemInfo.SupportsTextureFormat(TextureFormat.ASTC_12x12)
                | SystemInfo.SupportsTextureFormat(TextureFormat.ASTC_HDR_4x4)
                | SystemInfo.SupportsTextureFormat(TextureFormat.ASTC_HDR_5x5)
                | SystemInfo.SupportsTextureFormat(TextureFormat.ASTC_HDR_6x6)
                | SystemInfo.SupportsTextureFormat(TextureFormat.ASTC_HDR_8x8)
                | SystemInfo.SupportsTextureFormat(TextureFormat.ASTC_HDR_10x10)
                | SystemInfo.SupportsTextureFormat(TextureFormat.ASTC_HDR_12x12);

            SupportsEtc1 = SystemInfo.SupportsTextureFormat(TextureFormat.ETC_RGB4);
            SupportsEtc2 = SystemInfo.SupportsTextureFormat(TextureFormat.ETC2_RGBA8);
            SupportsDxt1 = SystemInfo.SupportsTextureFormat(TextureFormat.DXT1);
            SupportsDxt5 = SystemInfo.SupportsTextureFormat(TextureFormat.DXT5);
            SupportsBc4 = SystemInfo.SupportsTextureFormat(TextureFormat.BC4);
            SupportsBc5 = SystemInfo.SupportsTextureFormat(TextureFormat.BC5);
            SupportsBc6H = SystemInfo.SupportsTextureFormat(TextureFormat.BC6H);
            SupportsBc7 = SystemInfo.SupportsTextureFormat(TextureFormat.BC7);

#if !UNITY_6000_1_OR_NEWER
            SupportsPvrtc =
                SystemInfo.SupportsTextureFormat(TextureFormat.PVRTC_RGB2)
                | SystemInfo.SupportsTextureFormat(TextureFormat.PVRTC_RGBA2)
                | SystemInfo.SupportsTextureFormat(TextureFormat.PVRTC_RGB4)
                | SystemInfo.SupportsTextureFormat(TextureFormat.PVRTC_RGBA4);
#endif
        }
    }
}
