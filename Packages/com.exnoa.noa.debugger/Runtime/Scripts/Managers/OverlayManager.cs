using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace NoaDebugger
{
    sealed class OverlayManager : UIBehaviour
    {
        [SerializeField]
        RectTransform _overlayParentTransform;

        [SerializeField]
        RectTransform _applySafeAreaRect;

        [SerializeField]
        RectTransform _unApplySafeAreaRect;

        Transform _overlaySettingsParentTransform;
        Func<INoaDebuggerTool> _getCurrentTool;

        public Transform OverlayRoot => _overlayParentTransform;

        protected override void Awake()
        {
            Assert.IsNotNull(_overlayParentTransform);
            Assert.IsNotNull(_applySafeAreaRect);
            Assert.IsNotNull(_unApplySafeAreaRect);
        }

        public void Initialize(Transform overlaySettingsParent, Func<INoaDebuggerTool> getCurrentTool)
        {
            _overlaySettingsParentTransform = overlaySettingsParent;
            _getCurrentTool = getCurrentTool;

            ResetRootRectSize();
        }

        public void ResetRootRectSize()
        {
            bool appliesSafeArea = NoaDebuggerSettingsManager.GetNoaDebuggerSettings().AppliesOverlaySafeArea;
            RectTransform applyCanvas = appliesSafeArea ? _applySafeAreaRect : _unApplySafeAreaRect;
            Rect rect = applyCanvas.rect;
            Vector2 size = new Vector2(rect.width, rect.height);

            Vector2 padding = NoaDebuggerSettingsManager.GetNoaDebuggerSettings().OverlayPadding;
            size.x -= padding.x * 2;
            size.y -= padding.y * 2;

            _overlayParentTransform.anchorMin = new Vector2(0.5f, 0.5f);
            _overlayParentTransform.anchorMax = new Vector2(0.5f, 0.5f);
            _overlayParentTransform.sizeDelta = size;
            _overlayParentTransform.position = applyCanvas.position;
        }

        protected override void OnRectTransformDimensionsChange()
        {
            ResetRootRectSize();
        }

        public void PinOverlayTool()
        {
            INoaDebuggerTool currentTool = _getCurrentTool();
            if (currentTool is INoaDebuggerOverlayTool overlayTool)
            {
                overlayTool.TogglePin(_overlayParentTransform);
            }
        }

        public void ToggleOverlaySettings()
        {
            INoaDebuggerTool currentTool = _getCurrentTool();

            if (currentTool is INoaDebuggerOverlayTool overlayTool)
            {
                overlayTool.ToggleOverlaySettings(_overlaySettingsParentTransform);
            }
        }

        public INoaDebuggerOverlayTool GetOverlayToolFromOverlayFeatures(NoaDebug.OverlayFeatures feature)
        {
            return feature switch
            {
                NoaDebug.OverlayFeatures.Profiler => NoaDebugger.GetPresenter<ProfilerPresenter>(),
                NoaDebug.OverlayFeatures.ConsoleLog => NoaDebugger.GetPresenter<ConsoleLogPresenter>(),
                NoaDebug.OverlayFeatures.ApiLog => NoaDebugger.GetPresenter<ApiLogPresenter>(),
                _ => null
            };
        }

        public void SetOverlayEnabled(INoaDebuggerOverlayTool overlayTool, bool isEnabled, Action onChanged)
        {
            if (overlayTool.GetPinStatus() != isEnabled)
            {
                overlayTool.TogglePin(_overlayParentTransform);
                onChanged?.Invoke();
            }
        }
    }
}
