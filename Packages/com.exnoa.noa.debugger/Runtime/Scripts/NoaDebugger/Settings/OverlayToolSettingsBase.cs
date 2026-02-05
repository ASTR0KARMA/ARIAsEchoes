using System;
using UnityEngine;

namespace NoaDebugger
{
    [Serializable]
    abstract class OverlayToolSettingsBase : ToolSettingsBase
    {
        protected override string PlayerPrefsSuffix { get => NoaDebuggerPrefsDefine.PrefsKeySuffixOverlaySettings; }

        protected abstract NoaDebug.OverlayPosition DefaultPosition { get; }

        [SerializeField]
        protected EnumSettingParameter _position;

        public EnumSettingParameter Position => _position;

        protected override void _InitializeSettings()
        {
            _position = new EnumSettingParameter(DefaultPosition);
        }

        protected override void _SetDefaultValue()
        {
            _position.SetDefaultValue();
        }

        protected override void _ResetSettings()
        {
            _position.ResetSettings();
        }

        public override void SaveCore()
        {
            base.SaveCore();
            _position.OnSaved();
        }

        public override bool IsValueChanged()
        {
            return _position.IsChanged;
        }
    }
}
