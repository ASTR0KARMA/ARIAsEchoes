namespace NoaDebugger
{
    sealed class SelectableGraphicsDeviceData : SelectableGroup<InformationGraphicsDeviceData>
    {
        const string GROUP_NAME = "Graphics Device";
        const string KEY_NAME = "Name";
        const string KEY_VERSION = "Version";
        const string KEY_TYPE = "Type";
        const string KEY_VENDOR = "Vendor";
        const string KEY_MEMORY_SIZE = "Memory Size (MB)";
        const string KEY_MULTI_THREADED = "Multi Threaded";
        const string KEY_SHADER_LEVEL = "Shader Level";

        public override string GroupName => GROUP_NAME;

        public SelectableGraphicsDeviceData(InformationGraphicsDeviceData data)
        {
            AddItem(KEY_NAME, data._name);
            AddItem(KEY_VERSION, data._version);
            AddItem(KEY_TYPE, data._type);
            AddItem(KEY_VENDOR, data._vendor);
            AddItem(KEY_MEMORY_SIZE, data._memorySize);
            AddItem(KEY_MULTI_THREADED, data._multiThreaded);
            AddItem(KEY_SHADER_LEVEL, data._shaderLevel);
        }

        public override void Update(InformationGraphicsDeviceData data)
        {
            UpdateItem(KEY_NAME, data._name);
            UpdateItem(KEY_VERSION, data._version);
            UpdateItem(KEY_TYPE, data._type);
            UpdateItem(KEY_VENDOR, data._vendor);
            UpdateItem(KEY_MEMORY_SIZE, data._memorySize);
            UpdateItem(KEY_MULTI_THREADED, data._multiThreaded);
            UpdateItem(KEY_SHADER_LEVEL, data._shaderLevel);
        }
    }
}
