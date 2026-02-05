using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NoaDebugger
{
    sealed class NoaDebuggerMenuSettings : NoaDebuggerSettingsBase
    {
        public NoaDebuggerMenuSettings(NoaDebuggerSettings settings) : base(settings) { }

        public override void Init()
        {
            _settings.MenuList = GetDefaultMenuSettings();
        }

        public static List<MenuInfo> GetDefaultMenuSettings()
        {
            var menuList = new List<MenuInfo>() { };
            List<IMenuInfo> infos = NoaDebuggerMenuSettings.GetSortedIMenuInfoList();

            foreach (IMenuInfo info in infos)
            {
                if (info.GetType().Name == nameof(ToolDetailPresenter.ToolDetailMenuInfo) || info.GetType().Name == nameof(SettingsPresenter.SettingsMenuInfo))
                {
                    continue;
                }

                var menuInfo = new MenuInfo()
                {
                    Name = info.MenuName,
                    Enabled = true,
                    SortNo = info.SortNo
                };

                menuList.Add(menuInfo);
            }

            return menuList;
        }

        static List<IMenuInfo> GetSortedIMenuInfoList()
        {
            List<IMenuInfo> infos = new List<IMenuInfo>();

            if (Application.isPlaying)
            {
                List<INoaDebuggerTool> tools = NoaDebugger.AllPresenters().ToList();
                infos.AddRange(tools.Select(t => t.MenuInfo()));
            }
            else
            {
                infos = AssemblyUtils.CreateInterfaceInstances<IMenuInfo>().ToList();

                infos.RemoveAll(m => m.GetType().Name == nameof(NoaCustomMenuBase.CustomMenuInfo));
            }

            infos.Sort((a, b) => a.SortNo - b.SortNo);

            return infos;
        }

        public static List<MenuInfo> GetUpdatedMenuSettings(List<MenuInfo> baseList)
        {
            List<IMenuInfo> defaultInfos = NoaDebuggerMenuSettings.GetSortedIMenuInfoList();
            List<MenuInfo> updateInfos = new List<MenuInfo>();

            foreach (MenuInfo menu in baseList)
            {
                var updateInfo = updateInfos.FirstOrDefault(updateInfo => updateInfo.Name.Equals(menu.Name));
                var defaultInfo = defaultInfos.FirstOrDefault(defaultInfo => defaultInfo.MenuName.Equals(menu.Name));

                if (updateInfo != null || defaultInfo == null)
                {
                    continue;
                }

                var menuInfo = new MenuInfo
                {
                    Name = menu.Name,
                    Enabled = menu.Enabled,
                };

                updateInfos.Add(menuInfo);
            }

            foreach (IMenuInfo defaultInfo in defaultInfos)
            {
                if (defaultInfo.GetType().Name == nameof(ToolDetailPresenter.ToolDetailMenuInfo))
                {
                    continue;
                }
                if (defaultInfo.GetType().Name == nameof(SettingsPresenter.SettingsMenuInfo))
                {
                    continue;
                }

                MenuInfo customMenuInfo =
                    baseList.FirstOrDefault(customMenuInfo => customMenuInfo.Name.Equals(defaultInfo.MenuName));

                if (customMenuInfo != null)
                {
                    continue;
                }

                var menuInfo = new MenuInfo
                {
                    Name = defaultInfo.MenuName,
                    Enabled = true,
                };

                updateInfos.Insert(defaultInfo.SortNo, menuInfo);
            }

            var sortNo = 0;

            foreach (MenuInfo updateInfo in updateInfos)
            {
                updateInfo.SortNo = sortNo;
                sortNo++;
            }

            return updateInfos;
        }
    }
}
