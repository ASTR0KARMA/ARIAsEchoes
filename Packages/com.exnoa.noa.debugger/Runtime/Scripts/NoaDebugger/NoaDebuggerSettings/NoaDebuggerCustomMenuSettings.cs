using System.Collections.Generic;

namespace NoaDebugger
{
    sealed class NoaDebuggerCustomMenuSettings : NoaDebuggerSettingsBase
    {
        public NoaDebuggerCustomMenuSettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void Init()
        {
            _settings.CustomMenuList = new List<CustomMenuInfo>() { };
        }
    }
}
