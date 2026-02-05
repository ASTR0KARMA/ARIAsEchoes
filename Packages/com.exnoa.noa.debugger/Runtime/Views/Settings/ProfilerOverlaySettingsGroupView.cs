using UnityEngine;

namespace NoaDebugger
{
    sealed class ProfilerOverlaySettingsGroupView : MutableSettingsViewBase
    {
        [SerializeField] EnumSettingsPanel _profilerOverlayPosition;
        [SerializeField] FloatSettingsPanel _profilerOverlayScale;
        [SerializeField] EnumSettingsPanel _profilerOverlayAxis;
        [SerializeField] BoolSettingsPanel _profilerOverlayFpsEnable;
        [SerializeField] EnumSettingsPanel _profilerOverlayFpsTextType;
        [SerializeField] BoolSettingsPanel _profilerOverlayFpsShowGraph;
        [SerializeField] BoolSettingsPanel _profilerOverlayMemoryEnable;
        [SerializeField] EnumSettingsPanel _profilerOverlayMemoryTextType;
        [SerializeField] BoolSettingsPanel _profilerOverlayMemoryShowGraph;
        [SerializeField] BoolSettingsPanel _profilerOverlayRenderingEnable;
        [SerializeField] EnumSettingsPanel _profilerOverlayRenderingTextType;
        [SerializeField] BoolSettingsPanel _profilerOverlayRenderingShowGraph;

        protected override SettingsResetButtonType ResetButtonType
        {
            get { return SettingsResetButtonType.ProfilerOverlay; }
        }

        public override void Initialize(SettingsViewLinker linker)
        {
            var settings = linker._profilerOverlaySettings;
            _profilerOverlayPosition.Initialize(settings.Position);
            _profilerOverlayScale.Initialize(settings.Scale);
            _profilerOverlayAxis.Initialize(settings.Axis);
            _profilerOverlayFpsEnable.Initialize(settings.FpsSettings.IsEnable);
            _profilerOverlayFpsTextType.Initialize(settings.FpsSettings.TextType);
            _profilerOverlayFpsShowGraph.Initialize(settings.FpsSettings.IsShowGraph);
            _profilerOverlayMemoryEnable.Initialize(settings.MemorySettings.IsEnable);
            _profilerOverlayMemoryTextType.Initialize(settings.MemorySettings.TextType);
            _profilerOverlayMemoryShowGraph.Initialize(settings.MemorySettings.IsShowGraph);
            _profilerOverlayRenderingEnable.Initialize(settings.RenderingSettings.IsEnable);
            _profilerOverlayRenderingTextType.Initialize(settings.RenderingSettings.TextType);
            _profilerOverlayRenderingShowGraph.Initialize(settings.RenderingSettings.IsShowGraph);
        }
    }
}
