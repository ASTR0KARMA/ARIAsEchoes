namespace NoaDebugger
{
    sealed class SelectableNetworkData : SelectableGroup<InformationNetworkData>
    {
        const string GROUP_NAME = "Network";
        const string KEY_REACHABILITY = "Reachability";

        public override string GroupName => GROUP_NAME;

        public SelectableNetworkData(InformationNetworkData data)
        {
            AddItem(KEY_REACHABILITY, data._reachability);
        }

        public override void Update(InformationNetworkData data)
        {
            UpdateItem(KEY_REACHABILITY, data._reachability);
        }
    }
}
