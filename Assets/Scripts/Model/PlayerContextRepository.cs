using System;
using System.Collections.Generic;
using System.IO;
using Definitions;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace Model
{
    public class PlayerContextRepository : IInitializable, IDisposable
    {
        public event Action OnPlayerContextChanged;

        [Inject] private GameDefs _gameDefs;

        private PlayerContext _playerContext;
        private string _playerContextPath;


        public void Initialize()
        {
            _playerContextPath = $"{Application.persistentDataPath}/PlayerContext.json";
            Debug.Log($"PlayerContextPath: {_playerContextPath}");
            if (TryLoadingPlayerContext()) 
                return;

            CreateNewPlayerContext();
            Save(false);
        }

        public int GetBattleWinsCount() => _playerContext.Stats.BattleWins;
        public int GetBattleLosesCount() => _playerContext.Stats.BattleLoses;
        public int GetHitChipsCount() => _playerContext.Stats.HitChipsCount;
        public int GetChipsCount(string chipId) => _playerContext.ChipsCount.GetValueOrDefault(chipId, 0);
        public void ForeachChipsCount(Action<KeyValuePair<string, int>> action) { foreach (var pair in _playerContext.ChipsCount) action(pair);}
        public void ForeachFinishedStories(Action<int> action) => _playerContext.StoryProgress.FinishedStories.ForEach(action);

        public void UpdateChipsCount(string chipId, int newCount) { _playerContext.ChipsCount[chipId] = newCount; Save(); }
        public void UpdateBattleWinsCount(int battleWins) { _playerContext.Stats.BattleWins = battleWins; Save(); }
        public void UpdateBattleLosesCount(int battleLoses) { _playerContext.Stats.BattleLoses = battleLoses; Save(); }
        public void UpdateHitChipsCount(int hitChipsCount) { _playerContext.Stats.HitChipsCount = hitChipsCount; Save(); }
        public void AddFinishedStory(int finishedStory) { _playerContext.StoryProgress.FinishedStories.Add(finishedStory); Save(); }

        private void Save(bool withChanged = true)
        {
            var json = JsonConvert.SerializeObject(_playerContext, Formatting.Indented);
            File.WriteAllText(_playerContextPath, json);

            if (withChanged && OnPlayerContextChanged != null)
            {
                OnPlayerContextChanged();
            }
        }

        private bool TryLoadingPlayerContext()
        {
            if (File.Exists(_playerContextPath) == false)
                return false;

            var json = File.ReadAllText(_playerContextPath);
            _playerContext = JsonConvert.DeserializeObject<PlayerContext>(json);
            return true;
        }

        private void CreateNewPlayerContext()
        {
            var json = JsonConvert.SerializeObject(_gameDefs.InitialPlayerContext);
            _playerContext = JsonConvert.DeserializeObject<PlayerContext>(json);
        }

        public void Dispose()
        {
            OnPlayerContextChanged = null;
        }

    }
}