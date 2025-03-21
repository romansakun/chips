using Gameplay.Battle;
using Model;
using UnityEngine;
using Zenject;

namespace UI
{
    public class TryFinishGameAction : BaseGameplayViewModelAction
    {
        [Inject] private PlayerContextRepository _playerContext;

        protected override void Execute(GameplayViewModelContext context)
        {
            if (context.HittingChipsAndDefs.Count != 0) 
                return;

            foreach (var player in context.Players)
            {
                var playerContextRepository = player.PlayerType == PlayerType.User 
                    ? (IPlayerContextRepository) _playerContext 
                    : _playerContext.GetNpcContext(player.Id);
                foreach (var chipId in player.BetChips)
                {
                    var chipCount = playerContextRepository.GetChipsCount(chipId);
                    playerContextRepository.UpdateChipsCount(chipId, Mathf.Clamp(chipCount - 1, 0, int.MaxValue));
                }
                foreach (var chipId in player.WinningChips)
                {
                    var chipCount = playerContextRepository.GetChipsCount(chipId);
                    playerContextRepository.UpdateChipsCount(chipId, Mathf.Clamp(chipCount + 1, 0, int.MaxValue));
                }
            }
            foreach (var player in context.Players)
            {
                player.BetChips.Clear();
                player.WinningChips.Clear();
            }

        }
    }
}