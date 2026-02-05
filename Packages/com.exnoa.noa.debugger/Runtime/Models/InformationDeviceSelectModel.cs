using System;
using System.Collections.Generic;
using System.Globalization;

namespace NoaDebugger
{
    sealed class InformationDeviceSelectModel : InformationBaseSelectModel
    {
        SelectableDeviceGeneralData _selectableDeviceGeneral;
        SelectableOSData _selectableOS;
        SelectableProcessorData _selectableProcessor;
        SelectableGraphicsDeviceData _selectableGraphicsDevice;
        SelectableSystemMemoryData _selectableSystemMemory;
        SelectableDisplayData _selectableDisplay;
        SelectableGraphicsSupportData _selectableGraphicsSupport;
        SelectableTextureFormatSupportData _selectableTextureFormatSupport;
        SelectableFeatureSupportData _selectableFeatureSupport;
        SelectableNetworkData _selectableNetwork;
        SelectableSystemData _selectableSystem;
        SelectableInputData _selectableInput;

        public SelectableDeviceGeneralData DeviceGeneralInfo => _selectableDeviceGeneral;
        public SelectableOSData OSInfo => _selectableOS;
        public SelectableProcessorData ProcessorInfo => _selectableProcessor;
        public SelectableGraphicsDeviceData GraphicsDeviceInfo => _selectableGraphicsDevice;
        public SelectableSystemMemoryData SystemMemoryInfo => _selectableSystemMemory;
        public SelectableDisplayData DisplayInfo => _selectableDisplay;
        public SelectableGraphicsSupportData GraphicsSupportInfo => _selectableGraphicsSupport;
        public SelectableTextureFormatSupportData TextureFormatSupportInfo => _selectableTextureFormatSupport;
        public SelectableFeatureSupportData FeatureSupportInfo => _selectableFeatureSupport;
        public SelectableNetworkData NetworkInfo => _selectableNetwork;
        public SelectableSystemData SystemInfo => _selectableSystem;
        public SelectableInputData InputInfo => _selectableInput;

        readonly List<SelectableKeyValueBase> _selectableGroups;
        readonly ISelectableGroupProvider _groupProvider;

        public override string TabName => "Device";
        protected override ISelectableGroupProvider GroupProvider => _groupProvider;

        public InformationDeviceSelectModel() : base(InformationTabType.Device)
        {
            _selectableGroups = new List<SelectableKeyValueBase>();
            _groupProvider = new StandardGroupProviderAdapter(_selectableGroups);
        }

        public void UpdateDeviceInfo(DeviceInformationModel deviceInformationModel)
        {
            deviceInformationModel.OnUpdate();

            _UpdateOrCreateDeviceGeneral(deviceInformationModel.DeviceGeneral);
            _UpdateOrCreateOS(deviceInformationModel.OS);
            _UpdateOrCreateProcessor(deviceInformationModel.Processor);
            _UpdateOrCreateGraphicsDevice(deviceInformationModel.GraphicsDevice);
            _UpdateOrCreateSystemMemory(deviceInformationModel.SystemMemory);
            _UpdateOrCreateDisplay(deviceInformationModel.Display);
            _UpdateOrCreateGraphicsSupport(deviceInformationModel.GraphicsSupport);
            _UpdateOrCreateTextureFormatSupport(deviceInformationModel.TextureFormatSupport);
            _UpdateOrCreateFeatureSupport(deviceInformationModel.FeatureSupport);
            _UpdateOrCreateNetwork(deviceInformationModel.Network);
            _UpdateOrCreateSystem(deviceInformationModel.System);
            _UpdateOrCreateInput(deviceInformationModel.Input);
            _UpdateDeviceSelectablesList();
        }

        void _UpdateOrCreateDeviceGeneral(DeviceGeneralInformationGroup general)
        {
            var deviceGeneral = new InformationDeviceGeneralData
            {
                _model = general.Model,
                _type = general.Type.ToString(),
                _name = general.Name
            };

            if (_selectableDeviceGeneral == null)
            {
                _selectableDeviceGeneral = new SelectableDeviceGeneralData(deviceGeneral);
            }
            else
            {
                _selectableDeviceGeneral.Update(deviceGeneral);
            }
        }

        void _UpdateOrCreateOS(OSInformationGroup os)
        {
            var osInfo = new InformationOSData
            {
                _name = os.Name,
                _family = os.Family.ToString()
            };

            if (_selectableOS == null)
            {
                _selectableOS = new SelectableOSData(osInfo);
            }
            else
            {
                _selectableOS.Update(osInfo);
            }
        }

        void _UpdateOrCreateProcessor(ProcessorInformationGroup processor)
        {
            var processorInfo = new InformationProcessorData
            {
                _type = processor.Type,
                _count = processor.Count.ToString(),
                _frequency = processor.Frequency.ToString()
            };

            if (_selectableProcessor == null)
            {
                _selectableProcessor = new SelectableProcessorData(processorInfo);
            }
            else
            {
                _selectableProcessor.Update(processorInfo);
            }
        }

