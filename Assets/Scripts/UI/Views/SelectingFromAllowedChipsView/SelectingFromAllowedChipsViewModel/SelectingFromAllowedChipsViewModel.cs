using Definitions;
using Extensions;
using Factories;
using Gameplay.Battle;
using LogicUtility;
using LogicUtility.Nodes;
using Model;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI.SelectingFromAllowedChips
{
    public class SelectingFromAllowedChipsViewModel : ViewModel
    {
        [Inject] private LogicBuilderFactory _logicBuilder;
        [Inject] private UserContextRepository _userContext;
        [Inject] private GameDefs _gameDefs;
        [Inject] private SignalBus _signalBus;

        public IReactiveProperty<Vector2> CurrentWatchingChipCanvasPosition => _logicAgent.Context.CurrentWatchingChipCanvasPosition;
        public IReactiveProperty<int> NeedBetChipsCount => _logicAgent.Context.NeedBetChipsCount;
        public IReactiveProperty<int> BetChipsCount => _logicAgent.Context.BetChipsCount;
        public IReactiveProperty<bool> ShowSkipCurrentChipButton => _logicAgent.Context.ShowSkipCurrentChipButton;
        public IReactiveProperty<bool> ShowSkipBetChipButton => _logicAgent.Context.ShowSkipBetChipButton;
        public IReactiveProperty<bool> ShowMoveSkippedToWatchingChipButton => _logicAgent.Context.ShowMoveSkippedToWatchingChipButton;
        public IReactiveProperty<bool> ShowSelectWatchingChipToBetButton => _logicAgent.Context.ShowSelectWatchingChipToBetButton;
        public IReactiveProperty<bool> ShowReadyButton => _logicAgent.Context.ShowReadyButton;

        private LogicAgent<SelectingFromAllowedChipsViewModelContext> _logicAgent;

        public override void Initialize()
        {
            var builder = _logicBuilder.Create<SelectingFromAllowedChipsViewModelContext>();
            var createAllowedChipsAction = builder
                .AddAction<CalcBetChipsCountAction>()
                .JoinAction<LoadPlayerChipsAction>()
                .JoinAction<MovingAndRotationAllChipsAction>()
                .JoinAction<CalcWatchingChipCanvasPositionAction>()
                .JoinAction<SetVisibleStateCanvasObjectsAction>();

            var currentChipToBetAction = builder.AddAction<MoveCurrentWatchingChipToBetAction>();
            var currentChipToLeftSkippedAction = builder.AddAction<MoveCurrentWatchingChipToSkippedChipsAction>();
            var betChipToLeftSkippedChipsAction = builder.AddAction<MoveBetChipToSkippedChipsAction>();
            var returnToRightChipAction = builder.AddAction<MoveSkippedToWatchingChipAction>();
            var animateChipsMovingAction = builder
                .AddAction<AnimateMovingAndRotationAllChipsAction>()
                .JoinAction<SetVisibleStateCanvasObjectsAction>();

            builder
                .AddSelector<FirstScoreSelector<SelectingFromAllowedChipsViewModelContext>>()
                .AddQualifier<IsAllowedChipsNotCreatedQualifier>(createAllowedChipsAction)
                .AddQualifier<IsStartSwipeQualifier>(null)
                .AddQualifier<IsCurrentWatchingChipSelectedToBetQualifier>(currentChipToBetAction)
                .AddQualifier<IsCurrentWatchingChipSkippedQualifier>(currentChipToLeftSkippedAction)
                .AddQualifier<IsSkipBetChipQualifier>(betChipToLeftSkippedChipsAction)
                .AddQualifier<IsMoveSkippedToWatchingChipQualifier>(returnToRightChipAction)
                .SetAsRoot();

            currentChipToBetAction.DirectTo(animateChipsMovingAction);
            currentChipToLeftSkippedAction.DirectTo(animateChipsMovingAction);
            betChipToLeftSkippedChipsAction.DirectTo(animateChipsMovingAction);
            returnToRightChipAction.DirectTo(animateChipsMovingAction);

            _logicAgent = builder.Build();
        }

        public void SelectCurrentChipButtonClicked()
        {
            if (_logicAgent.IsExecuting)
                return;

            _logicAgent.Context.IsCurrentWatchingChipSelectedToBet = true;
            _logicAgent.Execute();
        }

        public void SkipCurrentChipButtonClicked()
        {
            if (_logicAgent.IsExecuting)
                return;

            _logicAgent.Context.IsCurrentWatchingChipSkipped = true;
            _logicAgent.Execute();
        }

        public void MoveSkippedToWatchingChipButtonClicked()
        {
            if (_logicAgent.IsExecuting)
                return;

            _logicAgent.Context.IsMoveSkippedToWatchingChip = true;
            _logicAgent.Execute();
        }

        public void SkipBetChipButtonClicked()
        {
            if (_logicAgent.IsExecuting)
                return;

            _logicAgent.Context.IsSkipBetChip = true;
            _logicAgent.Execute();
        }

        public void OnBeginDrag(PointerEventData eventData)
        { 
            if (_logicAgent.IsExecuting)
                return;

            _logicAgent.Context.Input = (DragInputType.OnBeginDrag, eventData);
            _logicAgent.Execute();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_logicAgent.IsExecuting)
                return;

            _logicAgent.Context.Input = (DragInputType.OnDrag, eventData);
            _logicAgent.Execute();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_logicAgent.IsExecuting)
                return;

            _logicAgent.Context.Input = (DragInputType.OnEndDrag, eventData);
            _logicAgent.Execute();
        }

        public void SetSharedBattleContext(SharedBattleContext contextShared)
        {
            _logicAgent.Context.Shared = contextShared;
            _logicAgent.Execute();
        }

        public void ReadyButtonClicked()
        {
            SetPlayersBetChips();

            _signalBus.Fire<PlayersBetChipsSetSignal>();
        }

        private void SetPlayersBetChips()
        {
            var needBetCount = _logicAgent.Context.Shared.NeedBetChipsCount;
            var players = _logicAgent.Context.Shared.Players;
            var myPlayer = players.Find(p => p.Type == PlayerType.MyPlayer);
            foreach (var pair in _logicAgent.Context.BetSelectedChips)
            {
                myPlayer.BetChips.Add(pair.Item2);
            }
            foreach (var player in players)
            {
                if (player.Type == PlayerType.MyPlayer) 
                    continue;

                var npcContext = _userContext.GetNpcContext(player.Id);
                npcContext.ForeachChips(pair =>
                {
                    var chipDef = _gameDefs.Chips[pair.Key];
                    var chipCount = pair.Value;
                    while (chipCount > 0 && player.BetChips.Count < needBetCount)
                    {
                        chipCount--;
                        player.BetChips.Add(chipDef);
                    }
                });
            }
        }

        public override void Dispose()
        {
            _logicAgent.Dispose();
        }
    }
}