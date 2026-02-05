namespace NoaDebugger
{
    sealed class SelectableOtherData : SelectableKeyValueBase, ISelectableGroup<InformationOtherData>
    {
        const string GROUP_NAME = "Other";
        const string KEY_PERSISTENT_DATA_PATH = "Persistent Data Path";
        const string KEY_STREAMING_ASSETS_PATH = "Streaming Assets Path";
        const string KEY_TEMPORARY_CACHE_PATH = "Temporary Cache Path";
        const string KEY_DATA_PATH = "Data Path";
        const string KEY_INSTALLER_NAME = "Installer Name";
        const string KEY_INSTALL_MODE = "Install Mode";
        const string KEY_COMMAND_LINE_ARGS = "Command Line Args";
        const string KEY_ABSOLUTE_URL = "Absolute URL";
        const string KEY_BUILD_GUID = "Build GUID";
        const string KEY_CLOUD_PROJECT_ID = "Cloud Project ID";

        public string GroupName => GROUP_NAME;

        public SelectableOtherData(InformationOtherData other)
        {
            AddItem(KEY_PERSISTENT_DATA_PATH, other._persistentDataPath);
            AddItem(KEY_STREAMING_ASSETS_PATH, other._streamingAssetsPath);
            AddItem(KEY_TEMPORARY_CACHE_PATH, other._temporaryCachePath);
            AddItem(KEY_DATA_PATH, other._dataPath);
            AddItem(KEY_INSTALLER_NAME, other._installerName);
            AddItem(KEY_INSTALL_MODE, other._installMode);
            AddItem(KEY_COMMAND_LINE_ARGS, other._commandLineArgs);
            AddItem(KEY_ABSOLUTE_URL, other._absoluteUrl);
            AddItem(KEY_BUILD_GUID, other._buildGuid);
            AddItem(KEY_CLOUD_PROJECT_ID, other._cloudProjectId);
        }

        public void Update(InformationOtherData other)
        {
            UpdateItem(KEY_PERSISTENT_DATA_PATH, other._persistentDataPath);
            UpdateItem(KEY_STREAMING_ASSETS_PATH, other._streamingAssetsPath);
            UpdateItem(KEY_TEMPORARY_CACHE_PATH, other._temporaryCachePath);
            UpdateItem(KEY_DATA_PATH, other._dataPath);
            UpdateItem(KEY_INSTALLER_NAME, other._installerName);
            UpdateItem(KEY_INSTALL_MODE, other._installMode);
            UpdateItem(KEY_COMMAND_LINE_ARGS, other._commandLineArgs);
            UpdateItem(KEY_ABSOLUTE_URL, other._absoluteUrl);
            UpdateItem(KEY_BUILD_GUID, other._buildGuid);
            UpdateItem(KEY_CLOUD_PROJECT_ID, other._cloudProjectId);
        }
    }
}
