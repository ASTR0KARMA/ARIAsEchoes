using UnityEngine;
using System.Collections.Generic;

namespace NoaDebugger
{
    sealed class UIElementRuntimeSettings : FeatureToolSettingsBase
    {
        protected override string PlayerPrefsKeyPrefix => "UIElement";

        [SerializeField]
        BoolSettingParameter _appliesUIElementSafeArea;

        [SerializeField]
        List<FloatSettingParameter> _uiElementPadding;

        public BoolSettingParameter AppliesUIElementSafeArea => _appliesUIElementSafeArea;

        public List<FloatSettingParameter> UIElementPadding =>  _uiElementPadding;

        protected override void _InitializeSettings()
        {
            _appliesUIElementSafeArea = new BoolSettingParameter(
                defaultValue: _noaDebuggerSettings.AppliesUIElementSafeArea);

            _uiElementPadding = new List<FloatSettingParameter>();
            var paddingX = new FloatSettingParameter(
                defaultValue: _noaDebuggerSettings.UIElementPadding.x,
                inputRangeMin: NoaDebuggerDefine.UI_ELEMENT_PADDING_MIN,
                inputRangeMax: NoaDebuggerDefine.UI_ELEMENT_PADDING_MAX,
                increment: NoaDebuggerDefine.DEFAULT_INT_SETTINGS_INCREMENT);
            var paddingY = new FloatSettingParameter(
                defaultValue: _noaDebuggerSettings.UIElementPadding.y,
                inputRangeMin: NoaDebuggerDefine.UI_ELEMENT_PADDING_MIN,
                inputRangeMax: NoaDebuggerDefine.UI_ELEMENT_PADDING_MAX,
                increment: NoaDebuggerDefine.DEFAULT_INT_SETTINGS_INCREMENT);

            _uiElementPadding.Add(paddingX);
            _uiElementPadding.Add(paddingY);
        }

        protected override void _LoadSettings(string prefsJson)
        {
            var loadInfo = JsonUtility.FromJson<UIElementRuntimeSettings>(prefsJson);

            if (loadInfo == null)
            {
                LogModel.LogWarning("Failed to parse JSON for UIElementRuntimeSettings. Using default values.");
                _SetDefaultValue();
                return;
            }

            _appliesUIElementSafeArea.ApplySavedSettings(loadInfo._appliesUIElementSafeArea);

            if (loadInfo._uiElementPadding != null && _uiElementPadding != null)
            {
                for (int i = 0; i < _uiElementPadding.Count && i < loadInfo._uiElementPadding.Count; i++)
                {
                    _uiElementPadding[i].ApplySavedSettings(loadInfo._uiElementPadding[i]);
                }
            }
        }

        protected override void _SetDefaultValue()
        {
            _appliesUIElementSafeArea.SetDefaultValue();
            if (_uiElementPadding != null)
            {
                foreach (var padding in _uiElementPadding)
                {
                    padding.SetDefaultValue();
                }
            }
        }

        protected override void _ResetSettings()
        {
            _appliesUIElementSafeArea.ResetSettings();
            if (_uiElementPadding != null)
            {
                foreach (var padding in _uiElementPadding)
                {
                    padding.ResetSettings();
                }
            }
        }

        public override void SaveCore()
        {
            base.SaveCore();
            _appliesUIElementSafeArea.OnSaved();
            if (_uiElementPadding != null)
            {
                foreach (var padding in _uiElementPadding)
                {
                    padding.OnSaved();
                }
            }
        }

        public override void ApplyCache(NoaDebuggerSettings originalSettings)
        {
            originalSettings.AppliesUIElementSafeArea = AppliesUIElementSafeArea.Value;
            if (UIElementPadding != null)
            {
                var x = UIElementPadding[0].Value;
                var y = UIElementPadding[1].Value;
                originalSettings.UIElementPadding = new Vector2(x, y);
            }
        }

        public override bool IsValueChanged()
        {
            bool result = false;
            result |= _appliesUIElementSafeArea.IsChanged;
            if (_uiElementPadding != null)
            {
                foreach (var padding in _uiElementPadding)
                {
                    result |= padding.IsChanged;
                }
            }
            return result;
        }
    }
}
