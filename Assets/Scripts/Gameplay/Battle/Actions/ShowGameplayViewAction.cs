using Factories;
using Managers;
using UI;
using Zenject;

namespace Gameplay.Battle
{
    public class ShowGameplayViewAction : BaseBattleLogicAction
    {
        [Inject] private ViewModelFactory _viewModelFactory;
        [Inject] private GuiManager _guiManager;

        protected override void Execute(BattleContext context)
        {
            _guiManager.Close<GameplayView>();
            var viewModel = _viewModelFactory.Create<GameplayViewModel>();
            var view = _guiManager.ShowAsync<GameplayView, GameplayViewModel>(viewModel);
        }
    }
}