using Gameplay;
using Installers;
using Zenject;

namespace UI
{
    public class FinishPlayerMoveAction : BaseGameplayViewModelAction
    {
        [Inject] private ColorsSettings _colorsSettings;
        [Inject] private GameplayObjectsHolder _gameplayObjects;

        protected override void Execute(GameplayViewModelContext context)
        {
            _gameplayObjects.AllowedScatterCircleSpriteRenderer.color = _colorsSettings.DefaultCircleColor;

            var nextPlayerType = context.HittingPlayer.NextPlayerTypeInTurn;
            context.HittingPlayer = context.Shared.Players.Find(p=>p.Type == nextPlayerType);
            context.IsHitSuccess.Value = false;
            context.IsHitFailed.Value = false;
        }
    }
}