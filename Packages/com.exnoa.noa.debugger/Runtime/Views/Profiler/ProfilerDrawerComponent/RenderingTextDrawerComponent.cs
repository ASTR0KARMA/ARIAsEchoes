using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class RenderingTextDrawerComponent : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI _setPassCalls;

        [SerializeField]
        TextMeshProUGUI _drawCalls;

        [SerializeField]
        TextMeshProUGUI _batches;

        [SerializeField]
        TextMeshProUGUI _triangles;

        [SerializeField]
        TextMeshProUGUI _vertices;

        void Awake()
        {
            Assert.IsNotNull(_setPassCalls);
            Assert.IsNotNull(_drawCalls);
            Assert.IsNotNull(_batches);
            Assert.IsNotNull(_triangles);
            Assert.IsNotNull(_vertices);
        }

        public void OnShowRenderingText(RenderingUnchangingInfo info, Color? enableTextColor = null)
        {
            Color enableColor = enableTextColor ?? NoaDebuggerDefine.TextColors.Dynamic;

            if (info.IsViewHyphen)
            {
                ProfilerDrawerHelper.ShowHyphenValue(_setPassCalls);
                ProfilerDrawerHelper.ShowHyphenValue(_drawCalls);
                ProfilerDrawerHelper.ShowHyphenValue(_batches);
                ProfilerDrawerHelper.ShowHyphenValue(_triangles);
                ProfilerDrawerHelper.ShowHyphenValue(_vertices);
            }
            else
            {
                _setPassCalls.text = ProfilerDrawerHelper.GetRenderingValueText(info.CurrentSetPassCalls, info.MaxSetPassCallsStr);
                _drawCalls.text = ProfilerDrawerHelper.GetRenderingValueText(info.CurrentDrawCalls, info.MaxDrawCallsStr);
                _batches.text = ProfilerDrawerHelper.GetRenderingValueText(info.CurrentBatches, info.MaxBatchesStr);
                _triangles.text = ProfilerDrawerHelper.GetRenderingValueText(info.CurrentTriangles, info.MaxTrianglesStr);
                _vertices.text = ProfilerDrawerHelper.GetRenderingValueText(info.CurrentVertices, info.MaxVerticesStr);

                _setPassCalls.color = enableColor;
                _drawCalls.color = enableColor;
                _batches.color = enableColor;
                _triangles.color = enableColor;
                _vertices.color = enableColor;
            }
        }

        void OnDestroy()
        {
            _setPassCalls = default;
            _drawCalls = default;
            _batches = default;
            _triangles = default;
            _vertices = default;
        }
    }
}
