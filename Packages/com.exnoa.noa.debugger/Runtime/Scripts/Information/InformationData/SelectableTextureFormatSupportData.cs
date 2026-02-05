namespace NoaDebugger
{
    sealed class SelectableTextureFormatSupportData : SelectableGroup<InformationTextureFormatSupportData>
    {
        const string GROUP_NAME = "Texture Format Support";
        const string KEY_ASTC = "ASTC";
        const string KEY_ETC1 = "ETC1";
        const string KEY_ETC2 = "ETC2";
        const string KEY_DXT1 = "DXT1";
        const string KEY_DXT5 = "DXT5";
        const string KEY_BC4 = "BC4";
        const string KEY_BC5 = "BC5";
        const string KEY_BC6H = "BC6H";
        const string KEY_BC7 = "BC7";
        const string KEY_PVRTC = "PVRTC";

        public override string GroupName => GROUP_NAME;

        public SelectableTextureFormatSupportData(InformationTextureFormatSupportData data)
        {
            AddItem(KEY_ASTC, data._astc);
            AddItem(KEY_ETC1, data._etc1);
            AddItem(KEY_ETC2, data._etc2);
            AddItem(KEY_DXT1, data._dxt1);
            AddItem(KEY_DXT5, data._dxt5);
            AddItem(KEY_BC4, data._bc4);
            AddItem(KEY_BC5, data._bc5);
            AddItem(KEY_BC6H, data._bc6H);
            AddItem(KEY_BC7, data._bc7);
#if !UNITY_6000_1_OR_NEWER
            AddItem(KEY_PVRTC, data._pvrtc);
#endif
        }

        public override void Update(InformationTextureFormatSupportData data)
        {
            UpdateItem(KEY_ASTC, data._astc);
            UpdateItem(KEY_ETC1, data._etc1);
            UpdateItem(KEY_ETC2, data._etc2);
            UpdateItem(KEY_DXT1, data._dxt1);
            UpdateItem(KEY_DXT5, data._dxt5);
            UpdateItem(KEY_BC4, data._bc4);
            UpdateItem(KEY_BC5, data._bc5);
            UpdateItem(KEY_BC6H, data._bc6H);
            UpdateItem(KEY_BC7, data._bc7);
#if !UNITY_6000_1_OR_NEWER
            UpdateItem(KEY_PVRTC, data._pvrtc);
#endif
        }
    }
}
