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

            //var hittingPlayer = context.Players[context.HittingPlayerIndex];

            context.HittingPlayerIndex = (context.HittingPlayerIndex + 1) % context.Players.Count;
            context.IsHitSuccess.Value = false;
            context.IsHitFailed.Value = false;
        }
    }
}