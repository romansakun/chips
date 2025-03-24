using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Managers;

namespace UI
{
    public class LoadingViewModel : ViewModel
    {
        public IReactiveProperty<int> LoadingProgress => _loadingProgress;
        private ReactiveProperty<int> _loadingProgress;

        private Queue<ILoadingItem> _loadingItems;

        public override void Initialize()
        {
            _loadingProgress = new ReactiveProperty<int>();
        }

        public async UniTask LoadItems(Queue<ILoadingItem> loadingItems)
        {
            _loadingItems = loadingItems;
            var itemsCount = _loadingItems.Count;
            var itemValue = 100 / itemsCount;
            
            foreach (var loadingItem in _loadingItems)
            {
                await loadingItem.Load();
                _loadingProgress.Value += itemValue;
            }
            _loadingProgress.Value = 100;
        }

    }
}