using Definitions;
using Factories;
using LogicUtility;
using LogicUtility.Nodes;
using UnityEngine;
using Zenject;

namespace UI
{
    public class RockPaperScissorsViewModel : ViewModel
    {
        [Inject] private LogicBuilderFactory _logicBuilder;

        public NpcViewBitModel LeftNpcViewBitModel { get; } = new();
        public NpcViewBitModel RightNpcViewBitModel { get; } = new();
        public TimerViewBitModel TimerViewBitModel { get; } = new();

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

             var startAndWaitRoundAction = builder
                 .AddAction<WaitShakingHandsAction>()
                 .JoinAction<SetHandsAction>()
                 .JoinAction<ProcessRoundResultAction>();
             var prepareFirstRoundAction = builder
                 .AddAction<PrepareAction>();
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
             _logicAgent.Execute();

             InitializeProperties();
        }

        private void InitializeProperties()
        {
            var context = _logicAgent.Context;
            TimerViewBitModel.SetContext(context.TimerViewBitModelContext);
            LeftNpcViewBitModel.SetContext(context.LeftNpcViewBitModelContext);
            RightNpcViewBitModel.SetContext(context.RightNpcViewBitModelContext);
        }

        public void OnPlayerHandClicked(RockPaperScissorsHand hand)
        {
            if (_logicAgent.IsExecuting)
                return;

            _logicAgent.Context.RoundPlayersHands[PlayerType.MyPlayer] = hand;
            _logicAgent.Execute();
        }

        private void OnLogicExecutionFinished(RockPaperScissorsViewModelContext context)
        {
            Debug.Log($"{_logicAgent.GetLog()}");
            var isNeedContinue = context.RoundPlayers.Count > 1 && context.RoundPlayers.Contains(PlayerType.MyPlayer) == false;
            if (isNeedContinue)
            {
                _logicAgent.Execute();
            }
            else
            {
                foreach (var pair in context.PlayersResults)
                {
                    Debug.Log($"{pair.Key} - {pair.Value}");
                }
            }
        }

        public override void Dispose()
        {
            _logicAgent.OnFinished -= OnLogicExecutionFinished;
            _logicAgent.Dispose();
        }

    }
}