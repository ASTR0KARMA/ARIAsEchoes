using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class BatteryTextDrawerComponent : MonoBehaviour
    {
        static readonly string InProgressLabel = "InProgress...";
        static readonly string ChargingLabel = "Charging";

        [SerializeField]
        TextMeshProUGUI _currentBatteryAndMinute;

        [SerializeField]
        TextMeshProUGUI _operatingTime;

        void Awake()
        {
            Assert.IsNotNull(_currentBatteryAndMinute);
            Assert.IsNotNull(_operatingTime);
        }

        public void OnShowBatteryText(BatteryUnchangingInfo info, Color? enableTextColor = null)
        {
            Color enableColor = enableTextColor ?? NoaDebuggerDefine.TextColors.Dynamic;

            if (info.IsViewHyphen)
            {
                ProfilerDrawerHelper.ShowHyphenValue(_currentBatteryAndMinute);
                ProfilerDrawerHelper.ShowHyphenValue(_operatingTime);
            }

            else if (info.Status == BatteryStatus.Unknown)
            {
                ProfilerDrawerHelper.ShowMissingValue(_currentBatteryAndMinute);
                ProfilerDrawerHelper.ShowMissingValue(_operatingTime);
            }
            else
            {
                string minuteLabel = $"{info.ConsumptionPerMinute.ToString(CultureInfo.InvariantCulture)}%";
                Color currentBatteryLabelColor = enableColor;

                if (info.Status == BatteryStatus.Profiling)
                {
                    minuteLabel = BatteryTextDrawerComponent.InProgressLabel;
                    currentBatteryLabelColor = NoaDebuggerDefine.TextColors.InProgress;
                }

                if (info.Status == BatteryStatus.Charging)
                {
                    minuteLabel = BatteryTextDrawerComponent.ChargingLabel;
                    currentBatteryLabelColor = NoaDebuggerDefine.TextColors.InProgress;
                }

                _currentBatteryAndMinute.text = $"{info.CurrentLevelPercent}%({minuteLabel})";
                _currentBatteryAndMinute.color = currentBatteryLabelColor;
                var span = new TimeSpan(0, 0, info.OperatingTimeSec);
                _operatingTime.text = span.ToString(@"hh\:mm\:ss");
                _operatingTime.color = enableColor;
            }
        }

        void OnDestroy()
        {
            _currentBatteryAndMinute = default;
            _operatingTime = default;
        }
    }
}
