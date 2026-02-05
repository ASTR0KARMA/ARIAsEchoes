using System;
using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;


namespace StateSystem
{
    public class GameStateMachine : Singleton<GameStateMachine>
    {
        [SerializeField] private List<GameState> _gameStates = new();

        private GameState _currentState;
        private bool _isInitialized = false;

        public IEnumerator Start()
        {
            _currentState = _gameStates[0];

            yield return null;

            _currentState.Enter();
            _isInitialized = true;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (_isInitialized) _currentState.Exit();
        }

        public void ChangeState(GameState state)
        {
            _currentState.Exit();
            _currentState = state;
            _currentState.Enter();
        }

        public bool IsCurrentState(GameState state)
        {
            return _currentState == state;
        }

        public GameState GetCurrentState()
        {
            return _currentState;
        }

    }
}