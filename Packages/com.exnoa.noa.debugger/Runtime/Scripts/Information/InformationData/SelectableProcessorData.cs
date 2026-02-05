namespace NoaDebugger
{
    sealed class SelectableProcessorData : SelectableGroup<InformationProcessorData>
    {
        const string GROUP_NAME = "Processor";
        const string KEY_TYPE = "Type";
        const string KEY_COUNT = "Count";
        const string KEY_FREQUENCY = "Frequency (MHz)";

        public override string GroupName => GROUP_NAME;

        public SelectableProcessorData(InformationProcessorData data)
        {
            AddItem(KEY_TYPE, data._type);
            AddItem(KEY_COUNT, data._count);
            AddItem(KEY_FREQUENCY, data._frequency);
        }

        public override void Update(InformationProcessorData data)
        {
            UpdateItem(KEY_TYPE, data._type);
            UpdateItem(KEY_COUNT, data._count);
            UpdateItem(KEY_FREQUENCY, data._frequency);
        }
    }
}
