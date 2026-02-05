namespace NoaDebugger
{
    sealed class SelectableDeviceGeneralData : SelectableGroup<InformationDeviceGeneralData>
    {
        const string GROUP_NAME = "General";
        const string KEY_MODEL = "Model";
        const string KEY_TYPE = "Type";
        const string KEY_NAME = "Name";

        public override string GroupName => GROUP_NAME;

        public SelectableDeviceGeneralData(InformationDeviceGeneralData data)
        {
            AddItem(KEY_MODEL, data._model);
            AddItem(KEY_TYPE, data._type);
            AddItem(KEY_NAME, data._name);
        }

        public override void Update(InformationDeviceGeneralData data)
        {
            UpdateItem(KEY_MODEL, data._model);
            UpdateItem(KEY_TYPE, data._type);
            UpdateItem(KEY_NAME, data._name);
        }
    }
}
