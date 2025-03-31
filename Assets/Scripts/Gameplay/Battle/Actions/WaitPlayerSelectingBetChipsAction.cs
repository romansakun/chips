using System.Linq;
using System.Threading.Tasks;
using Definitions;
using Factories;
using Managers;
using Model;
using UI;
using UnityEngine;
using Zenject;

namespace Gameplay.Battle
{
    public class WaitPlayerSelectingBetChipsAction : BaseBattleLogicAction
    {
        [Inject] private GuiManager _guiManager;
        [Inject] private GameDefs _gameDefs;
        [Inject] private ViewModelFactory _viewModelFactory;
        [Inject] private UserContextRepository _userContext;

        public override async Task ExecuteAsync(BattleContext context)
        {
            var viewModel = _viewModelFactory.Create<SelectingFromAllowedChipsViewModel>();
            viewModel.SetSharedBattleContext(context.Shared);

            var view = await _guiManager.ShowAsync<SelectingFromAllowedChipsView, SelectingFromAllowedChipsViewModel>(viewModel);

            // var myPlayer = context.Players.Find(p => p.PlayerType == PlayerType.MyPlayer);
            // myPlayer.BetChips = context.PlayerBetChipDefs.Select(c => c.Id).ToList();
            
            
            //_guiManager.Close(view);
        }

    

    }
}