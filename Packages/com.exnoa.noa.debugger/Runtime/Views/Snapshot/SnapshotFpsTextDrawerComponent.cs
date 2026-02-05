using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class SnapshotFpsTextDrawerComponent : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI _currentFps;

        [SerializeField]
        TextMeshProUGUI _maxAndMinFps;

        [SerializeField]
        TextMeshProUGUI _averageFps;

        void Awake()
        {
            Assert.IsNotNull(_currentFps);
            Assert.IsNotNull(_maxAndMinFps);
            Assert.IsNotNull(_averageFps);
        }

        public void OnShowFpsText(FpsUnchangingInfo info, Color? enableTextColor = null)
        {
            Color enableColor = enableTextColor ?? NoaDebuggerDefine.TextColors.Dynamic;

            if (info.IsViewHyphen)
            {
                ProfilerDrawerHelper.ShowHyphenValue(_currentFps);
                ProfilerDrawerHelper.ShowHyphenValue(_maxAndMinFps);
                ProfilerDrawerHelper.ShowHyphenValue(_averageFps);
            }
            else
            {
                string currentFpsText = ProfilerDrawerHelper.GetFpsText(info.CurrentFps);
                string elapsedTimeText = ProfilerDrawerHelper.GetElapsedTimeText(info.ElapsedTime);
                string maxFpsText = ProfilerDrawerHelper.GetFpsText(info.Max);
                string minFpsText = ProfilerDrawerHelper.GetFpsText(info.Min);

                _currentFps.text = $"{currentFpsText} ({elapsedTimeText})";
                _maxAndMinFps.text = ProfilerDrawerHelper.GetMaxMinText(maxFpsText, minFpsText);
                _averageFps.text = ProfilerDrawerHelper.GetFpsText(info.AverageFps);

                _currentFps.color = enableColor;
                _maxAndMinFps.color = enableColor;
                _averageFps.color = enableColor;
            }
        }

        void OnDestroy()
        {
            _currentFps = default;
            _maxAndMinFps = default;
            _averageFps = default;
        }
    }
}
