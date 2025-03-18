using Factories;
using LogicUtility;
using LogicUtility.Nodes;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI
{
    public class GameplayViewModel : ViewModel
    {
        [Inject] private LogicBuilderFactory _logicBuilder;
        [Inject] private ReloadManager _reloadManager;

        public IReactiveProperty<float> HitTimer => _logicAgent.Context.HitTimer;
        public IReactiveProperty<bool> ShowHitTimer => _logicAgent.Context.ShowHitTimer;
        public IReactiveProperty<bool> IsHitSuccess => _logicAgent.Context.IsHitSuccess;
        public IReactiveProperty<bool> IsHitFailed => _logicAgent.Context.IsHitFailed;

        private LogicAgent<GameplayViewModelContext> _logicAgent;

        public override void Initialize()
        {
            var builder = _logicBuilder.Create<GameplayViewModelContext>();
            var prepareStackAction = builder.AddAction<PrepareChipsStackAction>();
            var hitChipsAction = builder
                .AddAction<PrepareChipsStackAction>()
                .JoinAction<BitChipsStackAction>()
                .JoinAction<WaitChipsCollisionAction>();
            var setSuccessHitState = builder
                .AddAction<SetSuccessHitStateAction>()
                .JoinAction<CollectWinningChips>();
            var setFailHitState = builder
                .AddAction<SetFailHitStateAction>();
            var finishMove = builder
                .AddAction<FinishPlayerMoveAction>();

            var resultHitSelector = builder.AddSelector<FirstScoreSelector<GameplayViewModelContext>>();
            resultHitSelector
                .AddQualifier<IsPlayerCannotCollectWinningsChipsQualifier>(setFailHitState)
                .DirectTo(setSuccessHitState);

            builder
                .AddSelector<FirstScoreSelector<GameplayViewModelContext>>()
                .AddQualifier<IsBitChipsStackQualifier>(hitChipsAction)
                .AddQualifier<IsPrepareChipsStackQualifier>(prepareStackAction)
                .SetAsRoot();

            hitChipsAction.DirectTo(resultHitSelector);
            setFailHitState.DirectTo(finishMove);
            setSuccessHitState.DirectTo(finishMove);

            _logicAgent = builder.Build();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("OnBeginDrag GameplayViewModel");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("OnPointerClick GameplayViewModel");
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log("OnDrag GameplayViewModel");
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("OnEndDrag GameplayViewModel");
        }

        public void OnPrepareButtonClick()
        {
            if (_logicAgent.IsExecuting) 
                return;

            _logicAgent.Context.IsPrepareChipsButtonPressed = true;
            _logicAgent.Execute();
        }

        public void OnBitButtonClick()
        {
            if (_logicAgent.IsExecuting) 
                return;

            _logicAgent.Context.IsBitButtonPressed = true;
            _logicAgent.Execute();
        }

        public void OnReloadButtonClick()
        {
            _reloadManager.ReloadGame();
        }

        public override void Dispose()
        {
            _logicAgent.Dispose();
        }
    }
}