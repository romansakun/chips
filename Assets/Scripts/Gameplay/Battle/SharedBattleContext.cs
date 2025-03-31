using System.Collections.Generic;
using Definitions;

namespace Gameplay.Battle
{
    public class SharedBattleContext
    {
        public PlayerSharedContext FirstMovePlayer { get; set; } = new();
        public List<PlayerSharedContext> Players { get; set; } = new();
        public int NeedBetChipsCount { get; set; } = 0;
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