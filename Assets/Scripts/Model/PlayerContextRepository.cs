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
    public class PlayerContextRepository : IPlayerContextRepository, IInitializable, IDisposable
    {
        public event Action OnPlayerContextChanged;

        [Inject] private GameDefs _gameDefs;

        private Dictionary<string, NpcContextRepository> _npcContextRepositories;
        private PlayerContext _playerContext;
        private Random _playerRandom;
        private string _playerContextPath;

        private bool _willSave = false;

        public void Initialize()
        {
            _playerContextPath = $"{Application.persistentDataPath}/PlayerContext.json";
            if (TryLoadingPlayerContext()) 
                return;

            CreateNewPlayerContext();
        }

        public int RandomSeed() => _playerContext.RandomSeed;
        public float RandomRange(float min, float max) => _playerRandom.Next((int)(min * 10000), (int)(max * 10000)) / 10000f;
        public int GetRandomRange(int min, int max) => _playerRandom.Next(min, max);
        public int GetBattleWinsCount() => _playerContext.Stats.BattleWins;
        public int GetBattleLosesCount() => _playerContext.Stats.BattleLoses;
        public int GetHitChipsCount() => _playerContext.Stats.HitChipsCount;
        public int GetChipsCount(string chipId) => _playerContext.ChipsCount.GetValueOrDefault(chipId, 0);
        public void ForeachFinishedStories(Action<int> action) => _playerContext.StoryProgress.FinishedStories.ForEach(action);

        public NpcContextRepository GetNpcContext(string npcId) => _npcContextRepositories.GetValueOrDefault(npcId, null);

        public int GetAllChipsCount() 
        { 
            var count = 0; 
            foreach (var pair in _playerContext.ChipsCount) count += pair.Value; 
            return count;
        }

        public void ForeachChips(Action<KeyValuePair<string, int>> action)
        {
            foreach (var pair in _playerContext.ChipsCount)
            {
                action(pair);
            }
        }

        public void UpdateChipsCount(string chipId, int newCount) { _playerContext.ChipsCount[chipId] = newCount; Save(); }
        public void UpdateBattleWinsCount(int battleWins) { _playerContext.Stats.BattleWins = battleWins; Save(); }
        public void UpdateBattleLosesCount(int battleLoses) { _playerContext.Stats.BattleLoses = battleLoses; Save(); }
        public void UpdateHitChipsCount(int hitChipsCount) { _playerContext.Stats.HitChipsCount = hitChipsCount; Save(); }
        public void AddFinishedStory(int finishedStory) { _playerContext.StoryProgress.FinishedStories.Add(finishedStory); Save(); }

        public void CreateNewPlayerContext()
        {
            var json = JsonConvert.SerializeObject(_gameDefs.InitialPlayerContext);
            _playerContext = JsonConvert.DeserializeObject<PlayerContext>(json);
            _playerContext.RandomSeed = UnityEngine.Random.Range(0, int.MaxValue);
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

            var json = JsonConvert.SerializeObject(_playerContext, Formatting.Indented);
            await File.WriteAllTextAsync(_playerContextPath, json);
            Debug.Log($"PlayerContextPath: {_playerContextPath}\n{json}");
        }

        private bool TryLoadingPlayerContext()
        {
            if (File.Exists(_playerContextPath) == false)
                return false;

            var json = File.ReadAllText(_playerContextPath);
            _playerContext = JsonConvert.DeserializeObject<PlayerContext>(json);
            ProcessingPlayerContext();
            return true;
        }

        private void ProcessingPlayerContext()
        {
            _playerRandom = new Random(_playerContext.RandomSeed);
            _npcContextRepositories = new Dictionary<string, NpcContextRepository>(_playerContext.NpcContexts.Count);
            foreach (var pair in _playerContext.NpcContexts)
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