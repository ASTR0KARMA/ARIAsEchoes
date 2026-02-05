using System;
using UnityEngine;

namespace UpdateSystem
{
    [CreateAssetMenu(fileName = "TickUpdate", menuName = "Update System/Tick Update")]
    public class TickUpdateSO : UpdateSO
    {
        [SerializeField] private float _ticksPerSecond = 60;
        
        private float _accumulatedTime = 0.0f;
        private float _tickInterval;
        
        private void OnEnable()
        {
            _tickInterval = 1.0f / _ticksPerSecond;
        }

        public override void Invoke(float deltaTime)
        {
            _accumulatedTime += deltaTime;

            if (_accumulatedTime >= _tickInterval)
            {
                while (_accumulatedTime >= _tickInterval)
                    _accumulatedTime -= _tickInterval;
                
                base.Invoke(_tickInterval);
            }
            
        }
    }
}