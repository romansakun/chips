using Definitions;
using UnityEngine;

namespace UI.SelectingFromAllowedChips
{
    public class IsCurrentWatchingChipSkippedQualifier : BaseSelectingFromAllowedChipsViewModelQualifier
    {
        protected override float Score(SelectingFromAllowedChipsViewModelContext context)
        {
            var hasCurrentWatchingChip = context.CurrentWatchingChip != default;
            if (context.IsCurrentWatchingChipSkipped)
            {
                context.IsCurrentWatchingChipSkipped = false;
                return hasCurrentWatchingChip 
                    ? 1 
                    : 0;
            }

            var hasRightChips = context.RightSideChips.Count > 0;
            if (hasCurrentWatchingChip == false || hasRightChips == false)
                return 0;

            if (IsEndOfDrag(context, out var chip) == false) 
                return 0;

            if (context.CurrentWatchingChip.Item1 != chip)
                return 0;

            var endDragPosition = context.Input.Item2.position;
            var swipeDelta = endDragPosition - context.StartSwipePosition;
            if (swipeDelta.x < -50 && Mathf.Abs(swipeDelta.y) < 100)
            {
                context.Input = (DragInputType.None, default);
                return 1;
            }

            return 0;
        }
    }
}