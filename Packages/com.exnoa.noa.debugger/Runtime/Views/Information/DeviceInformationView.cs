using System.Linq;

namespace NoaDebugger
{
    sealed class DeviceInformationView : InformationCategoryView<DeviceInformationViewLinker>
    {
        protected override void _OnShow(DeviceInformationViewLinker linker)
        {
            base._OnShow(linker);

            SetDeviceGeneralGroup(linker._device, linker._isSelection);
            SetOSGroup(linker._os, linker._isSelection);
            SetProcessorGroup(linker._processor, linker._isSelection);
            SetGraphicsDeviceGroup(linker._graphicsDevice, linker._isSelection);
            SetSystemMemoryGroup(linker._systemMemory, linker._isSelection);
            SetDisplayGroup(linker._display, linker._isSelection);
            SetGraphicsSupportGroup(linker._graphicsSupport, linker._isSelection);
            SetTextureFormatSupportGroup(linker._textureFormatSupport, linker._isSelection);
            SetFeatureSupportGroup(linker._featureSupport, linker._isSelection);
            SetNetworkGroup(linker._network, linker._isSelection);
            SetSystemGroup(linker._system, linker._isSelection);
            SetInputGroup(linker._input, linker._isSelection);
            gameObject.SetActive(true);
        }

        void SetDeviceGeneralGroup(SelectableDeviceGeneralData data, bool isSelection)
        {
            SetGroupCommon(data.GroupName, data, isSelection);
        }

        void SetOSGroup(SelectableOSData data, bool isSelection)
        {
            SetGroupCommon(data.GroupName, data, isSelection);
        }

        void SetProcessorGroup(SelectableProcessorData data, bool isSelection)
        {
            SetGroupCommon(data.GroupName, data, isSelection);
        }

        void SetGraphicsDeviceGroup(SelectableGraphicsDeviceData data, bool isSelection)
        {
            SetGroupCommon(data.GroupName, data, isSelection);
        }

        void SetSystemMemoryGroup(SelectableSystemMemoryData data, bool isSelection)
        {
            SetGroupCommon(data.GroupName, data, isSelection);
        }

        void SetDisplayGroup(SelectableDisplayData data, bool isSelection)
        {
            SetGroupCommon(data.GroupName, data, isSelection);
        }

        void SetGraphicsSupportGroup(SelectableGraphicsSupportData data, bool isSelection)
        {
            string groupName = data.GroupName;
            foreach (var info in data.Items)
            {
                var keyValue = info.KeyValuePair;
#if UNITY_6000_1_OR_NEWER
                if (keyValue.Key == "Variable Rate Shading")
                {
                    SetValueByType(groupName, keyValue.Key, keyValue.Value);
                }
                else
#endif
#if UNITY_2021_2_OR_NEWER
                if (keyValue.Key == "Ray Tracing")
                {
                    SetValueByType(groupName, keyValue.Key, keyValue.Value);
                }
                else
#endif
                {
                    SetValueByType(groupName, keyValue.Key, keyValue.Value);
                }

                var isSelect = isSelection && info.IsSelect;
                InitChildPanel(groupName, keyValue.Key, isSelect, isSelection);
            }

            bool isChildSelected = isSelection && data.Items.Any(item => item.IsSelect);
            InitGroupPanel(groupName, isChildSelected, isSelection);
        }

        void SetTextureFormatSupportGroup(SelectableTextureFormatSupportData data, bool isSelection)
        {
            SetGroupCommon(data.GroupName, data, isSelection);
        }

        void SetFeatureSupportGroup(SelectableFeatureSupportData data, bool isSelection)
        {
            SetGroupCommon(data.GroupName, data, isSelection);
        }

        void SetNetworkGroup(SelectableNetworkData data, bool isSelection)
        {
            SetGroupCommon(data.GroupName, data, isSelection);
        }

        void SetSystemGroup(SelectableSystemData data, bool isSelection)
        {
            SetGroupCommon(data.GroupName, data, isSelection);
        }

        void SetInputGroup(SelectableInputData data, bool isSelection)
        {
            SetGroupCommon(data.GroupName, data, isSelection);
        }
    }

    sealed class DeviceInformationViewLinker : InformationCommonViewLinker
    {
        public SelectableDeviceGeneralData _device;
        public SelectableOSData _os;
        public SelectableProcessorData _processor;
        public SelectableGraphicsDeviceData _graphicsDevice;
        public SelectableSystemMemoryData _systemMemory;
        public SelectableDisplayData _display;
        public SelectableGraphicsSupportData _graphicsSupport;
        public SelectableTextureFormatSupportData _textureFormatSupport;
        public SelectableFeatureSupportData _featureSupport;
        public SelectableNetworkData _network;
        public SelectableSystemData _system;
        public SelectableInputData _input;
    }
}
