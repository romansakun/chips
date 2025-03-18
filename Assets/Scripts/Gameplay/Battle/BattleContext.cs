using System.Collections.Generic;
using LogicUtility;

namespace Gameplay.Battle
{
    public class BattleContext : IContext 
    {
        public BattleState State { get; set; }

        public List<PlayerData> Players { get; set; } = new List<PlayerData>();

        public List<int> PlayersOrder { get; set; } = new List<int>();

        public List<ChipData> CurrentChipsStack { get; set; } = new List<ChipData>();

        public void Dispose()
        {
            State = BattleState.Finished;
        }
    }
}