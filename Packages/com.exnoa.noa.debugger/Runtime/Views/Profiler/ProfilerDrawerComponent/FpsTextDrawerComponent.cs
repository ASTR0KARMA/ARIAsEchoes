using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class FpsTextDrawerComponent : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI _currentFps;

        [SerializeField]
        TextMeshProUGUI _maxAndMinAndAverageFps;

        [SerializeField]
        TextMeshProUGUI _refreshRate;

        void Awake()
        {
            Assert.IsNotNull(_currentFps);
            Assert.IsNotNull(_maxAndMinAndAverageFps);
            Assert.IsNotNull(_refreshRate);
        }

        public void OnShowFpsText(FpsUnchangingInfo info, Color? enableTextColor = null)
        {
            Color enableColor = enableTextColor ?? NoaDebuggerDefine.TextColors.Dynamic;

            if (info.IsViewHyphen)
            {
                ProfilerDrawerHelper.ShowHyphenValue(_currentFps);
                ProfilerDrawerHelper.ShowHyphenValue(_maxAndMinAndAverageFps);
                ProfilerDrawerHelper.ShowHyphenValue(_refreshRate);
            }
            else
            {
                string currentFpsText = ProfilerDrawerHelper.GetFpsText(info.CurrentFps);
                string elapsedTimeText = ProfilerDrawerHelper.GetElapsedTimeText(info.ElapsedTime);
                string maxFpsText = ProfilerDrawerHelper.GetFpsText(info.Max);
                string minFpsText = ProfilerDrawerHelper.GetFpsText(info.Min);
                string avgFpsText = ProfilerDrawerHelper.GetFpsText(info.AverageFps);
                _currentFps.text = $"{currentFpsText} ({elapsedTimeText})";

                _maxAndMinAndAverageFps.text =
                    ProfilerDrawerHelper.GetMaxMinAvgText(maxFpsText, minFpsText, avgFpsText);

                _currentFps.color = enableColor;
                _maxAndMinAndAverageFps.color = enableColor;

#pragma warning disable CS0618 
                var displayInfo = new DisplayInfo();
#pragma warning restore CS0618 
                _refreshRate.text = $"{displayInfo.RefreshRate}Hz";
                _refreshRate.color = enableColor;
            }
        }

        void OnDestroy()
        {
            _currentFps = default;
            _maxAndMinAndAverageFps = default;
            _refreshRate = default;
        }
    }
}
