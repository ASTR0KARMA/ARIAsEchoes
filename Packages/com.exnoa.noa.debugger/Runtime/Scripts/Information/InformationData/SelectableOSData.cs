namespace NoaDebugger
{
    sealed class SelectableOSData : SelectableGroup<InformationOSData>
    {
        const string GROUP_NAME = "OS";
        const string KEY_NAME = "Name";
        const string KEY_FAMILY = "Family";

        public override string GroupName => GROUP_NAME;

        public SelectableOSData(InformationOSData data)
        {
            AddItem(KEY_NAME, data._name);
            AddItem(KEY_FAMILY, data._family);
        }

        public override void Update(InformationOSData data)
        {
            UpdateItem(KEY_NAME, data._name);
            UpdateItem(KEY_FAMILY, data._family);
        }
    }
}
