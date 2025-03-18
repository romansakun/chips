using Gameplay.Chips;
using Installers;
using Zenject;

namespace UI
{
    public class SetFailHitStateAction : BaseGameplayViewModelAction
    {
        [Inject] private ColorsSettings _colorsSettings;
        [Inject] private GameplayObjectsHolder _gameplayObjects;
        protected override void Execute(GameplayViewModelContext context)
        {
            context.IsHitFailed.Value = true;
            _gameplayObjects.AllowedScatterCircleSpriteRenderer.color = _colorsSettings.FailedHitCircleColor;
        }
    }
}