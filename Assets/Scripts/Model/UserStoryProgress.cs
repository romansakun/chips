using System;
using System.Collections.Generic;

namespace Model
{
    [Serializable]
    public class UserStoryProgress
    {
        public List<int> FinishedStories = new List<int>();
    }
}