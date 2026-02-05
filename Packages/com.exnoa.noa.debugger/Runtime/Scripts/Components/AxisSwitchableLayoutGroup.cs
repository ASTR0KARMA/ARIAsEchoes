using UnityEngine.UI;

namespace NoaDebugger
{
    sealed class AxisSwitchableLayoutGroup : HorizontalOrVerticalLayoutGroup
    {
        public enum AxisType
        {
            Horizontal = 0,
            Vertical = 1,
        }

        public AxisType Axis { get; set; } = AxisType.Vertical;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            switch (Axis)
            {
                case AxisType.Horizontal:
                    CalcAlongAxis(0, false);
                    break;
                case AxisType.Vertical:
                    CalcAlongAxis(0, true);
                    break;
            }
        }

        public override void CalculateLayoutInputVertical()
        {
            switch (Axis)
            {
                case AxisType.Horizontal:
                    CalcAlongAxis(1, false);
                    break;
                case AxisType.Vertical:
                    CalcAlongAxis(1, true);
                    break;
            }
        }

        public override void SetLayoutHorizontal()
        {
            switch (Axis)
            {
                case AxisType.Horizontal:
                    SetChildrenAlongAxis(0, false);
                    break;
                case AxisType.Vertical:
                    SetChildrenAlongAxis(0, true);
                    break;
            }
        }

        public override void SetLayoutVertical()
        {
            switch (Axis)
            {
                case AxisType.Horizontal:
                    SetChildrenAlongAxis(1, false);
                    break;
                case AxisType.Vertical:
                    SetChildrenAlongAxis(1, true);
                    break;
            }
        }
    }
}
