using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class ThermalTextDrawerComponent : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI _currentThermal;

        [SerializeField]
        TextMeshProUGUI _maxAndMinThermal;

        [SerializeField]
        TextMeshProUGUI _averageThermal;

        void Awake()
        {
            Assert.IsNotNull(_currentThermal);
            Assert.IsNotNull(_maxAndMinThermal);
            Assert.IsNotNull(_averageThermal);
        }

        public void OnShowThermalText(ThermalUnchangingInfo info, Color? enableTextColor = null)
        {
            Color enableColor = enableTextColor ?? NoaDebuggerDefine.TextColors.Dynamic;

            if (info.IsViewHyphen)
            {
                ProfilerDrawerHelper.ShowHyphenValue(_currentThermal);
                ProfilerDrawerHelper.ShowHyphenValue(_maxAndMinThermal);
                ProfilerDrawerHelper.ShowHyphenValue(_averageThermal);
            }

            else if (info.CurrentThermalStatus == ThermalStatus.Unknown)
            {
                ProfilerDrawerHelper.ShowMissingValue(_currentThermal);
                ProfilerDrawerHelper.ShowMissingValue(_maxAndMinThermal);
                ProfilerDrawerHelper.ShowMissingValue(_averageThermal);
            }

            else if (info.CurrentTemperature <= -1)
            {
                string maxThermalStatusText = ThermalUnchangingInfo.ConvertThermalStatusText(info.MaxThermalStatus);
                string minThermalStatusText = ThermalUnchangingInfo.ConvertThermalStatusText(info.MinThermalStatus);

                _currentThermal.text = ThermalUnchangingInfo.ConvertThermalStatusText(info.CurrentThermalStatus);
                _maxAndMinThermal.text = ProfilerDrawerHelper.GetMaxMinText(maxThermalStatusText, minThermalStatusText);

                _currentThermal.color = enableColor;
                _maxAndMinThermal.color = enableColor;

                ProfilerDrawerHelper.ShowMissingValue(_averageThermal);
            }
            else
            {
                string maxTemperatureText = ProfilerDrawerHelper.GetTemperatureText(info.MaxTemperature);
                string minTemperatureText = ProfilerDrawerHelper.GetTemperatureText(info.MinTemperature);

                _currentThermal.text = ProfilerDrawerHelper.GetTemperatureText(info.CurrentTemperature);
                _maxAndMinThermal.text = ProfilerDrawerHelper.GetMaxMinText(maxTemperatureText, minTemperatureText);
                _averageThermal.text = ProfilerDrawerHelper.GetTemperatureText(info.AverageTemperature);

                _currentThermal.color = enableColor;
                _maxAndMinThermal.color = enableColor;
                _averageThermal.color = enableColor;
            }
        }

        void OnDestroy()
        {
            _currentThermal = default;
            _maxAndMinThermal = default;
            _averageThermal = default;
        }
    }
}
