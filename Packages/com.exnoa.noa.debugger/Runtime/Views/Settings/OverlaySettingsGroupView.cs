using System.Linq;
using UnityEngine;

namespace NoaDebugger
{
    sealed class OverlaySettingsGroupView : MutableSettingsViewBase
    {
        [SerializeField]
        FloatSettingsPanel _overlayBackgroundOpacity;
        [SerializeField]
        BoolSettingsPanel _appliesOverlaySafeArea;
        [SerializeField]
        MultipleFloatSettingsPanel _overlayPadding;

        protected override SettingsResetButtonType ResetButtonType
        {
            get { return SettingsResetButtonType.Overlay; }
        }

        public override void Initialize(SettingsViewLinker linker)
        {
            var settings = linker._commonOverlaySettings;
            _overlayBackgroundOpacity.Initialize(settings.OverlayBackgroundOpacity);
            _appliesOverlaySafeArea.Initialize(settings.AppliesOverlaySafeArea);

            var padding = settings.Padding.Cast<SettingParameterBase<float>>().ToList();
            _overlayPadding.Initialize(padding);
        }
    }
}
