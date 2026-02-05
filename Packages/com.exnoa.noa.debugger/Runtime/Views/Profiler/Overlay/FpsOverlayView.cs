using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class FpsOverlayView : ProfilerOverlayFeatureViewBase<(FpsUnchangingInfo, ProfilerFrameTimeViewInformation), FrameTimeChartDrawerComponent>
    {
        protected override Color TextColor => NoaDebuggerDefine.TextColors.ProfilerFps;

        [Header("Simple")]
        [SerializeField]
        GameObject _simpleTextArea;
        [SerializeField]
        TextMeshProUGUI _simpleCurrentFpsText;

        [Header("Full")]
        [SerializeField]
        GameObject _fullTextArea;
        [SerializeField]
        LayoutElement _fullTextAreaLayout;

        [Serializable]
        class FullTextArea
        {
            [SerializeField]
            TextMeshProUGUI _text;
            [SerializeField]
            LayoutElement _layout;

            public void SetText(string valueText)
            {
                _text.text = valueText;

                if (_layout.minWidth < _text.rectTransform.rect.width)
                {
                    _layout.minWidth = _text.rectTransform.rect.width;
                }
            }
        }

        [SerializeField]
        FullTextArea _fullAverageFpsText;
        [SerializeField]
        FullTextArea _fullMaxFpsText;
        [SerializeField]
        FullTextArea _fullMinFpsText;
        [SerializeField]
        FullTextArea _fullCurrentFpsText;

        protected override void OnInitialize()
        {
            Assert.IsNotNull(_simpleTextArea);
            Assert.IsNotNull(_simpleCurrentFpsText);
            Assert.IsNotNull(_fullTextArea);
            Assert.IsNotNull(_fullTextAreaLayout);
            Assert.IsNotNull(_fullAverageFpsText);
            Assert.IsNotNull(_fullMaxFpsText);
            Assert.IsNotNull(_fullMinFpsText);
            Assert.IsNotNull(_fullCurrentFpsText);
        }

        public override void UpdateView((FpsUnchangingInfo, ProfilerFrameTimeViewInformation) data)
        {
            if (!_rootObject.activeInHierarchy)
            {
                return;
            }

            _UpdateText(fpsInfo: data.Item1);
            _UpdateGraph(data);
        }

        protected override bool _IsShowGraph()
        {
            KeyValuePair<string, bool> isGraphShowingInfo = NoaDebuggerPrefsDefine.IsFrameTimeProfilingKeyValue;
            bool isGraphShowing = NoaDebuggerPrefs.GetBoolean(isGraphShowingInfo.Key, isGraphShowingInfo.Value);
            return isGraphShowing && _isShowGraphOnOverlay;
        }

        protected override void SetWidth(float width, bool isForce = false)
        {

            if (!isForce &&
                (width <= _textAreaLayout.minWidth || width <= _fullTextAreaLayout.minWidth))
            {
                return;
            }

            _textAreaLayout.minWidth = width;
            _fullTextAreaLayout.minWidth = width;

            _graphLayout.minWidth = width;
        }

        void _UpdateText(FpsUnchangingInfo fpsInfo)
        {
            if (fpsInfo == null)
            {
                return;
            }

            bool isViewHyphen = fpsInfo.IsViewHyphen;

            switch (_textType)
            {
                case NoaProfiler.OverlayTextType.Simple:
                    _simpleCurrentFpsText.text = isViewHyphen
                        ? NoaDebuggerDefine.HyphenValue
                        : $"{_GetValueText(fpsInfo.CurrentFps)} {ProfilerDrawerHelper.FpsUnits} ({ProfilerDrawerHelper.GetElapsedTimeText(fpsInfo.ElapsedTime)})";

                    _simpleTextArea.SetActive(true);
                    _fullTextArea.SetActive(false);

                    break;

                case NoaProfiler.OverlayTextType.Full:
                    _fullAverageFpsText.SetText($"{ProfilerDrawerHelper.AverageLabel}\n{_GetValueText(fpsInfo.AverageFps, isViewHyphen)}");
                    _fullMaxFpsText.SetText($"{ProfilerDrawerHelper.MaxLabel}\n{_GetValueText(fpsInfo.MaxStr, isViewHyphen)}");
                    _fullMinFpsText.SetText($"{ProfilerDrawerHelper.MinLabel}\n{_GetValueText(fpsInfo.MinStr, isViewHyphen)}");
                    _fullCurrentFpsText.SetText(isViewHyphen
                        ? $"{NoaDebuggerDefine.HyphenValue}\n{NoaDebuggerDefine.HyphenValue}"
                        : $"{_GetValueText(fpsInfo.CurrentFps)} {ProfilerDrawerHelper.FpsUnits}\n{ProfilerDrawerHelper.GetElapsedTimeText(fpsInfo.ElapsedTime)}");

                    _simpleTextArea.SetActive(false);
                    _fullTextArea.SetActive(true);

                    break;
            }
        }

        void _UpdateGraph((FpsUnchangingInfo, ProfilerFrameTimeViewInformation) data)
        {
            ProfilerFrameTimeViewInformation frameTimeInfo = data.Item2;

            if (frameTimeInfo != null && _IsShowGraph())
            {
                _graph.OnShowFrameTime(frameTimeInfo);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _simpleTextArea = default;
            _simpleCurrentFpsText = default;
            _fullTextArea = default;
            _fullTextAreaLayout = default;
            _fullAverageFpsText = default;
            _fullMaxFpsText = default;
            _fullMinFpsText = default;
            _fullCurrentFpsText = default;
        }
    }
}
