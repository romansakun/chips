using System.Threading.Tasks;
using Factories;
using Managers;
using Model;
using UI;
using Zenject;

namespace Gameplay.Battle
{
    public class WaitRockPaperScissorsViewAction : BaseBattleLogicAction
    {
        [Inject] private GuiManager _guiManager;
        [Inject] private ViewModelFactory _viewModelFactory;
        [Inject] private UserContextRepository _userContext;

        public override async Task ExecuteAsync (BattleContext context)
        {
            var viewModel = _viewModelFactory.Create<RockPaperScissorsViewModel>();
            viewModel.SetSharedContext(context.Shared);
            var view = await _guiManager.ShowAsync<RockPaperScissorsView, RockPaperScissorsViewModel>(viewModel);

            //_guiManager.Close(view);
        }
    }
}