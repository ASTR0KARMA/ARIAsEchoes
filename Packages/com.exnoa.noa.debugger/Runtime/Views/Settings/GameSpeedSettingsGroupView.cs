using UnityEngine;

namespace NoaDebugger
{
    sealed class GameSpeedSettingsGroupView : MutableSettingsViewBase
    {
        [SerializeField] BoolSettingsPanel _appliesGameSpeedChange;
        [SerializeField] FloatSettingsPanel _gameSpeedIncrement;
        [SerializeField] FloatSettingsPanel _maxGameSpeed;

        protected override SettingsResetButtonType ResetButtonType
        {
            get { return SettingsResetButtonType.GameSpeed; }
        }

        public override void Initialize(SettingsViewLinker linker)
        {
            var settings = linker._gameSpeedSettings;
            _appliesGameSpeedChange.Initialize(settings.AppliesGameSpeedChange);
            _gameSpeedIncrement.Initialize(settings.GameSpeedIncrement);
            _maxGameSpeed.Initialize(settings.MaxGameSpeed);
        }
    }
}
