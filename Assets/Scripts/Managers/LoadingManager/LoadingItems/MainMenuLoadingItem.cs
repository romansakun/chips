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

        public async UniTask Load()
        {
            var viewModel = _viewModelFactory.Create<SelectingFromAllowedChipsViewModel>();
            await _guiManager.ShowAsync<SelectingFromAllowedChipsView, SelectingFromAllowedChipsViewModel>(viewModel);

            while (viewModel.LogicAgent.IsExecuting)
            {
                await UniTask.Yield();
            }
        }
    }
}