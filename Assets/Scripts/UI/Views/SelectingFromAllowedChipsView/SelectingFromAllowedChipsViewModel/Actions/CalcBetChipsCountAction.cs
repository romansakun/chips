using Definitions;
using Model;
using UnityEngine;
using Zenject;

namespace UI.SelectingFromAllowedChips
{
    public class CalcBetChipsCountAction : BaseSelectingFromAllowedChipsViewModelAction
    {
        [Inject] private GameDefs _gameDefs;
        [Inject] private UserContextRepository _userContext;

        protected override void Execute(SelectingFromAllowedChipsViewModelContext context)
        {
            var betCount = _gameDefs.SelectingChipsForBetSettings.MaxBetChipsCount;
            foreach (var player in context.Shared.Players)
            {
                if (player.Type == PlayerType.MyPlayer) 
                    continue;

                var playerContext = _userContext.GetNpcContext(player.Id);
                var allPlayerChipsCount = playerContext.GetAllChipsCount();
                betCount = Mathf.Min(betCount, allPlayerChipsCount);
            }
            betCount = Mathf.Min(betCount, _userContext.GetAllChipsCount());

            context.Shared.NeedBetChipsCount = betCount;
        }

    }
}