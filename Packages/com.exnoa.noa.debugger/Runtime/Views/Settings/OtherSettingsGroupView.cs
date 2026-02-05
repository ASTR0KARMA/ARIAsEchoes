using UnityEngine;

namespace NoaDebugger
{
    sealed class OtherSettingsGroupView : MutableSettingsViewBase
    {
        [SerializeField] ReadOnlySettingsPanel _autoCreateEventSystem;
        [SerializeField] EnumSettingsPanel _errorNotificationType;

        protected override SettingsResetButtonType ResetButtonType
        {
            get { return SettingsResetButtonType.Other; }
        }

        public override void Initialize(SettingsViewLinker linker)
        {
            var settings = linker._otherSettings;
            var isAutoCreateEventSystemEnabled = settings.AutoCreateEventSystem ? NoaDebuggerDefine.SETTINGS_ENABLED_VALUE : NoaDebuggerDefine.SETTINGS_DISABLED_VALUE;
            _autoCreateEventSystem.Initialize(isAutoCreateEventSystemEnabled);
            _errorNotificationType.Initialize(settings.ErrorNotificationType);
        }
    }
}
