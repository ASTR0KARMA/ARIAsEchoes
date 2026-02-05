using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class LogScrollDrawer : MonoBehaviour
    {
        [SerializeField]
        GameObject _scrollContent;

        [SerializeField, Header("Log scroll")]
        ObjectPoolScroll _logScroll;
        [SerializeField]
        Button _scrollDown;
        [SerializeField]
        GameObject _noDataLabel;

        [SerializeField, Header("Filter")]
        GameObject _filterContent;
        [SerializeField]
        TMP_InputField _filterInput;
        [SerializeField]
        Button _clearFilter;
        List<LogViewLinker.LogPanelInfo> _logInfos;

        bool _isFloatingWindow;
        bool _isSelectingMode;

        bool _isOnEnable;


        public event UnityAction<string> OnChangeFilterText;

        void _OnValidateUI()
        {
            Assert.IsNotNull(_scrollContent);
            Assert.IsNotNull(_logScroll);
            Assert.IsNotNull(_scrollDown);
            Assert.IsNotNull(_noDataLabel);
            Assert.IsNotNull(_filterInput);
            Assert.IsNotNull(_clearFilter);
        }

        void Awake()
        {
            _OnValidateUI();
            _scrollDown.onClick.RemoveAllListeners();
            _scrollDown.onClick.AddListener(_OnScrollDownToBottom);
            _filterInput.onValueChanged.RemoveAllListeners();
            _filterInput.onValueChanged.AddListener(_OnChangeFilterInput);
            _clearFilter.onClick.RemoveAllListeners();
            _clearFilter.onClick.AddListener(_OnResetFilterText);
        }

        void OnEnable()
        {
            _isOnEnable = true;
        }

        public void Draw(LogViewLinker linker, bool isFloatingWindow)
        {
            _isFloatingWindow = isFloatingWindow;
            _isSelectingMode = linker._isSelectingMode;

            var logs = linker._logs;
            bool existLogs = logs != null && logs.Count > 0;
            _scrollContent.SetActive(existLogs);
            _noDataLabel.SetActive(!existLogs);

            if (existLogs)
            {
                bool scrollReset = _logScroll.verticalNormalizedPosition <= 0.01f &&
                                   !_scrollDown.gameObject.activeSelf &&
                                   !_logScroll.IsScrolling &&
                                   !_logScroll.IsDragging;

                _logInfos = linker._logs;

                if (linker._forceUpdate)
                {
                    _logScroll.Init(_logInfos.Count, _OnRefreshPanel);
                }

                _logScroll.OnScrolled = _OnScrolled;
                _logScroll.RefreshPanels();

                if (linker._resetScrollPos ?? (scrollReset || _isOnEnable))
                {
                    _logScroll.verticalNormalizedPosition = 0;
                }
            }

            _isOnEnable = false;
        }

        public void SwitchFilterContent(bool isShow)
        {
            _filterContent.SetActive(isShow);
        }


        void _OnScrolled()
        {
            bool isShow = !_logScroll.IsShowBottomPanel;
            _scrollDown.gameObject.SetActive(isShow);
        }

        void _OnScrollDownToBottom()
        {
            _logScroll.verticalNormalizedPosition = 0;
        }

        void _OnChangeFilterInput(string text)
        {
            OnChangeFilterText?.Invoke(text);
        }

        void _OnResetFilterText()
        {
            _filterInput.text = string.Empty;
        }


        void _OnRefreshPanel(int index, GameObject target)
        {
            if (index >= _logInfos.Count)
            {
                return;
            }

            var panel = target.GetComponent<LogPanel>();

            if (panel == null)
            {
                throw new Exception("Unable to fetch the LogPanel.");
            }

            var log = _logInfos[index];
            var isSelect = _IsSelect(log);
            panel.Draw(log, isSelect);
        }

        bool _IsSelect(LogViewLinker.LogPanelInfo log)
        {
            bool isSelect;
            if (_isFloatingWindow)
            {
                isSelect = log._isSelect;
            }
            else
            {
                isSelect = _isSelectingMode ? log._isPicOut : log._isSelect;
            }

            return isSelect;
        }
    }
}
