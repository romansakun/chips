using DG.Tweening;
using Factories;
using Gameplay.Battle;
using Managers;
using Model;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MainInstaller : MonoInstaller
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
            DOTween.SetTweensCapacity(1000,50);
        }

        public override void InstallBindings()
        {
            Container.Bind<AddressableManager>().AsSingle();
            Container.Bind<GuiManager>().AsSingle();
            Container.Bind<GameManager>().AsSingle();

            Container.BindInterfacesAndSelfTo<ReloadManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleController>().AsSingle();
            Container.BindInterfacesAndSelfTo<LocalizationManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<UserContextRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadingManager>().AsSingle();

            Container.Bind<ViewModelFactory>().AsSingle();
            Container.Bind<LogicBuilderFactory>().AsSingle();
        }

    }
}