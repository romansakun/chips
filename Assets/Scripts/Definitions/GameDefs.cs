using System;
using System.Collections.Generic;

namespace Definitions
{
    [Serializable]
    public class GameDefs 
    {
        public Dictionary<string, NpcDef> Npc = new Dictionary<string, NpcDef>();
        public Dictionary<string, ChipDef> Chips = new Dictionary<string, ChipDef>();
        public Dictionary<string, LocalizationDef> Localizations = new Dictionary<string, LocalizationDef>();
        public InitialPlayerContext InitialPlayerContext { get; set; }
        public GameplaySettings GameplaySettings { get; set; }
        public PreparingHitSettings PreparingHitSettings { get; set; }
        public SelectingChipsForBetSettings SelectingChipsForBetSettings { get; set; }
        public RockPaperScissorsSettings RockPaperScissorsSettings { get; set; }
    }
}