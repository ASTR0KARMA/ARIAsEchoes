using UnityEngine;

namespace NoaDebugger
{
    sealed class HierarchySettingsGroupView : MutableSettingsViewBase
    {
        [SerializeField] IntSettingsPanel _hierarchyLevels;

        protected override SettingsResetButtonType ResetButtonType
        {
            get { return SettingsResetButtonType.Hierarchy; }
        }

        public override void Initialize(SettingsViewLinker linker)
        {
            _hierarchyLevels.Initialize(linker._hierarchySettings.HierarchyLevels);
        }
    }
}
