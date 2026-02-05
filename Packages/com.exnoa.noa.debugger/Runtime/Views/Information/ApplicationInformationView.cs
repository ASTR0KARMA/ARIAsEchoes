using System.Linq;

namespace NoaDebugger
{
    sealed class ApplicationInformationView : InformationCategoryView<ApplicationInformationViewLinker>
    {
        protected override void _OnShow(ApplicationInformationViewLinker linker)
        {
            base._OnShow(linker);

            SetBuildGroup(linker._build, linker._isSelection);
            SetRuntimeGroup(linker._runtime, linker._isSelection);
            SetScreenGroup(linker._screen, linker._isSelection);
            SetGraphicsSettingsGroup(linker._graphicsSettings, linker._isSelection);
            SetLoggingGroup(linker._logging, linker._isSelection);
            SetOtherGroup(linker._other, linker._isSelection);
            gameObject.SetActive(true);
        }

        void SetBuildGroup(SelectableBuildData data, bool isSelection)
        {
            SetGroupCommon(data.GroupName, data, isSelection);
        }

        void SetRuntimeGroup(SelectableRuntimeData data, bool isSelection)
        {
            SetGroupCommon(data.GroupName, data, isSelection);
        }

        void SetScreenGroup(SelectableScreenData data, bool isSelection)
        {
            SetGroupCommon(data.GroupName, data, isSelection);
        }

        void SetGraphicsSettingsGroup(SelectableGraphicsSettingsData data, bool isSelection)
        {
            string groupName = data.GroupName;
            foreach (var info in data.Items)
            {
                var keyValue = info.KeyValuePair;
                if (keyValue.Key == "Render Scale (URP)")
                {
#if USE_PIPELINE_URP
                    SetFloat(groupName, keyValue.Key, keyValue.Value as IMutableParameter<float>);
#endif
                }
                else if (keyValue.Key == "Global Texture Mipmap Limit")
                {
#if UNITY_2022_2_OR_NEWER
                    SetInt(groupName, keyValue.Key, keyValue.Value as IMutableParameter<int>);
#endif
                }
                else
                {
                    SetValueByType(groupName, keyValue.Key, keyValue.Value);
                }

                var isSelect = isSelection && info.IsSelect;
                InitChildPanel(groupName, keyValue.Key, isSelect, isSelection);
            }

            bool isChildSelected = isSelection && data.Items.Any(item => item.IsSelect);
            InitGroupPanel(groupName, isChildSelected, isSelection);
        }

        void SetLoggingGroup(SelectableLoggingData data, bool isSelection)
        {
            SetGroupCommon(data.GroupName, data, isSelection);
        }

        void SetOtherGroup(SelectableOtherData data, bool isSelection)
        {
            SetGroupCommon(data.GroupName, data, isSelection);
        }
    }

    sealed class ApplicationInformationViewLinker : InformationCommonViewLinker
    {
        public SelectableBuildData _build;
        public SelectableRuntimeData _runtime;
        public SelectableScreenData _screen;
        public SelectableGraphicsSettingsData _graphicsSettings;
        public SelectableLoggingData _logging;
        public SelectableOtherData _other;
    }
}
