using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class ProfilerOverlayView : OverlayViewBase<ProfilerOverlayViewLinker>
    {
        [SerializeField]
        FpsOverlayView _fpsView;

        [SerializeField]
        MemoryOverlayView _memoryView;

        [SerializeField]
        RenderingOverlayView _renderingView;

        [SerializeField]
        AxisSwitchableLayoutGroup _layoutGroup;

        [SerializeField]
        ContentSizeFitter _contentSizeFitter;

        RectTransform _rect;

        public event Action OnEnabledAction;

        void _OnValidateUI()
        {
            Assert.IsNotNull(_fpsView);
            Assert.IsNotNull(_memoryView);
            Assert.IsNotNull(_renderingView);
            Assert.IsNotNull(_layoutGroup);
            Assert.IsNotNull(_contentSizeFitter);

            _rect = _contentSizeFitter.transform as RectTransform;
        }

        protected override void _Init()
        {
            _OnValidateUI();

            base._Init();
        }

        void OnEnable()
        {
            if (GlobalCoroutine.IsExecutable)
            {
                GlobalCoroutine.Run(OnEnableDelay());
            }
        }

        IEnumerator OnEnableDelay()
        {
            yield return new WaitForEndOfFrame();
            OnEnabledAction?.Invoke();

            _contentSizeFitter.SetLayoutHorizontal();
            _contentSizeFitter.SetLayoutVertical();

            LayoutRebuilder.ForceRebuildLayoutImmediate(_rect);

        }

        public override void OnUpdateOnce(ProfilerOverlayViewLinker linker)
        {
            base.OnUpdateOnce(linker);

            _ApplyAxis(linker._axis);
            _ApplyScale(linker._scale);
        }

        public override void OnUpdate(ProfilerOverlayViewLinker linker)
        {
            base.OnUpdate(linker);

            _fpsView.ShowView((linker._fpsInfo, linker._frameTimeInfo), _noaDebuggerSettings.ProfilerOverlayFpsSettings);

            _memoryView.ShowView(linker._memoryInfo, _noaDebuggerSettings.ProfilerOverlayMemorySettings);

            _renderingView.ShowView(linker._renderingInfo, _noaDebuggerSettings.ProfilerOverlayRenderingSettings);
        }

        protected override void _SetPosition(NoaDebug.OverlayPosition position)
        {
            base._SetPosition(position);

            TextAnchor? anchor = position switch
            {
                NoaDebug.OverlayPosition.UpperLeft => TextAnchor.UpperLeft,
                NoaDebug.OverlayPosition.UpperCenter => TextAnchor.UpperCenter,
                NoaDebug.OverlayPosition.UpperRight => TextAnchor.UpperRight,
                NoaDebug.OverlayPosition.MiddleLeft => TextAnchor.MiddleLeft,
                NoaDebug.OverlayPosition.MiddleRight => TextAnchor.MiddleRight,
                NoaDebug.OverlayPosition.LowerLeft => TextAnchor.LowerLeft,
                NoaDebug.OverlayPosition.LowerCenter => TextAnchor.LowerCenter,
                NoaDebug.OverlayPosition.LowerRight => TextAnchor.LowerRight,
                _ => null
            };

            if (anchor != null)
            {
                _layoutGroup.childAlignment = anchor.Value;
            }
        }

        void _ApplyAxis(NoaProfiler.OverlayAxis axis)
        {
            AxisSwitchableLayoutGroup.AxisType? axisType = axis switch
            {
                NoaProfiler.OverlayAxis.Horizontal => AxisSwitchableLayoutGroup.AxisType.Horizontal,
                NoaProfiler.OverlayAxis.Vertical => AxisSwitchableLayoutGroup.AxisType.Vertical,
                _ => null
            };

            if (axisType == null || axisType == _layoutGroup.Axis)
            {
                return;
            }

            _layoutGroup.Axis = axisType.Value;

            _layoutGroup.CalculateLayoutInputHorizontal();
            _layoutGroup.CalculateLayoutInputVertical();
            _layoutGroup.SetLayoutHorizontal();
            _layoutGroup.SetLayoutVertical();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rootRect);
        }

        void _ApplyScale(float scale)
        {
            _rootRect.localScale = new Vector3(scale, scale);
        }

        void OnDestroy()
        {
            _fpsView = default;
            _memoryView = default;
            _renderingView = default;
            _layoutGroup = default;
            _contentSizeFitter = default;
            _rect = default;
        }
    }

    sealed class ProfilerOverlayViewLinker : ViewLinkerBase
    {
        public NoaProfiler.OverlayAxis _axis;

        public float _scale;

        public NoaDebug.OverlayPosition _position;

        public FpsUnchangingInfo _fpsInfo;

        public ProfilerFrameTimeViewInformation _frameTimeInfo;

        public MemoryUnchangingInfo _memoryInfo;

        public RenderingUnchangingInfo _renderingInfo;
    }
}
