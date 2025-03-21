using System;

namespace Model
{
    [Serializable]
    public class PlayerStats
    {
        public int BattleWins { get; set; }
        public int BattleLoses { get; set; }
        public int HitChipsCount { get; set; }
    }
}