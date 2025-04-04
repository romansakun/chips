using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.Battle;
using Zenject;

namespace Managers
{
    public class MainMenuLoadingItem : ILoadingItem
    {
        [Inject] private BattleController _battleController;

        public UniTask Load()
        {
            //todo rework it when main menu will be ready

            //for testing:
            _battleController.StartBattle(new List<string>() {"Kuno1", "Kuno2"});
            return UniTask.CompletedTask;
        }
    }
}