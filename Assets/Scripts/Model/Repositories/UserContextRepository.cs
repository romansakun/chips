using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Definitions;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace Model
{
    public class UserContextRepository : IPlayerContextRepository, IInitializable, IDisposable
    {
        public event Action OnPlayerContextChanged;
        public event Action<int> OnBattleWinsCountChanged;

        [Inject] private GameDefs _gameDefs;

        private UserContext _userContext;
        private Dictionary<string, NpcContextRepository> _npcContextRepositories;
        private Random _playerRandom;
        private string _playerContextPath;
        private bool _willSave = false;

        public void Initialize()
        {
            _playerContextPath = $"{Application.persistentDataPath}/PlayerContext.json";
        }

        public int RandomSeed() => _userContext.RandomSeed;
        public float RandomRange(float min, float max) => _playerRandom.Next((int)(min * 10000), (int)(max * 10000)) / 10000f;
        public int GetRandomRange(int min, int max) => _playerRandom.Next(min, max);
        public string GetLocalizationLanguage() => _userContext.GameSettings.Language;
        public int GetBattleWinsCount() => _userContext.Stats.BattleWins;
        public int GetBattleLosesCount() => _userContext.Stats.BattleLoses;
        public int GetHitChipsCount() => _userContext.Stats.HitChipsCount;
        public int GetChipsCount(string chipId) => _userContext.ChipsCount.GetValueOrDefault(chipId, 0);
        public void ForeachFinishedStories(Action<int> action) => _userContext.StoryProgress.FinishedStories.ForEach(action);

        public NpcContextRepository GetNpcContext(string npcId) => _npcContextRepositories.GetValueOrDefault(npcId, null);

        public int GetAllChipsCount() 
        { 
            var count = 0; 
            foreach (var pair in _userContext.ChipsCount) count += pair.Value; 
            return count;
        }

        public void ForeachChips(Action<KeyValuePair<string, int>> action)
        {
            foreach (var pair in _userContext.ChipsCount)
            {
                action(pair);
            }
        }

        public void UpdateChipsCount(string chipId, int newCount) { _userContext.ChipsCount[chipId] = newCount; Save(); }
        public void UpdateBattleWinsCount(int battleWins)
        {
            _userContext.Stats.BattleWins = battleWins;
            OnBattleWinsCountChanged?.Invoke(battleWins);
            Save();
        }
        public void UpdateBattleLosesCount(int battleLoses) { _userContext.Stats.BattleLoses = battleLoses; Save(); }
        public void UpdateHitChipsCount(int hitChipsCount) { _userContext.Stats.HitChipsCount = hitChipsCount; Save(); }
        public void AddFinishedStory(int finishedStory) { _userContext.StoryProgress.FinishedStories.Add(finishedStory); Save(); }

        public void CreateNewPlayerContext()
        {
            var json = JsonConvert.SerializeObject(_gameDefs.InitialPlayerContext);
            _userContext = JsonConvert.DeserializeObject<UserContext>(json);
            _userContext.RandomSeed = UnityEngine.Random.Range(0, int.MaxValue);
            ProcessingPlayerContext();
            Save();
        }

        private async void Save()
        {
            OnPlayerContextChanged?.Invoke();

            if (_willSave) 
                return;

            _willSave = true;
            await UniTask.Yield();
            _willSave = false;

            var json = JsonConvert.SerializeObject(_userContext, Formatting.Indented);
            await File.WriteAllTextAsync(_playerContextPath, json);
            Debug.Log($"PlayerContextPath: {_playerContextPath}\n{json}");
        }

        public bool TryLoadingPlayerContext()
        {
            if (File.Exists(_playerContextPath) == false)
                return false;

            var json = File.ReadAllText(_playerContextPath);
            _userContext = JsonConvert.DeserializeObject<UserContext>(json);
            ProcessingPlayerContext();
            return true;
        }

        private void ProcessingPlayerContext()
        {
            _playerRandom = new Random(_userContext.RandomSeed);
            _npcContextRepositories = new Dictionary<string, NpcContextRepository>(_userContext.NpcContexts.Count);
            foreach (var pair in _userContext.NpcContexts)
            {
                var npcContextRepository = new NpcContextRepository(pair.Value);
                npcContextRepository.OnNpcContextChanged += OnNpcContextChanged;
                _npcContextRepositories.Add(pair.Key, npcContextRepository);
            }
        }

        private void OnNpcContextChanged(string npcId)
        {
            Save();
        }

        public void Dispose()
        {
            OnPlayerContextChanged = null;
            foreach (var pair in _npcContextRepositories)
            {
                pair.Value.OnNpcContextChanged -= OnNpcContextChanged;
                pair.Value.Dispose();
            }
        }

    }
}