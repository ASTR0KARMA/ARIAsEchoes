using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NoaDebugger
{
    abstract class ProfilerOverlayFeatureViewBase<TData, TChartComponent> : UIBehaviour
        where TChartComponent : ChartDrawerComponentBase
    {
        protected abstract Color TextColor { get; }

        [SerializeField]
        protected GameObject _rootObject;

        [SerializeField]
        protected TChartComponent _graph;

        [SerializeField]
        protected LayoutElement _textAreaLayout;

        [SerializeField]
        protected LayoutElement _graphLayout;

        RectTransform _textAreaRect;
        string _textColorCode;
        protected NoaProfiler.OverlayTextType _textType;
        protected bool _isShowGraphOnOverlay;

        protected override void Awake()
        {
            base.Awake();

            Assert.IsNotNull(_rootObject);
            Assert.IsNotNull(_graph);

            _textAreaRect = _textAreaLayout.transform as RectTransform;

            _textColorCode = ColorUtility.ToHtmlStringRGB(TextColor);

            OnInitialize();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            SetWidth(_textAreaRect.rect.width);
        }

        protected abstract void OnInitialize();

        public void ShowView(TData data, ProfilerOverlayFeatureSettings settings)
        {
            bool isShow = settings.Enabled;
            _SetActive(isShow);

            if (!isShow)
            {
                return;
            }

            _textType = settings.TextType;
            _isShowGraphOnOverlay = settings.Graph;

            _graph.SetActiveChart(_IsShowGraph());

            UpdateView(data);
        }

        public abstract void UpdateView(TData data);

        protected abstract bool _IsShowGraph();

        protected virtual void SetWidth(float width, bool isForce = false)
        {

            if (!isForce && width <= _textAreaLayout.minWidth)
            {
                return;
            }

            _textAreaLayout.minWidth = width;

            _graphLayout.minWidth = width;
        }

        void _SetActive(bool isActive)
        {
            if (_rootObject.activeSelf != isActive)
            {
                _rootObject.SetActive(isActive);
            }
        }

        protected string _GetLabelText(string label)
        {
            return $"{label}:";
        }

        protected string _GetValueText(object value, bool isViewHyphen = false, bool isValid = true)
        {
            if (isViewHyphen)
            {
                return NoaDebuggerDefine.HyphenValue;
            }

            if (!isValid)
            {
                return NoaDebuggerDefine.MISSING_VALUE;
            }

            return $"<color=#{_textColorCode}>{value}</color>";
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _rootObject = default;
            _graph = default;
            _textColorCode = default;
            _textType = default;
            _isShowGraphOnOverlay = default;
        }
    }
}
