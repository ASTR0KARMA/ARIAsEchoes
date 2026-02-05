using System;
using UnityEngine;

namespace NoaDebugger
{
    [Serializable]
    sealed class ProfilerOverlayRuntimeSettings : OverlayToolSettingsBase
    {
        protected override NoaDebug.OverlayPosition DefaultPosition => _noaDebuggerSettings.ProfilerOverlayPosition;

        protected override string PlayerPrefsKeyPrefix => "Profiler";

        [SerializeField]
        ProfilerOverlayRuntimeFeatureSettings _fpsSettings;

        [SerializeField]
        ProfilerOverlayRuntimeFeatureSettings _memorySettings;

        [SerializeField]
        ProfilerOverlayRuntimeFeatureSettings _renderingSettings;

        [SerializeField]
        EnumSettingParameter _axis;

        [SerializeField]
        FloatSettingParameter _scale;

        public ProfilerOverlayRuntimeFeatureSettings FpsSettings => _fpsSettings;

        public ProfilerOverlayRuntimeFeatureSettings MemorySettings => _memorySettings;

        public ProfilerOverlayRuntimeFeatureSettings RenderingSettings => _renderingSettings;

        public FloatSettingParameter Scale => _scale;

        public EnumSettingParameter Axis => _axis;

        protected override void _InitializeSettings()
        {
            base._InitializeSettings();

            _fpsSettings = new ProfilerOverlayRuntimeFeatureSettings(_noaDebuggerSettings.ProfilerOverlayFpsSettings);
            _memorySettings = new ProfilerOverlayRuntimeFeatureSettings(_noaDebuggerSettings.ProfilerOverlayMemorySettings);
            _renderingSettings = new ProfilerOverlayRuntimeFeatureSettings(_noaDebuggerSettings.ProfilerOverlayRenderingSettings);

            _scale = new FloatSettingParameter(
                defaultValue: _noaDebuggerSettings.ProfilerOverlayScale,
                inputRangeMin: NoaDebuggerDefine.ProfilerOverlayScaleMin,
                inputRangeMax: NoaDebuggerDefine.ProfilerOverlayScaleMax,
                increment: NoaDebuggerDefine.DEFAULT_FLOAT_SETTINGS_INCREMENT);

            _axis = new EnumSettingParameter(
                defaultValue: _noaDebuggerSettings.ProfilerOverlayAxis);
        }

        protected override void _LoadSettings(string prefsJson)
        {
            var loadInfo = JsonUtility.FromJson<ProfilerOverlayRuntimeSettings>(prefsJson);

            if (loadInfo == null)
            {
                LogModel.LogWarning("Failed to parse JSON for ProfilerOverlayRuntimeSettings. Using default values.");
                _SetDefaultValue();
                return;
            }

            _position.ApplySavedSettings(loadInfo._position);

            _fpsSettings.ApplySavedSettings(loadInfo._fpsSettings);
            _memorySettings.ApplySavedSettings(loadInfo._memorySettings);
            _renderingSettings.ApplySavedSettings(loadInfo._renderingSettings);
            _scale.ApplySavedSettings(loadInfo?._scale);
            _axis.ApplySavedSettings(loadInfo?._axis);
        }

        protected override void _SetDefaultValue()
        {
            base._SetDefaultValue();
            _fpsSettings.SetDefaultValue();
            _memorySettings.SetDefaultValue();
            _renderingSettings.SetDefaultValue();
            _scale.SetDefaultValue();
            _axis.SetDefaultValue();
        }

        protected override void _ResetSettings()
        {
            base._ResetSettings();
            _fpsSettings.ResetSettings();
            _memorySettings.ResetSettings();
            _renderingSettings.ResetSettings();
            _scale.ResetSettings();
            _axis.ResetSettings();
        }

        public override void SaveCore()
        {
            base.SaveCore();
            _fpsSettings.IsEnable.OnSaved();
            _fpsSettings.TextType.OnSaved();
            _fpsSettings.IsShowGraph.OnSaved();
            _memorySettings.IsEnable.OnSaved();
            _memorySettings.TextType.OnSaved();
            _memorySettings.IsShowGraph.OnSaved();
            _renderingSettings.IsEnable.OnSaved();
            _renderingSettings.TextType.OnSaved();
            _renderingSettings.IsShowGraph.OnSaved();
            _scale.OnSaved();
            _axis.OnSaved();
        }

        public override void ApplyCache(NoaDebuggerSettings originalSettings)
        {
            originalSettings.ProfilerOverlayFpsSettings.Enabled = FpsSettings.IsEnable.Value;
            originalSettings.ProfilerOverlayFpsSettings.Graph = FpsSettings.IsShowGraph.Value;
            originalSettings.ProfilerOverlayFpsSettings.TextType = (NoaProfiler.OverlayTextType) FpsSettings.TextType.Value;

            originalSettings.ProfilerOverlayMemorySettings.Enabled = MemorySettings.IsEnable.Value;
            originalSettings.ProfilerOverlayMemorySettings.Graph = MemorySettings.IsShowGraph.Value;
            originalSettings.ProfilerOverlayMemorySettings.TextType = (NoaProfiler.OverlayTextType)MemorySettings.TextType.Value;

            originalSettings.ProfilerOverlayRenderingSettings.Enabled = RenderingSettings.IsEnable.Value;
            originalSettings.ProfilerOverlayRenderingSettings.Graph = RenderingSettings.IsShowGraph.Value;
            originalSettings.ProfilerOverlayRenderingSettings.TextType = (NoaProfiler.OverlayTextType)RenderingSettings.TextType.Value;

            originalSettings.ProfilerOverlayAxis = (NoaProfiler.OverlayAxis) Axis.Value;
            originalSettings.ProfilerOverlayPosition = (NoaDebug.OverlayPosition) Position.Value;
            originalSettings.ProfilerOverlayScale = Scale.Value;
        }

        public ProfilerOverlayRuntimeFeatureSettings GetFeatureSettings(NoaProfiler.FeatureType featureType)
        {
            return featureType switch
            {
                NoaProfiler.FeatureType.Fps => _fpsSettings,
                NoaProfiler.FeatureType.Memory => _memorySettings,
                NoaProfiler.FeatureType.Rendering => _renderingSettings,
                _ => null
            };
        }

        public override bool IsValueChanged()
        {
            bool result = base.IsValueChanged();
            result |= _fpsSettings.IsEnable.IsChanged;
            result |= _fpsSettings.TextType.IsChanged;
            result |= _fpsSettings.IsShowGraph.IsChanged;
            result |= _memorySettings.IsEnable.IsChanged;
            result |= _memorySettings.TextType.IsChanged;
            result |= _memorySettings.IsShowGraph.IsChanged;
            result |= _renderingSettings.IsEnable.IsChanged;
            result |= _renderingSettings.TextType.IsChanged;
            result |= _renderingSettings.IsShowGraph.IsChanged;
            result |= _scale.IsChanged;
            result |= _axis.IsChanged;
            return result;
        }
    }
}
