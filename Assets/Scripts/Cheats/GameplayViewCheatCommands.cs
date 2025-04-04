#if WITH_CHEATS

using IngameDebugConsole;
using Managers;
using Zenject;

namespace Cheats
{
    public class GameplayViewCheatCommands
    {
        //[Inject] private GuiManager _guiManager;

        public void ProcessCommands()
        {
            //DebugLogConsole.AddCommand(nameof(SetOrder).ToLower(), string.Empty, SetOrder);
        }

    }
}
#endif