using System.Linq;
using UnityEngine;

namespace NoaDebugger
{
    sealed class UIElementSettingsGroupView : MutableSettingsViewBase
    {
        [SerializeField] BoolSettingsPanel _appliesUIElementSafeArea;
        [SerializeField] MultipleFloatSettingsPanel _uiElementPadding;

        protected override SettingsResetButtonType ResetButtonType
        {
            get { return SettingsResetButtonType.UiElement; }
        }

        public override void Initialize(SettingsViewLinker linker)
        {
            var settings = linker._uiElementSettings;
            _appliesUIElementSafeArea.Initialize(settings.AppliesUIElementSafeArea);

            var padding = settings.UIElementPadding.Cast<SettingParameterBase<float>>().ToList();
            _uiElementPadding.Initialize(padding);
        }
    }
}
