using System;
using UnityEngine;

namespace NoaDebugger
{
    static class GameSpeedModel
    {
        static float _gameSpeed = DefaultGameSpeed;

        public static float GameSpeed
        {
            get => GameSpeedModel._gameSpeed;
            private set
            {
                GameSpeedModel._gameSpeed = value;
                NoaDebuggerPrefs.SetFloat(NoaDebuggerPrefsDefine.PrefsKeyGameSpeed, value);
            }
        }

        public static float MinGameSpeed => NoaDebuggerDefine.MIN_GAME_SPEED;

        public static float MaxGameSpeed => NoaDebuggerSettingsManager.GetNoaDebuggerSettings().MaxGameSpeed;

        public static float DefaultGameSpeed => NoaDebuggerDefine.DEFAULT_GAME_SPEED;

        public static bool IsGamePlaying => Time.timeScale != 0.0f;

        public static void Initialize()
        {
            float gameSpeed = NoaDebuggerPrefs.GetFloat(NoaDebuggerPrefsDefine.PrefsKeyGameSpeed, DefaultGameSpeed);
            gameSpeed = Mathf.Clamp(gameSpeed, MinGameSpeed, MaxGameSpeed);
            GameSpeed = gameSpeed;
            GameSpeedModel._ApplyGameSpeed(gameSpeed);
        }

        public static void Reset()
        {
            GameSpeedModel.GameSpeed = DefaultGameSpeed;

            if (GameSpeedModel.IsGamePlaying)
            {
                GameSpeedModel._ApplyGameSpeed(GameSpeed);
            }
        }

        public static void Increment()
        {
            NoaDebuggerSettings settings = NoaDebuggerSettingsManager.GetNoaDebuggerSettings();
            float gameSpeed = GameSpeed;
            float gameSpeedIncrement = settings.GameSpeedIncrement;
            float maxGameSpeed = settings.MaxGameSpeed;

            if (gameSpeed <= MinGameSpeed)
            {
                int steps = Mathf.CeilToInt((GameSpeedModel.DefaultGameSpeed - gameSpeed) / gameSpeedIncrement);
                gameSpeed = DefaultGameSpeed - (gameSpeedIncrement * steps);
            }

            gameSpeed = (float)Math.Round(
                gameSpeed + gameSpeedIncrement,
                NoaDebuggerDefine.GAME_SPEED_SETTINGS_SIGNIFICANT_FRACTIONAL_DIGITS);

            GameSpeedModel.GameSpeed = Mathf.Min(gameSpeed, maxGameSpeed);

            if (GameSpeedModel.IsGamePlaying)
            {
                GameSpeedModel._ApplyGameSpeed(GameSpeed);
            }
        }

        public static void Maximize()
        {
            GameSpeedModel.GameSpeed = MaxGameSpeed;

            if (GameSpeedModel.IsGamePlaying)
            {
                GameSpeedModel._ApplyGameSpeed(GameSpeed);
            }
        }

        public static void Decrement()
        {
            NoaDebuggerSettings settings = NoaDebuggerSettingsManager.GetNoaDebuggerSettings();
            float gameSpeed = GameSpeed;
            float gameSpeedIncrement = settings.GameSpeedIncrement;
            float maxGameSpeed = settings.MaxGameSpeed;

            if (gameSpeed >= maxGameSpeed)
            {
                int steps = Mathf.CeilToInt((gameSpeed - GameSpeedModel.DefaultGameSpeed) / gameSpeedIncrement);
                gameSpeed = DefaultGameSpeed + (gameSpeedIncrement * steps);
            }

            gameSpeed = (float)Math.Round(
                gameSpeed - gameSpeedIncrement,
                NoaDebuggerDefine.GAME_SPEED_SETTINGS_SIGNIFICANT_FRACTIONAL_DIGITS);

            GameSpeedModel.GameSpeed = Mathf.Max(gameSpeed, MinGameSpeed);

            if (GameSpeedModel.IsGamePlaying)
            {
                GameSpeedModel._ApplyGameSpeed(GameSpeed);
            }
        }

        public static void Minimize()
        {
            GameSpeedModel.GameSpeed = MinGameSpeed;

            if (GameSpeedModel.IsGamePlaying)
            {
                GameSpeedModel._ApplyGameSpeed(GameSpeed);
            }
        }

        public static void Pause()
        {
            GameSpeedModel._ApplyGameSpeed(0.0f);
        }

        public static void Resume()
        {
            GameSpeedModel._ApplyGameSpeed(GameSpeed);
        }

        static void _ApplyGameSpeed(float gameSpeed)
        {
            if (NoaDebuggerSettingsManager.GetNoaDebuggerSettings().AppliesGameSpeedChange)
            {
                Time.timeScale = gameSpeed;
            }
        }
    }
}
