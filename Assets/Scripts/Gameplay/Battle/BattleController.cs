using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Factories;
using LogicUtility;
using LogicUtility.Nodes;
using UnityEngine;
using Zenject;

namespace Gameplay.Battle
{
    public class BattleController: IDisposable
    {
        [Inject] private LogicBuilderFactory _logicBuilder;

        private LogicAgent<BattleContext> _logicAgent;

        public BattleContext Context => _logicAgent.Context;

        private LogicAgent<BattleContext> CreateLogicAgent()
        {
            var builder = _logicBuilder.Create<BattleContext>();
            var selectingChipsForGameAction = builder
                .AddAction<WaitPlayerSelectingBetChipsAction>()
                .JoinAction<SetPlayersOrderByRockPaperScissorsStateAction>();

            var rockPaperScissorsGameAction = builder
                .AddAction<ShowRockPaperScissorsViewAction>()
                .JoinAction<WaitWhilePlayerRockPaperScissorsAction>()
                .JoinAction<HideRockPaperScissorsViewAction>()
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

        public async UniTask ExecuteBattle(List<string> players)
        {
            _logicAgent ??= CreateLogicAgent();
            var context = _logicAgent.Context;
            context.Reset();
            context.Players.AddRange(players);
            context.RightPlayer = players[0];
            context.LeftPlayer = players.Count > 1 
                ? players[1] 
                : string.Empty;
            context.State = BattleState.SelectingChipsForGame;

            while (context.IsDisposed == false && context.State != BattleState.Finished)
            {
                await _logicAgent.ExecuteAsync();
                Debug.Log(_logicAgent.GetLog());
            }
        }

        public void Dispose()
        {
            _logicAgent?.Dispose();
            _logicAgent = null;
        }

    }
}