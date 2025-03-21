using System;
using System.Collections.Generic;

namespace Model
{
    [Serializable]
    public class PlayerContext
    {
        public Dictionary<string, int> ChipsCount = new Dictionary<string, int>();
        public PlayerStoryProgress StoryProgress { get; set; } = new PlayerStoryProgress();
        public PlayerStats Stats { get; set; } = new PlayerStats();
        public Dictionary<string, NpcContext> NpcContexts { get; set; } = new Dictionary<string, NpcContext>();
        public int RandomSeed { get; set; }
    }

    [Serializable]
    public class PlayerStoryProgress
    {
        public List<int> FinishedStories = new List<int>();
    }
}