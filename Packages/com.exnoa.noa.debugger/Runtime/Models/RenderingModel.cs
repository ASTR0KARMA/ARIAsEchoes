using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NoaDebugger
{
    sealed class RenderingModel : ModelBase
    {
        static readonly string RenderingModelOnUpdate = "RenderingModelOnUpdate";

        ProfilerRecorder _setPassCallsRecorder;
        ProfilerRecorder _drawCallsRecorder;
        ProfilerRecorder _batchesRecorder;
        ProfilerRecorder _trianglesRecorder;
        ProfilerRecorder _verticesRecorder;

        public RenderingInfo RenderingInfo { get; private set; }

        public UnityAction OnRenderingInfoChanged { get; set; }

        public RenderingModel()
        {
            _setPassCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "SetPass Calls Count");
            _drawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
            _batchesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Batches Count");
            _trianglesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Triangles Count");
            _verticesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");

            RenderingInfo = new RenderingInfo();
            KeyValuePair<string, bool> isProfilingInfo = NoaDebuggerPrefsDefine.IsRenderProfilingKeyValue;
            KeyValuePair<string, bool> isGraphShowingInfo = NoaDebuggerPrefsDefine.IsRenderGraphShowingKeyValue;
            KeyValuePair<string, RenderingGraphTarget> renderGraphTargetInfo = NoaDebuggerPrefsDefine.RenderGraphTargetKeyValue;
            bool isProfiling = NoaDebuggerPrefs.GetBoolean(isProfilingInfo.Key, isProfilingInfo.Value);
            bool isGraphShowing = NoaDebuggerPrefs.GetBoolean(isGraphShowingInfo.Key, isGraphShowingInfo.Value);
            RenderingGraphTarget renderGraphTarget = (RenderingGraphTarget)NoaDebuggerPrefs.GetInt(renderGraphTargetInfo.Key, (int)renderGraphTargetInfo.Value);
            RenderingInfo.ToggleProfiling(isProfiling);
            RenderingInfo.ToggleGraphShowing(isGraphShowing);
            RenderingInfo.SwitchGraphTarget(renderGraphTarget);
            _HandleOnUpdate(isProfiling);
        }

        public void Dispose()
        {
            _setPassCallsRecorder.Dispose();
            _drawCallsRecorder.Dispose();
            _batchesRecorder.Dispose();
            _trianglesRecorder.Dispose();
            _verticesRecorder.Dispose();

            UpdateManager.DeleteAction(RenderingModelOnUpdate);
        }

        void _OnUpdate()
        {
            _OnUpdateRenderingCheck();
        }

        void _OnUpdateRenderingCheck()
        {
            if (!RenderingInfo.IsProfiling)
            {
                return;
            }

            RenderingInfo.StartProfiling();

#if UNITY_EDITOR
            long currentSetPassCalls = UnityStats.setPassCalls;
            long currentDrawCalls = UnityStats.drawCalls;
            long currentBatches = UnityStats.instancedBatches;
            long currentTriangles = UnityStats.triangles;
            long currentVertices = UnityStats.vertices;
#else
            long currentSetPassCalls = _setPassCallsRecorder.LastValue;
            long currentDrawCalls = _drawCallsRecorder.LastValue;
            long currentBatches = _batchesRecorder.LastValue;
            long currentTriangles = _trianglesRecorder.LastValue;
            long currentVertices = _verticesRecorder.LastValue;
#endif
            RenderingInfo.RefreshCurrent(currentSetPassCalls, currentDrawCalls, currentBatches, currentTriangles, currentVertices);
            OnRenderingInfoChanged?.Invoke();
        }

        public void ChangeProfilingState(bool isProfiling)
        {
            RenderingInfo.ToggleProfiling(isProfiling);

            NoaDebuggerPrefs.SetBoolean(NoaDebuggerPrefsDefine.IsRenderProfilingKeyValue.Key, isProfiling);
            _HandleOnUpdate(isProfiling);
        }

        public void ChangeGraphShowingState(bool isGraphShowing)
        {
            RenderingInfo.ToggleGraphShowing(isGraphShowing);

            NoaDebuggerPrefs.SetBoolean(NoaDebuggerPrefsDefine.IsRenderGraphShowingKeyValue.Key, isGraphShowing);
        }

        public void SwitchGraphTarget(RenderingGraphTarget target)
        {
            RenderingInfo.SwitchGraphTarget(target);

            NoaDebuggerPrefs.SetInt(NoaDebuggerPrefsDefine.RenderGraphTargetKeyValue.Key, (int)target);
        }

        void _HandleOnUpdate(bool isProfiling)
        {
            string key = RenderingModel.RenderingModelOnUpdate;

            if (isProfiling)
            {
                if (UpdateManager.ContainsKey(key))
                {
                    return;
                }

                _setPassCallsRecorder.Start();
                _drawCallsRecorder.Start();
                _batchesRecorder.Start();
                _trianglesRecorder.Start();
                _verticesRecorder.Start();

                RenderingInfo.ResetProfiledValue();

                UpdateManager.SetAction(key, _OnUpdate);
            }
            else
            {
                _setPassCallsRecorder.Stop();
                _drawCallsRecorder.Stop();
                _batchesRecorder.Stop();
                _trianglesRecorder.Stop();
                _verticesRecorder.Stop();

                UpdateManager.DeleteAction(key);
            }
        }
    }
}
