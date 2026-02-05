using System;
using System.Collections.Generic;
using UnityEngine;

namespace NoaDebugger
{
    [Serializable]
    sealed class CommonOverlayRuntimeSettings : ToolSettingsBase
    {
        protected override string PlayerPrefsSuffix { get => NoaDebuggerPrefsDefine.PrefsKeySuffixOverlaySettings; }
        protected override string PlayerPrefsKeyPrefix => "Common";

        [SerializeField]
        FloatSettingParameter _overlayBackgroundOpacity;

        [SerializeField]
        BoolSettingParameter _appliesOverlaySafeArea;

        [SerializeField]
        List<FloatSettingParameter> _padding;

        public FloatSettingParameter OverlayBackgroundOpacity => _overlayBackgroundOpacity;

        public BoolSettingParameter AppliesOverlaySafeArea => _appliesOverlaySafeArea;

        public List<FloatSettingParameter> Padding =>  _padding;

        protected override void _InitializeSettings()
        {
            _overlayBackgroundOpacity = new FloatSettingParameter(
                defaultValue: _noaDebuggerSettings.OverlayBackgroundOpacity,
                inputRangeMin: NoaDebuggerDefine.OverlayBackgroundOpacityMin,
                inputRangeMax: NoaDebuggerDefine.OverlayBackgroundOpacityMax,
                increment: NoaDebuggerDefine.DEFAULT_FLOAT_SETTINGS_INCREMENT);

            _appliesOverlaySafeArea = new BoolSettingParameter(
                defaultValue: _noaDebuggerSettings.AppliesOverlaySafeArea);

            _padding = new List<FloatSettingParameter>();
            var paddingX = new FloatSettingParameter(
                defaultValue: _noaDebuggerSettings.OverlayPadding.x,
                inputRangeMin: NoaDebuggerDefine.OVERLAY_PADDING_MIN,
                inputRangeMax: NoaDebuggerDefine.OVERLAY_PADDING_MAX,
                increment: NoaDebuggerDefine.DEFAULT_INT_SETTINGS_INCREMENT);
            var paddingY = new FloatSettingParameter(
                defaultValue: _noaDebuggerSettings.OverlayPadding.y,
                inputRangeMin: NoaDebuggerDefine.OVERLAY_PADDING_MIN,
                inputRangeMax: NoaDebuggerDefine.OVERLAY_PADDING_MAX,
                increment: NoaDebuggerDefine.DEFAULT_INT_SETTINGS_INCREMENT);

            _padding.Add(paddingX);
            _padding.Add(paddingY);
        }

        protected override void _LoadSettings(string prefsJson)
        {
            var loadInfo = JsonUtility.FromJson<CommonOverlayRuntimeSettings>(prefsJson);

            if (loadInfo == null)
            {
                LogModel.LogWarning("Failed to parse JSON for CommonOverlayRuntimeSettings. Using default values.");
                _SetDefaultValue();
                return;
            }

            _overlayBackgroundOpacity.ApplySavedSettings(loadInfo._overlayBackgroundOpacity);
            _appliesOverlaySafeArea.ApplySavedSettings(loadInfo._appliesOverlaySafeArea);
            if (loadInfo._padding != null && _padding != null)
            {
                for (int i = 0; i < _padding.Count && i < loadInfo._padding.Count; i++)
                {
                    _padding[i].ApplySavedSettings(loadInfo._padding[i]);
                }
            }
        }

        protected override void _SetDefaultValue()
        {
            _overlayBackgroundOpacity.SetDefaultValue();
            _appliesOverlaySafeArea.SetDefaultValue();
            if (_padding != null)
            {
                foreach (var padding in _padding)
                {
                    padding.SetDefaultValue();
                }
            }
        }

        protected override void _ResetSettings()
        {
            _overlayBackgroundOpacity.ResetSettings();
            _appliesOverlaySafeArea.ResetSettings();
            if (_padding != null)
            {
                foreach (var padding in _padding)
                {
                    padding.ResetSettings();
                }
            }
        }

        public override void SaveCore()
        {
            base.SaveCore();
            _overlayBackgroundOpacity.OnSaved();
            _appliesOverlaySafeArea.OnSaved();
            if (_padding != null)
            {
                foreach (var padding in _padding)
                {
                    padding.OnSaved();
                }
            }
        }

        public override void ApplyCache(NoaDebuggerSettings originalSettings)
        {
            originalSettings.OverlayBackgroundOpacity = _overlayBackgroundOpacity.Value;
            originalSettings.AppliesOverlaySafeArea = _appliesOverlaySafeArea.Value;
            if (_padding != null)
            {
                var x = _padding[0].Value;
                var y = _padding[1].Value;
                originalSettings.OverlayPadding = new Vector2(x, y);
            }
        }

        public override bool IsValueChanged()
        {
            bool result = false;
            result |= _overlayBackgroundOpacity.IsChanged;
            result |= _appliesOverlaySafeArea.IsChanged;
            if (_padding != null)
            {
                foreach (var padding in _padding)
                {
                    result |= padding.IsChanged;
                }
            }
            return result;
        }
    }
}
