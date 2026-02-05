namespace NoaDebugger
{
    sealed class SelectableSystemMemoryData : SelectableGroup<InformationSystemMemoryData>
    {
        const string GROUP_NAME = "System Memory";
        const string KEY_TOTAL_SIZE = "Total Size (MB)";

        public override string GroupName => GROUP_NAME;

        public SelectableSystemMemoryData(InformationSystemMemoryData data)
        {
            AddItem(KEY_TOTAL_SIZE, data._totalSize);
        }

        public override void Update(InformationSystemMemoryData data)
        {
            UpdateItem(KEY_TOTAL_SIZE, data._totalSize);
        }
    }
}
