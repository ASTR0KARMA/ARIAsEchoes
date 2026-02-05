using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoaDebugger
{
    /// <summary>
    /// Represents a collection of the runtime information.
    /// </summary>
    public sealed class RuntimeInformationGroup
    {
        /// <summary>
        /// Gets the platform the game is running on.
        /// </summary>
        public RuntimePlatform Platform { get; }

        /// <summary>
        /// Gets the currently active Scene.
        /// </summary>
        public Scene CurrentScene => SceneManager.GetActiveScene();

        /// <summary>
        /// Gets the time in seconds since the last non-additive scene finished loading.
        /// </summary>
        public float ScenePlayTime => Time.timeSinceLevelLoad;

        /// <summary>
        /// Gets the real time in seconds since the game started.
        /// </summary>
        public float AppPlayTime => Time.realtimeSinceStartup;

        /// <summary>
        /// Gets or sets the scale at which time passes.
        /// </summary>
        public float TimeScale
        {
            get => Time.timeScale;
            set => Time.timeScale = value;
        }

        /// <summary>
        /// True if the player should run when the application is in the background; otherwise false.
        /// </summary>
        public bool RunInBackground
        {
            get => Application.runInBackground;
            set => Application.runInBackground = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeInformationGroup"/>.
        /// </summary>
        internal RuntimeInformationGroup()
        {
            const string groupName = "Runtime";
            Platform = Application.platform;
            TimeScaleParameter = new FloatInformationParameter(groupName, "TimeScale", TimeScale, value => TimeScale = value);
            RunInBackgroundParameter = new BoolInformationParameter(groupName, "RunInBackground", RunInBackground, value => RunInBackground = value);
        }

        internal void UpdateSettings()
        {
            TimeScaleParameter.ChangeValue(TimeScale);
            RunInBackgroundParameter.ChangeValue(RunInBackground);
        }

        internal FloatInformationParameter TimeScaleParameter { get; }
        internal BoolInformationParameter RunInBackgroundParameter { get; }
    }
}
