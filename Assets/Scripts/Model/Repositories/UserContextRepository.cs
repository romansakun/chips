using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Definitions;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace Model
{
    public class UserContextRepository : IPlayerContextRepository, IInitializable, IDisposable
    {
        public event Action OnPlayerContextChanged;
        public event Action<int> OnBattleWinsCountChanged;

        [Inject] private GameDefs _gameDefs;

        private UserContext _userContext;
        private Dictionary<string, NpcContextRepository> _npcContextRepositories;
        private string _playerContextPath;
        private bool _willSave = false;

        public void Initialize()
        {
            _playerContextPath = $"{Application.persistentDataPath}/PlayerContext.json";
        }

        public string GetLocalizationLanguage() => _userContext.GameSettings.Language;

        public float GetPreparingForce() => _userContext.GameplayControl.PreparedForce;
        public float GetPreparingHeight() => _userContext.GameplayControl.PreparedHeight;
        public float GetPreparingAngle() => _userContext.GameplayControl.PreparedAngle;
        public float GetPreparingTorque() => _userContext.GameplayControl.PreparedTorque;

        public int GetBattleWinsCount() => _userContext.Stats.BattleWins;
        public int GetBattleLosesCount() => _userContext.Stats.BattleLoses;
        public int GetHitChipsCount() => _userContext.Stats.HitChipsCount;
        public int GetChipsCount(string chipId) => _userContext.ChipsCount.GetValueOrDefault(chipId, 0);
        public void ForeachFinishedStories(Action<int> action) => _userContext.StoryProgress.FinishedStories.ForEach(action);

        private void AddNpcContext(string npcDefId)
        {
            if (_gameDefs.Npc.TryGetValue(npcDefId, out NpcDef npcDef) == false)
                throw new Exception($"There is no npc with id: {npcDefId}");

            var json = JsonConvert.SerializeObject(npcDef);
            var npcContext = JsonConvert.DeserializeObject<NpcContext>(json);
            _userContext.NpcContexts.Add(npcDefId, npcContext);

            var npcContextRepository = new NpcContextRepository(npcContext);
            npcContextRepository.OnNpcContextChanged += OnNpcContextChanged;
            _npcContextRepositories.Add(npcDefId, npcContextRepository);

            Save();
        }

        public NpcContextRepository GetNpcContext(string npcId)
        {
            if (_npcContextRepositories.ContainsKey(npcId) == false)
                AddNpcContext(npcId);

            return _npcContextRepositories[npcId];
        }

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

        public void UpdateChipsCount(string chipId, int newCount)
        {
            _userContext.ChipsCount[chipId] = newCount; 
            Save();
        }

        public void UpdateBattleWinsCount(int battleWins)
        {
            _userContext.Stats.BattleWins = battleWins;
            OnBattleWinsCountChanged?.Invoke(battleWins);
            Save();
        }

        public void UpdateBattleLosesCount(int battleLoses)
        {
            _userContext.Stats.BattleLoses = battleLoses; 
            Save();
        }

        public void UpdateHitChipsCount(int hitChipsCount)
        {
            _userContext.Stats.HitChipsCount = hitChipsCount; 
            Save();
        }

        public void AddFinishedStory(int finishedStory)
        {
            _userContext.StoryProgress.FinishedStories.Add(finishedStory); 
            Save();
        }

        public void UpdatePreparedForce(float force)
        {
            _userContext.GameplayControl.PreparedForce = force;
            Save();
        }

        public void UpdatePreparedHeight(float height)
        {
            _userContext.GameplayControl.PreparedHeight = height;
            Save();
        }

        public void UpdatePreparedAngle(float angle)
        {
            _userContext.GameplayControl.PreparedAngle = angle;
            Save();
        }

        public void UpdatePreparedTorque(float torque)
        {
            _userContext.GameplayControl.PreparedTorque = torque;
            Save();
        }

        public void CreateNewPlayerContext()
        {
            var json = JsonConvert.SerializeObject(_gameDefs.InitialPlayerContext);
            _userContext = JsonConvert.DeserializeObject<UserContext>(json);
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