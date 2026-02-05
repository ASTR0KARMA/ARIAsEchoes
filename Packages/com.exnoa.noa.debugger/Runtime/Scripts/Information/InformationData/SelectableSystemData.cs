namespace NoaDebugger
{
    sealed class SelectableSystemData : SelectableGroup<InformationDeviceSystemData>
    {
        const string GROUP_NAME = "System";
        const string KEY_LANGUAGE = "Language";
        const string KEY_REGION = "Region";
        const string KEY_TIME_ZONE = "Time Zone";
        const string KEY_LOCAL_DATE_TIME = "Local Date Time";

        public override string GroupName => GROUP_NAME;

        public SelectableSystemData(InformationDeviceSystemData data)
        {
            AddItem(KEY_LANGUAGE, data._language);
            AddItem(KEY_REGION, data._region);
            AddItem(KEY_TIME_ZONE, data._timeZone);
            AddItem(KEY_LOCAL_DATE_TIME, data._localDateTime);
        }

        public override void Update(InformationDeviceSystemData data)
        {
            UpdateItem(KEY_LANGUAGE, data._language);
            UpdateItem(KEY_REGION, data._region);
            UpdateItem(KEY_TIME_ZONE, data._timeZone);
            UpdateItem(KEY_LOCAL_DATE_TIME, data._localDateTime);
        }
    }
}
