#if NOA_DEBUGGER
using UnityEditor;
using UnityEngine;

namespace NoaDebugger
{
    sealed class PackageEditorUIElementSettings : PackageEditorSettingsBase
    {
        bool _appliesUIElementSafeArea;

        Vector2 _uiElementPadding;

        public PackageEditorUIElementSettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void ResetTmpDataWithSettings()
        {
            _appliesUIElementSafeArea = _settings.AppliesUIElementSafeArea;
            _uiElementPadding = _settings.UIElementPadding;
        }

        public override void ApplySettings()
        {
            _settings.AppliesUIElementSafeArea = _appliesUIElementSafeArea;
            _settings.UIElementPadding = _uiElementPadding;
        }

        public override void ResetDefault()
        {
            _appliesUIElementSafeArea = NoaDebuggerDefine.DEFAULT_UI_ELEMENT_SAFE_AREA_APPLICABLE;
            _uiElementPadding = new Vector2(NoaDebuggerDefine.DEFAULT_UI_ELEMENT_PADDING_X, NoaDebuggerDefine.DEFAULT_UI_ELEMENT_PADDING_Y);
        }

        public override void DrawGUI()
        {
            _DisplaySettingsCategoryHeader("UI Element", ResetDefault);
            _appliesUIElementSafeArea = EditorGUILayout.Toggle("Apply safe area", _appliesUIElementSafeArea);
            _uiElementPadding = EditorGUILayout.Vector2Field("Padding", _uiElementPadding);
        }
    }
}
#endif
