namespace UI
{
    public class MoveBetChipToSkippedChipsAction : BaseSelectingFromAllowedChipsViewModelAction
    {
        protected override void Execute(SelectingFromAllowedChipsViewModelContext context)
        {
            if (context.BetSelectedChips.Count == 0)
                return;

            var chip = context.BetSelectedChips[0];
            context.BetSelectedChips.RemoveAt(0);
            context.LeftSideChips.Insert(0, chip);
            context.BetChipsCount.Value = context.BetSelectedChips.Count;
        }
    }
}