using UnityEngine;
using UnityEngine.UI;

namespace NoaDebugger
{
    [RequireComponent(typeof(ScrollRect))]
    sealed class ScrollRectHandler : MonoBehaviour
    {
        void Awake()
        {
            var scrollRect = GetComponent<ScrollRect>();

            if (scrollRect == null)
            {
                return;
            }

            scrollRect.scrollSensitivity = UnityInputUtils.ScrollSensitivity;
        }

    }
}
