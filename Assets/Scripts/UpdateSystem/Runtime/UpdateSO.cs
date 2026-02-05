using System;
using System.Collections.Generic;
using StateSystem;
using UnityEngine;

namespace UpdateSystem
{
    [CreateAssetMenu(fileName = "Update", menuName = "Update System/Update")]
    public class UpdateSO : ScriptableObject
    {
        [SerializeField] private List<GameState> _updateCondition = new ();
        private Action<float> _onUpdate;

        public void Register(Action<float> action)
        {
            _onUpdate += action;
        }

        public void Unregister(Action<float> action)
        {
            _onUpdate -= action;
        }

        public virtual void Invoke(float deltaTime)
        {
            if (_updateCondition.Count == 0)
            {
                _onUpdate?.Invoke(deltaTime);
                return;
            }
            
            foreach (GameState state in _updateCondition)
            {
                if (state.IsStateActive)
                {
                    _onUpdate?.Invoke(deltaTime);
                    break;
                }
            }
        }
    }
}