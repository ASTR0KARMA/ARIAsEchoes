namespace NoaDebugger
{
    sealed class InformationTextureFormatSupportData
    {
        public string _astc;
        public string _etc1;
        public string _etc2;
        public string _dxt1;
        public string _dxt5;
        public string _bc4;
        public string _bc5;
        public string _bc6H;
        public string _bc7;
#if !UNITY_6000_1_OR_NEWER
        public string _pvrtc;
#endif
    }
}
