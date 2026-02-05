#if USE_PIPELINE_URP
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
#endif

namespace NoaDebugger
{
    static class UrpRenderScaleModel
    {
        public static float GetRenderScale()
        {
#if USE_PIPELINE_URP
            var pipelineAsset = GraphicsSettings.defaultRenderPipeline as UniversalRenderPipelineAsset;
            if (pipelineAsset == null)
            {
                return -1;
            }

            return pipelineAsset.renderScale;
#else
            return -1;
#endif
        }

        public static void SetRenderScale(float value)
        {
#if USE_PIPELINE_URP
            var pipelineAsset = GraphicsSettings.defaultRenderPipeline as UniversalRenderPipelineAsset;
            if (pipelineAsset == null)
            {
                return;
            }

            pipelineAsset.renderScale = value;
#else
#endif
        }
    }
}
