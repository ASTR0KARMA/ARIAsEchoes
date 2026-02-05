using UnityEngine;

namespace NoaDebugger
{
    sealed class InformationSettingsGroupView : MutableSettingsViewBase
    {
        [SerializeField] BoolSettingsPanel _informationIsSaving;

        protected override SettingsResetButtonType ResetButtonType
        {
            get { return SettingsResetButtonType.Information; }
        }

        public override void Initialize(SettingsViewLinker linker)
        {
            _informationIsSaving.Initialize(linker._informationSettings.SaveChanges);
        }
    }
}
