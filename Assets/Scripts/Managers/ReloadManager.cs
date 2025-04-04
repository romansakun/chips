using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Zenject;

namespace Managers
{
    public class ReloadManager : IInitializable, IDisposable
    {
        [Inject] private DiContainer _container;
        [Inject] private GuiManager _guiManager;
        [Inject] private GameManager _gameManager;
        [Inject] private AddressableManager _addressableManager;

        public void Initialize()
        {
#if WITH_CHEATS
            AddDebugCommands();
#endif
        }

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

        public void Dispose()
        {
#if WITH_CHEATS
            RemoveDebugCommands();
#endif
        }

#if WITH_CHEATS
        private void AddDebugCommands()
        {
            IngameDebugConsole.DebugLogConsole.AddCommand(nameof(ReloadGame).ToLower(), string.Empty, ReloadGame);
        }

        private void RemoveDebugCommands()
        {
            IngameDebugConsole.DebugLogConsole.RemoveCommand(ReloadGame);
        }
#endif
    }
}