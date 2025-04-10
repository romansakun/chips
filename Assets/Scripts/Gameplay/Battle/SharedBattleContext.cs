using System.Collections.Generic;
using Definitions;

namespace Gameplay.Battle
{
    public class SharedBattleContext
    {
        public List<PlayerSharedContext> Players { get; } = new();
        public PlayerSharedContext FirstMovePlayer { get; set; }
        public int NeedBetChipsCount { get; set; } = 0;

        public void Reset()
        {
            FirstMovePlayer = null;
            Players.Clear();
        }
    }

    public class PlayerSharedContext
    {
        public string Id { get; set; }
        public PlayerType Type { get; set; }
        public PlayerType NextPlayerTypeInTurn { get; set; }
        public List<ChipDef> BetChips { get; set; } = new();
        public List<ChipDef> WinningChips { get; set; } = new();
    }
}