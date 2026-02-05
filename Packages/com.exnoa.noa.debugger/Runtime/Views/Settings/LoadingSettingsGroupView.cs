using UnityEngine;

namespace NoaDebugger
{
    sealed class LoadingSettingsGroupView : SettingsViewBase
    {
        [SerializeField] ReadOnlySettingsPanel _autoInitialize;

        public override void Initialize(SettingsViewLinker linker)
        {
            var isAutoInitializeEnabled = linker._loadingSettings.AutoInitialize ? NoaDebuggerDefine.SETTINGS_ENABLED_VALUE : NoaDebuggerDefine.SETTINGS_DISABLED_VALUE;
            _autoInitialize.Initialize(isAutoInitializeEnabled);
        }
    }
}
