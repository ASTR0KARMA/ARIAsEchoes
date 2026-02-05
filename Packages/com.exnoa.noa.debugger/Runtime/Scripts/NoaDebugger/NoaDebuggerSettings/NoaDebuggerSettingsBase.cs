namespace NoaDebugger
{
    abstract class NoaDebuggerSettingsBase
    {
        readonly protected NoaDebuggerSettings _settings;

        protected NoaDebuggerSettingsBase(NoaDebuggerSettings settings)
        {
            _settings = settings;
        }

        abstract public void Init();
    }
}
