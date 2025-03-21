using System;
using System.Collections.Generic;

namespace Gameplay.Battle
{
    [Serializable]
    public class PlayerData
    {
        public string Id { get; set; }
        public PlayerType PlayerType { get; set; }
        //public bool IsHittingNow { get; set; }
        public List<string> BetChips { get; set; } = new List<string>();
        public List<string> WinningChips { get; set; } = new List<string>();

        //public PlayerRockPaperScissorsHand Hand { get; set; }
        public int MovementOrder { get; set; }
    }
}