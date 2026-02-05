using UnityEngine;

namespace NoaDebugger
{
    /// <summary>
    /// Represents a collection of the screen information.
    /// </summary>
    public sealed class ScreenInformationGroup
    {
        /// <summary>
        /// Gets the current width of the screen window in pixels.
        /// </summary>
        public int Width => Screen.width;

        /// <summary>
        /// Gets the current height of the screen window in pixels.
        /// </summary>
        public int Height => Screen.height;

        /// <summary>
        /// Gets or sets the target frame rate at which Unity tries to render your game.
        /// </summary>
        public int TargetFrameRate
        {
            get => Application.targetFrameRate;
            set => Application.targetFrameRate = value;
        }

        /// <summary>
        /// True if full-screen mode for the application is enabled; otherwise false.
        /// </summary>
        public bool IsFullScreen
        {
            get => Screen.fullScreen;
            set => Screen.fullScreen = value;
        }

        /// <summary>
        /// Gets or sets the display mode of your application.
        /// </summary>
        public FullScreenMode FullScreenMode
        {
            get => Screen.fullScreenMode;
            set => Screen.fullScreenMode = value;
        }

        /// <summary>
        /// Gets or sets the logical orientation of the screen.
        /// </summary>
        public ScreenOrientation Orientation
        {
            get => Screen.orientation;
            set => Screen.orientation = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenInformationGroup"/>.
        /// </summary>
        internal ScreenInformationGroup()
        {
            const string groupName = "Screen";
            TargetFrameRateParameter = new IntInformationParameter(groupName, "TargetFrameRate", TargetFrameRate, value => TargetFrameRate = value);
            IsFullScreenParameter = new BoolInformationParameter(groupName, "IsFullScreen", IsFullScreen, value => IsFullScreen = value);
            FullScreenModeParameter = new EnumInformationParameter(groupName, "FullScreenMode", FullScreenMode, value => FullScreenMode = (FullScreenMode)value);
            OrientationParameter = new EnumInformationParameter(groupName, "Orientation", Orientation, value =>
            {
                if (Orientation != (ScreenOrientation)value)
                {
                    Orientation = (ScreenOrientation)value;
                }
            });
        }

        internal void UpdateSettings()
        {
            TargetFrameRateParameter.ChangeValue(TargetFrameRate);
            IsFullScreenParameter.ChangeValue(IsFullScreen);
            FullScreenModeParameter.ChangeValue(FullScreenMode);
            OrientationParameter.ChangeValue(Orientation);
        }

        internal IntInformationParameter TargetFrameRateParameter { get; }
        internal BoolInformationParameter IsFullScreenParameter { get; }
        internal EnumInformationParameter FullScreenModeParameter { get; }
        internal EnumInformationParameter OrientationParameter { get; }
    }
}
