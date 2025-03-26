using Model;
using Zenject;

namespace UI
{
    public class MoveCurrentWatchingChipToBetAction : BaseSelectingFromAllowedChipsViewModelAction
    {
        [Inject] UserContextRepository _userContext;

        protected override void Execute(SelectingFromAllowedChipsViewModelContext context)
        {
            if (context.CurrentWatchingChip == default)
                return;

            context.BetSelectedChips.Insert(0, context.CurrentWatchingChip);
            context.BetChipsCount.Value = context.BetSelectedChips.Count;
            context.CurrentWatchingChip = default;
            if (context.RightSideChips.Count > 0)
            {
                context.CurrentWatchingChip = context.RightSideChips[0];
                context.RightSideChips.RemoveAt(0);
            } 
            else if (context.LeftSideChips.Count > 0)
            {
                context.CurrentWatchingChip = context.LeftSideChips[0];
                context.LeftSideChips.RemoveAt(0);
            }
        }
    }
}