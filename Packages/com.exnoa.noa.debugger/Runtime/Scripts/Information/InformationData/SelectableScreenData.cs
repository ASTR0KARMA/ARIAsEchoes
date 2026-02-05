namespace NoaDebugger
{
    sealed class SelectableScreenData : SelectableKeyValueBase, ISelectableGroup<InformationScreenData>
    {
        const string GROUP_NAME = "Screen";
        const string KEY_WIDTH = "Width";
        const string KEY_HEIGHT = "Height";
        const string KEY_TARGET_FRAME_RATE = "Target Frame Rate";
        const string KEY_FULL_SCREEN = "Full Screen";
        const string KEY_FULL_SCREEN_MODE = "Full Screen Mode";
        const string KEY_ORIENTATION = "Orientation";

        public string GroupName => GROUP_NAME;

        public SelectableScreenData(InformationScreenData screen)
        {
            AddItem(KEY_WIDTH, screen._width);
            AddItem(KEY_HEIGHT, screen._height);
            AddItem(KEY_TARGET_FRAME_RATE, screen._targetFrameRate);
            AddItem(KEY_FULL_SCREEN, screen._fullScreen);
            AddItem(KEY_FULL_SCREEN_MODE, screen._fullScreenMode);
            AddItem(KEY_ORIENTATION, screen._orientation);
        }

        public void Update(InformationScreenData screen)
        {
            UpdateItem(KEY_WIDTH, screen._width);
            UpdateItem(KEY_HEIGHT, screen._height);
            UpdateItem(KEY_TARGET_FRAME_RATE, screen._targetFrameRate);
            UpdateItem(KEY_FULL_SCREEN, screen._fullScreen);
            UpdateItem(KEY_FULL_SCREEN_MODE, screen._fullScreenMode);
            UpdateItem(KEY_ORIENTATION, screen._orientation);
        }
    }
}
