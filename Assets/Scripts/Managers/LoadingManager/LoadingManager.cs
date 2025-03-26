using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Factories;
using UI;
using Zenject;

namespace Managers
{
    public class LoadingManager : IInitializable
    {
        [Inject] private GuiManager _guiManager;
        [Inject] private DiContainer _diContainer;
        [Inject] private ViewModelFactory _viewModelFactory;

        public async void Initialize()
        {
            var viewModel = _viewModelFactory.Create<LoadingViewModel>();
            var view = await _guiManager.ShowAsync<LoadingView, LoadingViewModel>(viewModel);

            await viewModel.LoadItems(GetLoadingItems());
            while (view.CanNotBeClosed())
            {
                await UniTask.Yield();
            }
            _guiManager.Close(view);
        }

        private Queue<ILoadingItem> GetLoadingItems()
        {
            var gameDefsLoading = _diContainer.Instantiate<GameDefsLoadingItem>();
            var userContextLoading = _diContainer.Instantiate<UserContextLoadingItem>();
            var localizationLoading = _diContainer.Instantiate<LocalizationLoadingItem>();
            var mainMenuLoading = _diContainer.Instantiate<MainMenuLoadingItem>();

            var result = new Queue<ILoadingItem>();
            result.Enqueue(gameDefsLoading);
            result.Enqueue(userContextLoading);
            result.Enqueue(localizationLoading);
            result.Enqueue(mainMenuLoading);
            return result;
        }

    }
}