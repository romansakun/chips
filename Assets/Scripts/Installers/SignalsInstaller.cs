using Definitions;
using Zenject;

namespace Installers
{
    public class SignalsInstaller : Installer<SignalsInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<PlayersBetChipsSetSignal>();
            Container.DeclareSignal<PlayersMoveOrderSetSignal>();
        }
    }
}