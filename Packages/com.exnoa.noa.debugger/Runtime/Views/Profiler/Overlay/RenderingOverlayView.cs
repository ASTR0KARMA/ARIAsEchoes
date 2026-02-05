using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class RenderingOverlayView : ProfilerOverlayFeatureViewBase<RenderingUnchangingInfo, RenderingChartDrawerComponent>
    {
        protected override Color TextColor => NoaDebuggerDefine.TextColors.ProfilerRendering;

        [SerializeField]
        TextMeshProUGUI _renderingLabelsText;

        [SerializeField]
        TextMeshProUGUI _renderingValuesText;

        [SerializeField]
        TextMeshProUGUI _graphLabelText;

        protected override void OnInitialize()
        {
            Assert.IsNotNull(_renderingLabelsText);
            Assert.IsNotNull(_renderingValuesText);
            Assert.IsNotNull(_graphLabelText);
        }

        public override void UpdateView(RenderingUnchangingInfo data)
        {
            if (!_rootObject.activeInHierarchy || data == null)
            {
                return;
            }

            _UpdateText(data);
            _UpdateGraph(data);
        }

        protected override bool _IsShowGraph()
        {
            KeyValuePair<string, bool> isGraphShowingInfo = NoaDebuggerPrefsDefine.IsRenderGraphShowingKeyValue;
            bool isGraphShowing = NoaDebuggerPrefs.GetBoolean(isGraphShowingInfo.Key, isGraphShowingInfo.Value);
            return isGraphShowing && _isShowGraphOnOverlay;
        }

        void _UpdateText(RenderingUnchangingInfo data)
        {
            var textInfoList = new List<(string, long, string)>();
            switch (_textType)
            {
                case NoaProfiler.OverlayTextType.Simple:
                    textInfoList.Add(_GetTextInfoFromRenderingGraphTarget(data, data.GraphTarget));
                    break;

                case NoaProfiler.OverlayTextType.Full:
                    textInfoList.Add(_GetTextInfoFromRenderingGraphTarget(data, RenderingGraphTarget.SetPassCalls));
                    textInfoList.Add(_GetTextInfoFromRenderingGraphTarget(data, RenderingGraphTarget.DrawCalls));
                    textInfoList.Add(_GetTextInfoFromRenderingGraphTarget(data, RenderingGraphTarget.Batches));
                    textInfoList.Add(_GetTextInfoFromRenderingGraphTarget(data, RenderingGraphTarget.Triangles));
                    textInfoList.Add(_GetTextInfoFromRenderingGraphTarget(data, RenderingGraphTarget.Vertices));
                    break;
            }

            string[] labels = textInfoList.Select(x => _GetLabelText(x.Item1)).ToArray();
            string[] values = textInfoList.Select(x => _GetValueText(x.Item2, x.Item3, data)).ToArray();

            _renderingLabelsText.text = string.Join("\n", labels);
            _renderingValuesText.text = string.Join("\n", values);
        }

        void _UpdateGraph(RenderingUnchangingInfo data)
        {
            if (!_IsShowGraph())
            {
                return;
            }

            _graph.OnShowRenderingChart(data);

            _graphLabelText.text = data.GraphTarget switch
            {
                RenderingGraphTarget.SetPassCalls => ProfilerDrawerHelper.SetPassCallsLabel,
                RenderingGraphTarget.DrawCalls => ProfilerDrawerHelper.DrawCallsLabel,
                RenderingGraphTarget.Batches => ProfilerDrawerHelper.BatchesLabel,
                RenderingGraphTarget.Triangles => ProfilerDrawerHelper.TrianglesLabel,
                RenderingGraphTarget.Vertices => ProfilerDrawerHelper.VerticesLabel,
                _ => default
            };
        }

        (string, long, string) _GetTextInfoFromRenderingGraphTarget(
            RenderingUnchangingInfo data,
            RenderingGraphTarget target)
        {
            return target switch
            {
                RenderingGraphTarget.SetPassCalls => (ProfilerDrawerHelper.SetPassCallsLabel, data.CurrentSetPassCalls, data.MaxSetPassCallsStr),
                RenderingGraphTarget.DrawCalls => (ProfilerDrawerHelper.DrawCallsLabel, data.CurrentDrawCalls, data.MaxDrawCallsStr),
                RenderingGraphTarget.Batches => (ProfilerDrawerHelper.BatchesLabel, data.CurrentBatches, data.MaxBatchesStr),
                RenderingGraphTarget.Triangles => (ProfilerDrawerHelper.TrianglesLabel, data.CurrentTriangles, data.MaxTrianglesStr),
                RenderingGraphTarget.Vertices => (ProfilerDrawerHelper.VerticesLabel, data.CurrentVertices, data.MaxVerticesStr),
                _ => default
            };
        }

        string _GetValueText(long currentValue, string maxValueStr, RenderingUnchangingInfo data)
        {
            bool isViewHyphen = data.IsViewHyphen;

            string valueText = _GetValueText(currentValue, isViewHyphen);

            if (isViewHyphen)
            {
                return valueText;
            }

            return $"{valueText}({maxValueStr})";
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _renderingLabelsText = default;
            _renderingValuesText = default;
            _graphLabelText = default;
        }
    }
}
