namespace UI
{
    public class IsAllowedChipsNotCreatedQualifier : BaseSelectingFromAllowedChipsViewModelQualifier
    {
        protected override float Score(SelectingFromAllowedChipsViewModelContext context)
        {
            return context.AllPlayersChips.Count == 0 
                ? 1 
                : 0;
        }
    }
}