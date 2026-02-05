#if NOA_DEBUGGER
using System;
using UnityEditor;

namespace NoaDebugger
{
    sealed class PackageEditorGameSpeedSettings : PackageEditorSettingsBase
    {
        bool _appliesGameSpeedChange;

        float _gameSpeedIncrement;

        float _maxGameSpeed;

        public PackageEditorGameSpeedSettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void ResetTmpDataWithSettings()
        {
            _appliesGameSpeedChange = _settings.AppliesGameSpeedChange;
            _gameSpeedIncrement = _settings.GameSpeedIncrement;
            _maxGameSpeed = _settings.MaxGameSpeed;
        }

        public override void ApplySettings()
        {
            _settings.AppliesGameSpeedChange = _appliesGameSpeedChange;
            _settings.GameSpeedIncrement = _gameSpeedIncrement;
            _settings.MaxGameSpeed = _maxGameSpeed;
        }

        public override void ResetDefault()
        {
            _appliesGameSpeedChange = NoaDebuggerDefine.DEFAULT_GAME_SPEED_CHANGE_APPLICABLE;
            _gameSpeedIncrement = NoaDebuggerDefine.DEFAULT_GAME_SPEED_INCREMENT;
            _maxGameSpeed = NoaDebuggerDefine.DEFAULT_MAX_GAME_SPEED;
        }

        public override void DrawGUI()
        {
            _DisplaySettingsCategoryHeader("Game Speed", ResetDefault);

            _appliesGameSpeedChange = EditorGUILayout.Toggle("Apply game speed change", _appliesGameSpeedChange);

            _gameSpeedIncrement = EditorGUILayout.Slider(
                "Game speed increment",
                _gameSpeedIncrement,
                NoaDebuggerDefine.MIN_GAME_SPEED_INCREMENT,
                NoaDebuggerDefine.MAX_GAME_SPEED_INCREMENT);

            _gameSpeedIncrement = (float)Math.Round(_gameSpeedIncrement, NoaDebuggerDefine.GAME_SPEED_SETTINGS_SIGNIFICANT_FRACTIONAL_DIGITS);

            _maxGameSpeed = EditorGUILayout.Slider(
                "Max game speed",
                _maxGameSpeed,
                NoaDebuggerDefine.MIN_CONFIGURABLE_MAX_GAME_SPEED,
                NoaDebuggerDefine.MAX_CONFIGURABLE_MAX_GAME_SPEED);

            _maxGameSpeed = (float)Math.Round(_maxGameSpeed, NoaDebuggerDefine.GAME_SPEED_SETTINGS_SIGNIFICANT_FRACTIONAL_DIGITS);
        }
    }
}
#endif
