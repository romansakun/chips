using Factories;
using Managers;
using UI;
using Zenject;

namespace Gameplay.Battle
{
    public class ShowAllowedPlayerChipsViewAction : BaseBattleLogicAction
    {
        [Inject] private GuiManager _guiManager;
        [Inject] private ViewModelFactory _viewModelFactory;

        protected override void Execute(BattleContext context)
        {
            var viewModel = _viewModelFactory.Create<SelectingFromAllowedChipsViewModel>();
            var view = _guiManager.ShowAsync<SelectingFromAllowedChipsView, SelectingFromAllowedChipsViewModel>(viewModel);
        }
    }
}