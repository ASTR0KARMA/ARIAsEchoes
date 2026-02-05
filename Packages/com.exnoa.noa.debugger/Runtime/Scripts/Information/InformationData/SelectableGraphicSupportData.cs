namespace NoaDebugger
{
    sealed class SelectableGraphicsSupportData : SelectableGroup<InformationGraphicsSupportData>
    {
        const string GROUP_NAME = "Graphics Support";
        const string KEY_MAX_TEXTURE_SIZE = "Max Texture Size";
        const string KEY_MAX_CUBEMAP_SIZE = "Max Cubemap Size";
        const string KEY_INSTANCING = "Instancing";
        const string KEY_COMPUTE_SHADERS = "Compute Shaders";
        const string KEY_GEOMETRY_SHADERS = "Geometry Shaders";
        const string KEY_TESSELLATION_SHADERS = "Tessellation Shaders";
        const string KEY_VARIABLE_RATE_SHADING = "Variable Rate Shading";
        const string KEY_SHADOWS = "Shadows";
        const string KEY_RAW_SHADOW_DEPTH_SAMPLING = "Raw Shadow Depth Sampling";
        const string KEY_NPOT_SUPPORT = "NPOT Support";
        const string KEY_3D_TEXTURES = "3D Textures";
        const string KEY_3D_RENDER_TEXTURES = "3D Render Textures";
        const string KEY_2D_ARRAY_TEXTURES = "2D Array Textures";
        const string KEY_CUBEMAP_ARRAY_TEXTURES = "Cubemap Array Textures";
        const string KEY_SPARSE_TEXTURES = "Sparse Textures";
        const string KEY_COPY_TEXTURE_SUPPORT = "Copy Texture Support";
        const string KEY_RAY_TRACING = "Ray Tracing";
        const string KEY_RENDERING_THREADING_MODE = "Rendering Threading Mode";

        public override string GroupName => GROUP_NAME;

        public SelectableGraphicsSupportData(InformationGraphicsSupportData data)
        {
            AddItem(KEY_MAX_TEXTURE_SIZE, data._maxTextureSize);
            AddItem(KEY_MAX_CUBEMAP_SIZE, data._maxCubemapSize);
            AddItem(KEY_INSTANCING, data._supportsInstancing);
            AddItem(KEY_COMPUTE_SHADERS, data._supportsComputeShaders);
            AddItem(KEY_GEOMETRY_SHADERS, data._supportsGeometryShaders);
            AddItem(KEY_TESSELLATION_SHADERS, data._supportsTessellationShaders);
#if UNITY_6000_1_OR_NEWER
            AddItem(KEY_VARIABLE_RATE_SHADING, data._supportsVariableRateShading);
#endif
            AddItem(KEY_SHADOWS, data._supportsShadows);
            AddItem(KEY_RAW_SHADOW_DEPTH_SAMPLING, data._supportsRawShadowDepthSampling);
            AddItem(KEY_NPOT_SUPPORT, data._npotSupport);
            AddItem(KEY_3D_TEXTURES, data._supports3DTextures);
            AddItem(KEY_3D_RENDER_TEXTURES, data._supports3DRenderTextures);
            AddItem(KEY_2D_ARRAY_TEXTURES, data._supports2DArrayTextures);
            AddItem(KEY_CUBEMAP_ARRAY_TEXTURES, data._supportsCubemapArrayTextures);
            AddItem(KEY_SPARSE_TEXTURES, data._supportsSparseTextures);
            AddItem(KEY_COPY_TEXTURE_SUPPORT, data._copyTextureSupport);
#if UNITY_2021_2_OR_NEWER
            AddItem(KEY_RAY_TRACING, data._supportsRayTracing);
#endif
            AddItem(KEY_RENDERING_THREADING_MODE, data._renderingThreadingMode);
        }

        public override void Update(InformationGraphicsSupportData data)
        {
            UpdateItem(KEY_MAX_TEXTURE_SIZE, data._maxTextureSize);
            UpdateItem(KEY_MAX_CUBEMAP_SIZE, data._maxCubemapSize);
            UpdateItem(KEY_INSTANCING, data._supportsInstancing);
            UpdateItem(KEY_COMPUTE_SHADERS, data._supportsComputeShaders);
            UpdateItem(KEY_GEOMETRY_SHADERS, data._supportsGeometryShaders);
            UpdateItem(KEY_TESSELLATION_SHADERS, data._supportsTessellationShaders);
#if UNITY_6000_1_OR_NEWER
            UpdateItem(KEY_VARIABLE_RATE_SHADING, data._supportsVariableRateShading);
#endif
            UpdateItem(KEY_SHADOWS, data._supportsShadows);
            UpdateItem(KEY_RAW_SHADOW_DEPTH_SAMPLING, data._supportsRawShadowDepthSampling);
            UpdateItem(KEY_NPOT_SUPPORT, data._npotSupport);
            UpdateItem(KEY_3D_TEXTURES, data._supports3DTextures);
            UpdateItem(KEY_3D_RENDER_TEXTURES, data._supports3DRenderTextures);
            UpdateItem(KEY_2D_ARRAY_TEXTURES, data._supports2DArrayTextures);
            UpdateItem(KEY_CUBEMAP_ARRAY_TEXTURES, data._supportsCubemapArrayTextures);
            UpdateItem(KEY_SPARSE_TEXTURES, data._supportsSparseTextures);
            UpdateItem(KEY_COPY_TEXTURE_SUPPORT, data._copyTextureSupport);
#if UNITY_2021_2_OR_NEWER
            UpdateItem(KEY_RAY_TRACING, data._supportsRayTracing);
#endif
            UpdateItem(KEY_RENDERING_THREADING_MODE, data._renderingThreadingMode);
        }
    }
}
