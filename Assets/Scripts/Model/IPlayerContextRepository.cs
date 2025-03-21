using System;
using System.Collections.Generic;

namespace Model
{
    public interface IPlayerContextRepository
    {
        int GetBattleWinsCount();
        int GetBattleLosesCount();
        int GetHitChipsCount();
        int GetChipsCount(string chipId);
        void ForeachChipsCount(Action<KeyValuePair<string, int>> action);
        void ForeachFinishedStories(Action<int> action);

        void UpdateChipsCount(string chipId, int newCount);
        void UpdateBattleWinsCount(int battleWins);
        void UpdateBattleLosesCount(int battleLoses);
        void UpdateHitChipsCount(int hitChipsCount);
        void AddFinishedStory(int finishedStory);
    }
}