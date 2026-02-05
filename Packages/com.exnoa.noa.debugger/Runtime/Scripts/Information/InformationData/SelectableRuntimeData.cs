namespace NoaDebugger
{
    sealed class SelectableRuntimeData : SelectableGroup<InformationRuntimeData>
    {
        const string GROUP_NAME = "Runtime";
        const string KEY_PLATFORM = "Platform";
        const string KEY_CURRENT_SCENE = "Current Scene";
        const string KEY_SCENE_PLAY_TIME = "Scene Play Time";
        const string KEY_APP_PLAY_TIME = "App Play Time";
        const string KEY_TIME_SCALE = "Time Scale";
        const string KEY_RUN_IN_BACKGROUND = "Run In Background";

        public override string GroupName => GROUP_NAME;

        public SelectableRuntimeData(InformationRuntimeData runtime)
        {
            AddItem(KEY_PLATFORM, runtime._platform);
            AddItem(KEY_CURRENT_SCENE, runtime._currentScene);
            AddItem(KEY_SCENE_PLAY_TIME, runtime._scenePlayTime);
            AddItem(KEY_APP_PLAY_TIME, runtime._appPlayTime);
            AddItem(KEY_TIME_SCALE, runtime._timeScale);
            AddItem(KEY_RUN_IN_BACKGROUND, runtime._runInBackground);
        }

        public override void Update(InformationRuntimeData runtime)
        {
            UpdateItem(KEY_PLATFORM, runtime._platform);
            UpdateItem(KEY_CURRENT_SCENE, runtime._currentScene);
            UpdateItem(KEY_SCENE_PLAY_TIME, runtime._scenePlayTime);
            UpdateItem(KEY_APP_PLAY_TIME, runtime._appPlayTime);
            UpdateItem(KEY_TIME_SCALE, runtime._timeScale);
            UpdateItem(KEY_RUN_IN_BACKGROUND, runtime._runInBackground);
        }
    }
}
