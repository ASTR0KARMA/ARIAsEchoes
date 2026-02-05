using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class FpsGaugeDrawerComponent : MonoBehaviour
    {
        [SerializeField]
        GaugeChart _fpsGauge;

        void Awake()
        {
            Assert.IsNotNull(_fpsGauge);
        }

        public void OnShowFpsGauge(FpsUnchangingInfo info)
        {
            if (info.IsViewHyphen)
            {
                _fpsGauge.MaxValue = 0;
                _fpsGauge.Value = 0;
                return;
            }

            if (Application.targetFrameRate <= 0)
            {
#pragma warning disable CS0618 
                var displayInfo = new DisplayInfo();
#pragma warning restore CS0618 
                _fpsGauge.MaxValue = displayInfo.RefreshRate;
            }
            else
            {
                _fpsGauge.MaxValue = Application.targetFrameRate;
            }

            _fpsGauge.Value = info.CurrentFps;
        }

        void OnDestroy()
        {
            _fpsGauge = default;
        }
    }
}
