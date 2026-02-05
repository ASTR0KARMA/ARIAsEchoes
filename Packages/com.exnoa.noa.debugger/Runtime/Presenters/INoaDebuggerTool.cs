using UnityEngine;

namespace NoaDebugger
{
    interface IMenuInfo
    {
        string Name { get; }

        string MenuName { get; }

        int SortNo { get; }
    }

    enum ToolNotificationStatus
    {
        None = 0,

        Error = 1
    }

    interface INoaDebuggerTool
    {
        void Init();

        ToolNotificationStatus NotifyStatus { get; }

        IMenuInfo MenuInfo();

        void ShowView(Transform parent);

        void AlignmentUI(bool isReverse);

        void OnHidden();

        void OnToolDispose();
    }

    interface INoaDebuggerOverlayTool
    {
        bool GetPinStatus();

        void TogglePin(Transform parent);

        void InitOverlay(Transform parent);

        bool GetSettingsStatus();

        void ToggleOverlaySettings(Transform parent);
    }

    interface INoaDebuggerFloatingTool
    {
        bool GetPinStatus();

        void TogglePin(Transform parent);

        void InitFloatingWindow(Transform parent);
    }
}
