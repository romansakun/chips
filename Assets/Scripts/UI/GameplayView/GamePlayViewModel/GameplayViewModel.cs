using Factories;
using LogicUtility;
using LogicUtility.Nodes;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI
{
    public class GameplayViewModel : ViewModel
    {
        [Inject] LogicBuilderFactory _logicBuilder;

        private LogicAgent<GameplayViewModelContext> _logicAgent;
        
        public override void Initialize()
        {
            var builder = _logicBuilder.Create<GameplayViewModelContext>();
            var prepareStackAction = builder.AddAction<PrepareChipsStackAction>();
            var bitStackAction = builder
                .AddAction<PrepareChipsStackAction>()
                .JoinAction<BitChipsStackAction>()
                .JoinAction<WaitChipsCollisionAction>()
                .JoinAction<CollectWinningChips>();

            var rootSelector = builder
                .AddSelector<FirstScoreSelector<GameplayViewModelContext>>()
                .AddQualifier<IsBitChipsStackQualifier>(bitStackAction)
                .AddQualifier<IsPrepareChipsStackQualifier>(prepareStackAction)
                .SetAsRoot();

            _logicAgent = builder.Build();
        }

        public override void Dispose()
        {
            
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

    }
}