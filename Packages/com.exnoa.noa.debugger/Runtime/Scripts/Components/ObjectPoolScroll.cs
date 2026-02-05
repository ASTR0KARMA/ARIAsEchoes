using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class ObjectPoolScroll : ScrollRect
    {
        [SerializeField, Header("Generate panel object")]
        GameObject _panelPrefab;

        [SerializeField, Header("Panel spacing")]
        float _panelSpace;

        [SerializeField, Header("Padding")]
        float _leftPadding;
        [SerializeField]
        float _rightPadding;
        [SerializeField]
        float _topPadding;
        [SerializeField]
        float _bottomPadding;

        int _panelNum;
        Dictionary<int, PanelData> _showingPanels = new Dictionary<int, PanelData>();
        public Dictionary<int, PanelData> ShowingPanels { get { return new Dictionary<int, PanelData>(_showingPanels); } }
        List<PanelData> _reservedPanels = new List<PanelData>();
        Vector2 _panelSize;

        bool _isOnce;
        bool _isOnEnable;

        int _showFirstIndex;
        int _showLastIndex;

        const string SHOW_PANEL_NAME = "Panel";
        const string HIDE_PANEL_NAME = "Panel(Reserved)";

        UnityAction<int, GameObject> _refreshPanel;

        public int VisiblePanelCountY
        {
            get
            {
                return Mathf.FloorToInt(viewport.rect.height / _panelSize.y);
            }
        }

        public bool IsShowBottomPanel => _showLastIndex == _panelNum - 1;

        public bool IsScrolling { get; private set; }

        public bool IsDragging { get; private set; }

        public UnityAction OnScrolled { get; set; }

        public void Init(int panelNum, UnityAction<int, GameObject> refreshPanel)
        {
            if (!_isOnce)
            {
                _refreshPanel = refreshPanel;
                onValueChanged.RemoveAllListeners();
                onValueChanged.AddListener(_ShowPanelsWithinScrollRange);
                _panelPrefab.SetActive(false);
                _isOnce = true;
            }

            if (panelNum < _panelNum)
            {
                _HideAllPanels();
            }

            _panelNum = panelNum;
            _showFirstIndex = -1;
            _showLastIndex = -1;
            _RefreshContent();
            _ShowPanelsWithinScrollRange(normalizedPosition);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _isOnEnable = true;
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            if (_isOnEnable)
            {
                _ShowPanelsWithinScrollRange(normalizedPosition);
                _isOnEnable = false;
            }

            IsScrolling = false;
        }

        public void RefreshPanels()
        {
            _RefreshPanels(true);
        }

        public void OverwriteContentWidth(float width, bool isPadding = true)
        {
            var padding = Vector2.zero;
            if (isPadding)
            {
                padding = new Vector2()
                {
                    x = _leftPadding + _rightPadding,
                    y = _topPadding + _bottomPadding,
                };
            }

            var contentSize = new Vector2()
            {
                x = width + padding.x,
                y = content.sizeDelta.y,
            };

            content.sizeDelta = contentSize;
        }

        void _RefreshContent()
        {
            var panelRect = _panelPrefab.GetComponent<RectTransform>();
            _PanelStretch(panelRect);

            var padding = new Vector2()
            {
                x = _leftPadding + _rightPadding,
                y = _topPadding + _bottomPadding,
            };

            var panelSizeDelta = panelRect.sizeDelta;
            _panelSize = new Vector2
            {
                x = panelSizeDelta.x,
                y = panelSizeDelta.y + _panelSpace,
            };

            var contentSize = new Vector2()
            {
                x = _panelSize.x + padding.x,
                y = _panelSize.y * _panelNum + padding.y - _panelSpace,
            };

            var contentAnchor = new Vector2(0, 1);
            content.anchorMax = contentAnchor;
            content.anchorMin = contentAnchor;
            content.pivot = contentAnchor;
            content.sizeDelta = contentSize;
        }

        public override void OnScroll(PointerEventData data)
        {
            base.OnScroll(data);
            IsScrolling = true;
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            IsDragging = true;
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            IsDragging = false;
        }

        void _ShowPanelsWithinScrollRange(Vector2 normalizedPos)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (!_isOnce)
            {
                return;
            }

            var contentRect = content.rect;
            var viewPortRect = viewport.rect;

            var scrollAreaSize = new Vector2()
            {
                x = contentRect.width - viewPortRect.width,
                y = contentRect.height - viewPortRect.height,
            };

            Vector2 scrollPos = scrollAreaSize * normalizedPos;

            var scrollUpper = new Vector2()
            {
                x = scrollPos.x + viewPortRect.width,
                y = scrollPos.y + viewPortRect.height,
            };

            var scrollLower = new Vector2()
            {
                x = scrollPos.x - _panelSize.x,
                y = scrollPos.y - _panelSize.y,
            };

            int showFirstIndex = -1;
            int showLastIndex = -1;

            for(int i = 0; i < _panelNum; i++)
            {
                float panelPosY = _panelSize.y * (_panelNum - i - 1);

                bool isShow = true;
                isShow &= scrollUpper.y > panelPosY; 
                isShow &= scrollLower.y < panelPosY; 

                if (isShow)
                {
                    if (i < _showFirstIndex || _showLastIndex < i)
                    {
                        _ShowPanel(i);
                    }

                    if (showFirstIndex == -1)
                    {
                        showFirstIndex = i;
                        showLastIndex = i;
                    }

                    showLastIndex = showLastIndex > i ? showLastIndex : i;
                }
                else
                {
                    _HidePanel(i);
                }
            }

            _showFirstIndex = showFirstIndex;
            _showLastIndex = showLastIndex;

            _RefreshPanels();
            OnScrolled?.Invoke();
        }

        void _ShowPanel(int index)
        {
            if (_showingPanels.ContainsKey(index))
            {
                return;
            }

            PanelData panel = null;

            if (_reservedPanels.Count > 0)
            {
                panel = _reservedPanels[0];
                _reservedPanels.RemoveAt(0);
            }
            else
            {
                panel = PanelData.Instantiate(_panelPrefab, content);
            }

            _showingPanels.Add(index, panel);
            _refreshPanel?.Invoke(index, panel._gameObject);
            panel.SetActive(true);
        }

        void _HidePanel(int index)
        {
            if (!_showingPanels.ContainsKey(index))
            {
                return;
            }

            PanelData panel = _showingPanels[index];

            _showingPanels.Remove(index);
            _reservedPanels.Add(panel);
            panel._gameObject.name = ObjectPoolScroll.HIDE_PANEL_NAME;
            panel.SetActive(false);
        }

        void _PanelStretch(RectTransform panelRect)
        {
            var padding = new Vector2()
            {
                x = _leftPadding + _rightPadding,
                y = _topPadding + _bottomPadding,
            };

            var panelStretch = new Vector2()
            {
                x = viewport.rect.width - padding.x, 
                y = panelRect.rect.height,
            };
            panelRect.sizeDelta = panelStretch;
        }

        void _RefreshPanels(bool forceRefresh = false)
        {
            var panelAnchor = new Vector2(0, 1);

            foreach(var panel in _showingPanels)
            {
                var rect = panel.Value._gameObject.GetComponent<RectTransform>();

                rect.gameObject.name = ObjectPoolScroll.SHOW_PANEL_NAME + panel.Key;
                rect.anchorMax = panelAnchor;
                rect.anchorMin = panelAnchor;
                rect.pivot = panelAnchor;
                _PanelStretch(rect);

                var padding = new Vector2()
                {
                    x = _leftPadding,
                    y = _topPadding,
                };

                var panelPos = new Vector2()
                {
                    x = padding.x,
                    y = -_panelSize.y * panel.Key - padding.y,
                };
                rect.transform.localPosition = panelPos;

                if (forceRefresh)
                {
                    _refreshPanel?.Invoke(panel.Key, panel.Value._gameObject);
                }
            }
        }

        void _HideAllPanels()
        {
            foreach(var showPanel in _showingPanels)
            {
                var panel = showPanel.Value;
                panel.SetActive(false);
                _reservedPanels.Add(showPanel.Value);
            }
            _showingPanels.Clear();
        }

        internal class PanelData
        {
            public readonly GameObject _gameObject;
            CanvasGroup _canvasGroup;
            PanelData(GameObject gameObject)
            {
                _gameObject = gameObject;
                _canvasGroup = gameObject.GetComponent<CanvasGroup>();
                if (_canvasGroup == null)
                {
                    _canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }

            public void SetActive(bool isActive)
            {
                _canvasGroup.alpha = isActive ? 1f : 0f;
                _canvasGroup.blocksRaycasts = isActive;
            }

            public static PanelData Instantiate(GameObject prefab, Transform content)
            {
                var obj = GameObject.Instantiate(prefab, content);
                obj.SetActive(true);
                return new PanelData(obj);
            }
        }
    }
}
