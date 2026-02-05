using System.Collections.Generic;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NoaDebugger
{
    sealed class FrameTimeModel : ModelBase
    {
        static readonly string OnChangedBackgroundState = "FrameTimeModelOnBackgroundStateChanged";

        public FrameTimeInfo FrameTimeInfo { get; }

        public UnityAction OnFrameTimeInfoChanged { get; set; }

        bool _isSuspended;

        bool _isResumed;

        public FrameTimeModel()
        {
            KeyValuePair<string, bool> isEnabledInfo = NoaDebuggerPrefsDefine.IsFrameTimeProfilingKeyValue;
            KeyValuePair<string, bool> isActiveInfo = NoaDebuggerPrefsDefine.IsFpsProfilingKeyValue;
            var isEnabled = NoaDebuggerPrefs.GetBoolean(isEnabledInfo.Key, isEnabledInfo.Value);
            var isActive = NoaDebuggerPrefs.GetBoolean(isActiveInfo.Key, isActiveInfo.Value);
            FrameTimeInfo = new FrameTimeInfo(NoaDebuggerDefine.ProfilerChartHistoryCount);
            OnToggleEnabled(isEnabled);
            OnToggleActive(isActive);

            if (FpsModel.ShouldStopInBackground)
            {
                ApplicationBackgroundManager.SetAction(OnChangedBackgroundState, OnBackgroundStateChanged);
            }
#if UNITY_WEBGL && !UNITY_EDITOR
            else
            {
                ApplicationBackgroundManager.OnVisibilityChanged += OnBackgroundStateChanged;
            }
#endif

#if UNITY_EDITOR
            EditorApplication.pauseStateChanged += OnEditorPauseStateChanged;
#endif
        }

        public void Dispose()
        {
            FrameTimeInfo.ClearHistory();
            FrameTimeMeasurer.GetInstance().OnUpdateFrameTime -= OnUpdateFrameTime;

            ApplicationBackgroundManager.DeleteAction(FrameTimeModel.OnChangedBackgroundState);
#if UNITY_EDITOR
            EditorApplication.pauseStateChanged -= OnEditorPauseStateChanged;
#endif
        }

        public void ToggleEnabled(bool isEnabled)
        {
            NoaDebuggerPrefs.SetBoolean(NoaDebuggerPrefsDefine.IsFrameTimeProfilingKeyValue.Key, isEnabled);
            OnToggleEnabled(isEnabled);
        }

        public void ToggleActive(bool isActive)
        {
            OnToggleActive(isActive);
        }

        void OnToggleEnabled(bool isEnabled)
        {
            if (FrameTimeInfo.IsEnabled == isEnabled)
            {
                return;
            }

            FrameTimeInfo.IsEnabled = isEnabled;

            if (isEnabled && FrameTimeInfo.IsActive)
            {
                FrameTimeMeasurer.GetInstance().OnUpdateFrameTime += OnUpdateFrameTime;
            }
            else
            {
                FrameTimeInfo.ClearHistory();
                FrameTimeMeasurer.GetInstance().OnUpdateFrameTime -= OnUpdateFrameTime;
            }
        }

        void OnToggleActive(bool isActive)
        {
            if (FrameTimeInfo.IsActive == isActive)
            {
                return;
            }

            FrameTimeInfo.IsActive = isActive;

            if (isActive && FrameTimeInfo.IsEnabled)
            {
                FrameTimeMeasurer.GetInstance().OnUpdateFrameTime += OnUpdateFrameTime;
            }
            else
            {
                FrameTimeMeasurer.GetInstance().OnUpdateFrameTime -= OnUpdateFrameTime;
            }
        }

        void OnUpdateFrameTime(double updateMilliseconds, double renderingMilliseconds, double othersMilliseconds)
        {
            if (_isSuspended || !ApplicationBackgroundManager.IsTabVisible)
            {
                return;
            }

            if (_isResumed)
            {
                _isResumed = false;
                return;
            }

            FrameTimeInfo.AddHistory(
                (float)updateMilliseconds,
                (float)renderingMilliseconds,
                (float)othersMilliseconds);

            OnFrameTimeInfoChanged?.Invoke();
        }

        void OnBackgroundStateChanged(bool isBackground)
        {
            _isSuspended = isBackground;
            _isResumed = !_isSuspended;
        }

#if UNITY_EDITOR
        void OnEditorPauseStateChanged(PauseState state)
        {
            _isSuspended = state == PauseState.Paused;
            _isResumed = state == PauseState.Unpaused;
        }
#endif
    }
}
