using Gameplay;
using Installers;
using Zenject;

namespace UI
{
    public class SetSuccessHitStateAction : BaseGameplayViewModelAction
    {
        [Inject] private ColorsSettings _colorsSettings;
        [Inject] private GameplayObjectsHolder _gameplayObjects;
        protected override void Execute(GameplayViewModelContext context)
        {
            _gameplayObjects.AllowedScatterCircleSpriteRenderer.color = _colorsSettings.SuccessHitCircleColor;

            context.IsHitSuccess.Value = true;
        }
    }
}