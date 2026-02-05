using System;
using UnityEngine;
using UnityEngine.Events;

namespace NoaDebugger
{
    sealed class AutoRefreshSwitcher
    {
        static readonly string OnUpdatePrefix = "AutoRefreshSwitcherOnUpdate";
        readonly string _onUpdateKey;

        public bool IsAutoRefresh { get; private set; }

        float _autoRefreshTimer;

        readonly UnityAction _onUpdate;
        readonly string _prefsKey;
        readonly float _refreshInterval;

        public AutoRefreshSwitcher(UnityAction onUpdate, string prefsKey, float refreshInterval)
        {
            _onUpdateKey = AutoRefreshSwitcher.OnUpdatePrefix + Guid.NewGuid();
            _onUpdate = onUpdate;
            _refreshInterval = refreshInterval;

            _prefsKey = prefsKey;
            IsAutoRefresh = NoaDebuggerPrefs.GetBoolean(_prefsKey, false);
            _HandleOnUpdate(IsAutoRefresh);
        }

        public void UpdateAutoRefresh(bool isAutoRefresh)
        {
            NoaDebuggerPrefs.SetBoolean(_prefsKey, isAutoRefresh);
            IsAutoRefresh = isAutoRefresh;
            _HandleOnUpdate(isAutoRefresh);
        }

        void _HandleOnUpdate(bool isAutoRefresh)
        {
            if (isAutoRefresh)
            {
                if (UpdateManager.ContainsKey(_onUpdateKey))
                {
                    return;
                }

                UpdateManager.SetAction(_onUpdateKey, _OnUpdate);
            }

            else
            {
                _autoRefreshTimer = 0;
                UpdateManager.DeleteAction(_onUpdateKey);
            }
        }

        void _OnUpdate()
        {
            _autoRefreshTimer += Time.deltaTime;

            if (_autoRefreshTimer > _refreshInterval)
            {
                _onUpdate?.Invoke();
                _autoRefreshTimer = 0;
            }
        }
    }
}
