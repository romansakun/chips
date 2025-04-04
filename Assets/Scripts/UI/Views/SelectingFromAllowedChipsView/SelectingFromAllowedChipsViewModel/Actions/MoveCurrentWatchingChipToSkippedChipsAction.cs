using Model;
using Zenject;

namespace UI.SelectingFromAllowedChips
{
    public class MoveCurrentWatchingChipToSkippedChipsAction : BaseSelectingFromAllowedChipsViewModelAction
    {
        [Inject] UserContextRepository _userContext;
        
        protected override void Execute(SelectingFromAllowedChipsViewModelContext context)
        {
            context.LeftSideChips.Insert(0, context.CurrentWatchingChip);
            context.CurrentWatchingChip = default;
            if (context.RightSideChips.Count == 0)
                return;

            context.CurrentWatchingChip = context.RightSideChips[0];
            context.RightSideChips.RemoveAt(0);
        }
    }
}