using System;
using System.Collections.Generic;
using Definitions;
using Gameplay.Battle;

namespace Gameplay.Battle
{
    [Serializable]
    public class PlayerData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ChipDef> Chips { get; set; } = new List<ChipDef>();
        public List<ChipDef> SelectedChips { get; set; } = new List<ChipDef>();
        public PlayerRockPaperScissorsHand Hand { get; set; }
    }
}