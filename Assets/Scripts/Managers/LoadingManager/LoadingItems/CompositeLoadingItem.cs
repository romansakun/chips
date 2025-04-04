using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Managers
{
    public class CompositeLoadingItem : ILoadingItem
    {
        private readonly IEnumerable<ILoadingItem> _loadingItems;
        private readonly List<UniTask> _loadingTasks = new();

        public CompositeLoadingItem(IEnumerable<ILoadingItem> loadingItems)
        {
            _loadingItems = loadingItems;
        }

        public async UniTask Load()
        {
            foreach (var loadingItem in _loadingItems)
            {
                _loadingTasks.Add(loadingItem.Load());
            }
            await UniTask.WhenAll(_loadingTasks);
        }
    }
}