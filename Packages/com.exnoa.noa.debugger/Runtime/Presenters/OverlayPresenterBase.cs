using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NoaDebugger
{
    abstract class OverlayPresenterBase<TOverlayView, TOverlaySettingsView, TOverlayToolSettings, TViewLinker>
        where TOverlayView : OverlayViewBase<TViewLinker>
        where TOverlaySettingsView : OverlaySettingsViewBase<TOverlayToolSettings, OverlaySettingsViewLinker<TOverlayToolSettings>>
        where TOverlayToolSettings : OverlayToolSettingsBase, new()
        where TViewLinker : ViewLinkerBase
    {
        readonly TOverlayView _overlayViewPrefab;
        TOverlayView _overlayView;
        Transform _overlayViewRoot;

        readonly TOverlaySettingsView _overlaySettingsViewPrefab;
        OverlaySettingsViewBase<TOverlayToolSettings, OverlaySettingsViewLinker<TOverlayToolSettings>> _overlaySettingsView;

        public TOverlayToolSettings _overlayToolSettings;

        readonly string _isOverlayEnablePrefsKey;

        bool _isOverlayEnable;
        bool _isOverlaySettingsEnable;
        bool _isNeedApplySettings;
        bool _cachedIsUIReverse;

        protected NoaDebuggerSettings _settings;

        public event Action<TOverlayView> OnInitAction;

        public event Action OnUpdateSettings;

        public bool IsOverlayEnable
        {
            get { return _isOverlayEnable; }
            set
            {
                _isOverlayEnable = value;

                NoaDebuggerPrefs.SetBoolean(_isOverlayEnablePrefsKey, _isOverlayEnable);
            }
        }

        public bool IsShowOverlay => IsOverlayEnable && NoaDebuggerVisibilityManager.IsOverlayVisible;

        public bool IsOverlaySettingsEnable => _isOverlaySettingsEnable;

        protected abstract ViewLinkerBase _CreateOverlayViewLinker();

        public OverlayPresenterBase(
            TOverlayView overlayPrefab,
            TOverlaySettingsView overlaySettingsPrefab,
            string prefsKeyPrefix)
        {
            _overlayViewPrefab = overlayPrefab;
            _overlaySettingsViewPrefab = overlaySettingsPrefab;

            if (_overlayToolSettings != null)
            {
                _overlayToolSettings.OnUpdateSettings += _OnUpdateSettings;
            }

            _settings = NoaDebuggerSettingsManager.GetNoaDebuggerSettings();

            _isOverlayEnablePrefsKey = prefsKeyPrefix + NoaDebuggerPrefsDefine.PrefsKeySuffixOverlayEnabled;

            _isOverlayEnable = NoaDebuggerPrefs.GetBoolean(
                _isOverlayEnablePrefsKey,
                NoaDebuggerDefine.DefaultOverlayEnabled);

            SettingsEventModel.OnUpdateCommonOverlaySettings -= _OnUpdateCommonOverlaySettings;
            SettingsEventModel.OnUpdateCommonOverlaySettings += _OnUpdateCommonOverlaySettings;

            _isNeedApplySettings = true;
        }

        public void InstantiateOverlay(Transform parent)
        {
            if (_overlayView != null)
            {
                return;
            }

            _overlayView = Object.Instantiate(_overlayViewPrefab, parent);
            OnInitAction?.Invoke(_overlayView);
        }

        public void ShowOverlayView(TViewLinker linker)
        {
            if (_overlayView == null)
            {
                return;
            }

            if (_isNeedApplySettings)
            {
                var overlayLinker = new OverlayViewLinker<TViewLinker>(linker);

                _overlayView.Show(overlayLinker);
                _isNeedApplySettings = false;
            }
            else
            {
                _overlayView.OnUpdate(linker);
            }
        }

        public void ShowOverlayView()
        {
            var linker = _CreateOverlayViewLinker() as TViewLinker;
            ShowOverlayView(linker);
        }

        public void UpdateOverlayView()
        {
            var linker = _CreateOverlayViewLinker() as TViewLinker;
            _overlayView.OnUpdateOnce(linker);
        }

        void _OnUpdateCommonOverlaySettings()
        {
            if (_overlayView == null)
            {
                return;
            }
            _overlayView.ApplyOverlaySettings();
        }

        void _DestroyOverlay()
        {
            if (_overlayView == null)
            {
                return;
            }

            Object.Destroy(_overlayView.gameObject);
            _overlayView = null;
            _isNeedApplySettings = true;
        }

        public void ToggleActiveOverlayView(Transform parent)
        {
            _overlayViewRoot = parent;
            IsOverlayEnable = !IsOverlayEnable;

            if (IsOverlayEnable)
            {
                if (NoaDebuggerVisibilityManager.IsOverlayRootActive)
                {
                    InstantiateOverlay(parent);
                }
                else
                {
                    NoaDebuggerVisibilityManager.AddOverlaySetActiveAction(_OnOverlayRootActiveChanged);
                }
            }
            else
            {
                NoaDebuggerVisibilityManager.RemoveOverlaySetActiveAction(_OnOverlayRootActiveChanged);
                _DestroyOverlay();
            }
        }

        void _OnOverlayRootActiveChanged(bool active)
        {
            if (active && IsOverlayEnable)
            {
                InstantiateOverlay(_overlayViewRoot);
            }
        }

        void _InstantiateOverlaySettings(Transform parent)
        {
            if (_overlaySettingsView != null)
            {
                return;
            }

            _overlaySettingsView = Object.Instantiate(_overlaySettingsViewPrefab, parent)  as OverlaySettingsViewBase<TOverlayToolSettings, OverlaySettingsViewLinker<TOverlayToolSettings>>;
            _overlaySettingsView.AlignmentUI(_cachedIsUIReverse);

            SettingsEventModel.OnSettingsValueChanged += _overlaySettingsView.SetActiveFooterArea;
        }

        public void DestroyOverlaySettings()
        {
            if (_overlaySettingsView == null)
            {
                return;
            }

            SettingsEventModel.OnSettingsValueChanged -= _overlaySettingsView.SetActiveFooterArea;

            Object.Destroy(_overlaySettingsView.gameObject);
            _overlaySettingsView = null;

            _isOverlaySettingsEnable = false;
        }

        public void ToggleActiveOverlaySettingsView(GameObject mainViewObj, Transform parent)
        {
            _isOverlaySettingsEnable = !_isOverlaySettingsEnable;

            if (_isOverlaySettingsEnable)
            {
                _InstantiateOverlaySettings(parent);

                var linker = new OverlaySettingsViewLinker<TOverlayToolSettings>()
                {
                    Settings = _overlayToolSettings
                };

                _overlaySettingsView.Show(linker);
            }
            else if (_overlaySettingsView != null)
            {
                _overlaySettingsView.Hide();
            }

            mainViewObj.SetActive(!_isOverlaySettingsEnable);
        }

        public void ApplySettings()
        {
            _OnUpdateSettings();
        }

        void _OnUpdateSettings()
        {
            if (_overlayView == null)
            {
                _isNeedApplySettings = true;
                return;
            }

            _isNeedApplySettings = false;

            OnUpdateSettings?.Invoke();
        }

        public void AlignmentUI(bool isReverse)
        {
            _cachedIsUIReverse = isReverse;

            if (_overlaySettingsView != null)
            {
                _overlaySettingsView.AlignmentUI(isReverse);
            }
        }
    }
}
