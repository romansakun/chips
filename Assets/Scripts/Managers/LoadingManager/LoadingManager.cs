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

        private readonly Queue<ILoadingItem> _loadingItems = new();

        public async void Initialize()
        {
            SetLoadingItems();

            var viewModel = _viewModelFactory.Create<LoadingViewModel>();
            var view = await _guiManager.ShowAsync<LoadingView, LoadingViewModel>(viewModel);
            await viewModel.LoadItems(_loadingItems);
            while (view.CanNotBeClosed())
            {
                await UniTask.Yield();
            }
            _guiManager.Close(view);
        }

        private void SetLoadingItems()
        {
            var gameDefsLoadingItem = _diContainer.Instantiate<GameDefsLoadingItem>();
            var userContextLoadingItem = _diContainer.Instantiate<UserContextLoadingItem>();
            var localizationLoading = _diContainer.Instantiate<LocalizationLoadingItem>();
            var mainMenuLoadingItem = _diContainer.Instantiate<MainMenuLoadingItem>();

            _loadingItems.Enqueue(gameDefsLoadingItem);
            _loadingItems.Enqueue(userContextLoadingItem);
            _loadingItems.Enqueue(localizationLoading);
            _loadingItems.Enqueue(mainMenuLoadingItem);
        }

    }
}