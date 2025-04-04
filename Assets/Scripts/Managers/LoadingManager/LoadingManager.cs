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
            var result = new Queue<ILoadingItem>();
            AddLoadingItem<BindingChipsPoolLoadingItem>(result);
            AddLoadingItem<GameDefsLoadingItem>(result);
            AddLoadingItem<UserContextLoadingItem>(result);
            AddLoadingItem<LocalizationLoadingItem>(result);
            AddLoadingItem<MainMenuLoadingItem>(result);
            return result;
        }

        private void AddLoadingItem<T>(Queue<ILoadingItem> loadingItems) where T : ILoadingItem
        {
            var loadingItem = _diContainer.Instantiate<T>();
            loadingItems.Enqueue(loadingItem);
        }

    }
}