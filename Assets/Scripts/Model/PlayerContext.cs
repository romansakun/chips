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
    }

    [Serializable]
    public class PlayerStats
    {
        public int BattleWins { get; set; }
        public int BattleLoses { get; set; }
        public int HitChipsCount { get; set; }
    }  

    [Serializable]
    public class PlayerStoryProgress
    {
        public List<int> FinishedStories = new List<int>();
    }
}