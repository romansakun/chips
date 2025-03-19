using System.Collections.Generic;
using Definitions;
using LogicUtility;

namespace Gameplay.Battle
{
    public class BattleContext : IContext 
    {
        public BattleState State { get; set; }

        public List<PlayerData> Players { get; set; } = new List<PlayerData>();

        public List<int> PlayersOrder { get; set; } = new List<int>();

        public List<ChipDef> CurrentChipsStack { get; set; } = new List<ChipDef>();

        public void Dispose()
        {
            State = BattleState.Finished;
        }
    }
}