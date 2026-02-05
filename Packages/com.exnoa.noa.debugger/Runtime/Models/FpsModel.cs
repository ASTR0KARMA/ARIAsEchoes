using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NoaDebugger
{
    sealed class FpsModel : ModelBase
    {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        public static bool ShouldStopInBackground => true;
#else
        public static bool ShouldStopInBackground
        {
            get
            {
                if (DeviceModel.IsAndroid || DeviceModel.IsIOS)
                {
                    return true;
                }

                return false;
            }
        }
#endif

        static readonly string FpsModelOnUpdate = "FpsModelOnUpdate";
        static readonly string ProfilerModelOnChangedBackgroundState = "ProfilerModelOnChangedBackgroundState";
        static readonly int IgnoreFrameCountOnStart = 10;
        static readonly int IgnoreFrameCountOnReturnFromBackground = 10;
#if UNITY_EDITOR
        static readonly int IgnoreFrameCountOnRestart = 10;
#endif

        bool _isBackground;
        int _currentFrameCount = 0;
        float _time = 0.0f;
        int _ignoreFrameCount = FpsModel.IgnoreFrameCountOnStart;

        public FpsInfo FpsInfo { get; private set; }

        public UnityAction OnFpsInfoChanged { get; set; }

        public FpsModel()
        {
            _ResetFpsInfo();
            KeyValuePair<string, bool> isProfilingInfo = NoaDebuggerPrefsDefine.IsFpsProfilingKeyValue;
            var isProfiling = NoaDebuggerPrefs.GetBoolean(isProfilingInfo.Key, isProfilingInfo.Value);
            FpsInfo.SetIsProfiling(isProfiling);

            _HandleOnUpdate(isProfiling);

            if (ShouldStopInBackground)
            {
                ApplicationBackgroundManager.SetAction(ProfilerModelOnChangedBackgroundState, _OnChangedBackgroundStateWithFlag);
            }
#if UNITY_WEBGL && !UNITY_EDITOR
            else
            {
                ApplicationBackgroundManager.OnVisibilityChanged += _OnChangedBackgroundState;
            }
#endif

#if UNITY_EDITOR
            UnityEditor.EditorApplication.pauseStateChanged += _OnChangedPauseState;
#endif
        }

        public void Dispose()
        {
            ApplicationBackgroundManager.DeleteAction(ProfilerModelOnChangedBackgroundState);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.pauseStateChanged -= _OnChangedPauseState;
#endif

            UpdateManager.DeleteAction(FpsModelOnUpdate);
        }

        void _OnUpdate()
        {
            if (!FpsInfo.IsProfiling || _isBackground || !ApplicationBackgroundManager.IsTabVisible)
            {
                return;
            }

            _currentFrameCount++;
            float deltaTime = Time.unscaledDeltaTime;
            _time += deltaTime;

            if (_ignoreFrameCount > 0)
            {
                _ignoreFrameCount--;
                if (_ignoreFrameCount == 0)
                {
                    _ResetCounter();
                }
                return;
            }

            if (_time >= 0.5f)
            {
                FpsInfo.StartProfiling();

                int fps = Mathf.FloorToInt(_currentFrameCount / _time);

                int totalFrames = FpsInfo.TotalFrames + _currentFrameCount;
                float totalSeconds = FpsInfo.TotalSeconds + _time;

                FpsInfo.Refresh(fps, deltaTime, totalFrames, totalSeconds);
                OnFpsInfoChanged?.Invoke();

                _ResetCounter();
            }
        }

        void _ResetFpsInfo()
        {
            bool isProfiling = FpsInfo?.IsProfiling ?? NoaDebuggerPrefsDefine.IsFpsProfilingKeyValue.Value;
            FpsInfo = new FpsInfo();
            FpsInfo.SetIsProfiling(isProfiling);

            _ResetCounter();
        }

        void _ResetCounter()
        {
            _currentFrameCount = 0;
            _time = 0;
        }

        void _OnChangedBackgroundStateWithFlag(bool isBackground)
        {
            _isBackground = isBackground;

            _OnChangedBackgroundState(isBackground);
        }

        void _OnChangedBackgroundState(bool isBackground)
        {
            if (!isBackground)
            {
                _ignoreFrameCount = FpsModel.IgnoreFrameCountOnReturnFromBackground;
            }
        }

#if UNITY_EDITOR
        void _OnChangedPauseState(UnityEditor.PauseState state)
        {
            switch (state)
            {
                case UnityEditor.PauseState.Unpaused:
                    _ignoreFrameCount = FpsModel.IgnoreFrameCountOnRestart;
                    break;
            }
        }
#endif

        public void ChangeFpsProfilingState(bool isProfiling)
        {
            FpsInfo.SetIsProfiling(isProfiling);

            NoaDebuggerPrefs.SetBoolean(NoaDebuggerPrefsDefine.IsFpsProfilingKeyValue.Key, isProfiling);
            _HandleOnUpdate(isProfiling);
        }

        void _HandleOnUpdate(bool isProfiling)
        {
            string key = FpsModel.FpsModelOnUpdate;

            if (isProfiling)
            {
                if (UpdateManager.ContainsKey(key))
                {
                    return;
                }

                _ResetFpsInfo();

                UpdateManager.SetAction(key, _OnUpdate);
            }

            else
            {
                UpdateManager.DeleteAction(key);
            }
        }
    }
}
