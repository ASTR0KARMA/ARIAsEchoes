using System.Collections;
using System.Collections.Generic;
using CursorSystem;
using Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : PersistentSingleton<SceneLoader>
{
    [SerializeField] private List<SceneSO> _awakeScenes = new List<SceneSO>();
    
    public override void Awake()
    {
        base.Awake();
        
        foreach (SceneSO scene in _awakeScenes)
        {
            LoadScene(scene);
        }
    }
    
    public void LoadScene(SceneSO scene)
    {
        //if (!IsSceneNameValid(scene.SceneName)) throw new System.Exception($"Scene name {scene.SceneName} is not valid.");
        
        if (scene.IsAdditive)
        {
            LoadAdditiveScene(scene);
            
            
            foreach (SceneSO additionalScene in scene.AdditionalScenes)
            {
                if(!IsSceneNameValid(additionalScene.SceneName)) continue;
            
                LoadAdditiveScene(additionalScene);
            }
        }
        else
        {
            StartCoroutine(LoadSingleScene(scene));
        }
    }

    private IEnumerator LoadSingleScene(SceneSO scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene.SceneName);
        if (operation != null)
        {
            operation.allowSceneActivation = false;

            while (!operation.isDone)
            {
                if (operation.progress >= 0.9f)
                {
                    foreach (SceneSO additionalScene in scene.AdditionalScenes)
                    {
                        //if(!IsSceneNameValid(additionalScene.SceneName)) throw new System.Exception($"Scene name {additionalScene.SceneName} is not valid.");

                        LoadAdditiveScene(additionalScene);
                    }

                    operation.allowSceneActivation = true;

                    if (CursorManager.Instance.HasValue)
                    {
                        CursorManager.Instance.Value.SetCursorVisible(scene.IsCursorVisible);
                        CursorManager.Instance.Value.SetCursorLockMode(scene.CursorLockMode);
                        CursorManager.Instance.Value.ResetCursorIcon();
                    }

                    break;
                }

                yield return null;
            }
        }
    }

    private void LoadAdditiveScene(SceneSO scene)
    {
        SceneManager.LoadSceneAsync(scene.SceneName, LoadSceneMode.Additive);
    }

    private static bool IsSceneNameValid(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);

        return scene.IsValid();
    }
}
