using System;

namespace NoaDebugger
{
    sealed class InformationScreenData
    {
        public string _width;
        public string _height;
        public IMutableParameter<int> _targetFrameRate;
        public IMutableParameter<bool> _fullScreen;
        public IMutableParameter<Enum> _fullScreenMode;
        public IMutableParameter<Enum> _orientation;
    }
}
