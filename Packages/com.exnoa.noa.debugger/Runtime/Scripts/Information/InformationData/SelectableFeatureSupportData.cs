namespace NoaDebugger
{
    sealed class SelectableFeatureSupportData : SelectableGroup<InformationFeatureSupportData>
    {
        const string GROUP_NAME = "Feature Support";
        const string KEY_AUDIO = "Audio";
        const string KEY_ACCELEROMETER = "Accelerometer";
        const string KEY_GYROSCOPE = "Gyroscope";
        const string KEY_LOCATION_SERVICE = "Location Service";
        const string KEY_VIBRATION = "Vibration";

        public override string GroupName => GROUP_NAME;

        public SelectableFeatureSupportData(InformationFeatureSupportData data)
        {
            AddItem(KEY_AUDIO, data._audio);
            AddItem(KEY_ACCELEROMETER, data._accelerometer);
            AddItem(KEY_GYROSCOPE, data._gyroscope);
            AddItem(KEY_LOCATION_SERVICE, data._locationService);
            AddItem(KEY_VIBRATION, data._vibration);
        }

        public override void Update(InformationFeatureSupportData data)
        {
            UpdateItem(KEY_AUDIO, data._audio);
            UpdateItem(KEY_ACCELEROMETER, data._accelerometer);
            UpdateItem(KEY_GYROSCOPE, data._gyroscope);
            UpdateItem(KEY_LOCATION_SERVICE, data._locationService);
            UpdateItem(KEY_VIBRATION, data._vibration);
        }
    }
}
