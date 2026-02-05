using System;
using System.Collections.Generic;
using Singleton;
using UnityEngine;


namespace UpdateSystem
{
    [DefaultExecutionOrder(-10)]
    public class UpdateManager : Singleton<UpdateManager>
    {
        [SerializeField] private List<UpdateSO> _onUpdate = new();
        [SerializeField] private List<UpdateSO> _onFixedUpdate = new();
        [SerializeField] private List<UpdateSO> _onLateUpdate = new();
        [SerializeField] private List<UpdateSO> _onRenderUpdate = new();
        
        private void OnEnable()
        {
            Application.onBeforeRender += RenderUpdate;
        }

        private void Update()
        {
            foreach (UpdateSO updateSo in _onUpdate)
            {
                updateSo.Invoke(Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            foreach (UpdateSO updateSo in _onFixedUpdate)
            {
                updateSo.Invoke(Time.fixedDeltaTime);
            }
        }

        private void LateUpdate()
        {
            foreach (UpdateSO updateSo in _onLateUpdate)
            {
                updateSo.Invoke(Time.deltaTime);
            }
        }

        private void RenderUpdate()
        {
            foreach (UpdateSO updateSo in _onRenderUpdate)
            {
                updateSo.Invoke(Time.deltaTime);
            }
        }

        private void OnDisable()
        {
            Application.onBeforeRender -= RenderUpdate;
        }
    }
}