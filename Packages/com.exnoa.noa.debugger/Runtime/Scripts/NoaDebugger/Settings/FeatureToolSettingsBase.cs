using System;
using UnityEngine;

namespace NoaDebugger
{
    [Serializable]
    abstract class FeatureToolSettingsBase : ToolSettingsBase
    {
        protected override string PlayerPrefsSuffix { get => NoaDebuggerPrefsDefine.PrefsKeySuffixFeatureSettings; }
    }
}
