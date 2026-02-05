using Singleton;
using UnityEngine;


namespace CursorSystem
{
    public class CursorManager : PersistentSingleton<CursorManager>
    {
        [SerializeField] private Texture2D _defaultCursorIcon;
        [SerializeField] private Vector2 _defaultHotspot = Vector2.zero;


        public override void Awake()
        {
            base.Awake();

            if (!Instance.HasValue || Instance.Value != this) return;

            ResetCursorIcon();

            SetCursorVisible(true);
            SetCursorLockMode(CursorLockMode.None);

        }

        public void SetCursorVisible(bool visible)
        {
            Cursor.visible = visible;
        }

        public void SetCursorLockMode(CursorLockMode mode)
        {
            Cursor.lockState = mode;
        }

        public void ResetCursorIcon()
        {
            SetCursor(_defaultCursorIcon, _defaultHotspot);
        }

        public void SetCursorIcon(Texture2D icon, Vector2 hotspot)
        {
            SetCursor(icon, hotspot);
        }

        private void SetCursor(Texture2D icon, Vector2 hotspot)
        {
            if (!icon) return;
            Cursor.SetCursor(icon, hotspot, CursorMode.Auto);
        }
    }
}