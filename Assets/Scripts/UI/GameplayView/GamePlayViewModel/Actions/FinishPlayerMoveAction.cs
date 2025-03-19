using System.Threading.Tasks;
using Gameplay;
using Installers;
using Zenject;

namespace UI
{
    public class FinishPlayerMoveAction : BaseGameplayViewModelAction
    {
        [Inject] private ColorsSettings _colorsSettings;
        [Inject] private GameplayObjectsHolder _gameplayObjects;

        public override async Task ExecuteAsync(GameplayViewModelContext context)
        {
            if (context.IsHitFailed.Value)
            {
                //переделать без task.delay (для webgl)
                await Task.Delay(500);
                if (context.IsDisposed)
                    return;
            }

            _gameplayObjects.AllowedScatterCircleSpriteRenderer.color = _colorsSettings.DefaultCircleColor;

            context.IsHitSuccess.Value = false;
            context.IsHitFailed.Value = false;
        }
    }
}