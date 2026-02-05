using UnityEngine;
using UnityEngine.Rendering;

namespace NoaDebugger
{
    /// <summary>
    /// Represents a collection of the build information.
    /// </summary>
    public sealed class GraphicsSettingsInformationGroup
    {
        /// <summary>
        /// Gets the <see cref="RenderPipelineAsset"/> type that defines the active render pipeline.
        /// </summary>
        public string RenderPipeline { get; }

        /// <summary>
        /// True if SRP batcher at runtime is enabled; otherwise false.
        /// </summary>
        public bool UseScriptableRenderPipelineBatching
        {
            get
            {
                if (IsBuiltInRenderPipeline)
                {
                    LogModel.LogWarning("This setting has no effect in the built-in render pipeline.");
                }
                return GraphicsSettings.useScriptableRenderPipelineBatching;
            }
            set
            {
                if (IsBuiltInRenderPipeline)
                {
                    LogModel.LogWarning("This setting has no effect in the built-in render pipeline.");
                }
                GraphicsSettings.useScriptableRenderPipelineBatching = value;
            }
        }

        /// <summary>
        /// Gets or sets how many of the multiplier for the internal rendering resolution.
        /// </summary>
        /// <remarks>
        /// Returns -1 outside the URP environment.
        /// </remarks>
        public float UrpRenderScale
        {
#if USE_PIPELINE_URP
            get => UrpRenderScaleModel.GetRenderScale();
            set => UrpRenderScaleModel.SetRenderScale(value);
#else
            get
            {
                LogModel.LogWarning("Values cannot be retrieved outside the URP environment.");
                return -1;
            }
            set
            {
                LogModel.LogWarning("You cannot change the value outside the URP environment.");
            }
#endif
        }

        /// <summary>
        /// Gets the active color space.
        /// </summary>
        public ColorSpace ActiveColorSpace => QualitySettings.activeColorSpace;

        /// <summary>
        /// Gets the desired color space.
        /// </summary>
        public ColorSpace DesiredColorSpace => QualitySettings.desiredColorSpace;

        /// <summary>
        /// Gets or sets the current graphics quality level.
        /// </summary>
        public int QualityLevel
        {
            get => QualitySettings.GetQualityLevel();
            set => QualitySettings.SetQualityLevel(value);
        }

        /// <summary>
        /// Gets or sets the level of Multi-Sample Anti-aliasing (MSAA) that the GPU performs.
        /// </summary>
        public int AntiAliasing
        {
            get => QualitySettings.antiAliasing;
            set => QualitySettings.antiAliasing = value;
        }

        /// <summary>
        /// Gets or sets the real-time shadows type to be used.
        /// </summary>
        public ShadowQuality ShadowQuality
        {
            get => QualitySettings.shadows;
            set => QualitySettings.shadows = value;
        }

        /// <summary>
        /// Gets or sets the default resolution of the shadow maps.
        /// </summary>
        public ShadowResolution ShadowResolution
        {
            get => QualitySettings.shadowResolution;
            set => QualitySettings.shadowResolution = value;
        }

        /// <summary>
        /// Gets or sets the shadow drawing distance.
        /// </summary>
        public float ShadowDistance
        {
            get => QualitySettings.shadowDistance;
            set => QualitySettings.shadowDistance = value;
        }

        /// <summary>
        /// Gets or sets the number of cascades to use for directional light shadows.
        /// </summary>
        public int ShadowCascades
        {
            get => QualitySettings.shadowCascades;
            set => QualitySettings.shadowCascades = value;
        }

        /// <summary>
        /// Gets or sets the global multiplier for the LOD's switching distance.
        /// </summary>
        public float LodBias
        {
            get => QualitySettings.lodBias;
            set => QualitySettings.lodBias = value;
        }

        /// <summary>
        /// Gets or sets how many of the highest-resolution mips of each texture Unity does not upload at the given quality level.
        /// </summary>
        /// <remarks>
        /// Returns -1 in Unity environments prior to version 2022.2.
        /// </remarks>
        public int GlobalTextureMipmapLimit
        {
#if UNITY_2022_2_OR_NEWER
            get => QualitySettings.globalTextureMipmapLimit;
            set => QualitySettings.globalTextureMipmapLimit = value;
#else
            get
            {
                LogModel.LogWarning("Value cannot be retrieved in Unity versions prior to 2022.2.");
                return -1;
            }
            set
            {
                LogModel.LogWarning("Value cannot be changed in Unity versions prior to 2022.2.");
            }
#endif
        }

        /// <summary>
        /// Gets or sets the number of vertical syncs that should pass between each frame.
        /// </summary>
        public int VSyncCount
        {
            get => QualitySettings.vSyncCount;
            set => QualitySettings.vSyncCount = value;
        }

        const string RENDER_PIPELINE_TYPE_BUILT_IN = "Built-in";
        const string RENDER_PIPELINE_TYPE_HDRP = "HDRP";
        const string RENDER_PIPELINE_TYPE_URP = "URP";
        const string RENDER_PIPELINE_TYPE_OTHER = "Other";

