using Cysharp.Threading.Tasks;
using Gameplay.Chips;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class LoadingManager : IInitializable
    {
        [Inject] private GameManager _gameManager;
        [Inject] private Chip.Factory _chipFactory;

        public async void Initialize()
        {
            await LoadAsync();

            _gameManager.Start();
        }

        //todo сделать шкалу загрузки ?
        private async UniTask LoadAsync()
        {
            while (_chipFactory.Ready == false)
            {
                Debug.Log("ChipFactory not ready..");
                await UniTask.Yield();
            }

            Debug.Log("ChipFactory ready!");
        }
    }
}