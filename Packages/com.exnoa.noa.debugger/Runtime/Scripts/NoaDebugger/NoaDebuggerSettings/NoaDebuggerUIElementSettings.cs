using UnityEngine;

namespace NoaDebugger
{
    sealed class NoaDebuggerUIElementSettings : NoaDebuggerSettingsBase
    {
        public NoaDebuggerUIElementSettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void Init()
        {
            _settings.AppliesUIElementSafeArea = NoaDebuggerDefine.DEFAULT_UI_ELEMENT_SAFE_AREA_APPLICABLE;
            _settings.UIElementPadding = new Vector2(NoaDebuggerDefine.DEFAULT_UI_ELEMENT_PADDING_X, NoaDebuggerDefine.DEFAULT_UI_ELEMENT_PADDING_Y);
        }
    }
}
