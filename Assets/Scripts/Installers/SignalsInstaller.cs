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
            Container.DeclareSignal<ShowViewSignal>();
            Container.DeclareSignal<CloseViewSignal>();
            Container.DeclareSignal<LogicAgentCreatedSignal>();
            Container.DeclareSignal<LogicAgentDisposedSignal>();
        }
    }

}