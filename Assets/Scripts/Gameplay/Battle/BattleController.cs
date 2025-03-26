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

        private LogicAgent<BattleContext> CreateLogicAgent()
        {
            var builder = _logicBuilder.Create<BattleContext>();
            var selectingChipsForGameAction = builder
                .AddAction<ShowAllowedPlayerChipsViewAction>()
                .JoinAction<WaitWhilePlayerSelectChipsAction>()
                .JoinAction<HideAllowedPlayerChipsAction>()
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

        public async UniTask ExecuteBattle(List<PlayerData> players)
        {
            _logicAgent ??= CreateLogicAgent();
            var context = _logicAgent.Context;

            context.Players = players;
            context.State = BattleState.SelectingChipsForGame;

            while (context.State != BattleState.Finished)
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