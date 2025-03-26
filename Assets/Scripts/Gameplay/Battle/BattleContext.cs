using System.Collections.Generic;
using Definitions;
using LogicUtility;

namespace Gameplay.Battle
{
    public class BattleContext : IContext 
    {
        public BattleState State { get; set; }

        public List<string> Players { get; } = new List<string>();

        public List<int> PlayersOrder { get; } = new List<int>();

        public List<ChipDef> CurrentChipsStack { get; } = new List<ChipDef>();
        public int NeedBetChipsCount { get; set; }
        public List<ChipDef> PlayerBetChipDefs { get; } = new List<ChipDef>();

        public bool IsDisposed { get; private set; }

        public void Reset()
        {
            State = BattleState.SelectingChipsForGame;
            Players.Clear();
            PlayersOrder.Clear();
            CurrentChipsStack.Clear();
            PlayerBetChipDefs.Clear();
            NeedBetChipsCount = 0;
            IsDisposed = false;
        }

        public void Dispose()
        {
            IsDisposed = true;
        }

    }
}