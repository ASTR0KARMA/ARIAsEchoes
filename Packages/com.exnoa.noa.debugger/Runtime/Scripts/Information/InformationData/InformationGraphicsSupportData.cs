namespace NoaDebugger
{
    sealed class InformationGraphicsSupportData
    {
        public string _maxTextureSize;
        public string _maxCubemapSize;
        public string _supportsInstancing;
        public string _supportsComputeShaders;
        public string _supportsGeometryShaders;
        public string _supportsTessellationShaders;
#if UNITY_6000_1_OR_NEWER
        public string _supportsVariableRateShading;
#endif
        public string _supportsShadows;
        public string _supportsRawShadowDepthSampling;
        public string _npotSupport;
        public string _supports3DTextures;
        public string _supports3DRenderTextures;
        public string _supports2DArrayTextures;
        public string _supportsCubemapArrayTextures;
        public string _supportsSparseTextures;
        public string _copyTextureSupport;
#if UNITY_2021_2_OR_NEWER
        public string _supportsRayTracing;
#endif
        public string _renderingThreadingMode;
    }
}
