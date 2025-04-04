using Factories;
using Managers;
using UI.Gameplay;
using Zenject;

namespace Gameplay.Battle
{
    public class ShowGameplayViewAction : BaseBattleLogicAction
    {
        [Inject] private ViewModelFactory _viewModelFactory;
        [Inject] private GuiManager _guiManager;

        protected override async void Execute(BattleContext context)
        {
            var viewModel = _viewModelFactory.Create<GameplayViewModel>();
            viewModel.SetSharedContext(context.Shared); 
            await _guiManager.ShowAsync<GameplayView, GameplayViewModel>(viewModel);
        }
    }
}