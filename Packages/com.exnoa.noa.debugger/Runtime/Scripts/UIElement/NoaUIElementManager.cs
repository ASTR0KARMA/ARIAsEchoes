using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class NoaUIElementManager : UIBehaviour
    {
        [SerializeField]
        RectTransform _applySafeAreaRect;
        [SerializeField]
        RectTransform _unApplySafeAreaRect;
        [SerializeField]
        RectTransform _uiElementRootPosition;
        [SerializeField]
        Transform _upperLeftPosition;
        [SerializeField]
        Transform _upperCenterPosition;
        [SerializeField]
        Transform _upperRightPosition;
        [SerializeField]
        Transform _middleLeftPosition;
        [SerializeField]
        Transform _middleCenterPosition;
        [SerializeField]
        Transform _middleRightPosition;
        [SerializeField]
        Transform _lowerLeftPosition;
        [SerializeField]
        Transform _lowerCenterPosition;
        [SerializeField]
        Transform _lowerRightPosition;

        [SerializeField]
        GameObject _textElementPrefab;
        [SerializeField]
        GameObject _buttonElementPrefab;

        Dictionary<AnchorType, bool> _doingAlignmentFlags = new Dictionary<AnchorType, bool>();

        public Transform RootTransform => _uiElementRootPosition;

        public INoaUIElementView[] RegisteredElements
        {
            get
            {
                if (_registeredElements == null)
                {
                    return Array.Empty<INoaUIElementView>();
                }

                return _registeredElements.Values.ToArray();
            }
        }
        Dictionary<string, INoaUIElementView> _registeredElements = new Dictionary<string, INoaUIElementView>();

        public void ResetRootRectSize()
        {
            bool appliesSafeArea = NoaDebuggerSettingsManager.GetNoaDebuggerSettings().AppliesUIElementSafeArea;
            RectTransform applyCanvas = appliesSafeArea ? _applySafeAreaRect : _unApplySafeAreaRect;
            Vector2 size = new Vector2(applyCanvas.rect.width, applyCanvas.rect.height);

            Vector2 padding = NoaDebuggerSettingsManager.GetNoaDebuggerSettings().UIElementPadding;
            size.x -= padding.x * 2;
            size.y -= padding.y * 2;

            _uiElementRootPosition.anchorMin = new Vector2(0.5f, 0.5f);
            _uiElementRootPosition.anchorMax = new Vector2(0.5f, 0.5f);
            _uiElementRootPosition.sizeDelta = size;
            _uiElementRootPosition.position = applyCanvas.position;
        }

        protected override void OnRectTransformDimensionsChange()
        {
            ResetRootRectSize();
        }

        void Update()
        {
            _CleanUpRegisteredElements();
        }

        void _CleanUpRegisteredElements()
        {
            var keysToRemove = new List<string>();

            foreach (var kvp in _registeredElements)
            {
                if (kvp.Value == null || kvp.Value.IsDisposed)
                {
                    keysToRemove.Add(kvp.Key);
                }
            }

            foreach (var key in keysToRemove)
            {
                _registeredElements.Remove(key);
            }
        }

        public void RegisterUIElement(INoaUIElement element)
        {
            if (element == null)
            {
                LogModel.LogWarning("Warning: Tried to register a null UI element.");
                return;
            }

            if (string.IsNullOrEmpty(element.Key))
            {
                LogModel.LogWarning("Warning: Tried to register a UI element with a null or empty key.");
                return;
            }

            if (_registeredElements.ContainsKey(element.Key))
            {
                var elementView = _registeredElements[element.Key];
                if (elementView.Element.GetType() == element.GetType())
                {
                    _InitializeUIElement(element, _registeredElements[element.Key]);
                }
                else
                {
                    UnregisterUIElement(element.Key);
                    _CreateUIElement(element);
                }
            }
            else
            {
                _CreateUIElement(element);
            }
        }

        public void UnregisterUIElement(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                LogModel.LogWarning("Warning: Tried to unregister a UI element with a null or empty key.");
                return;
            }

            if (!_registeredElements.ContainsKey(key))
            {
                LogModel.LogWarning($"Warning: Tried to unregister non-existing UI element with key '{key}'.");
                return;
            }

            var elementView = _registeredElements[key];

            if (elementView is { IsDisposed: false })
            {
                Destroy(elementView.GameObject);
            }

            _registeredElements.Remove(key);
        }

        public void UnregisterAllUIElements()
        {
            foreach (var elementView in _registeredElements.Values)
            {
                Destroy(elementView.GameObject);
            }
            _registeredElements.Clear();
        }

        public bool IsUIElementRegistered(string key = null)
        {
            if(_registeredElements == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(key))
            {
                return _registeredElements.Count > 0;
            }
            return _registeredElements.ContainsKey(key);
        }

        public void SetUIElementVisibility(string key, bool visible)
        {
            if (string.IsNullOrEmpty(key))
            {
                LogModel.LogWarning("Warning: Tried to set visibility for a UI element with a null or empty key.");
                return;
            }

            if (!_registeredElements.ContainsKey(key))
            {
                LogModel.LogWarning($"Warning: Tried to set visibility for non-existing UI element with key '{key}'.");
                return;
            }

            _registeredElements[key].GameObject.SetActive(visible);
        }

        public bool IsUIElementVisible(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            if (!_registeredElements.ContainsKey(key))
            {
                return false;
            }
            return _registeredElements[key].GameObject.activeSelf;
        }

        public void SetVerticalAlignment(AnchorType anchorType)
        {
            GlobalCoroutine.Run(_SetAlignment<VerticalLayoutGroup>(anchorType));
        }

        public void SetHorizontalAlignment(AnchorType anchorType)
        {
            GlobalCoroutine.Run(_SetAlignment<HorizontalLayoutGroup>(anchorType));
        }

        IEnumerator _SetAlignment<T>(AnchorType anchorType)
            where T : HorizontalOrVerticalLayoutGroup
        {
            Type verticalType = typeof(VerticalLayoutGroup);
            Type horizontalType = typeof(HorizontalLayoutGroup);

            if (typeof(T) != verticalType &&
                typeof(T) != horizontalType)
            {
                LogModel.LogWarning("Warning: Tried to set alignment with an invalid type.");
                yield break;
            }

            while(_doingAlignmentFlags.ContainsKey(anchorType) && _doingAlignmentFlags[anchorType] == true)
            {
                yield return null;
            }
            _doingAlignmentFlags[anchorType] = true;

            Transform anchorTransform = _GetAnchorTransform(anchorType);
            Type applyType = typeof(T);

            Type destroyType = applyType == verticalType ? horizontalType : verticalType;

            TextAnchor childAlignment = TextAnchor.UpperLeft;
            if(anchorTransform.TryGetComponent<HorizontalOrVerticalLayoutGroup>(out var original))
            {
                childAlignment = original.childAlignment;
            }

            if(anchorTransform.TryGetComponent(destroyType, out Component destroyTarget))
            {
                Destroy(destroyTarget);
                yield return null; 
            }

            if (!anchorTransform.TryGetComponent(applyType, out _))
            {
                var layoutGroup = anchorTransform.gameObject.AddComponent(applyType) as T;
                if (layoutGroup != null)
                {
                    layoutGroup.childAlignment = childAlignment;
                    layoutGroup.childControlWidth = true;
                    layoutGroup.childControlHeight = true;
                    layoutGroup.childForceExpandWidth = false;
                    layoutGroup.childForceExpandHeight = false;
                }
            }

            _doingAlignmentFlags[anchorType] = false;
        }

        void _CreateUIElement(INoaUIElement element)
        {
            var elementView = _CreatePrefabForElementView(element);
            _InitializeUIElement(element, elementView);

            _registeredElements.Add(element.Key, elementView);
        }

        INoaUIElementView _CreatePrefabForElementView(INoaUIElement element)
        {
            if (element is NoaUIObjectElement)
            {
                var obj = new GameObject(element.Key).AddComponent<NoaUIObjectElementView>();
                var layoutGroup = obj.gameObject.AddComponent<VerticalLayoutGroup>();
                layoutGroup.childControlWidth = true;
                layoutGroup.childControlHeight = true;
                layoutGroup.childForceExpandWidth = false;
                layoutGroup.childForceExpandHeight = false;

                return obj;
            }

            if (element is NoaUITextElement)
            {
                return Instantiate(_textElementPrefab).GetComponent<NoaUITextElementView>();
            }

            if (element is NoaUIButtonElement)
            {
                return Instantiate(_buttonElementPrefab).GetComponent<NoaUIButtonElementView>();
            }

            return null;
        }

        void _InitializeUIElement(INoaUIElement element, INoaUIElementView elementView)
        {
            if (elementView.GameObject.TryGetComponent(out NoaUIObjectElementView objectElementView))
            {
                objectElementView.Initialize((NoaUIObjectElement)element);
            }

            if (elementView.GameObject.TryGetComponent(out NoaUITextElementView textElementView))
            {
                textElementView.Initialize((NoaUITextElement)element);
            }

            if (elementView.GameObject.TryGetComponent(out NoaUIButtonElementView buttonElementView))
            {
                buttonElementView.Initialize((NoaUIButtonElement)element);
            }

            var anchorTransform = _GetAnchorTransform(element.AnchorType);

            if (element.Parent != null)
            {
                _SetParentLayoutComponents(element);
                elementView.GameObject.transform.SetParent(element.Parent, false);
            }
            else if (anchorTransform != null)
            {
                elementView.GameObject.transform.SetParent(anchorTransform, false);
            }
            else
            {
                elementView.GameObject.transform.SetParent(_uiElementRootPosition, false);
            }
        }

        void _SetParentLayoutComponents(INoaUIElement element)
        {
            if (!element.Parent.TryGetComponent(out VerticalLayoutGroup verticalLayoutGroup))
            {
                var layoutGroup = element.Parent.gameObject.AddComponent<VerticalLayoutGroup>();
                layoutGroup.childControlWidth = true;
                layoutGroup.childControlHeight = true;
                layoutGroup.childForceExpandWidth = false;
                layoutGroup.childForceExpandHeight = false;
            }
            if (!element.Parent.TryGetComponent(out ContentSizeFitter contentSizeFitter))
            {
                var sizeFitter = element.Parent.gameObject.AddComponent<ContentSizeFitter>();
                sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            }

        }

        Transform _GetAnchorTransform(AnchorType anchorType)
        {
            switch (anchorType)
            {
                case AnchorType.UpperLeft:
                    return _upperLeftPosition;
                case AnchorType.UpperCenter:
                    return _upperCenterPosition;
                case AnchorType.UpperRight:
                    return _upperRightPosition;
                case AnchorType.MiddleLeft:
                    return _middleLeftPosition;
                case AnchorType.MiddleCenter:
                    return _middleCenterPosition;
                case AnchorType.MiddleRight:
                    return _middleRightPosition;
                case AnchorType.LowerLeft:
                    return _lowerLeftPosition;
                case AnchorType.LowerCenter:
                    return _lowerCenterPosition;
                case AnchorType.LowerRight:
                    return _lowerRightPosition;
                default:
                    return null;
            }
        }

        protected override void OnDestroy()
        {
            foreach (var elementView in _registeredElements.Values)
            {
                if (elementView == null || elementView.IsDisposed)
                {
                    continue;
                }

                Destroy(elementView.GameObject);
            }
            _registeredElements.Clear();
            _registeredElements = null;
        }
    }
}
