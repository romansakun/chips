using Cysharp.Threading.Tasks;
using Gameplay.Chips;
using Model;
using Zenject;

namespace Managers
{
    public class LoadingManager : IInitializable
    {
        [Inject] private PlayerContextRepository _playerContext;
        [Inject] private GameManager _gameManager;
        [Inject] private Chip.Factory _chipFactory;

        public async void Initialize()
        {
            await LoadAsync();

            //_playerContext.CreateNewPlayerContext();
            _gameManager.Start();
        }

        private async UniTask LoadAsync()
        {
            while (_chipFactory.Ready == false)
            {
                await UniTask.Yield();
            }
        }
    }
}