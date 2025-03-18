using System;
using System.Collections.Generic;
using Gameplay.Battle;

namespace Gameplay.Battle
{
    [Serializable]
    public class PlayerData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ChipData> Chips { get; set; } = new List<ChipData>();
        public List<ChipData> SelectedChips { get; set; } = new List<ChipData>();
        public PlayerRockPaperScissorsHand Hand { get; set; }
    }
}