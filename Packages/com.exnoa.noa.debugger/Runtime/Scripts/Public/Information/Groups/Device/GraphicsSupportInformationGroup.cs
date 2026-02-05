using UnityEngine;
using UnityEngine.Rendering;

namespace NoaDebugger
{
    /// <summary>
    /// Represents a collection of the graphics support information.
    /// </summary>
    public sealed class GraphicsSupportInformationGroup
    {
        /// <summary>
        /// Gets the maximum texture size in pixels.
        /// </summary>
        public int MaxTextureSize { get; }

        /// <summary>
        /// Gets the maximum cubemap texture size in pixels.
        /// </summary>
        public int MaxCubemapSize { get; }

        /// <summary>
        /// True if GPU draw call instancing is supported; otherwise false.
        /// </summary>
        public bool SupportsInstancing { get; }

        /// <summary>
        /// True if compute shaders are supported; otherwise false.
        /// </summary>
        public bool SupportsComputeShaders { get; }

        /// <summary>
        /// True if geometry shaders are supported; otherwise false.
        /// </summary>
        public bool SupportsGeometryShaders { get; }

        /// <summary>
        /// True if tessellation shaders are supported; otherwise false.
        /// </summary>
        public bool SupportsTessellationShaders { get; }

        /// <summary>
        /// True if variable rate shading is supported; otherwise false.
        /// </summary>
        /// <remarks>
        /// Returns false in Unity environments prior to version 6000.1.
        /// </remarks>
        public bool SupportsVariableRateShading
        {
#if UNITY_6000_1_OR_NEWER

            get;
#else
            get
            {
                LogModel.LogWarning("Value cannot be retrieved in Unity versions prior to 6000.1.");
                return false;
            }
#endif
        }

        /// <summary>
        /// True if built-in shadows are supported; otherwise false.
        /// </summary>
        public bool SupportsShadows { get; }

        /// <summary>
        /// True if sampling raw depth from shadowmaps is supported; otherwise false.
        /// </summary>
        public bool SupportsRawShadowDepthSampling { get; }

        /// <summary>
        /// Gets what NPOT (non-power of two size) texture support does the GPU provide.
        /// </summary>
        public NPOTSupport NpotSupport { get; }

        /// <summary>
        /// True if 3D (volume) textures are supported; otherwise false.
        /// </summary>
        public bool Supports3DTextures { get; }

        /// <summary>
        /// True if 3D (volume) RenderTextures are supported; otherwise false.
        /// </summary>
        public bool Supports3DRenderTextures { get; }

        /// <summary>
        /// True if 2D Array textures are supported; otherwise false.
        /// </summary>
        public bool Supports2DArrayTextures { get; }

        /// <summary>
        /// True if cubemap array textures are supported; otherwise false.
        /// </summary>
        public bool SupportsCubemapArrayTextures { get; }

        /// <summary>
        /// True if sparse textures are supported; otherwise false.
        /// </summary>
        public bool SupportsSparseTextures { get; }

        /// <summary>
        /// Gets the support for various Graphics.CopyTexture cases.
        /// </summary>
        public CopyTextureSupport CopyTextureSupport { get; }

        /// <summary>
        /// True if ray tracing is supported; otherwise false.
        /// </summary>
        /// <remarks>
        /// Returns false in Unity environments prior to version 2021.2
        /// </remarks>
        public bool SupportsRayTracing
        {
#if UNITY_2021_2_OR_NEWER

            get;
#else
            get
            {
                LogModel.LogWarning("Value cannot be retrieved in Unity versions prior to 2021.2");
                return false;
            }
#endif
        }

        /// <summary>
        /// Gets application's actual rendering threading mode.
        /// </summary>
        public RenderingThreadingMode RenderingThreadingMode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsSupportInformationGroup"/>.
        /// </summary>
        internal GraphicsSupportInformationGroup()
        {
            MaxTextureSize = SystemInfo.maxTextureSize;
            MaxCubemapSize = SystemInfo.maxCubemapSize;
            SupportsInstancing = SystemInfo.supportsInstancing;
            SupportsComputeShaders = SystemInfo.supportsComputeShaders;
            SupportsGeometryShaders = SystemInfo.supportsGeometryShaders;
            SupportsTessellationShaders = SystemInfo.supportsTessellationShaders;
#if UNITY_6000_1_OR_NEWER
            SupportsVariableRateShading = SystemInfo.supportsVariableRateShading;
#endif
            SupportsShadows = SystemInfo.supportsShadows;
            SupportsRawShadowDepthSampling = SystemInfo.supportsRawShadowDepthSampling;
            NpotSupport = SystemInfo.npotSupport;
            Supports3DTextures = SystemInfo.supports3DTextures;
            Supports3DRenderTextures = SystemInfo.supports3DRenderTextures;
            Supports2DArrayTextures = SystemInfo.supports2DArrayTextures;
            SupportsCubemapArrayTextures = SystemInfo.supportsCubemapArrayTextures;
            SupportsSparseTextures = SystemInfo.supportsSparseTextures;
            CopyTextureSupport = SystemInfo.copyTextureSupport;
#if UNITY_2021_2_OR_NEWER
            SupportsRayTracing = SystemInfo.supportsRayTracing;
#endif
            RenderingThreadingMode = SystemInfo.renderingThreadingMode;
        }
    }
}
