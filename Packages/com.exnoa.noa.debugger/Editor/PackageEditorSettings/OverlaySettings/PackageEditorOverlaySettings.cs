#if NOA_DEBUGGER
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NoaDebugger
{
    sealed class PackageEditorOverlaySettings : PackageEditorSettingsBase
    {
        float _overlayBackgroundOpacity;

        bool _appliesOverlaySafeArea;

        Vector2 _overlayPadding;

        readonly List<PackageEditorSettingsBase> _featureSettings;

        public PackageEditorOverlaySettings(NoaDebuggerSettings settings) : base(settings)
        {
            _featureSettings = new List<PackageEditorSettingsBase>()
            {
                new PackageEditorProfilerOverlaySettings(settings),
                new PackageEditorConsoleLogOverlaySettings(settings),
                new PackageEditorApiLogOverlaySettings(settings),
            };
        }

        public override void ResetTmpDataWithSettings()
        {
            _overlayBackgroundOpacity = _settings.OverlayBackgroundOpacity;
            _appliesOverlaySafeArea = _settings.AppliesOverlaySafeArea;
            _overlayPadding = _settings.OverlayPadding;

            _featureSettings.ForEach(settings => settings.ResetTmpDataWithSettings());
        }

        public override void ApplySettings()
        {
            _settings.OverlayBackgroundOpacity = _overlayBackgroundOpacity;
            _settings.AppliesOverlaySafeArea = _appliesOverlaySafeArea;
            _settings.OverlayPadding = _overlayPadding;

            _featureSettings.ForEach(settings => settings.ApplySettings());
        }

        public override void ResetDefault()
        {
            _overlayBackgroundOpacity = NoaDebuggerDefine.DEFAULT_OVERLAY_BACKGROUND_OPACITY;
            _appliesOverlaySafeArea = NoaDebuggerDefine.DEFAULT_OVERLAY_SAFE_AREA_APPLICABLE;
            _overlayPadding = new Vector2(NoaDebuggerDefine.DEFAULT_OVERLAY_PADDING_X, NoaDebuggerDefine.DEFAULT_OVERLAY_PADDING_Y);

            _featureSettings.ForEach(settings => settings.ResetDefault());
        }

        public override void DrawGUI()
        {
            _DisplaySettingsCategoryHeader("Overlay", ResetDefault);

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                _overlayBackgroundOpacity = EditorGUILayout.Slider(
                    "Background opacity",
                    _overlayBackgroundOpacity,
                    NoaDebuggerDefine.OverlayBackgroundOpacityMin,
                    NoaDebuggerDefine.OverlayBackgroundOpacityMax);
                _appliesOverlaySafeArea = EditorGUILayout.Toggle("Apply safe area", _appliesOverlaySafeArea);
                _overlayPadding = EditorGUILayout.Vector2Field("Padding", _overlayPadding);

                EditorGUILayout.Separator();

                foreach (var settings in _featureSettings)
                {
                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        settings.DrawGUI();
                    }
                }
            }
        }
    }
}
#endif
