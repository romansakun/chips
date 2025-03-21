using System;
using System.Collections.Generic;

namespace Model
{
    [Serializable]
    public class NpcContext 
    {
        public string DefId { get; set; }
        public Dictionary<string, int> ChipsCount = new Dictionary<string, int>();
        public PlayerStats Stats { get; set; } = new PlayerStats();
    }
}