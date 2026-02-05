using UnityEngine;
using UnityEngine.Assertions;

namespace NoaDebugger
{
    sealed class ConsoleLogView : LogViewBase
    {

        [SerializeField, Header("Log details")]
        ConsoleLogDetailView _logDetail;

        protected override void _OnInit()
        {
            Assert.IsNotNull(_logDetail);
        }

        protected override void _OnUpdateDetail(ILogDetail detail)
        {
            if (detail is not ConsoleLogDetail log)
            {
                return;
            }

            _logDetail.SetLogDetailText(log.LogDetailStringForDisplay);
            _logDetail.SetCopyButton(OnCopy);
        }
    }
}
