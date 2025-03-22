using Definitions;
using Factories;
using Gameplay.Chips;
using LogicUtility;
using LogicUtility.Nodes;
using Managers;
using Model;
using UnityEngine.EventSystems;
using Zenject;

namespace UI
{
    public class SelectingFromAllowedChipsViewModel : ViewModel
    {
        [Inject] private LogicBuilderFactory _logicBuilder;
        [Inject] private GameDefs _gameDefs;
        [Inject] private Chip.Factory _chipFactory;
        [Inject] private AddressableManager _addressableManager;
        [Inject] private PlayerContextRepository _playerContext;

        private LogicAgent<SelectingFromAllowedChipsViewModelContext> _logicAgent;

        public override void Initialize()
        {
            var builder = _logicBuilder.Create<SelectingFromAllowedChipsViewModelContext>();
            var createAllowedChipsAction = builder.AddAction<LoadPlayerChipsAction>();
            var selectChipAction = builder.AddAction<MoveSelectFirstRightAllowedChipToBetAction>();
            var skipChipAction = builder.AddAction<MoveFirstRightAllowedChipToSkippedChipsAction>();
            var unselectChipAction = builder.AddAction<MoveSelectedFirstChipToSkippedChipsAction>();
            var returnToRightChipAction = builder.AddAction<MoveLastSkippedChipToRightAllowedChipsAction>();
            // var hitChipsAction = builder
            //     .AddAction<PrepareChipsStackAction>()
            //     .JoinAction<BitChipsStackAction>()
            //     .JoinAction<WaitChipsCollisionAction>();
            // var setSuccessHitState = builder
            //     .AddAction<SetSuccessHitStateAction>()
            //     .JoinAction<CollectWinningChips>();
            // var setFailHitState = builder
            //     .AddAction<SetFailHitStateAction>();
            // var finishMove = builder
            //     .AddAction<FinishPlayerMoveAction>()
            //     .JoinAction<TryFinishGameAction>();
            //
            // var resultHitSelector = builder.AddSelector<FirstScoreSelector<SelectingFromAllowedChipsViewModelContext>>();
            // resultHitSelector
            //     .AddQualifier<IsPlayerCannotCollectWinningsChipsQualifier>(setFailHitState)
            //     .DirectTo(setSuccessHitState);
            //
            builder
                .AddSelector<FirstScoreSelector<SelectingFromAllowedChipsViewModelContext>>()
                .AddQualifier<IsAllowedChipsNotCreatedQualifier>(createAllowedChipsAction)
                .AddQualifier<IsFirstRightAllowedChipsClickedQualifier>(selectChipAction)
                .AddQualifier<IsStartSwipeQualifier>(null)
                .AddQualifier<IsRightSwipedQualifier>(skipChipAction)
                .AddQualifier<IsFirstSelectedChipClickedQualifier>(unselectChipAction)
                .AddQualifier<IsLeftSwipedQualifier>(returnToRightChipAction)
                .SetAsRoot();

            // hitChipsAction.DirectTo(resultHitSelector);
            // setFailHitState.DirectTo(finishMove);
            // setSuccessHitState.DirectTo(finishMove);

            _logicAgent = builder.Build();
            _logicAgent.Execute();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_logicAgent.IsExecuting || eventData.dragging)
                return;

            _logicAgent.Context.Input = (InputType.OnPointerClick, eventData);
            _logicAgent.Execute();
        }

        public void OnBeginDrag(PointerEventData eventData)
        { 
            if (_logicAgent.IsExecuting)
                return;

            _logicAgent.Context.Input = (InputType.OnBeginDrag, eventData);
            _logicAgent.Execute();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_logicAgent.IsExecuting)
                return;

            _logicAgent.Context.Input = (InputType.OnDrag, eventData);
            _logicAgent.Execute();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_logicAgent.IsExecuting)
                return;

            _logicAgent.Context.Input = (InputType.OnEndDrag, eventData);
            _logicAgent.Execute();
        }

        public override void Dispose()
        {
            _logicAgent.Dispose();
        }
    }
}