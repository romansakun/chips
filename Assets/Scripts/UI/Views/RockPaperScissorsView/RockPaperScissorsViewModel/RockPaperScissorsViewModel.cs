using System.Collections.Generic;
using Definitions;
using Extensions;
using Factories;
using Gameplay.Battle;
using LogicUtility;
using LogicUtility.Nodes;
using UnityEngine;
using Zenject;

namespace UI.RockPaperScissors
{
    public class RockPaperScissorsViewModel : ViewModel
    {
        [Inject] private LogicBuilderFactory _logicBuilder;
        [Inject] private SignalBus _signalBus;

        public NpcViewPartModel LeftNpcViewComponentModel { get; } = new();
        public NpcViewPartModel RightNpcViewComponentModel { get; } = new();
        public TimerViewPartModel TimerViewPartModel { get; } = new();
        public IReactiveProperty<bool> ShowTitleInfoText => _logicAgent.Context.TitleInfoTextVisible;
        public IReactiveProperty<bool> ShowPlayerInfoText => _logicAgent.Context.PlayerInfoTextVisible;
        public IReactiveProperty<bool> ShowHandsButtons => _logicAgent.Context.HandButtonsVisible;
        public IReactiveProperty<bool> ShowPlayerHand => _logicAgent.Context.PlayerChosenHandVisible;
        public IReactiveProperty<Sprite> PlayerHandSprite => _logicAgent.Context.PlayerChosenHandSprite;
        public IReactiveProperty<Vector3> PlayerChosenHandScale => _logicAgent.Context.PlayerChosenHandScale;
        public IReactiveProperty<Sprite> RockButtonSprite => _logicAgent.Context.RockButtonSprite;
        public IReactiveProperty<Sprite> PaperButtonSprite => _logicAgent.Context.PaperButtonSprite;
        public IReactiveProperty<Sprite> ScissorsButtonSprite => _logicAgent.Context.ScissorsButtonSprite;
        public IReactiveProperty<string> PlayerInfoText => _logicAgent.Context.PlayerInfoText;
        public IReactiveProperty<string> TitleInfoText => _logicAgent.Context.TitleInfoText;

        private LogicAgent<RockPaperScissorsViewModelContext> _logicAgent;

        public override void Initialize()
        {
             var builder = _logicBuilder.Create<RockPaperScissorsViewModelContext>();

             var prepareFirstRoundAction = builder
                 .AddAction<PrepareAction>();
             var startAndWaitRoundAction = builder
                 .AddAction<WaitShakingHandsAction>()
                 .JoinAction<SetHandsAction>()
                 .JoinAction<ProcessRoundResultAction>();
             var prepareNextRoundAction = builder
                 .AddAction<PrepareNextRoundAction>();

             var rootSelector = builder
                 .AddSelector<FirstScoreSelector<RockPaperScissorsViewModelContext>>()
                 .AddQualifier<IsFirstExecutingQualifier>(prepareFirstRoundAction)
                 .SetAsRoot();
 
             prepareFirstRoundAction.DirectTo(prepareNextRoundAction);
             startAndWaitRoundAction.DirectTo(prepareNextRoundAction);
             rootSelector.DirectTo(startAndWaitRoundAction);

             _logicAgent = builder.Build();
             _logicAgent.OnFinished += OnLogicExecutionFinished;

             InitializeProperties();
        }

        private void InitializeProperties()
        {
            var context = _logicAgent.Context;
            TimerViewPartModel.SetContext(context.TimerViewPartModelContext);
            LeftNpcViewComponentModel.SetContext(context.LeftNpcViewComponentModelContext);
            RightNpcViewComponentModel.SetContext(context.RightNpcViewComponentModelContext);
        }

        public void OnPlayerHandClicked(RockPaperScissorsHand hand)
        {
            if (_logicAgent.IsExecuting)
                return;

            _logicAgent.Context.RoundPlayersHands[PlayerType.MyPlayer] = hand;
            _logicAgent.Execute();
        }

        public void SetSharedContext(SharedBattleContext sharedBattleContext)
        {
            _logicAgent.Context.Shared = sharedBattleContext;
            _logicAgent.Execute();
        }

        private void OnLogicExecutionFinished(RockPaperScissorsViewModelContext context)
        {
            var isOverForMyPlayer = context.RoundPlayers.Contains(PlayerType.MyPlayer) == false;
            var isNeedNextRound = context.RoundPlayers.Count > 1;
            var isNeedAutoExecuting = isNeedNextRound && isOverForMyPlayer;
            if (isNeedAutoExecuting)
            {
                _logicAgent.Execute();
            }
        }

        public override void Dispose()
        {
            _logicAgent.OnFinished -= OnLogicExecutionFinished;
            _logicAgent.Dispose();
        }
        
        
#if WITH_CHEATS
        public void SetOrderCheat()
        {
            var context = _logicAgent.Context;
            var playersResults = new Dictionary<PlayerType, int>(context.PlayersResults);
            var pair = playersResults.GetMaxByValue();
            playersResults.Remove(pair.Key);
            context.Shared.FirstMovePlayer = context.Shared.Players.Find(p => p.Type == pair.Key);
            var player = context.Shared.FirstMovePlayer;
            while (playersResults.Count > 0)
            {
                pair = playersResults.GetMaxByValue();
                playersResults.Remove(pair.Key);
                player.NextPlayerTypeInTurn = pair.Key;
                player = context.Shared.Players.Find(p => p.Type == pair.Key);
            }
            player.NextPlayerTypeInTurn = context.Shared.FirstMovePlayer.Type;

            _signalBus.Fire<PlayersMoveOrderSetSignal>();
        }

#endif

    }
}