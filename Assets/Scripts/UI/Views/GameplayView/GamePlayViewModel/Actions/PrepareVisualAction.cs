using Definitions;
using Gameplay;
using Installers;
using Zenject;

namespace UI.Gameplay
{
    public class PrepareVisualAction : BaseGameplayViewModelAction
    {
        [Inject] private GameplayObjectsHolder _gameplayObjects;
        [Inject] private CameraController _cameraController;
        [Inject] private ColorsSettings _colorsSettings;


        protected override void Execute(GameplayViewModelContext context)
        {
            _gameplayObjects.AllowedScatterCircleSpriteRenderer.color = _colorsSettings.DefaultCircleColor;
            _cameraController.ResetPosition();
            context.IsPlayerCanHitNow.SetWithForceChangeInvoke(false);
            context.LeftNpcContext.Visible.Value = context.HittingPlayer.Type == PlayerType.LeftPlayer;
            context.RightNpcContext.Visible.Value = context.HittingPlayer.Type == PlayerType.RightPlayer;
        }

    }
}