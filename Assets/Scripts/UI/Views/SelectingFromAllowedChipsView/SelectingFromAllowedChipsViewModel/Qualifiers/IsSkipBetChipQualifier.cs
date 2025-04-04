using Definitions;

namespace UI.SelectingFromAllowedChips
{
    public class IsSkipBetChipQualifier : BaseSelectingFromAllowedChipsViewModelQualifier
    {
        protected override float Score(SelectingFromAllowedChipsViewModelContext context)
        {
            var hasCurrentWatchingChip = context.CurrentWatchingChip != default;
            var hasBetChips = context.BetSelectedChips.Count > 0;
            if (context.IsSkipBetChip)
            {
                context.IsSkipBetChip = false;
                return hasBetChips && hasCurrentWatchingChip
                    ? 1 
                    : 0;
            }

            if (hasBetChips == false || hasCurrentWatchingChip == false)
                return 0;

            if (IsEndOfDrag(context, out var chip) == false) 
                return 0;

            if (context.BetSelectedChips[0].Item1 != chip)
                return 0;

            var endDragPosition = context.Input.Item2.position;
            var swipeDelta = endDragPosition - context.StartSwipePosition;
            if (swipeDelta.x < -50 && swipeDelta.y < -50)
            {
                context.Input = (DragInputType.None, default);
                return 1;
            }

            return 0;
        }

    }
}