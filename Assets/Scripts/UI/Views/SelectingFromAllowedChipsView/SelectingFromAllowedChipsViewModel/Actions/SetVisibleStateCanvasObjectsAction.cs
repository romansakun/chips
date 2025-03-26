using Model;
using Zenject;

namespace UI
{
    public class SetVisibleStateCanvasObjectsAction : BaseSelectingFromAllowedChipsViewModelAction
    {
        [Inject] private UserContextRepository _userContext;

        protected override void Execute(SelectingFromAllowedChipsViewModelContext context)
        {
            var hasWatchingChip = context.CurrentWatchingChip != default;
            var watchingChipCount = hasWatchingChip 
                ? _userContext.GetChipsCount(context.CurrentWatchingChip.Item2.Id) 
                : 0;

            var hasBetChips = context.BetSelectedChips.Count > 0;
            var hasSkippedChips = context.LeftSideChips.Count > 0;
            var hasRightChips = context.RightSideChips.Count > 0;
            var canSelectBetChips = context.BetChipsCount.Value < context.NeedBetChipsCount.Value;

            context.ShowCurrentWatchingChipCount.Value = watchingChipCount > 1;
            context.ShowSelectWatchingChipToBetButton.Value = hasWatchingChip && canSelectBetChips;
            context.ShowSkipBetChipButton.Value = hasBetChips && hasWatchingChip;
            context.ShowSkipCurrentChipButton.Value = hasRightChips;
            context.ShowMoveSkippedToWatchingChipButton.Value = hasSkippedChips;
            context.ShowReadyButton.Value = context.BetChipsCount.Value == context.NeedBetChipsCount.Value;
        }
    }
}