namespace NoaDebugger
{
    sealed class SelectableDisplayData : SelectableGroup<InformationDisplayData>
    {
        const string GROUP_NAME = "Display";
        const string KEY_RESOLUTION = "Resolution";
        const string KEY_ASPECT = "Aspect";
        const string KEY_REFRESH_RATE = "Refresh Rate (Hz)";
        const string KEY_DPI = "DPI";
        const string KEY_SAFE_AREA = "Safe Area";
        const string KEY_HDR = "HDR";

        public override string GroupName => GROUP_NAME;

        public SelectableDisplayData(InformationDisplayData data)
        {
            AddItem(KEY_RESOLUTION, data._resolution);
            AddItem(KEY_ASPECT, data._aspect);
            AddItem(KEY_REFRESH_RATE, data._refreshRate);
            AddItem(KEY_DPI, data._dpi);
            AddItem(KEY_SAFE_AREA, data._safeArea);
            AddItem(KEY_HDR, data._hdr);
        }

        public override void Update(InformationDisplayData data)
        {
            UpdateItem(KEY_RESOLUTION, data._resolution);
            UpdateItem(KEY_ASPECT, data._aspect);
            UpdateItem(KEY_REFRESH_RATE, data._refreshRate);
            UpdateItem(KEY_DPI, data._dpi);
            UpdateItem(KEY_SAFE_AREA, data._safeArea);
            UpdateItem(KEY_HDR, data._hdr);
        }
    }
}
