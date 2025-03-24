using Definitions;

namespace UI
{
    public class IsStartSwipeQualifier : BaseSelectingFromAllowedChipsViewModelQualifier
    {
        protected override float Score(SelectingFromAllowedChipsViewModelContext context)
        {
            if (context.Input.Item1 != InputType.OnBeginDrag)
                return 0;

            context.StartSwipePosition = context.Input.Item2.position;
            context.Input = (InputType.None, default);
            return 1;
        }
    }
}