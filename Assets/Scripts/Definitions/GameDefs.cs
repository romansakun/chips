using System;
using System.Collections.Generic;

namespace Definitions
{
    [Serializable]
    public class GameDefs 
    {
        public InitialPlayerContext InitialPlayerContext { get; set; }
        public GameplaySettings GameplaySettings { get; set; }
        public Dictionary<string, ChipDef> Chips = new Dictionary<string, ChipDef>();
    }
}