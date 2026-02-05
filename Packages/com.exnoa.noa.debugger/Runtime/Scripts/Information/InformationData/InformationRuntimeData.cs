namespace NoaDebugger
{
    sealed class InformationRuntimeData
    {
        public string _platform;
        public string _currentScene;
        public string _scenePlayTime;
        public string _appPlayTime;
        public IMutableParameter<float> _timeScale;
        public IMutableParameter<bool> _runInBackground;
    }
}
