using System.Threading.Tasks;
using Factories;
using Managers;
using Model;
using UI;
using UnityEngine;
using Zenject;

namespace Gameplay.Battle
{
    public class ShowRockPaperScissorsViewAction : BaseBattleLogicAction
    {
        [Inject] private GuiManager _guiManager;
        [Inject] private ViewModelFactory _viewModelFactory;
        [Inject] private UserContextRepository _userContext;

        public override async Task ExecuteAsync (BattleContext context)
        {
            Debug.Log(nameof(ShowRockPaperScissorsViewAction));
            
            var viewModel = _viewModelFactory.Create<RockPaperScissorsViewModel>();
            await _guiManager.ShowAsync<RockPaperScissorsView, RockPaperScissorsViewModel>(viewModel);
        }
    }
}