using System;
using UnityEngine.SceneManagement;

namespace NoaDebugger
{
    [Obsolete("Use the 'NoaDebugger.*InformationModel' classes without obsolete attribute instead.")]
    sealed class UnityInformationModel : ModelBase
    {
        public UnityInfo UnityInfo { private set; get; }
        public RuntimeInfo RuntimeInfo { private set; get; }
        public FeaturesInfo FeaturesInfo { private set; get; }
        public GraphicsInfo GraphicsInfo { private set; get; }

        public UnityInformationModel()
        {
            UnityInfo = new UnityInfo();

            RuntimeInfo = new RuntimeInfo();

            FeaturesInfo = new FeaturesInfo();

            GraphicsInfo = new GraphicsInfo();

            SceneManager.activeSceneChanged += _ActiveSceneChanged;
        }

        public void OnUpdate()
        {
            RuntimeInfo.RefreshTime();
            RuntimeInfo.RefreshRenderPipelineSettings();
            UnityInfo.Refresh();
        }

        void _ActiveSceneChanged(Scene thisScene, Scene nextScene)
        {
            RuntimeInfo.RefreshScene();
        }

        public IKeyValueParser[] CreateExportData()
        {
            return new IKeyValueParser[]
            {
                KeyObjectParser.CreateFromClass(UnityInfo, "Unity Info"),
                KeyObjectParser.CreateFromClass(RuntimeInfo, "Runtime"),
                KeyObjectParser.CreateFromClass(FeaturesInfo, "Features"),
                KeyObjectParser.CreateFromClass(GraphicsInfo, "Graphics"),
            };
        }
    }
}
