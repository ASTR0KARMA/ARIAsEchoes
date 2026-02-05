namespace NoaDebugger
{
    sealed class SelectableInputData : SelectableGroup<InformationInputData>
    {
        const string GROUP_NAME = "Input";
        const string KEY_TOUCHSCREEN = "Touchscreen";
        const string KEY_MOUSE = "Mouse";
        const string KEY_KEYBOARD = "Keyboard";
        const string KEY_GAMEPADS = "Gamepads";
        const string KEY_XR = "XR";

        public override string GroupName => GROUP_NAME;

        public SelectableInputData(InformationInputData data)
        {
            AddItem(KEY_TOUCHSCREEN, data._hasTouchscreen);
            AddItem(KEY_MOUSE, data._hasMouse);
            AddItem(KEY_KEYBOARD, data._hasKeyboard);
            AddItem(KEY_GAMEPADS, data._gamepads);
            AddItem(KEY_XR, data._enabledXR);
        }

        public override void Update(InformationInputData data)
        {
            UpdateItem(KEY_TOUCHSCREEN, data._hasTouchscreen);
            UpdateItem(KEY_MOUSE, data._hasMouse);
            UpdateItem(KEY_KEYBOARD, data._hasKeyboard);
            UpdateItem(KEY_GAMEPADS, data._gamepads);
            UpdateItem(KEY_XR, data._enabledXR);
        }
    }
}
