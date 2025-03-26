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
            CalcBetChipsCount(context);

            var viewModel = _viewModelFactory.Create<SelectingFromAllowedChipsViewModel>();
            var view = await _guiManager.ShowAsync<SelectingFromAllowedChipsView, SelectingFromAllowedChipsViewModel>(viewModel);
            while (context.PlayerBetChipDefs.Count < context.NeedBetChipsCount)
            {
                await Task.Yield();
            }

            _guiManager.Close(view);
        }

        private void CalcBetChipsCount(BattleContext context)
        {
            var betCount = _gameDefs.SelectingChipsForBetSettings.MaxBetChipsCount;
            foreach (var player in context.Players)
            {
                var playerContext = _userContext.GetNpcContext(player);
                var allPlayerChipsCount = playerContext.GetAllChipsCount();
                betCount = Mathf.Min(betCount, allPlayerChipsCount);
            }
            betCount = Mathf.Min(betCount, _userContext.GetAllChipsCount());
            context.NeedBetChipsCount = betCount;
        }

    }
}