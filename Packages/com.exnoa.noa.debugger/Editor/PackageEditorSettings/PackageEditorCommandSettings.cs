#if NOA_DEBUGGER
using NoaDebugger.DebugCommand;
using UnityEditor;

namespace NoaDebugger
{
    sealed class PackageEditorCommandSettings : PackageEditorSettingsBase
    {
        CommandDisplayFormat _commandFormatLandscape;

        CommandDisplayFormat _commandFormatPortrait;

        public PackageEditorCommandSettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void ResetTmpDataWithSettings()
        {
            _commandFormatLandscape = _settings.CommandFormatLandscape;
            _commandFormatPortrait = _settings.CommandFormatPortrait;
        }

        public override void ApplySettings()
        {
            _settings.CommandFormatLandscape = _commandFormatLandscape;
            _settings.CommandFormatPortrait = _commandFormatPortrait;
        }

        public override void ResetDefault()
        {
            _commandFormatLandscape = NoaDebuggerDefine.DEFAULT_COMMAND_FORMAT_LANDSCAPE;
            _commandFormatPortrait = NoaDebuggerDefine.DEFAULT_COMMAND_FORMAT_PORTRAIT;
        }

        public override void DrawGUI()
        {
            _DisplaySettingsCategoryHeader("Command", ResetDefault);
            _commandFormatLandscape = (CommandDisplayFormat)EditorGUILayout.EnumPopup("Landscape format", _commandFormatLandscape);
            _commandFormatPortrait = (CommandDisplayFormat)EditorGUILayout.EnumPopup("Portrait format", _commandFormatPortrait);
        }
    }
}
#endif
