namespace NoaDebugger
{
    sealed class NoaDebuggerHierarchySettings : NoaDebuggerSettingsBase
    {
        public NoaDebuggerHierarchySettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void Init()
        {
            _settings.HierarchyLevels = NoaDebuggerDefine.DEFAULT_HIERARCHY_LEVELS;
        }
    }
}
