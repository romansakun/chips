using Factories;
using Gameplay.Battle;
using Gameplay.Chips;
using Managers;
using Model;
using Zenject;

namespace Installers
{
    public class MainInstaller : MonoInstaller 
    {
        public override void InstallBindings()
        {
            Container.Bind<ReloadManager>().AsSingle();
            Container.Bind<AddressableManager>().AsSingle();
            Container.Bind<GuiManager>().AsSingle();
            Container.Bind<GameManager>().AsSingle();
            Container.Bind<BattleController>().AsSingle();

            Container.BindInterfacesAndSelfTo<LocalizationManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerContextRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadingManager>().AsSingle();

            //Factories:
            Container.Bind<ViewModelFactory>().AsSingle();
            Container.Bind<LogicBuilderFactory>().AsSingle();
            Container.BindFactory<Chip, Chip.Factory>();
        }
    }
}