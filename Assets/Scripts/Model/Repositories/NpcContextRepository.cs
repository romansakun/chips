using System;
using System.Collections.Generic;

namespace Model
{
    public class NpcContextRepository : IPlayerContextRepository, IDisposable
    {
        public event Action<string> OnNpcContextChanged;

        private readonly NpcContext _npcContext;

        public NpcContextRepository(NpcContext npcContext)
        {
            _npcContext = npcContext;
        }

        public int GetBattleWinsCount() => _npcContext.Stats.BattleWins;
        public int GetBattleLosesCount() => _npcContext.Stats.BattleLoses;
        public int GetHitChipsCount() => _npcContext.Stats.HitChipsCount;
        public int GetChipsCount(string chipId) => _npcContext.ChipsCount.GetValueOrDefault(chipId, 0);

        public int GetAllChipsCount()
        {
            var count = 0; 
            foreach (var pair in _npcContext.ChipsCount)
            {
                count += pair.Value;
            }
            return count;
        }

        public void ForeachChips(Action<KeyValuePair<string, int>> action)
        {
            foreach (var pair in _npcContext.ChipsCount)
            {
                action(pair);
            }
        }

        public void ForeachFinishedStories(Action<int> action) { }

        public void UpdateChipsCount(string chipId, int newCount) { _npcContext.ChipsCount[chipId] = newCount; Save(); }
        public void UpdateBattleWinsCount(int battleWins) { _npcContext.Stats.BattleWins = battleWins; Save(); }
        public void UpdateBattleLosesCount(int battleLoses) { _npcContext.Stats.BattleLoses = battleLoses; Save(); }
        public void UpdateHitChipsCount(int hitChipsCount) { _npcContext.Stats.HitChipsCount = hitChipsCount; Save(); }
        public void AddFinishedStory(int finishedStory) { }

        private void Save()
        {
            OnNpcContextChanged?.Invoke(_npcContext.Id);
        }

        public void Dispose()
        {
            OnNpcContextChanged = null;
        }

    }
}