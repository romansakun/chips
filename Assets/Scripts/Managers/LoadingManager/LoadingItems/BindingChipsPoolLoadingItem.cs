using Cysharp.Threading.Tasks;
using Gameplay.Chips;
using Zenject;

namespace Managers
{
    public class BindingChipsPoolLoadingItem : ILoadingItem
    {
        [Inject] private DiContainer _diContainer;
        [Inject] private AddressableManager _addressableManager;

        public async UniTask Load()
        {
            var chipPrefab = await _addressableManager.LoadPrefabAsync<Chip>();
            _diContainer.BindMemoryPool<Chip, Chip.Pool>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(chipPrefab)
                .AsSingle();

            await UniTask.Yield();
        }
    }
}