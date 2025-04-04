using Definitions;

namespace UI.SelectingFromAllowedChips
{
    public class IsStartSwipeQualifier : BaseSelectingFromAllowedChipsViewModelQualifier
    {
        protected override float Score(SelectingFromAllowedChipsViewModelContext context)
        {
            if (context.Input.Item1 != DragInputType.OnBeginDrag)
                return 0;

            context.StartSwipePosition = context.Input.Item2.position;
            context.Input = (DragInputType.None, default);
            return 1;
        }
    }
}