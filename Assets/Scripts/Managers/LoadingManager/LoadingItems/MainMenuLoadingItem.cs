using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Factories;
using Gameplay.Battle;
using Model;
using UI;
using Zenject;

namespace Managers
{
    public class MainMenuLoadingItem : ILoadingItem
    {
        [Inject] private GuiManager _guiManager;
        [Inject] private ViewModelFactory _viewModelFactory;
        [Inject] private BattleController _battleController;

        public UniTask Load()
        {
            // var viewModel = _viewModelFactory.Create<SelectingFromAllowedChipsViewModel>();
            // await _guiManager.ShowAsync<SelectingFromAllowedChipsView, SelectingFromAllowedChipsViewModel>(viewModel);
            //
            // while (viewModel.LogicAgent.IsExecuting)
            // {
            //     await UniTask.Yield();
            // }

            _battleController.ExecuteBattle(new List<string>() {"Kuno1", "Kuno2"}).Forget();
            return UniTask.CompletedTask;
        }
    }
}