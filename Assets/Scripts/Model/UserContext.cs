using System;
using System.Collections.Generic;

namespace Model
{
    [Serializable]
    public class UserContext
    {
        public Dictionary<string, int> ChipsCount = new Dictionary<string, int>();
        public UserStoryProgress StoryProgress { get; set; } = new UserStoryProgress();
        public UserStats Stats { get; set; } = new UserStats();
        public Dictionary<string, NpcContext> NpcContexts { get; set; } = new Dictionary<string, NpcContext>();
        public UserGameSettings GameSettings { get; set; } = new UserGameSettings();
        public int RandomSeed { get; set; }
    }
}