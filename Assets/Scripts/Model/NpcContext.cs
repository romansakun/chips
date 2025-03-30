using System;
using System.Collections.Generic;

namespace Model
{
    [Serializable]
    public class NpcContext 
    {
        public string Id { get; set; }
        public Dictionary<string, int> ChipsCount = new Dictionary<string, int>();
        public UserStats Stats { get; set; } = new UserStats();
    }
}