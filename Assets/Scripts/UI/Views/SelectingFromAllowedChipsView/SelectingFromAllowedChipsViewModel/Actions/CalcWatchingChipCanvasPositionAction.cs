using Definitions;
using Gameplay;
using Managers;
using Zenject;

namespace UI.SelectingFromAllowedChips
{
    public class CalcWatchingChipCanvasPositionAction : BaseSelectingFromAllowedChipsViewModelAction
    {
        [Inject] private CameraController _cameraController;
        [Inject] private GuiManager _guiManager;
        [Inject] private GameDefs _gameDefs;

        protected override void Execute(SelectingFromAllowedChipsViewModelContext context)
        {
            var currentChipTransform = context.CurrentWatchingChip.Item1.Facade.Transform;
            var currentChipTransformScreenPoint = _cameraController.Camera.WorldToScreenPoint(currentChipTransform.position);
            var betChipScreenPoint = _cameraController.Camera.WorldToScreenPoint(_gameDefs.SelectingChipsForBetSettings.SelectedChipsBetPosition);
            context.CurrentWatchingChipCanvasPosition.Value = _guiManager.ScreenPointToLocalPoint(currentChipTransformScreenPoint);
            context.BetChipCanvasPosition.Value = _guiManager.ScreenPointToLocalPoint(betChipScreenPoint);
        }
    }
}