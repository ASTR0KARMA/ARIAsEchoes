using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NoaDebugger
{
    class BlockableScrollRect : ScrollRect
    {
        public bool _canMoveScroll = true;

        protected virtual void _Init()
        {
            _canMoveScroll = true;
        }

        public void ResetScrollPosition()
        {
            normalizedPosition = new Vector2(0, 1);
        }


        public override void OnDrag(PointerEventData eventData)
        {
            if (!_canMoveScroll)
            {
                return;
            }

            base.OnDrag(eventData);
        }
    }
}
