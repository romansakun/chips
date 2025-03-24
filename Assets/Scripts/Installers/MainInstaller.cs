using Factories;
using Gameplay.Battle;
using Gameplay.Chips;
using Managers;
using Model;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MainInstaller : MonoInstaller
    {
        [Inject] private PrefabsSettings _prefabsSettings;

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        public override void InstallBindings()
        {
            Container.Bind<AddressableManager>().AsSingle();
            Container.Bind<GuiManager>().AsSingle();
            Container.Bind<GameManager>().AsSingle();
            Container.Bind<BattleController>().AsSingle();

            Container.BindInterfacesAndSelfTo<ReloadManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<LocalizationManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<UserContextRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadingManager>().AsSingle();

            Container.Bind<ViewModelFactory>().AsSingle();
            Container.Bind<LogicBuilderFactory>().AsSingle();
            Container.BindMemoryPool<Chip, Chip.Pool>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(_prefabsSettings.ChipPrefab)
                .AsSingle();
        }

    }
}