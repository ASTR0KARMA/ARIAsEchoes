using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class ProfilerOverlaySettingsGroupPanel : MonoBehaviour
    {
        [SerializeField]
        BoolSettingsPanel _enableToggle;

        [SerializeField]
        EnumSettingsPanel _textType;

        [SerializeField]
        BoolSettingsPanel _graphToggle;

        ProfilerOverlayRuntimeFeatureSettings _settingsCache;

        void Awake()
        {
            Assert.IsNotNull(_enableToggle);
            Assert.IsNotNull(_textType);
            Assert.IsNotNull(_graphToggle);
        }

        public void Initialize(ProfilerOverlayRuntimeFeatureSettings settings)
        {
            _settingsCache = settings;
            _enableToggle.Initialize(settings?.IsEnable);
            _textType.Initialize(settings?.TextType);
            _graphToggle.Initialize(settings?.IsShowGraph);
        }

        public void Refresh()
        {
            _enableToggle.Refresh();
            _textType.Refresh();
            _graphToggle.Refresh();
        }
    }

    [Serializable]
    sealed class ProfilerOverlayRuntimeFeatureSettings
    {
        [SerializeField]
        BoolSettingParameter _isEnable;

        [SerializeField]
        EnumSettingParameter _textType;

        [SerializeField]
        BoolSettingParameter _isShowGraph;

        public BoolSettingParameter IsEnable => _isEnable;

        public EnumSettingParameter TextType => _textType;

        public BoolSettingParameter IsShowGraph => _isShowGraph;

        public ProfilerOverlayRuntimeFeatureSettings(ProfilerOverlayFeatureSettings overlaySettings)
        {
            _isEnable = new BoolSettingParameter(overlaySettings.Enabled);
            _textType = new EnumSettingParameter(overlaySettings.TextType);
            _isShowGraph = new BoolSettingParameter(overlaySettings.Graph);
        }

        public void ApplySavedSettings(ProfilerOverlayRuntimeFeatureSettings savedSettings)
        {
            _isEnable.ApplySavedSettings(savedSettings?._isEnable);
            _textType.ApplySavedSettings(savedSettings?._textType);
            _isShowGraph.ApplySavedSettings(savedSettings?._isShowGraph);
        }

        public void SetDefaultValue()
        {
            _isEnable.SetDefaultValue();
            _textType.SetDefaultValue();
            _isShowGraph.SetDefaultValue();
        }

        public void ResetSettings()
        {
            _isEnable.ResetSettings();
            _textType.ResetSettings();
            _isShowGraph.ResetSettings();
        }
    }
}
