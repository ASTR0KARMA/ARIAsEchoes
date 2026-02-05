using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class ProfilerOverlaySettingsView : OverlaySettingsViewBase<ProfilerOverlayRuntimeSettings, OverlaySettingsViewLinker<ProfilerOverlayRuntimeSettings>>
    {
        [SerializeField]
        ProfilerOverlaySettingsGroupPanel _fpsGroup;

        [SerializeField]
        ProfilerOverlaySettingsGroupPanel _memoryGroup;

        [SerializeField]
        ProfilerOverlaySettingsGroupPanel _renderingGroup;

        [SerializeField]
        EnumSettingsPanel _position;

        [SerializeField]
        FloatSettingsPanel _scale;

        [SerializeField]
        EnumSettingsPanel _axis;

        void _OnValidateUI()
        {
            Assert.IsNotNull(_fpsGroup);
            Assert.IsNotNull(_memoryGroup);
            Assert.IsNotNull(_renderingGroup);
            Assert.IsNotNull(_position);
            Assert.IsNotNull(_scale);
            Assert.IsNotNull(_axis);
        }

        protected override void _Init()
        {
            base._Init();
            _OnValidateUI();
        }

        protected override void _OnShow(OverlaySettingsViewLinker<ProfilerOverlayRuntimeSettings> linker)
        {
            base._OnShow(linker);

            var settings = linker.Settings;
            _fpsGroup.Initialize(settings.FpsSettings);
            _memoryGroup.Initialize(settings.MemorySettings);
            _renderingGroup.Initialize(settings.RenderingSettings);
            _position.Initialize(settings.Position);
            _scale.Initialize(settings.Scale);
            _axis.Initialize(settings.Axis);
        }

        protected override void Refresh()
        {
            _fpsGroup.Refresh();
            _memoryGroup.Refresh();
            _renderingGroup.Refresh();
            _position.Refresh();
            _scale.Refresh();
            _axis.Refresh();
        }
    }
}
