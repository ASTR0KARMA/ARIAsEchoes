using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace NoaDebugger
{
    sealed class InformationApplicationSelectModel : InformationBaseSelectModel
    {
        SelectableBuildData _selectableBuild;
        SelectableRuntimeData _selectableRuntime;
        SelectableScreenData _selectableScreen;
        SelectableGraphicsSettingsData _selectableGraphicsSettings;
        SelectableLoggingData _selectableLogging;
        SelectableOtherData _selectableOther;

        public SelectableBuildData BuildInfo => _selectableBuild;
        public SelectableRuntimeData RuntimeInfo => _selectableRuntime;
        public SelectableScreenData ScreenInfo => _selectableScreen;
        public SelectableGraphicsSettingsData GraphicsSettingsInfo => _selectableGraphicsSettings;
        public SelectableLoggingData LoggingInfo => _selectableLogging;
        public SelectableOtherData OtherInfo => _selectableOther;

        public Action<string, bool> OnSelectGroup;
        public Action<string, string, bool> OnSelectChild;

        public override string TabName { get { return "App"; } }

        readonly List<SelectableKeyValueBase> _selectableGroups;
        readonly ISelectableGroupProvider _groupProvider;
        protected override ISelectableGroupProvider GroupProvider => _groupProvider;

        public InformationApplicationSelectModel() : base(InformationTabType.App)
        {
            _selectableGroups = new　List<SelectableKeyValueBase>();
            _groupProvider = new StandardGroupProviderAdapter(_selectableGroups);
        }

        public void UpdateApplicationInfo(ApplicationInformationModel applicationInformationModel)
        {
            applicationInformationModel.OnUpdate();
            BuildInformationGroup build = applicationInformationModel.Build;
            RuntimeInformationGroup runtime = applicationInformationModel.Runtime;
            ScreenInformationGroup screen = applicationInformationModel.Screen;
            GraphicsSettingsInformationGroup graphics = applicationInformationModel.GraphicsSettings;
            LoggingInformationGroup logging = applicationInformationModel.Logging;
            ApplicationOtherInformationGroup other = applicationInformationModel.ApplicationOther;

            _UpdateOrCreateBuild(build);
            _UpdateOrCreateRuntime(runtime);
            _UpdateOrCreateScreen(screen);
            _UpdateOrCreateGraphicsSettings(graphics);
            _UpdateOrCreateLogging(logging);
            _UpdateOrCreateOther(other);
            _UpdateApplicationSelectablesList();
        }

        void _UpdateOrCreateBuild(BuildInformationGroup build)
        {
            var appBuild = new InformationBuildData
            {
                _companyName = build.CompanyName,
                _productName = build.ProductName,
                _identifier = build.Identifier,
                _version = build.Version,
                _unityVersion = build.UnityVersion,
                _scriptingBackend = build.ScriptingBackend,
                _debugBuild = build.IsDebugBuild.ToString()
            };

            if (_selectableBuild == null)
            {
                _selectableBuild = new SelectableBuildData(appBuild);
            }
            else
            {
                _selectableBuild.Update(appBuild);
            }
        }

        void _UpdateOrCreateRuntime(RuntimeInformationGroup runtime)
        {
            var appRuntime = new InformationRuntimeData
            {
                _platform = runtime.Platform.ToString(),
                _currentScene = runtime.CurrentScene.name,
                _scenePlayTime = runtime.ScenePlayTime.ToString(CultureInfo.InvariantCulture),
                _appPlayTime = runtime.AppPlayTime.ToString(CultureInfo.InvariantCulture),
                _timeScale = runtime.TimeScaleParameter,
                _runInBackground = runtime.RunInBackgroundParameter
            };

            if (_selectableRuntime == null)
            {
                _selectableRuntime = new SelectableRuntimeData(appRuntime);
            }
            else
            {
                _selectableRuntime.Update(appRuntime);
            }
        }

        void _UpdateOrCreateScreen(ScreenInformationGroup screen)
        {
            var appScreen = new InformationScreenData
            {
                _width = screen.Width.ToString(),
                _height = screen.Height.ToString(),
                _targetFrameRate = screen.TargetFrameRateParameter,
                _fullScreen = screen.IsFullScreenParameter,
                _fullScreenMode = screen.FullScreenModeParameter,
                _orientation = screen.OrientationParameter
            };

            if (_selectableScreen == null)
            {
                _selectableScreen = new SelectableScreenData(appScreen);
            }
            else
            {
                _selectableScreen.Update(appScreen);
            }
        }

        void _UpdateOrCreateGraphicsSettings(GraphicsSettingsInformationGroup graphics)
        {
            var appGraphics = new InformationGraphicsSettingsData
            {
                _renderPipeline = graphics.RenderPipeline,
                _srpBatching = graphics.UseScriptableRenderPipelineBatchingParameter,
                _activeColorSpace = graphics.ActiveColorSpace.ToString(),
                _desiredColorSpace = graphics.DesiredColorSpace.ToString(),
                _qualityLevel = graphics.QualityLevelParameter,
                _antiAliasing = graphics.AntiAliasingParameter,
                _shadowQuality = graphics.ShadowQualityParameter,
                _shadowResolution = graphics.ShadowResolutionParameter,
                _shadowDistance = graphics.ShadowDistanceParameter,
                _shadowCascades = graphics.ShadowCascadesParameter,
                _lodBias = graphics.LodBiasParameter,
    #if USE_PIPELINE_URP
                _urpRenderScale = graphics.UrpRenderScaleParameter,
    #endif
    #if UNITY_2022_2_OR_NEWER
                _globalTextureMipmapLimit = graphics.GlobalTextureMipmapLimitParameter,
    #endif
                _vSyncCount = graphics.VSyncCountParameter
            };

            if (_selectableGraphicsSettings == null)
            {
                _selectableGraphicsSettings = new SelectableGraphicsSettingsData(appGraphics);
            }
            else
            {
                _selectableGraphicsSettings.Update(appGraphics);
            }
        }

        void _UpdateOrCreateLogging(LoggingInformationGroup logging)
        {
            var appLogging = new InformationLoggingData
            {
                _enabled = logging.EnabledParameter,
                _filterLogType = logging.FilterLogTypeParameter,
                _exceptionStackTraceLogType = logging.ExceptionStackTraceLogTypeParameter,
                _assertStackTraceLogType = logging.AssertStackTraceLogTypeParameter,
                _errorStackTraceLogType = logging.ErrorStackTraceLogTypeParameter,
                _warningStackTraceLogType = logging.WarningStackTraceLogTypeParameter,
                _logStackTraceLogType = logging.LogStackTraceLogTypeParameter,
                _consoleLogPath = logging.ConsoleLogPath
            };

            if (_selectableLogging == null)
            {
                _selectableLogging = new SelectableLoggingData(appLogging);
            }
            else
            {
                _selectableLogging.Update(appLogging);
            }
        }

        void _UpdateOrCreateOther(ApplicationOtherInformationGroup other)
        {
            var appOther = new InformationOtherData
            {
                _persistentDataPath = other.PersistentDataPath,
                _streamingAssetsPath = other.SteamingAssetsPath,
                _temporaryCachePath = other.TemporaryCachePath,
                _dataPath = other.DataPath,
                _installerName = other.InstallerName,
                _installMode = other.InstallMode.ToString(),
                _commandLineArgs = _ToCollectionString(other.CommandLineArgs),
                _absoluteUrl = other.AbsoluteUrl,
                _buildGuid = other.BuildGuid,
                _cloudProjectId = other.CloudProjectId
            };

            if (_selectableOther == null)
            {
                _selectableOther = new SelectableOtherData(appOther);
            }
            else
            {
                _selectableOther.Update(appOther);
            }
        }

        void _UpdateApplicationSelectablesList()
        {
            _selectableGroups.Clear();

            if (_selectableBuild != null)
            {
                _selectableGroups.Add(_selectableBuild);
            }
            if (_selectableRuntime != null)
            {
                _selectableGroups.Add(_selectableRuntime);
            }
            if (_selectableScreen != null)
            {
                _selectableGroups.Add(_selectableScreen);
            }
            if (_selectableGraphicsSettings != null)
            {
                _selectableGroups.Add(_selectableGraphicsSettings);
            }
            if (_selectableLogging != null)
            {
                _selectableGroups.Add(_selectableLogging);
            }
            if (_selectableOther != null)
            {
                _selectableGroups.Add(_selectableOther);
            }
        }

        string _ToCollectionString<T>(IEnumerable<T> collection)
        {
            var sb = new StringBuilder();

            foreach (T element in collection)
            {
                if (sb.Length > 0)
                {
                    sb.Append(", ");
                }

                sb.Append($"\"{element}\"");
            }

            return sb.ToString();
        }
    }
}
