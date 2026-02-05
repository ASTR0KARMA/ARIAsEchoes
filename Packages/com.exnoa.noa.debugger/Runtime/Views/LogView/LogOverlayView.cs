using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class LogOverlayView : OverlayViewBase<LogOverlayViewLinker>
    {
        const float FADE_TIME = 1f;

        const string DEVICE_ORIENTATION_KEY_PREFIX = "LogOverlayViewDeviceOrientation";
        string DeviceOrientationKey => LogOverlayView.DEVICE_ORIENTATION_KEY_PREFIX + gameObject.name;

        [SerializeField]
        RectTransform _root;

        [SerializeField]
        CanvasGroup _canvasGroup;

        [SerializeField]
        UIBehaviourComponent _uiBehaviour;

        [SerializeField, Header("Log counters")]
        TextMeshProUGUI _logCounter;
        [SerializeField]
        TextMeshProUGUI _warningCounter;
        [SerializeField]
        TextMeshProUGUI _errorCounter;

        [SerializeField, Header("Log entry")]
        GameObject _logEntryPrefab;
        [SerializeField]
        RectTransform _logEntryParent;
        RectOffset _logEntryParentLayoutPadding = new RectOffset();
        float _logEntryParentLayoutSpacing = 0;

        OverlayLogPanel _logEntryReference;

        Queue<OverlayLogPanel> _displayedLogEntryList = new Queue<OverlayLogPanel>();
        Stack<OverlayLogPanel> _reservedLogEntryList = new Stack<OverlayLogPanel>();

        float beforeMinimumOpacity = -1;
        IEnumerator _hideCoroutine = null;
        IEnumerator _fadeOutCoroutine = null;

        float _tmpFontScale;
        int _tmpMaximumLogCount;

        public event Action OnEnabledAction;

        void _OnValidateUI()
        {
            Assert.IsNotNull(_root);
            Assert.IsNotNull(_canvasGroup);
            Assert.IsNotNull(_uiBehaviour);
            Assert.IsNotNull(_logEntryPrefab);
            Assert.IsNotNull(_logEntryParent);
        }

        protected override void _Init()
        {
            base._Init();
            _OnValidateUI();
            _uiBehaviour.OnRectTransformDimensionsChanged += _OnTargetRectTransformDimensionsChanged;
            DeviceOrientationManager.DeleteAction(DeviceOrientationKey);
            DeviceOrientationManager.SetAction(DeviceOrientationKey, _OnDeviceOrientation);

            _InstantiateReferenceLogEntry();

            var layout = _logEntryParent.GetComponent<HorizontalOrVerticalLayoutGroup>();
            if (layout != null)
            {
                _logEntryParentLayoutPadding = layout.padding;
                _logEntryParentLayoutSpacing = layout.spacing;
            }

            bool isPortrait = DeviceOrientationManager.IsPortrait;
            _RefreshWindowWidth(isPortrait);
        }

        void OnEnable()
        {
            if (GlobalCoroutine.IsExecutable)
            {
                GlobalCoroutine.Run(OnEnableDelay());
            }
        }

        IEnumerator OnEnableDelay()
        {
            yield return new WaitForEndOfFrame();
            OnEnabledAction?.Invoke();
        }

        public override void OnUpdateOnce(LogOverlayViewLinker linker)
        {
            base.OnUpdateOnce(linker);

            if (_root.gameObject.activeInHierarchy)
            {
                float newMinimumOpacity = linker._minimumOpacity;
                bool opacityChanged = beforeMinimumOpacity != newMinimumOpacity;
                if (opacityChanged)
                {
                    ApplyMinimumOpacitySetting(linker._minimumOpacity);
                }
            }
            else
            {
                ApplyMinimumOpacitySetting(linker._minimumOpacity);
            }

            beforeMinimumOpacity = linker._minimumOpacity;
        }

        public override void OnUpdate(LogOverlayViewLinker linker)
        {
            base.OnUpdate(linker);

            if (_logCounter != null)
            {
                _logCounter.text = linker._logTypeToggles._messageNum.ToString();
            }
            if (_warningCounter != null)
            {
                _warningCounter.text = linker._logTypeToggles._warningNum.ToString();
            }
            if (_errorCounter != null)
            {
                _errorCounter.text = linker._logTypeToggles._errorNum.ToString();
            }

            _RefreshLogEntries(linker._logs, linker._showTimestamp);

            _RefreshFontScale(linker._fontScale, linker._maximumLogCount);

            bool needManagedHide = linker._logs.Any(log => log._hasLoggedOnce == false);
            if (needManagedHide)
            {
                _ManagedHideDelay(linker._activeDulation, linker._minimumOpacity);
            }
        }

        void _OnTargetRectTransformDimensionsChanged()
        {
            bool isPortrait = DeviceOrientationManager.IsPortrait;
            _RefreshWindowWidth(isPortrait);
        }

        void _OnDeviceOrientation(bool isPortrait)
        {
            _RefreshWindowWidth(isPortrait);
        }

        void _RefreshWindowWidth(bool isPortrait)
        {
            int windowRate = isPortrait ? 2 : 3; 

            var parentRect = _root.parent.GetComponent<RectTransform>();
            _root.sizeDelta = new Vector2(parentRect.sizeDelta.x / windowRate, _root.sizeDelta.y);
        }

        void _RefreshFontScale(float fontScale, int maximumNum)
        {
            bool isChangeFontScale = false;

            if(Mathf.Approximately(fontScale,_tmpFontScale) == false)
            {
                _tmpFontScale = fontScale;

                foreach(var entry in _displayedLogEntryList)
                {
                    entry.ChangeFontScale(fontScale);
                }
                foreach(var entry in _reservedLogEntryList)
                {
                    entry.ChangeFontScale(fontScale);
                }
                _logEntryReference.ChangeFontScale(fontScale);
                LayoutRebuilder.ForceRebuildLayoutImmediate(_logEntryReference.RectTransform);

                isChangeFontScale = true;
            }

            if(maximumNum != _tmpMaximumLogCount || isChangeFontScale)
            {
                _tmpMaximumLogCount = maximumNum;
                _RefreshWindowHeight(maximumNum);
            }
        }

        void _RefreshWindowHeight(int maximumNum)
        {
            var entriesHeight = _logEntryReference.RectTransform.sizeDelta.y * maximumNum; 
            entriesHeight += _logEntryParentLayoutPadding.top + _logEntryParentLayoutPadding.bottom; 
            entriesHeight += _logEntryParentLayoutSpacing * (maximumNum - 1); 
            _logEntryParent.sizeDelta = new Vector2(_logEntryParent.sizeDelta.x, entriesHeight);

            LayoutRebuilder.ForceRebuildLayoutImmediate(_root);
        }

        void _RefreshLogEntries(List<LogViewLinker.LogPanelInfo> allLogs, bool isShowTimestamp)
        {
            _InstantiateLogEntries(allLogs.Count);
            _HideExistLogEntries(allLogs);
            _DisplayNewLogEntry(allLogs, isShowTimestamp);
        }

        void _InstantiateLogEntries(int logNum)
        {
            int logCount = _displayedLogEntryList.Count + _reservedLogEntryList.Count;
            for (int i = logCount; i < logNum; i++)
            {
                var entry = Instantiate(_logEntryPrefab, _logEntryParent);
                entry.SetActive(false);
                OverlayLogPanel panel = entry.GetComponent<OverlayLogPanel>();
                _reservedLogEntryList.Push(panel);
                _tmpFontScale = -1; 
            }
        }

        void _DisplayNewLogEntry(List<LogViewLinker.LogPanelInfo> logs, bool isShowTimestamp)
        {
            foreach (var log in logs)
            {
                bool isDisplayed = false;

                foreach (var panel in _displayedLogEntryList)
                {
                    if (panel.Equal(log))
                    {
                        isDisplayed = true;
                        panel.Draw(log, isShowTimestamp); 
                        break;
                    }
                }

                if (isDisplayed == false)
                {
                    OverlayLogPanel displayPanel = null;

                    _reservedLogEntryList.TryPop(out displayPanel);

                    if (displayPanel == null)
                    {
                        _displayedLogEntryList.TryDequeue(out displayPanel);
                    }

                    displayPanel.Draw(log, isShowTimestamp);
                    displayPanel.gameObject.SetActive(true);
                    displayPanel.gameObject.transform.SetSiblingIndex(0);
                    _displayedLogEntryList.Enqueue(displayPanel);
                }
            }

            var displayPanels = _displayedLogEntryList.OrderBy(panel => panel.LogSerialNumber).ToList();
            foreach (var panel in displayPanels)
            {
                panel.transform.SetSiblingIndex(0);
            }
        }

        void _HideExistLogEntries(List<LogViewLinker.LogPanelInfo> logs)
        {
            Queue<OverlayLogPanel> newQueue = new Queue<OverlayLogPanel>(_displayedLogEntryList.Count);
            foreach (var panel in _displayedLogEntryList)
            {
                bool isHideTarget = true;
                foreach (var log in logs)
                {
                    if (panel.Equal(log))
                    {
                        newQueue.Enqueue(panel);
                        isHideTarget = false;
                        break;
                    }
                }

                if (isHideTarget)
                {
                    panel.ResetHashCode();
                    panel.gameObject.SetActive(false);
                    _reservedLogEntryList.Push(panel);
                }
            }

            _displayedLogEntryList = newQueue;
        }

        void _InstantiateReferenceLogEntry()
        {
            var entry = Instantiate(_logEntryPrefab, _root);
            entry.name = "LogEntryReference";

            var le = entry.AddComponent<LayoutElement>();
            le.ignoreLayout = true;

            var cg = entry.AddComponent<CanvasGroup>();
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;

            var csf = entry.AddComponent<ContentSizeFitter>();
            csf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            _logEntryReference = entry.GetComponent<OverlayLogPanel>();
        }

        void _ManagedHideDelay(float activeDuration, float minimumOpacity)
        {
            if (_hideCoroutine != null)
            {
                GlobalCoroutine.Stop(_hideCoroutine);
                _hideCoroutine = null;
            }
            if(_fadeOutCoroutine != null)
            {
                GlobalCoroutine.Stop(_fadeOutCoroutine);
                _fadeOutCoroutine = null;
            }

            _canvasGroup.alpha = 1f;
            _hideCoroutine = _HideAfterDelay(activeDuration, minimumOpacity);
            GlobalCoroutine.Run(_hideCoroutine);
        }

        IEnumerator _HideAfterDelay(float delayTime, float minimumOpacity)
        {
            float startTime = Time.realtimeSinceStartup;
            float elapsedTime = 0;

            while(elapsedTime < delayTime)
            {
                elapsedTime = Time.realtimeSinceStartup - startTime;
                yield return null;
            }

            _hideCoroutine = null;
            _fadeOutCoroutine = _FadeOut(minimumOpacity);
            GlobalCoroutine.Run(_fadeOutCoroutine);
        }

        IEnumerator _FadeOut(float minimumOpacity)
        {
            float startTime = Time.realtimeSinceStartup;
            float elapsedTime = 0;

            while(elapsedTime < FADE_TIME)
            {
                elapsedTime = Time.realtimeSinceStartup - startTime;
                float alpha = Mathf.Lerp(1, minimumOpacity, elapsedTime / FADE_TIME);
                _canvasGroup.alpha = alpha;
                yield return null;
            }

            ApplyMinimumOpacitySetting(minimumOpacity);
            _fadeOutCoroutine = null;
        }

        void ApplyMinimumOpacitySetting(float minimumOpacity)
        {
            _canvasGroup.alpha = minimumOpacity;
        }

        void OnDestroy()
        {
            DeviceOrientationManager.DeleteAction(DeviceOrientationKey);

            if (_hideCoroutine != null)
            {
                if (GlobalCoroutine.IsExecutable)
                {
                    GlobalCoroutine.Stop(_hideCoroutine);
                }
                _hideCoroutine = null;
            }
            if (_fadeOutCoroutine != null)
            {
                if (GlobalCoroutine.IsExecutable)
                {
                    GlobalCoroutine.Stop(_fadeOutCoroutine);
                }
                _fadeOutCoroutine = null;
            }

            _root = default;
            _logCounter = default;
            _warningCounter = default;
            _errorCounter = default;
            _logEntryPrefab = default;
            _logEntryParent = default;
            _logEntryParentLayoutPadding = default;
            _displayedLogEntryList = default;
            _reservedLogEntryList = default;
            _logEntryReference = default;
        }
    }

    sealed class LogOverlayViewLinker : ViewLinkerBase
    {
        public NoaDebug.OverlayPosition _position;

        public float _minimumOpacity;

        public float _fontScale;

        public int _maximumLogCount;

        public bool _showTimestamp;

        public float _activeDulation;

        public List<LogViewLinker.LogPanelInfo> _logs;

        public LogViewLinker.LogTypeToggles _logTypeToggles;
    }
}
