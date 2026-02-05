using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NoaDebugger
{
    abstract class OverlaySettingsViewBase<TOverlayToolSettings, TOverlaySettingsViewLinker> : ViewBase<TOverlaySettingsViewLinker>
        where TOverlayToolSettings : OverlayToolSettingsBase, new()
        where TOverlaySettingsViewLinker : OverlaySettingsViewLinker<TOverlayToolSettings>
    {
        [SerializeField]
        BlockableScrollRect _scroll;

        [SerializeField]
        Button _resetButton;

        [SerializeField]
        GameObject _footerButtonsArea;

        [SerializeField]
        Button _saveButton;

        protected TOverlayToolSettings _overlayToolSettings;

        protected override void _Init()
        {
            Assert.IsNotNull(_scroll);
            Assert.IsNotNull(_resetButton);
            Assert.IsNotNull(_footerButtonsArea);
            Assert.IsNotNull(_saveButton);

            _resetButton.onClick.AddListener(_OnResetButton);
            _saveButton.onClick.AddListener(_OnSaveButton);
        }

        protected override void _OnShow(TOverlaySettingsViewLinker linker)
        {
            _overlayToolSettings = linker.Settings;

            gameObject.SetActive(true);

            SetActiveFooterArea();
        }

        protected override void _OnHide()
        {
            gameObject.SetActive(false);
        }

        public void AlignmentUI(bool isReverse)
        {
            Vector2 tmp = _scroll.content.pivot;
            tmp.y = isReverse ? 0 : 1;
            _scroll.content.pivot = tmp;

            GlobalCoroutine.Run(_WaitSetScrollPosition());
        }

        IEnumerator _WaitSetScrollPosition()
        {
            yield return null;

            _scroll.ResetScrollPosition();
        }

        void _OnResetButton()
        {
            _overlayToolSettings.Reset();
            Refresh();
            SetActiveFooterArea();
        }

        void _OnSaveButton()
        {
            _overlayToolSettings.Save();
            SetActiveFooterArea();
        }

        protected abstract void Refresh();

        public virtual void SetActiveFooterArea()
        {
            _SetActiveFooterArea(NoaDebuggerSettingsManager.HasUnsavedNoaDebuggerSettingsCache<TOverlayToolSettings>());
        }

        protected void _SetActiveFooterArea(bool isActive)
        {
            _footerButtonsArea.SetActive(isActive);
        }
    }

    class OverlaySettingsViewLinker<TOverlayToolSettings> : ViewLinkerBase
        where TOverlayToolSettings : OverlayToolSettingsBase, new()
    {
        public TOverlayToolSettings Settings { get; set; }
    }
}
