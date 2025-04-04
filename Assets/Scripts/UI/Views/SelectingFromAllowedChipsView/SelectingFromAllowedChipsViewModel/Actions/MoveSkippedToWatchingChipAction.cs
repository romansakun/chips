namespace UI.SelectingFromAllowedChips
{
    public class MoveSkippedToWatchingChipAction : BaseSelectingFromAllowedChipsViewModelAction
    {
        protected override void Execute(SelectingFromAllowedChipsViewModelContext context)
        {
            if (context.CurrentWatchingChip != default)
            {
                context.RightSideChips.Insert(0, context.CurrentWatchingChip);
            }
            var chip = context.LeftSideChips[0];
            context.CurrentWatchingChip = chip;
            context.LeftSideChips.Remove(chip);
        }
    }
}