        internal bool IsBuiltInRenderPipeline =>
            RenderPipeline.Equals(GraphicsSettingsInformationGroup.RENDER_PIPELINE_TYPE_BUILT_IN);

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsSettingsInformationGroup"/>.
        /// </summary>
        internal GraphicsSettingsInformationGroup()
        {
            RenderPipelineAsset renderPipelineAsset = GraphicsSettings.currentRenderPipeline;

            RenderPipeline = (renderPipelineAsset == null)
                ? GraphicsSettingsInformationGroup.RENDER_PIPELINE_TYPE_BUILT_IN
                : renderPipelineAsset.GetType().Name switch
                {
                    "HDRenderPipelineAsset" => GraphicsSettingsInformationGroup.RENDER_PIPELINE_TYPE_HDRP,
                    "UniversalRenderPipelineAsset" => GraphicsSettingsInformationGroup.RENDER_PIPELINE_TYPE_URP,
                    _ => GraphicsSettingsInformationGroup.RENDER_PIPELINE_TYPE_OTHER
                };

            const string groupName = "GraphicsSettings";
            UseScriptableRenderPipelineBatchingParameter = IsBuiltInRenderPipeline
                ? new BoolInformationParameter(groupName, "UseScriptableRenderPipelineBatching", false, _ => { })
                : new BoolInformationParameter(groupName, "UseScriptableRenderPipelineBatching", UseScriptableRenderPipelineBatching, value => UseScriptableRenderPipelineBatching = value);
#if USE_PIPELINE_URP
            UrpRenderScaleParameter = new FloatInformationParameter(groupName, "UrpRenderScale", UrpRenderScale, value => UrpRenderScale = value);
#endif
            QualityLevelParameter = new IntInformationParameter(groupName, "QualityLevel", QualityLevel, value => QualityLevel = value);
            AntiAliasingParameter = new IntInformationParameter(groupName, "AntiAliasing", AntiAliasing, value => AntiAliasing = value);
            ShadowQualityParameter = new EnumInformationParameter(groupName, "ShadowQuality", ShadowQuality, value => ShadowQuality = (ShadowQuality)value);
            ShadowResolutionParameter = new EnumInformationParameter(groupName, "ShadowResolution", ShadowResolution, value => ShadowResolution = (ShadowResolution)value);
            ShadowDistanceParameter = new FloatInformationParameter(groupName, "ShadowDistance", ShadowDistance, value => ShadowDistance = value);
            ShadowCascadesParameter = new IntInformationParameter(groupName, "ShadowCascades", ShadowCascades, value => ShadowCascades = value);
            LodBiasParameter = new FloatInformationParameter(groupName, "LodBias", LodBias, value => LodBias = value);
#if UNITY_2022_2_OR_NEWER
            GlobalTextureMipmapLimitParameter = new IntInformationParameter(groupName, "GlobalTextureMipmapLimit", GlobalTextureMipmapLimit, value => GlobalTextureMipmapLimit = value);
#endif
            VSyncCountParameter = new IntInformationParameter(groupName, "VSyncCount", VSyncCount, value => VSyncCount = value);
        }

        internal void UpdateSettings()
        {
            if (!IsBuiltInRenderPipeline)
            {
                UseScriptableRenderPipelineBatchingParameter.ChangeValue(UseScriptableRenderPipelineBatching);
            }
#if USE_PIPELINE_URP
            UrpRenderScaleParameter.ChangeValue(UrpRenderScale);
#endif
            QualityLevelParameter.ChangeValue(QualityLevel);
            AntiAliasingParameter.ChangeValue(AntiAliasing);
            ShadowQualityParameter.ChangeValue(ShadowQuality);
            ShadowResolutionParameter.ChangeValue(ShadowResolution);
            ShadowDistanceParameter.ChangeValue(ShadowDistance);
            ShadowCascadesParameter.ChangeValue(ShadowCascades);
            LodBiasParameter.ChangeValue(LodBias);
#if UNITY_2022_2_OR_NEWER
            GlobalTextureMipmapLimitParameter.ChangeValue(GlobalTextureMipmapLimit);
#endif
            VSyncCountParameter.ChangeValue(VSyncCount);
        }

        internal BoolInformationParameter UseScriptableRenderPipelineBatchingParameter { get; }
#if USE_PIPELINE_URP
        internal FloatInformationParameter UrpRenderScaleParameter { get; }
#endif
        internal IntInformationParameter QualityLevelParameter { get; }
        internal IntInformationParameter AntiAliasingParameter { get; }
        internal EnumInformationParameter ShadowQualityParameter { get; }
        internal EnumInformationParameter ShadowResolutionParameter { get; }
        internal FloatInformationParameter ShadowDistanceParameter { get; }
        internal IntInformationParameter ShadowCascadesParameter { get; }
        internal FloatInformationParameter LodBiasParameter { get; }
#if UNITY_2022_2_OR_NEWER
        internal IntInformationParameter GlobalTextureMipmapLimitParameter { get; }
#endif
        internal IntInformationParameter VSyncCountParameter { get; }
    }
}
