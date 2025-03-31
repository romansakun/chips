using System;
using System.Collections.Generic;
using Definitions;
using Extensions;
using Factories;
using LogicUtility;
using LogicUtility.Nodes;
using Managers;
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
            var selectingChipsForGameAction = builder
                .AddAction<WaitPlayerSelectingBetChipsAction>();
                //.JoinAction<SetPlayersOrderByRockPaperScissorsStateAction>();

            var rockPaperScissorsGameAction = builder
                .AddAction<WaitRockPaperScissorsViewAction>()
                .JoinAction<SetChipsBattleStateAction>();

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
                if (isNpc || isPlayer)
                {
                    result.Players.Add(new PlayerSharedContext()
                    {
                        Id = isNpc ? npcIds.GetAndRemove(0) : string.Empty,
                        Type = playerType
                    });
                }
            }
            return result;
        }

        public void Dispose()
        {
            Unsubscribes();
            _logicAgent.Dispose();
        }

    }
}