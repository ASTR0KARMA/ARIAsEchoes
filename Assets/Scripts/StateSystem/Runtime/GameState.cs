using System;
using CursorSystem;
using UnityEngine;

namespace StateSystem
{
    [CreateAssetMenu(menuName = "State System/GameState", fileName = "GameState")]
    public class GameState : ScriptableObject
    {
        [SerializeField] public string Name = "";
        public Action OnEnter;
        public Action OnExit;
        public bool IsStateActive { get; private set; }

        public bool ChangeCursorSettings = false;
        public bool IsCursorVisible = true;
        public CursorLockMode CursorLockMode = CursorLockMode.None;
    
        public void Enter()
        {
            IsStateActive = true;

            if (ChangeCursorSettings && CursorManager.Instance.HasValue)
            {
                CursorManager.Instance.Value.SetCursorVisible(IsCursorVisible);
                CursorManager.Instance.Value.SetCursorLockMode(CursorLockMode);
            }
            
            OnEnter?.Invoke();
        }

        public void Exit()
        {
            IsStateActive = false;
            OnExit?.Invoke();
        }
    }
}