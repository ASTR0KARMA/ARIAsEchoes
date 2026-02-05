using System;
using Unity.Cinemachine;
using UnityEngine;
using UpdateSystem;

public class CinemachineCameraUpdater : MonoBehaviour
{
    [SerializeField] private CinemachineBrain _brain;
    [SerializeField] private UpdateSO _update;

    private void OnEnable()
    {
        _update.Register(OnUpdate);
    }

    private void OnDisable()
    {
        _update.Unregister(OnUpdate);
    }

    private void OnUpdate(float _)
    {
        _brain.ManualUpdate();
    }
}
