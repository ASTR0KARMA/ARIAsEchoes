using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    abstract class ChartDrawerComponentBase : MonoBehaviour
    {
        [SerializeField]
        GameObject _rootObject;

        [SerializeField]
        protected StackedAreaChart _chart;

        void Awake()
        {
            Assert.IsNotNull(_rootObject);
            Assert.IsNotNull(_chart);

            OnInitialize();
        }

        protected abstract void OnInitialize();

        public void SetActiveChart(bool isActive)
        {
            if (_rootObject.activeSelf != isActive)
            {
                _rootObject.SetActive(isActive);
            }
        }

        void OnDestroy()
        {
            _rootObject = default;
            _chart = default;
        }
    }
}
