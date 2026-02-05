using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NoaDebugger
{
    abstract class OverlayViewBase<TViewLinker>
        : ViewBase<OverlayViewLinker<TViewLinker>>
        where TViewLinker : ViewLinkerBase
    {
        [SerializeField]
        Graphic[] _opacityChangeTargets;

        protected RectTransform _rootRect;
        protected NoaDebuggerSettings _noaDebuggerSettings;

        void _OnValidateUI()
        {
            foreach (Graphic target in _opacityChangeTargets)
            {
                Assert.IsNotNull(target);
            }

            Assert.IsNotNull(_rootRect);
        }

        void _ApplyOverlaySettings()
        {
            var settings = NoaDebuggerSettingsManager.GetNoaDebuggerSettings();

            float opacity = Mathf.Clamp(
                settings.OverlayBackgroundOpacity,
                NoaDebuggerDefine.OverlayBackgroundOpacityMin,
                NoaDebuggerDefine.OverlayBackgroundOpacityMax);

            foreach (Graphic target in _opacityChangeTargets)
            {
                Color newColor = target.color;
                newColor.a = opacity;
                target.color = newColor;
            }
        }

        protected override void _Init()
        {
            _rootRect = transform as RectTransform;
            _OnValidateUI();
            _ApplyOverlaySettings();
            _noaDebuggerSettings = NoaDebuggerSettingsManager.GetNoaDebuggerSettings();
        }

        protected override void _OnShow(OverlayViewLinker<TViewLinker> linker)
        {
            OnUpdateOnce(linker.ViewLinker);
        }

        public virtual void OnUpdate(TViewLinker linker) { }

        public virtual void OnUpdateOnce(TViewLinker linker)
        {
            NoaDebug.OverlayPosition position = NoaDebug.OverlayPosition.UpperLeft;
            if (linker is LogOverlayViewLinker logLinker)
            {
                position = logLinker._position;
            }
            else if (linker is ProfilerOverlayViewLinker profilerLinker)
            {
                position = profilerLinker._position;
            }
            _SetPosition(position);

            OnUpdate(linker);
        }

        protected virtual void _SetPosition(NoaDebug.OverlayPosition position)
        {
            switch (position)
            {
                case NoaDebug.OverlayPosition.UpperLeft:
                    _rootRect.anchorMin = new Vector2(0, 1);
                    _rootRect.anchorMax = new Vector2(0, 1);
                    _rootRect.pivot = new Vector2(0, 1);

                    break;

                case NoaDebug.OverlayPosition.UpperCenter:
                    _rootRect.anchorMin = new Vector2(0.5f, 1);
                    _rootRect.anchorMax = new Vector2(0.5f, 1);
                    _rootRect.pivot = new Vector2(0.5f, 1);

                    break;

                case NoaDebug.OverlayPosition.UpperRight:
                    _rootRect.anchorMin = new Vector2(1, 1);
                    _rootRect.anchorMax = new Vector2(1, 1);
                    _rootRect.pivot = new Vector2(1, 1);

                    break;

                case NoaDebug.OverlayPosition.MiddleLeft:
                    _rootRect.anchorMin = new Vector2(0, 0.5f);
                    _rootRect.anchorMax = new Vector2(0, 0.5f);
                    _rootRect.pivot = new Vector2(0, 0.5f);

                    break;

                case NoaDebug.OverlayPosition.MiddleRight:
                    _rootRect.anchorMin = new Vector2(1, 0.5f);
                    _rootRect.anchorMax = new Vector2(1, 0.5f);
                    _rootRect.pivot = new Vector2(1, 0.5f);

                    break;

                case NoaDebug.OverlayPosition.LowerLeft:
                    _rootRect.anchorMin = new Vector2(0, 0);
                    _rootRect.anchorMax = new Vector2(0, 0);
                    _rootRect.pivot = new Vector2(0, 0);

                    break;

                case NoaDebug.OverlayPosition.LowerCenter:
                    _rootRect.anchorMin = new Vector2(0.5f, 0);
                    _rootRect.anchorMax = new Vector2(0.5f, 0);
                    _rootRect.pivot = new Vector2(0.5f, 0);

                    break;

                case NoaDebug.OverlayPosition.LowerRight:
                    _rootRect.anchorMin = new Vector2(1, 0);
                    _rootRect.anchorMax = new Vector2(1, 0);
                    _rootRect.pivot = new Vector2(1, 0);

                    break;
            }

            _rootRect.anchoredPosition = new Vector2(0, 0);
        }

        public void ApplyOverlaySettings()
        {
            _ApplyOverlaySettings();
        }
    }

    sealed class OverlayViewLinker<TViewLinker> : ViewLinkerBase
        where TViewLinker : ViewLinkerBase
    {
        public TViewLinker ViewLinker { get; }

        public OverlayViewLinker(TViewLinker viewLinker)
        {
            ViewLinker = viewLinker;
        }
    }
}
