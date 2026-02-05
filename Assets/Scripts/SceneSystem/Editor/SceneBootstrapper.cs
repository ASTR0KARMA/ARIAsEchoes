using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class SceneBootstrapper : Editor
{
    private static bool _enabled = false;
    
    [InitializeOnLoadMethod]
    public static void Initialize()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        EditorBuildSettings.sceneListChanged += OnSceneListChanged;
        
        _enabled = EditorSceneManager.playModeStartScene != null;
    }

    private static void OnSceneListChanged()
    {
        if (!_enabled) return;
        EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[0].path);
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (!_enabled) return;
    }
    
    [MenuItem("Scene Management/Enable Scene Bootstrapper")]
    public static void EnableBootstrapper()
    {
        _enabled = true;
        
        EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[0].path);
    }
    
    [MenuItem("Scene Management/Disable Scene Bootstrapper")]
    public static void DisableBootstrapper()
    {
        _enabled = false;
        
        EditorSceneManager.playModeStartScene = null;
    }
    
    [MenuItem("Scene Management/Enable Scene Bootstrapper", true)]
    public static bool ValidateEnableBootstrapper()
    {
        return !_enabled;
    }
    
    [MenuItem("Scene Management/Disable Scene Bootstrapper", true)]
    public static bool ValidateDisableBootstrapper()
    {
        return _enabled;
    }
}