        void _UpdateOrCreateGraphicsDevice(GraphicsDeviceInformationGroup graphicsDevice)
        {
            var graphicsDeviceInfo = new InformationGraphicsDeviceData
            {
                _name = graphicsDevice.Name,
                _version = graphicsDevice.Version,
                _type = graphicsDevice.Type.ToString(),
                _vendor = graphicsDevice.Vendor,
                _memorySize = graphicsDevice.MemorySize.ToString(),
                _multiThreaded = graphicsDevice.MultiThreaded.ToString(),
                _shaderLevel = graphicsDevice.ShaderLevel.ToString()
            };

            if (_selectableGraphicsDevice == null)
            {
                _selectableGraphicsDevice = new SelectableGraphicsDeviceData(graphicsDeviceInfo);
            }
            else
            {
                _selectableGraphicsDevice.Update(graphicsDeviceInfo);
            }
        }

        void _UpdateOrCreateSystemMemory(SystemMemoryInformationGroup systemMemory)
        {
            var systemMemoryInfo = new InformationSystemMemoryData
            {
                _totalSize = systemMemory.TotalSize.ToString()
            };

            if (_selectableSystemMemory == null)
            {
                _selectableSystemMemory = new SelectableSystemMemoryData(systemMemoryInfo);
            }
            else
            {
                _selectableSystemMemory.Update(systemMemoryInfo);
            }
        }

        void _UpdateOrCreateDisplay(DisplayInformationGroup display)
        {
            var displayInfo = new InformationDisplayData
            {
                _resolution = display.Resolution.ToString(),
                _aspect = display.Aspect.ToString(CultureInfo.InvariantCulture),
                _refreshRate = display.RefreshRate.ToString(),
                _dpi = display.Dpi.ToString(CultureInfo.InvariantCulture),
                _safeArea = display.SafeArea.ToString(),
                _hdr = display.Hdr.ToString()
            };

            if (_selectableDisplay == null)
            {
                _selectableDisplay = new SelectableDisplayData(displayInfo);
            }
            else
            {
                _selectableDisplay.Update(displayInfo);
            }
        }

        void _UpdateOrCreateGraphicsSupport(GraphicsSupportInformationGroup graphicsSupport)
        {
            var graphicsSupportInfo = new InformationGraphicsSupportData
            {
                _maxTextureSize = graphicsSupport.MaxTextureSize.ToString(),
                _maxCubemapSize = graphicsSupport.MaxCubemapSize.ToString(),
                _supportsInstancing = graphicsSupport.SupportsInstancing.ToString(),
                _supportsComputeShaders = graphicsSupport.SupportsComputeShaders.ToString(),
                _supportsGeometryShaders = graphicsSupport.SupportsGeometryShaders.ToString(),
                _supportsTessellationShaders = graphicsSupport.SupportsTessellationShaders.ToString(),
#if UNITY_6000_1_OR_NEWER
                _supportsVariableRateShading = graphicsSupport.SupportsVariableRateShading.ToString(),
#endif
                _supportsShadows = graphicsSupport.SupportsShadows.ToString(),
                _supportsRawShadowDepthSampling = graphicsSupport.SupportsRawShadowDepthSampling.ToString(),
                _npotSupport = graphicsSupport.NpotSupport.ToString(),
                _supports3DTextures = graphicsSupport.Supports3DTextures.ToString(),
                _supports3DRenderTextures = graphicsSupport.Supports3DRenderTextures.ToString(),
                _supports2DArrayTextures = graphicsSupport.Supports2DArrayTextures.ToString(),
                _supportsCubemapArrayTextures = graphicsSupport.SupportsCubemapArrayTextures.ToString(),
                _supportsSparseTextures = graphicsSupport.SupportsSparseTextures.ToString(),
                _copyTextureSupport = graphicsSupport.CopyTextureSupport.ToString(),
#if UNITY_2021_2_OR_NEWER
                _supportsRayTracing = graphicsSupport.SupportsRayTracing.ToString(),
#endif
                _renderingThreadingMode = graphicsSupport.RenderingThreadingMode.ToString()
            };

            if (_selectableGraphicsSupport == null)
            {
                _selectableGraphicsSupport = new SelectableGraphicsSupportData(graphicsSupportInfo);
            }
            else
            {
                _selectableGraphicsSupport.Update(graphicsSupportInfo);
            }
        }

