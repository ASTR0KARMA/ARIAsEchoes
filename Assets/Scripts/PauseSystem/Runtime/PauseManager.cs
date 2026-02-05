using System;
using System.Collections;
using Singleton;
using StateSystem;
using UnityEngine;
using UnityEngine.InputSystem;


namespace PauseSystem
{
    public class PauseManager : Singleton<PauseManager>
    {
        public Action OnPause;
        public Action OnResume;

        public bool ShouldUnpauseUsingAction = true;

        [SerializeField] private GameState _pauseState = null;
        [SerializeField] private GameState _playState = null;
        [SerializeField] private InputActionReference _pauseAction = null;

        public IEnumerator Start()
        {
            yield return null;

            _pauseAction.action.Enable();
            _pauseAction.action.performed += OnPauseAction;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            
            _pauseAction.action.Disable();
            _pauseAction.action.performed -= OnPauseAction;
        }

        private void OnPauseAction(InputAction.CallbackContext context)
        {
            if (GameStateMachine.Instance.HasValue && GameStateMachine.Instance.Value.IsCurrentState(_pauseState) && ShouldUnpauseUsingAction)
            {
                Resume();
            }
            else if (GameStateMachine.Instance.HasValue && GameStateMachine.Instance.Value.IsCurrentState(_playState))
            {
                Pause();
            }
        }

        public void Pause()
        {
            if(GameStateMachine.Instance.HasValue)
                GameStateMachine.Instance.Value.ChangeState(_pauseState);

            Time.timeScale = 0;
            OnPause?.Invoke();
        }

        public void Resume()
        {
            Time.timeScale = 1;
            OnResume?.Invoke();

            if(GameStateMachine.Instance.HasValue)
                GameStateMachine.Instance.Value.ChangeState(_playState);
        }
    }
}
