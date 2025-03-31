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
            var viewModel = _viewModelFactory.Create<GameplayViewModel>()
                .SetSharedContext(context.Shared);
            
            var view = _guiManager.ShowAsync<GameplayView, GameplayViewModel>(viewModel);
        }
    }
}