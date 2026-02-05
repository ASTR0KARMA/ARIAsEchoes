using UnityEngine;

namespace NoaDebugger
{
    sealed class CommandSettingsGroupView : MutableSettingsViewBase
    {
        [SerializeField] EnumSettingsPanel _commandFormatLandscape;
        [SerializeField] EnumSettingsPanel _commandFormatPortrait;

        protected override SettingsResetButtonType ResetButtonType
        {
            get { return SettingsResetButtonType.Command; }
        }

        public override void Initialize(SettingsViewLinker linker)
        {
            _commandFormatLandscape.Initialize(linker._commandSettings.CommandFormatLandscape);
            _commandFormatPortrait.Initialize(linker._commandSettings.CommandFormatPortrait);
        }
    }
}
