#if NOA_DEBUGGER
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace NoaDebugger
{
    sealed class PackageEditorMenuSettings : PackageEditorSettingsBase
    {
        List<MenuInfo> _menuList;

        ReorderableList _reorderableMenuList;

        public PackageEditorMenuSettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void ResetTmpDataWithSettings()
        {
            _menuList = new List<MenuInfo>();
            foreach (var menuInfo in _settings.MenuList)
            {
                _menuList.Add(new MenuInfo()
                {
                    Name = menuInfo.Name,
                    Enabled = menuInfo.Enabled,
                    SortNo = menuInfo.SortNo
                });
            }

            void onDrawMenuElement(Rect rect, int index, bool isActive, bool isFocused)
            {
                MenuInfo data = _menuList[index];
                data.Enabled = EditorGUI.Toggle(rect, data.Name, data.Enabled);
            }

            _reorderableMenuList = new ReorderableList(_menuList, typeof(MenuInfo))
            {
                headerHeight = 0,
                footerHeight = 0,
                displayAdd = false,
                displayRemove = false,
                drawElementCallback = onDrawMenuElement,
                drawHeaderCallback = rect =>
                                     {
                                         EditorGUI.LabelField(rect, "");
                                     },
            };
        }

        public override void ApplySettings()
        {
            List<MenuInfo> infos = new List<MenuInfo>();
            for (int i = 0; i < _menuList.Count; i++)
            {
                var info = _menuList[i];

                infos.Add(
                    new MenuInfo()
                    {
                        Name = info.Name,
                        Enabled = info.Enabled,
                        SortNo = i
                    });
            }

            _settings.MenuList = infos;
        }

        public override void ResetDefault()
        {
            _menuList = NoaDebuggerMenuSettings.GetDefaultMenuSettings();
        }

        public override void DrawGUI()
        {
            _DisplaySettingsCategoryHeader("Menu", ResetDefault);
            _reorderableMenuList.DoLayoutList();
        }
    }
}
#endif
