using System;

namespace NoaDebugger
{
    sealed class InformationGraphicsSettingsData
    {
        public string _renderPipeline;
        public IMutableParameter<bool> _srpBatching;
        public string _activeColorSpace;
        public string _desiredColorSpace;
        public IMutableParameter<int> _qualityLevel;
        public IMutableParameter<int> _antiAliasing;
        public IMutableParameter<Enum> _shadowQuality;
        public IMutableParameter<Enum> _shadowResolution;
        public IMutableParameter<float> _shadowDistance;
        public IMutableParameter<int> _shadowCascades;
        public IMutableParameter<float> _lodBias;
        public IMutableParameter<float> _urpRenderScale;
#if UNITY_2022_2_OR_NEWER
        public IMutableParameter<int> _globalTextureMipmapLimit;
#endif
        public IMutableParameter<int> _vSyncCount;
    }
}
