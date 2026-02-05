using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace NoaDebugger
{
    /// <summary>
    /// Runtime Information
    /// </summary>
    [Obsolete("Use the 'NoaDebugger.*InformationGroup' classes instead.")]
    public sealed class RuntimeInfo
    {
        /// <summary>
        /// Total elapsed time
        /// </summary>
        public float PlayTime { private set; get; }

        /// <summary>
        /// Elapsed time in the current scene
        /// </summary>
        public float LevelPlayTime { private set; get; }

        /// <summary>
        /// Current scene name
        /// </summary>
        public string CurrentLevelSceneName { private set; get; }

        /// <summary>
        /// Current scene index number
        /// </summary>
        public int CurrentLevelBuildIndex { private set; get; }

        /// <summary>
        /// Quality level
        /// </summary>
        public int QualityLevel { private set; get; }

        /// <summary>
        /// RenderPipeline
        /// </summary>
        public string RenderPipeline { private set; get; }

        /// <summary>
        /// SRPBatcher
        /// </summary>
        public string SRPBatcher { private set; get; }

        /// <summary>
        /// Generates RuntimeInfo
        /// </summary>
        internal RuntimeInfo()
        {
            PlayTime = Time.realtimeSinceStartup;
            LevelPlayTime = Time.timeSinceLevelLoad;
            CurrentLevelSceneName = SceneManager.GetActiveScene().name;
            CurrentLevelBuildIndex = SceneManager.GetActiveScene().buildIndex;
            QualityLevel = QualitySettings.GetQualityLevel();
            RefreshRenderPipelineSettings();
        }

        /// <summary>
        /// Updates the elapsed time
        /// </summary>
        internal void RefreshTime()
        {
            PlayTime = Time.realtimeSinceStartup;
            LevelPlayTime = Time.timeSinceLevelLoad;
        }

        /// <summary>
        /// Updates the scene information
        /// </summary>
        internal void RefreshScene()
        {
            CurrentLevelSceneName = SceneManager.GetActiveScene().name;
            CurrentLevelBuildIndex = SceneManager.GetActiveScene().buildIndex;
        }

        /// <summary>
        /// Updates the render pipeline settings
        /// </summary>
        internal void RefreshRenderPipelineSettings()
        {
            SRPBatcher = GraphicsSettings.useScriptableRenderPipelineBatching ? "true" : "Not Supported";
            var pipeline = GraphicsSettings.currentRenderPipeline;
            string renderPipelineName = "Other";

            if (pipeline == null)
            {
                renderPipelineName = "Built-in";
            }
            else
            {
                string typeName = pipeline.GetType().Name;

                switch (typeName)
                {
                    case "HDRenderPipelineAsset":
                        renderPipelineName = "HDRP";

                        break;

                    case "UniversalRenderPipelineAsset":
                        renderPipelineName = "URP";

                        break;
                }
            }

            RenderPipeline = renderPipelineName;
        }
    }
}
