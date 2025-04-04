using System;
using System.Collections.Generic;

namespace Model
{
    [Serializable]
    public class UserContext
    {
        public Dictionary<string, int> ChipsCount = new();
        public UserStoryProgress StoryProgress { get; set; } = new();
        public UserStats Stats { get; set; } = new();
        public Dictionary<string, NpcContext> NpcContexts { get; set; } = new();
        public UserGameplayControl GameplayControl { get; set; } = new();
        public UserGameSettings GameSettings { get; set; } = new();
    }
}