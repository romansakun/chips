using Definitions;
using Gameplay;
using Installers;
using Zenject;

namespace UI.Gameplay
{
    public class FinishPlayerMoveAction : BaseGameplayViewModelAction
    {
        [Inject] private ColorsSettings _colorsSettings;
        [Inject] private GameplayObjectsHolder _gameplayObjects;

        protected override void Execute(GameplayViewModelContext context)
        {
            var nextPlayerType = context.HittingPlayer.NextPlayerTypeInTurn;
            context.HittingPlayer = context.Shared.Players.Find(p=> p.Type == nextPlayerType);
            context.IsPlayerCanHitNow.Value = context.HittingPlayer.Type == PlayerType.MyPlayer;
            context.IsHitSuccess.Value = false;
            context.IsHitFailed.Value = false;
        }
    }
}