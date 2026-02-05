#if NOA_DEBUGGER
using UnityEditor;

namespace NoaDebugger
{
    sealed class PackageEditorOtherSettings : PackageEditorSettingsBase
    {
        bool _isAutoCreateEventSystem;

        ErrorNotificationType _errorNotificationType;

        public PackageEditorOtherSettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void ResetTmpDataWithSettings()
        {
            _isAutoCreateEventSystem = _settings.AutoCreateEventSystem;
            _errorNotificationType = _settings.ErrorNotificationType;
        }

        public override void ApplySettings()
        {
            _settings.AutoCreateEventSystem = _isAutoCreateEventSystem;
            _settings.ErrorNotificationType = _errorNotificationType;
        }

        public override void ResetDefault()
        {
            _isAutoCreateEventSystem = NoaDebuggerDefine.DEFAULT_AUTO_CREATE_EVENT_SYSTEM;
            _errorNotificationType = NoaDebuggerDefine.DEFAULT_ERROR_NOTIFICATION_TYPE;
        }

        public override void DrawGUI()
        {
            _DisplaySettingsCategoryHeader("Others", ResetDefault);

            _isAutoCreateEventSystem = EditorGUILayout.Toggle("Auto create EventSystem", _isAutoCreateEventSystem );
            _errorNotificationType = (ErrorNotificationType)EditorGUILayout.EnumPopup("Error notification", _errorNotificationType);
        }
    }
}
#endif
