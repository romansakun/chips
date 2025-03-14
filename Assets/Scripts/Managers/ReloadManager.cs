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

        public void ReloadGame()
        {
            _addressableManager.Dispose();
            _guiManager.Dispose();
            _gameManager.Dispose();
            _container.UnbindAll();

            var sceneCount = SceneManager.sceneCount;
            for (var i = 1; i < sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                Addressables.ResourceManager.CleanupSceneInstances(scene);
                SceneManager.UnloadSceneAsync(scene);
            }
            var mainScene = SceneManager.GetSceneByBuildIndex(0);
            Addressables.ResourceManager.CleanupSceneInstances(mainScene);
            SceneManager.UnloadSceneAsync(mainScene);

            Resources.UnloadUnusedAssets();

            SceneManager.LoadScene(mainScene.name);
        }

    }
}