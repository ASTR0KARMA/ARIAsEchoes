using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace PauseSystem
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _pauseUI = null;
        [SerializeField] private float _fadeDuration = 0.1f;
        [SerializeField] private bool _resumeOutsideMain = true;
        [SerializeField] private SceneSO _mainMenuScene;
        [SerializeField] private GameObject _mainPauseMenu;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _quitButton;

        private bool _isPausing = false;
        private bool _isResuming = false;
        private float _fadeValue = 0;

        private IEnumerator _pauseCoroutine;
        private IEnumerator _resumeCoroutine;

        public void Awake()
        {
            if (_pauseUI.gameObject == gameObject)
                Debug.LogError(
                    "The canvas is on the same GameObject as the script. This is not recommended and will cause issues.");

            _pauseCoroutine = PauseCoroutine();
            _resumeCoroutine = ResumeCoroutine();
        }

        public void OnEnable()
        {
            _resumeButton.onClick.AddListener(OnResumeButtonClicked);
            _quitButton.onClick.AddListener(OnQuitButtonClicked);

            ShowMainPauseMenu();
            _pauseUI.gameObject.SetActive(false);

            if (!PauseManager.Instance.HasValue) return;

            PauseManager.Instance.Value.OnPause += OnPause;
            PauseManager.Instance.Value.OnResume += OnResume;
        }

        public void OnDisable()
        {
            _resumeButton.onClick.RemoveListener(OnResumeButtonClicked); 
            _quitButton.onClick.RemoveListener(OnQuitButtonClicked);

            if (!PauseManager.Instance.HasValue) return;

            PauseManager.Instance.Value.OnPause -= OnPause;
            PauseManager.Instance.Value.OnResume -= OnResume;
        }

        private void OnPause()
        {
            ShowMainPauseMenu();

            if (!PauseManager.Instance.HasValue) return;

            PauseManager.Instance.Value.ShouldUnpauseUsingAction = _resumeOutsideMain;

            if (_isResuming)
            {
                StopCoroutine(_resumeCoroutine);
                _isResuming = false;
            }

            _pauseCoroutine = PauseCoroutine();
            StartCoroutine(_pauseCoroutine);
        }

        private IEnumerator PauseCoroutine()
        {
            _isPausing = true;
            _pauseUI.alpha = 0;
            _pauseUI.gameObject.SetActive(true);

            while (_fadeValue < 1)
            {
                _fadeValue = Mathf.Min(1, _fadeValue + Time.unscaledDeltaTime / _fadeDuration);
                _pauseUI.alpha = Mathf.Clamp01(_fadeValue);
                yield return null;
            }

            _isPausing = false;
        }

        private void OnResume()
        {
            if (_isPausing)
            {
                StopCoroutine(_pauseCoroutine);
                _isPausing = false;
            }

            _resumeCoroutine = ResumeCoroutine();
            StartCoroutine(_resumeCoroutine);
        }

        private IEnumerator ResumeCoroutine()
        {
            _isResuming = true;
            _pauseUI.alpha = 1;
            _pauseUI.gameObject.SetActive(true);

            while (_fadeValue > 0)
            {
                _fadeValue = Mathf.Max(0, _fadeValue - Time.unscaledDeltaTime / _fadeDuration);
                _pauseUI.alpha = Mathf.Clamp01(_fadeValue);
                yield return null;
            }

            _pauseUI.gameObject.SetActive(false);
            _isResuming = false;
        }

        public void ShowMainPauseMenu()
        {
            _mainPauseMenu.SetActive(true);
        }


        private void OnResumeButtonClicked()
        {
            if (!PauseManager.Instance.HasValue) return;

            PauseManager.Instance.Value.Resume();
        }

        private void OnQuitButtonClicked()
        {
            if (!PauseManager.Instance.HasValue) return;

            PauseManager.Instance.Value.Resume();
            
            if (!SceneLoader.Instance.HasValue) return;

            SceneLoader.Instance.Value.LoadScene(_mainMenuScene);
        }
    }
}