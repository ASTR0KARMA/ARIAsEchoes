using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class ParameterGroupViewFactory : MonoBehaviour
    {
        [SerializeField]
        Transform _contentRoot;

        [SerializeField]
        ParameterGroupView _groupView;

        [SerializeField]
        ReadOnlySettingsPanel _readOnlySettingsPanel;

        [SerializeField]
        BoolSettingsPanel _boolSettingsPanel;

        [SerializeField]
        IntSettingsPanel _intSettingsPanel;

        [SerializeField]
        FloatSettingsPanel _floatSettingsPanel;

        [SerializeField]
        EnumSettingsPanel _enumSettingsPanel;

        [SerializeField]
        StringSettingsPanel _stringSettingsPanel;

        internal ReadOnlySettingsPanel ReadOnlySettingsPanel => _readOnlySettingsPanel;
        internal BoolSettingsPanel BoolSettingsPanel => _boolSettingsPanel;
        internal IntSettingsPanel IntSettingsPanel => _intSettingsPanel;
        internal FloatSettingsPanel FloatSettingsPanel => _floatSettingsPanel;
        internal EnumSettingsPanel EnumSettingsPanel => _enumSettingsPanel;
        internal StringSettingsPanel StringSettingsPanel => _stringSettingsPanel;

        void OnValidate()
        {
            Assert.IsNotNull(_contentRoot);
            Assert.IsNotNull(_groupView);
            Assert.IsNotNull(_readOnlySettingsPanel);
            Assert.IsNotNull(_boolSettingsPanel);
            Assert.IsNotNull(_intSettingsPanel);
            Assert.IsNotNull(_floatSettingsPanel);
            Assert.IsNotNull(_enumSettingsPanel);
            Assert.IsNotNull(_stringSettingsPanel);
        }

        internal ParameterGroupView CreateGroupView(string groupName)
        {
            ParameterGroupView groupView = Instantiate(_groupView, _contentRoot);
            groupView.gameObject.SetActive(true);
            groupView.Header = groupName;

            return groupView;
        }
    }
}
