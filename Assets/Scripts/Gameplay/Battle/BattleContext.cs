using System.Collections.Generic;
using Definitions;
using LogicUtility;

namespace Gameplay.Battle
{
    public class BattleContext : IContext 
    {
        public BattleState State { get; set; }

        public SharedBattleContext Shared { get; set; }

        public bool IsPlayerBetChipsSet { get; set; }
        public string LeftPlayer { get; set; }
        public string RightPlayer { get; set; }

        //public List<ChipDef> CurrentChipsStack { get; } = new ();
        public List<ChipDef> PlayerBetChipDefs { get; } = new ();

        public bool IsDisposed { get; private set; }

        public void Reset()
        {
            State = BattleState.SelectingChipsForGame;
            //SharedContext = new BattleSharedContext();
            //CurrentChipsStack.Clear();
            PlayerBetChipDefs.Clear();
        }

        public void Dispose()
        {
            IsDisposed = true;
        }

    }
}