        void _UpdateOrCreateTextureFormatSupport(TextureFormatSupportInformationGroup textureFormatSupport)
        {
            var textureFormatSupportInfo = new InformationTextureFormatSupportData
            {
                _astc = textureFormatSupport.SupportsAstc.ToString(),
                _etc1 = textureFormatSupport.SupportsEtc1.ToString(),
                _etc2 = textureFormatSupport.SupportsEtc2.ToString(),
                _dxt1 = textureFormatSupport.SupportsDxt1.ToString(),
                _dxt5 = textureFormatSupport.SupportsDxt5.ToString(),
                _bc4 = textureFormatSupport.SupportsBc4.ToString(),
                _bc5 = textureFormatSupport.SupportsBc5.ToString(),
                _bc6H = textureFormatSupport.SupportsBc6H.ToString(),
                _bc7 = textureFormatSupport.SupportsBc7.ToString(),
#if !UNITY_6000_1_OR_NEWER
                _pvrtc = textureFormatSupport.SupportsPvrtc.ToString()
#endif
            };

            if (_selectableTextureFormatSupport == null)
            {
                _selectableTextureFormatSupport = new SelectableTextureFormatSupportData(textureFormatSupportInfo);
            }
            else
            {
                _selectableTextureFormatSupport.Update(textureFormatSupportInfo);
            }
        }

        void _UpdateOrCreateFeatureSupport(FeatureSupportInformationGroup featureSupport)
        {
            var featureSupportInfo = new InformationFeatureSupportData
            {
                _audio = featureSupport.SupportsAudio.ToString(),
                _accelerometer = featureSupport.SupportsAccelerometer.ToString(),
                _gyroscope = featureSupport.SupportsGyroscope.ToString(),
                _locationService = featureSupport.SupportsLocationService.ToString(),
                _vibration = featureSupport.SupportsVibration.ToString()
            };

            if (_selectableFeatureSupport == null)
            {
                _selectableFeatureSupport = new SelectableFeatureSupportData(featureSupportInfo);
            }
            else
            {
                _selectableFeatureSupport.Update(featureSupportInfo);
            }
        }

        void _UpdateOrCreateNetwork(NetworkInformationGroup network)
        {
            var networkInfo = new InformationNetworkData
            {
                _reachability = network.Reachability.ToString()
            };

            if (_selectableNetwork == null)
            {
                _selectableNetwork = new SelectableNetworkData(networkInfo);
            }
            else
            {
                _selectableNetwork.Update(networkInfo);
            }
        }

        void _UpdateOrCreateSystem(SystemInformationGroup system)
        {
            var systemInfo = new InformationDeviceSystemData
            {
                _language = system.Language.ToString(),
                _region = system.Region.Name,
                _timeZone = system.TimeZone.DisplayName,
                _localDateTime = system.LocalDateTime.ToString(CultureInfo.InvariantCulture)
            };

            if (_selectableSystem == null)
            {
                _selectableSystem = new SelectableSystemData(systemInfo);
            }
            else
            {
                _selectableSystem.Update(systemInfo);
            }
        }

        void _UpdateOrCreateInput(InputInformationGroup input)
        {
            var inputInfo = new InformationInputData
            {
                _hasTouchscreen = input.HasTouchscreen.ToString(),
                _hasMouse = input.HasMouse.ToString(),
                _hasKeyboard = input.HasKeyboard.ToString(),
                _gamepads = input.HasGamepads.ToString(),
                _enabledXR = input.EnabledXR.ToString()
            };

            if (_selectableInput == null)
            {
                _selectableInput = new SelectableInputData(inputInfo);
            }
            else
            {
                _selectableInput.Update(inputInfo);
            }
        }

        void _UpdateDeviceSelectablesList()
        {
            _selectableGroups.Clear();

            if (_selectableDeviceGeneral != null)
            {
                _selectableGroups.Add(_selectableDeviceGeneral);
            }
            if (_selectableOS != null)
            {
                _selectableGroups.Add(_selectableOS);
            }
            if (_selectableProcessor != null)
            {
                _selectableGroups.Add(_selectableProcessor);
            }
            if (_selectableGraphicsDevice != null)
            {
                _selectableGroups.Add(_selectableGraphicsDevice);
            }
            if (_selectableSystemMemory != null)
            {
                _selectableGroups.Add(_selectableSystemMemory);
            }
            if (_selectableDisplay != null)
            {
                _selectableGroups.Add(_selectableDisplay);
            }
            if (_selectableGraphicsSupport != null)
            {
                _selectableGroups.Add(_selectableGraphicsSupport);
            }
            if (_selectableTextureFormatSupport != null)
            {
                _selectableGroups.Add(_selectableTextureFormatSupport);
            }
            if (_selectableFeatureSupport != null)
            {
                _selectableGroups.Add(_selectableFeatureSupport);
            }
            if (_selectableNetwork != null)
            {
                _selectableGroups.Add(_selectableNetwork);
            }
            if (_selectableSystem != null)
            {
                _selectableGroups.Add(_selectableSystem);
            }
            if (_selectableInput != null)
            {
                _selectableGroups.Add(_selectableInput);
            }
        }
    }
}
