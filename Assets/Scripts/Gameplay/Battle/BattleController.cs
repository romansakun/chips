using System;
using System.Collections.Generic;
using Definitions;
using Extensions;
using Factories;
using LogicUtility;
using LogicUtility.Nodes;
using Managers;
using Model;
using UnityEngine;
using Zenject;

namespace Gameplay.Battle
{
    public class BattleController: IInitializable, IDisposable
    {
        [Inject] private LogicBuilderFactory _logicBuilder;
        [Inject] private GuiManager _guiManager;
        [Inject] private SignalBus _signalBus;

        private LogicAgent<BattleContext> _logicAgent;

        public void Initialize()
        {
            Subscribes();
            _logicAgent = CreateLogicAgent();
            
#if WITH_CHEATS
            AddDebugCommands();
#endif
        }

        private void Subscribes()
        {
            _signalBus.Subscribe<PlayersBetChipsSetSignal>(OnPlayersBetChipsSetSignal);
            _signalBus.Subscribe<PlayersMoveOrderSetSignal>(OnPlayersMoveOrderSetSignal);
        }

        private void Unsubscribes()
        {
            _signalBus.Unsubscribe<PlayersBetChipsSetSignal>(OnPlayersBetChipsSetSignal);
            _signalBus.Unsubscribe<PlayersMoveOrderSetSignal>(OnPlayersMoveOrderSetSignal);
        }

        private LogicAgent<BattleContext> CreateLogicAgent()
        {
            var builder = _logicBuilder.Create<BattleContext>();
            var selectingChipsForGameAction = builder.AddAction<ShowPlayerSelectingBetChipsViewAction>();
            var rockPaperScissorsGameAction = builder.AddAction<ShowRockPaperScissorsViewAction>();

            //todo: rework it
            var chipsBattleAction = builder
                .AddAction<ShowGameplayViewAction>()
                .JoinAction<WaitWhileOnePlayerWinAction>()
                .JoinAction<HideGameplayViewAction>()
                .JoinAction<SetFinishedStateAction>();
            var rootSelector = builder
                .AddSelector<FirstScoreSelector<BattleContext>>()
                .AddQualifier<IsSelectingChipsForGameStateQualifier>(selectingChipsForGameAction)
                .AddQualifier<IsSetPlayersOrderByRockPaperScissorsStateQualifier>(rockPaperScissorsGameAction)
                .AddQualifier<IsChipsBattleStateQualifier>(chipsBattleAction)
                .SetAsRoot();
            //

            return builder.Build();
        }

        private void OnPlayersBetChipsSetSignal()
        {
            _guiManager.CloseAll();
            _logicAgent.Context.State = BattleState.SetPlayersOrderByRockPaperScissors;
            _logicAgent.Execute();
        }

        private void OnPlayersMoveOrderSetSignal()
        {
            _guiManager.CloseAll();
            _logicAgent.Context.State = BattleState.ChipsBattle;
            _logicAgent.Execute();
        }

        public void StartBattle(IEnumerable<string> opponentPlayerIds)
        {
            _logicAgent.Context.Shared = CreateSharedContext(opponentPlayerIds);
            if (_logicAgent.Context.Shared.Players.Count < 2)
            {
                Debug.LogError("Not enough players!");
                return;
            }
            _logicAgent.Context.State = BattleState.SelectingChipsForGame;
            _logicAgent.Execute();

            Debug.Log(_logicAgent.GetLog());
        }

        private SharedBattleContext CreateSharedContext(IEnumerable<string> opponentPlayerIds)
        {
            var result = new SharedBattleContext();
            var npcIds = new List<string>(opponentPlayerIds);
            foreach (var playerType in PlayerTypeExt.AllPlayers)
            {
                var isPlayer = playerType == PlayerType.MyPlayer;
                var isNpc = isPlayer == false && npcIds.Count > 0;
                if (isPlayer)
                {
                    result.Players.Add(new PlayerSharedContext()
                    {
                        Type = playerType
                    });
                }
                else if (isNpc)
                {
                    var npcId = npcIds.GetAndRemove(0);
                    var npcContext = _userContext.GetNpcContext(npcId);
                    if (npcContext.GetAllChipsCount() > 0)
                    {
                        result.Players.Add(new PlayerSharedContext()
                        {
                            Id = npcId,
                            Type = playerType
                        });
                    }
                }
            }
            return result;
        }

        public void Dispose()
        {
            Unsubscribes();
            _logicAgent.Dispose();

#if WITH_CHEATS
            RemoveDebugCommands();
#endif
        }

#if WITH_CHEATS

        [Inject] private UserContextRepository _userContext;
        [Inject] private GameDefs _gameDefs;

        public void GoToChipsBattle()
        {
            _guiManager.CloseAll();

            SetPlayersBetChips();
            var myPlayer = _logicAgent.Context.Shared.Players.Find(p => p.Type == PlayerType.MyPlayer);
            myPlayer.NextPlayerTypeInTurn = PlayerType.MyPlayer;
            _logicAgent.Context.Shared.FirstMovePlayer = myPlayer;
            _logicAgent.Context.State = BattleState.ChipsBattle;
            _logicAgent.Execute();
        }

        private void SetPlayersBetChips()
        {
            var needBetCount = 3;
            var players = _logicAgent.Context.Shared.Players;
            foreach (var player in players)
            {
                if (player.Type == PlayerType.MyPlayer) 
                    continue;

                var npcContext = _userContext.GetNpcContext(player.Id);
                npcContext.ForeachChips(pair =>
                {
                    if (player.BetChips.Count < needBetCount && pair.Value > 0)
                    {
                        var chipDef = _gameDefs.Chips[pair.Key];
                        player.BetChips.AddManyTimes(chipDef, pair.Value);
                    }
                });
            }
        }

        private void AddDebugCommands()
        {
            IngameDebugConsole.DebugLogConsole.AddCommand(nameof(GoToChipsBattle).ToLower(), string.Empty, GoToChipsBattle);
        }

        private void RemoveDebugCommands()
        {
            IngameDebugConsole.DebugLogConsole.RemoveCommand(GoToChipsBattle);
        }
#endif

    }
}