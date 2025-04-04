using System.Threading.Tasks;
using Factories;
using Managers;
using UI.RockPaperScissors;
using Zenject;

namespace Gameplay.Battle
{
    public class ShowRockPaperScissorsViewAction : BaseBattleLogicAction
    {
        [Inject] private GuiManager _guiManager;
        [Inject] private ViewModelFactory _viewModelFactory;
        
        public override async Task ExecuteAsync (BattleContext context)
        {
            var viewModel = _viewModelFactory.Create<RockPaperScissorsViewModel>();
            viewModel.SetSharedContext(context.Shared);
            await _guiManager.ShowAsync<RockPaperScissorsView, RockPaperScissorsViewModel>(viewModel);
        }
    }
}