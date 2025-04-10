using Definitions;
using UnityEngine;

namespace UI.SelectingFromAllowedChips
{
    public class IsCurrentWatchingChipSelectedToBetQualifier : BaseSelectingFromAllowedChipsViewModelQualifier
    {
        protected override float Score(SelectingFromAllowedChipsViewModelContext context)
        {
            var canSelectBetChips = context.BetChipsCount.Value < context.NeedBetChipsCount.Value;
            var hasCurrentWatchingChip = context.CurrentWatchingChip != default;
            if (context.IsCurrentWatchingChipSelectedToBet)
            {
                context.IsCurrentWatchingChipSelectedToBet = false;
                return hasCurrentWatchingChip && canSelectBetChips
                    ? 1 
                    : 0;
            }

            if (hasCurrentWatchingChip == false || canSelectBetChips == false)
                return 0;

            if (IsEndOfDrag(context, out var chip) == false) 
                return 0;

            if (context.CurrentWatchingChip.Item1 != chip)
                return 0;

            var endDragPosition = context.Input.Item2.position;
            var swipeDelta = endDragPosition - context.StartSwipePosition;
            if (swipeDelta.y > 50 && Mathf.Abs(swipeDelta.x) < 100)
            {
                context.Input = (DragInputType.None, default);
                return 1;
            }

            return 0;
        }
    }
}