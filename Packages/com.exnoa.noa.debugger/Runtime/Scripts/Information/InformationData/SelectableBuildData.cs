namespace NoaDebugger
{
    sealed class SelectableBuildData : SelectableGroup<InformationBuildData>
    {
        const string GROUP_NAME = "Build";
        const string KEY_COMPANY_NAME = "Company Name";
        const string KEY_PRODUCT_NAME = "Product Name";
        const string KEY_IDENTIFIER = "Identifier";
        const string KEY_VERSION = "Version";
        const string KEY_UNITY_VERSION = "Unity Version";
        const string KEY_SCRIPTING_BACKEND = "Scripting Backend";
        const string KEY_DEBUG_BUILD = "Debug Build";

        public override string GroupName => GROUP_NAME;

        public SelectableBuildData(InformationBuildData build)
        {
            AddItem(KEY_COMPANY_NAME, build._companyName);
            AddItem(KEY_PRODUCT_NAME, build._productName);
            AddItem(KEY_IDENTIFIER, build._identifier);
            AddItem(KEY_VERSION, build._version);
            AddItem(KEY_UNITY_VERSION, build._unityVersion);
            AddItem(KEY_SCRIPTING_BACKEND, build._scriptingBackend);
            AddItem(KEY_DEBUG_BUILD, build._debugBuild);
        }

        public override void Update(InformationBuildData build)
        {
            UpdateItem(KEY_COMPANY_NAME, build._companyName);
            UpdateItem(KEY_PRODUCT_NAME, build._productName);
            UpdateItem(KEY_IDENTIFIER, build._identifier);
            UpdateItem(KEY_VERSION, build._version);
            UpdateItem(KEY_UNITY_VERSION, build._unityVersion);
            UpdateItem(KEY_SCRIPTING_BACKEND, build._scriptingBackend);
            UpdateItem(KEY_DEBUG_BUILD, build._debugBuild);
        }
    }
}
