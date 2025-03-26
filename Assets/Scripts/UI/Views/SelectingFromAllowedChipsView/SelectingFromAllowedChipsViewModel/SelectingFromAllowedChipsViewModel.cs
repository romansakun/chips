using Definitions;
using Factories;
using LogicUtility;
using LogicUtility.Nodes;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI
{
    public class SelectingFromAllowedChipsViewModel : ViewModel
    {
        [Inject] private LogicBuilderFactory _logicBuilder;

        public IReactiveProperty<Vector2> CurrentWatchingChipCanvasPosition => _logicAgent.Context.CurrentWatchingChipCanvasPosition;
        public IReactiveProperty<int> BetChipsCount => _logicAgent.Context.BetChipsCount;
        public IReactiveProperty<bool> ShowSkipCurrentChipButton => _logicAgent.Context.ShowSkipCurrentChipButton;
        public IReactiveProperty<bool> ShowSkipBetChipButton => _logicAgent.Context.ShowSkipBetChipButton;
        public IReactiveProperty<bool> ShowMoveSkippedToWatchingChipButton => _logicAgent.Context.ShowMoveSkippedToWatchingChipButton;
        public IReactiveProperty<bool> ShowSelectWatchingChipToBetButton => _logicAgent.Context.ShowSelectWatchingChipToBetButton;

        private LogicAgent<SelectingFromAllowedChipsViewModelContext> _logicAgent;
        public LogicAgent<SelectingFromAllowedChipsViewModelContext> LogicAgent => _logicAgent;

        public override void Initialize()
        {
            var builder = _logicBuilder.Create<SelectingFromAllowedChipsViewModelContext>();
            var createAllowedChipsAction = builder
                .AddAction<LoadPlayerChipsAction>()
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
            _logicAgent.Execute();
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

        public override void Dispose()
        {
            _logicAgent.Dispose();
        }
    }
}