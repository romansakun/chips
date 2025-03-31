using Definitions;
using Model;
using UnityEngine;
using Zenject;

namespace UI
{
    public class TryFinishGameAction : BaseGameplayViewModelAction
    {
        [Inject] private UserContextRepository _userContext;

        protected override void Execute(GameplayViewModelContext context)
        {
            if (context.HittingChipsAndDefs.Count != 0) 
                return;

            foreach (var player in context.Shared.Players)
            {
                var playerContextRepository = player.Type == PlayerType.MyPlayer 
                    ? (IPlayerContextRepository) _userContext 
                    : _userContext.GetNpcContext(player.Id);

                foreach (var chipDef in player.BetChips)
                {
                    var chipCount = playerContextRepository.GetChipsCount(chipDef.Id);
                    playerContextRepository.UpdateChipsCount(chipDef.Id, Mathf.Clamp(chipCount - 1, 0, int.MaxValue));
                }
                foreach (var chipDef in player.WinningChips)
                {
                    var chipCount = playerContextRepository.GetChipsCount(chipDef.Id);
                    playerContextRepository.UpdateChipsCount(chipDef.Id, Mathf.Clamp(chipCount + 1, 0, int.MaxValue));
                }
            }

            foreach (var player in context.Shared.Players)
            {
                player.BetChips.Clear();
                player.WinningChips.Clear();
            }

        }
    }
}