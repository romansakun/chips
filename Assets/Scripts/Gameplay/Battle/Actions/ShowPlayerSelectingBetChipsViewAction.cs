using System.Threading.Tasks;
using Factories;
using Managers;
using UI.SelectingFromAllowedChips;
using Zenject;

namespace Gameplay.Battle
{
    public class ShowPlayerSelectingBetChipsViewAction : BaseBattleLogicAction
    {
        [Inject] private GuiManager _guiManager;
        [Inject] private ViewModelFactory _viewModelFactory;

        public override async Task ExecuteAsync(BattleContext context)
        {
            var viewModel = _viewModelFactory.Create<SelectingFromAllowedChipsViewModel>();
            viewModel.SetSharedBattleContext(context.Shared);
            await _guiManager.ShowAsync<SelectingFromAllowedChipsView, SelectingFromAllowedChipsViewModel>(viewModel);
        }
    }
}