using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NoaDebugger
{
    sealed class TargetFrameRateModel : ModelBase
    {
        static readonly int[] PresetChoices = { -1, 5, 15, 30, 60, 72, 90, 120, 144, 165, 240, 360 };

        static readonly string DefaultLabel = "Default";
        static readonly string TargetFrameRateLabel = "Target Frame Rate";

        int _cachedTargetFrameRate;
        List<int> _targetFrameRateList;
        string[] _targetFrameRateChoicesArray;
        bool _isNeedUpdateList;

        public TargetFrameRateModel()
        {
            _cachedTargetFrameRate = -1;

            _isNeedUpdateList = true;
        }

        public string[] GetChoicesArrayIfNeedUpdate()
        {
            if (_cachedTargetFrameRate == Application.targetFrameRate && !_isNeedUpdateList)
            {
                return null;
            }

            _isNeedUpdateList = false;

            _targetFrameRateList = TargetFrameRateModel.PresetChoices.ToList();

            bool addListUnique(int value)
            {
                if (0 < value && !_targetFrameRateList.Contains(value))
                {
                    _targetFrameRateList.Add(value);

                    return true;
                }

                return false;
            }

            addListUnique(_cachedTargetFrameRate);
            bool isUniqueCurrent = addListUnique(Application.targetFrameRate);

            _targetFrameRateList = _targetFrameRateList.OrderBy(x => x).ToList();

            _targetFrameRateChoicesArray = _targetFrameRateList.Select(_GetTargetFrameRateText).ToArray();

            if (isUniqueCurrent)
            {
                _cachedTargetFrameRate = Application.targetFrameRate;
            }

            return _targetFrameRateChoicesArray;
        }

        public int GetCurrentIndex()
        {
            return Array.IndexOf(_targetFrameRateList.ToArray(), Application.targetFrameRate);
        }

        public void SetTargetFrameRate(int targetFrameRateIndex)
        {
            Application.targetFrameRate = _targetFrameRateList[targetFrameRateIndex];
        }

        string _GetTargetFrameRateText(int targetFrameRate)
        {
            string valueStr = targetFrameRate == -1
                ? TargetFrameRateModel.DefaultLabel
                : targetFrameRate.ToString();

            return $"{TargetFrameRateModel.TargetFrameRateLabel} {valueStr}";
        }

        public void Dispose()
        {
            _cachedTargetFrameRate = default;
            _targetFrameRateList = default;
            _targetFrameRateChoicesArray = default;
            _isNeedUpdateList = default;
        }
    }
}
