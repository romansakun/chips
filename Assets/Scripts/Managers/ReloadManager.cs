using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Zenject;

namespace Managers
{
    public class ReloadManager
    {
        [Inject] private DiContainer _container;
        [Inject] private GuiManager _guiManager;
        [Inject] private GameManager _gameManager;
        [Inject] private AddressableManager _addressableManager;

        public async void ReloadGame()
        {
            _guiManager.Dispose();
            _gameManager.Dispose();
            _addressableManager.Dispose();
            _container.UnbindAll();

            var scene = SceneManager.GetActiveScene();
            Addressables.ResourceManager.CleanupSceneInstances(scene);
            await Resources.UnloadUnusedAssets();

            SceneManager.LoadScene(scene.name);
        }

    }
}