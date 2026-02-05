using System;
using System.Collections;
using UnityEngine;

namespace NoaDebugger
{
    static class FrameSteppingModel
    {
        public static readonly float PressActionFirstInterval = 0.2f;

        public static readonly float PressActionSecondInterval = 0.2f;

        static readonly float FrameTimeSecondsAt60FPS = 0.02f;

        static bool IsApplyGameSpeedChange => NoaDebuggerSettingsManager.GetNoaDebuggerSettings().AppliesGameSpeedChange;

        static bool _isStepping = false;

        public static IEnumerator FrameStepping(Action onFrameStepping)
        {
            if (!IsApplyGameSpeedChange)
            {
                onFrameStepping?.Invoke();
                yield break;
            }

            if (NoaControllerManager.IsGamePlaying)
            {
                NoaControllerManager.TogglePauseResume();
            }

            var timescale = NoaControllerManager.GameSpeed;
            yield return StepFrame(onFrameStepping, timescale);
        }

        static IEnumerator StepFrame(Action onStepFrame = null, float timeScale = 1.0f)
        {
            if (_isStepping)
            {
                yield break;
            }

            Time.timeScale = timeScale;
            _isStepping = true;
            float waitSeconds = Time.timeScale * FrameSteppingModel.FrameTimeSecondsAt60FPS;
            yield return new WaitForSeconds(waitSeconds);
            onStepFrame?.Invoke();
            Time.timeScale = 0f;
            _isStepping = false;
        }
    }
}
