using UnityEngine;

namespace NoaDebugger
{
    sealed class LogSettingsGroupView : MutableSettingsViewBase
    {
        [SerializeField] IntSettingsPanel _consoleLogCount;
        [SerializeField] IntSettingsPanel _apiLogCount;

        protected override SettingsResetButtonType ResetButtonType
        {
            get { return SettingsResetButtonType.Log; }
        }

        public override void Initialize(SettingsViewLinker linker)
        {
            _consoleLogCount.Initialize(linker._logSettings.ConsoleLogCount);
            _apiLogCount.Initialize(linker._logSettings.ApiLogCount);
        }
    }
}
