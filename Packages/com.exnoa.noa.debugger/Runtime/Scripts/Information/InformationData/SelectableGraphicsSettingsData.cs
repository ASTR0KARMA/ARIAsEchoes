namespace NoaDebugger
{
    sealed class SelectableGraphicsSettingsData : SelectableGroup<InformationGraphicsSettingsData>
    {
        const string GROUP_NAME = "Graphics Settings";
        const string KEY_RENDER_PIPELINE = "Render Pipeline";
        const string KEY_SRP_BATCHING = "SRP Batching";
        const string KEY_ACTIVE_COLOR_SPACE = "Active Color Space";
        const string KEY_DESIRED_COLOR_SPACE = "Desired Color Space";
        const string KEY_QUALITY_LEVEL = "Quality Level";
        const string KEY_ANTI_ALIASING = "Anti Aliasing";
        const string KEY_SHADOW_QUALITY = "Shadow Quality";
        const string KEY_SHADOW_RESOLUTION = "Shadow Resolution";
        const string KEY_SHADOW_DISTANCE = "Shadow Distance";
        const string KEY_SHADOW_CASCADES = "Shadow Cascades";
        const string KEY_LOD_BIAS = "LOD Bias";
        const string KEY_URP_RENDER_SCALE = "Render Scale (URP)";
        const string KEY_GLOBAL_TEXTURE_MIPMAP_LIMIT = "Global Texture Mipmap Limit";
        const string KEY_VSYNC_COUNT = "V-Sync Count";

        public override string GroupName => GROUP_NAME;

        public SelectableGraphicsSettingsData(InformationGraphicsSettingsData graphicsSettings)
        {
            AddItem(KEY_RENDER_PIPELINE, graphicsSettings._renderPipeline);
            AddItem(KEY_SRP_BATCHING, graphicsSettings._srpBatching);
            AddItem(KEY_ACTIVE_COLOR_SPACE, graphicsSettings._activeColorSpace);
            AddItem(KEY_DESIRED_COLOR_SPACE, graphicsSettings._desiredColorSpace);
            AddItem(KEY_QUALITY_LEVEL, graphicsSettings._qualityLevel);
            AddItem(KEY_ANTI_ALIASING, graphicsSettings._antiAliasing);
            AddItem(KEY_SHADOW_QUALITY, graphicsSettings._shadowQuality);
            AddItem(KEY_SHADOW_RESOLUTION, graphicsSettings._shadowResolution);
            AddItem(KEY_SHADOW_DISTANCE, graphicsSettings._shadowDistance);
            AddItem(KEY_SHADOW_CASCADES, graphicsSettings._shadowCascades);
            AddItem(KEY_LOD_BIAS, graphicsSettings._lodBias);
            AddItem(KEY_URP_RENDER_SCALE, graphicsSettings._urpRenderScale);
#if UNITY_2022_2_OR_NEWER
            AddItem(KEY_GLOBAL_TEXTURE_MIPMAP_LIMIT, graphicsSettings._globalTextureMipmapLimit);
#endif
            AddItem(KEY_VSYNC_COUNT, graphicsSettings._vSyncCount);
        }

        public override void Update(InformationGraphicsSettingsData graphicsSettings)
        {
            UpdateItem(KEY_RENDER_PIPELINE, graphicsSettings._renderPipeline);
            UpdateItem(KEY_SRP_BATCHING, graphicsSettings._srpBatching);
            UpdateItem(KEY_ACTIVE_COLOR_SPACE, graphicsSettings._activeColorSpace);
            UpdateItem(KEY_DESIRED_COLOR_SPACE, graphicsSettings._desiredColorSpace);
            UpdateItem(KEY_QUALITY_LEVEL, graphicsSettings._qualityLevel);
            UpdateItem(KEY_ANTI_ALIASING, graphicsSettings._antiAliasing);
            UpdateItem(KEY_SHADOW_QUALITY, graphicsSettings._shadowQuality);
            UpdateItem(KEY_SHADOW_RESOLUTION, graphicsSettings._shadowResolution);
            UpdateItem(KEY_SHADOW_DISTANCE, graphicsSettings._shadowDistance);
            UpdateItem(KEY_SHADOW_CASCADES, graphicsSettings._shadowCascades);
            UpdateItem(KEY_LOD_BIAS, graphicsSettings._lodBias);
            UpdateItem(KEY_URP_RENDER_SCALE, graphicsSettings._urpRenderScale);
#if UNITY_2022_2_OR_NEWER
            UpdateItem(KEY_GLOBAL_TEXTURE_MIPMAP_LIMIT, graphicsSettings._globalTextureMipmapLimit);
#endif
            UpdateItem(KEY_VSYNC_COUNT, graphicsSettings._vSyncCount);
        }
    }
}
