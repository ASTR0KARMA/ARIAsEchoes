using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneSO", menuName = "Scriptable Objects/SceneSO")]
public class SceneSO : ScriptableObject
{
    public string SceneName;
    public List<SceneSO> AdditionalScenes = new List<SceneSO>();
    
    public bool IsAdditive;
    public bool IsCursorVisible = true;
    public CursorLockMode CursorLockMode = CursorLockMode.None;
}
