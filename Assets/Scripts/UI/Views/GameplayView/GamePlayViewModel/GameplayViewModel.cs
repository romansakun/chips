using System.Threading.Tasks;
using Definitions;
using Factories;
using Gameplay.Battle;
using LogicUtility;
using LogicUtility.Nodes;
using Managers;
using Model;
using UnityEngine.EventSystems;
using Zenject;

namespace UI.Gameplay
{
    //todo: replace cheats
    public class GameplayViewModel : ViewModel
    {
        [Inject] private LogicBuilderFactory _logicBuilder;
        [Inject] private ReloadManager _reloadManager;
        [Inject] private DiContainer _diContainer;
        [Inject] private GameDefs _gameDefs;
        [Inject] private UserContextRepository _userContext;

        public NpcViewPartModel LeftNpc { get; } = new();
        public NpcViewPartModel RightNpc { get; } = new();
        public TimerViewPartModel HitTimer { get;} = new();
        public PreparingHitViewPartModel PreparingForceHit { get; } = new();
        public PreparingHitViewPartModel PreparingTorqueHit { get; } = new();
        public PreparingHitViewPartModel PreparingAngleHit { get; } = new();
        public PreparingHitViewPartModel PreparingHeightHit { get; } = new();
      
        public IReactiveProperty<bool> IsHitStarted => _logicAgent.Context.IsHitStarted;
        public IReactiveProperty<bool> IsPlayerCanHitNow => _logicAgent.Context.IsPlayerCanHitNow;

        private readonly ReactiveProperty<bool> _showPreparingButtons = new();
        private readonly ReactiveProperty<bool> _showPreparingViewPart = new();
        public IReactiveProperty<bool> ShowPreparingButtons => _showPreparingButtons;
        public IReactiveProperty<bool> ShowPreparingViewPart => _showPreparingViewPart;

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
                .AddAction<FinishPlayerMoveAction>()
                .JoinAction<TryFinishGameAction>();

            var resultHitSelector = builder.AddSelector<FirstScoreSelector<GameplayViewModelContext>>();
            resultHitSelector
                .AddQualifier<IsPlayerCannotCollectWinningsChipsQualifier>(setFailHitState)
                .DirectTo(setSuccessHitState);

            builder
                .AddSelector<FirstScoreSelector<GameplayViewModelContext>>()
                .AddQualifier<IsHitChipsStackQualifier>(hitChipsAction)
                .AddQualifier<IsPrepareChipsStackQualifier>(prepareStackAction)
                .SetAsRoot();

            hitChipsAction.DirectTo(resultHitSelector);
            setFailHitState.DirectTo(finishMove);
            setSuccessHitState.DirectTo(finishMove);

            _logicAgent = builder.Build();
            _logicAgent.OnFinished += OnLogicFinished;
            InitializeProperties();

#if WITH_CHEATS
            AddDebugCommands();
#endif
        }

        private void InitializeProperties()
        {
            var context = _logicAgent.Context;
            LeftNpc.SetContext(context.LeftNpcContext);
            RightNpc.SetContext(context.RightNpcContext);
            HitTimer.SetContext(context.HitTimerContext);
            PreparingForceHit.SetContext(context.PreparingForceContext);
            PreparingAngleHit.SetContext(context.PreparingAngleContext);
            PreparingTorqueHit.SetContext(context.PreparingTorqueContext);
            PreparingHeightHit.SetContext(context.PreparingHeightContext);

            PreparingForceHit.OnSliderValueChanged += OnPreparingForceHitSliderValueChanged;
            PreparingAngleHit.OnSliderValueChanged += OnPreparingAngleHitSliderValueChanged;
            PreparingTorqueHit.OnSliderValueChanged += OnPreparingTorqueHitSliderValueChanged;
            PreparingHeightHit.OnSliderValueChanged += OnPreparingHeightHitSliderValueChanged;

            SetVisiblePreparingViewParts(true);
            OnPreparingForceHitSliderValueChanged(_userContext.GetPreparingForce());
            OnPreparingAngleHitSliderValueChanged(_userContext.GetPreparingAngle());
            OnPreparingTorqueHitSliderValueChanged(_userContext.GetPreparingTorque());
            OnPreparingHeightHitSliderValueChanged(_userContext.GetPreparingHeight());
            SetVisiblePreparingViewParts(false);
        }

        private void OnPreparingHeightHitSliderValueChanged(float value)
        {
            var heightContext = _logicAgent.Context.PreparingHeightContext;
            if (heightContext.Visible.Value == false)
                return;

            var valuesRange = _gameDefs.GameplaySettings.PrepareHeightRange;
            ProcessPreparingHitSliderValue(heightContext, valuesRange, value);
        }

        private void OnPreparingTorqueHitSliderValueChanged(float value)
        {
            var torqueContext = _logicAgent.Context.PreparingTorqueContext;
            if (torqueContext.Visible.Value == false)
                return;

            var valuesRange = _gameDefs.GameplaySettings.PrepareTorqueRange;
            ProcessPreparingHitSliderValue(torqueContext, valuesRange, value);
        }

        private void OnPreparingAngleHitSliderValueChanged(float value)
        {
            var angleContext = _logicAgent.Context.PreparingAngleContext;
            if (angleContext.Visible.Value == false)
                return;

            var valuesRange = _gameDefs.GameplaySettings.PrepareAngleRange;
            ProcessPreparingHitSliderValue(angleContext, valuesRange, value);
        }

        private void OnPreparingForceHitSliderValueChanged(float value)
        {
            var forceContext = _logicAgent.Context.PreparingForceContext;
            if (forceContext.Visible.Value == false)
                return;

            var valuesRange = _gameDefs.GameplaySettings.PrepareForceRange;
            ProcessPreparingHitSliderValue(forceContext, valuesRange, value);
        }

        private void ProcessPreparingHitSliderValue(PreparingHitViewPartModelContext preparingContext, float[] valuesRange, float value)
        {
            preparingContext.ValueSlider.SetWithoutChangeInvoke(value);
            var min = valuesRange[0];
            var max = valuesRange[1];
            var oneThird = (max - min) / 3;
            var needValue = (max-min) * value + min;
            preparingContext.NeedValue = needValue;
            var isMinimal = needValue < oneThird + min;
            var isMedium = needValue >= oneThird + min && needValue <= max - oneThird;
            var isMaximum = needValue > max - oneThird;

            //todo: change info sprites in preparingContext
            if (isMinimal)
            {
            }
            else if (isMedium)
            {
            }
            else if (isMaximum)
            {
            }
            //
        }

        public void SetSharedContext(SharedBattleContext sharedBattleContext)
        {
            _logicAgent.Context.Shared = sharedBattleContext;
            _logicAgent.Context.HittingPlayer = sharedBattleContext.FirstMovePlayer;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _showPreparingViewPart.Value = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void OnPrepareButtonClick()
        {
            _showPreparingButtons.Value = !_showPreparingButtons.Value;
        }

        public void OnBitButtonClick()
        {
            _showPreparingViewPart.Value = false;
            if (_logicAgent.IsExecuting) 
                return;

            _logicAgent.Context.IsTimeToHitChips = true;
            _logicAgent.Execute();
        }

        public void OnPrepareForceButtonClick()
        {
            SetVisiblePreparingViewParts(false);
            TouchPreparingHitContext(_logicAgent.Context.PreparingForceContext);
        }

        public void OnPrepareTorqueButtonClick()
        {
            SetVisiblePreparingViewParts(false);
            TouchPreparingHitContext(_logicAgent.Context.PreparingTorqueContext);
        }

        public void OnPrepareHeightButtonClick()
        {
            SetVisiblePreparingViewParts(false);
            TouchPreparingHitContext(_logicAgent.Context.PreparingHeightContext);
        }

        public void OnPrepareAngleButtonClick()
        {
            SetVisiblePreparingViewParts(false);
            TouchPreparingHitContext(_logicAgent.Context.PreparingAngleContext);
        }

        private void SetVisiblePreparingViewParts(bool state)
        {
            _logicAgent.Context.PreparingForceContext.Visible.Value = state;
            _logicAgent.Context.PreparingTorqueContext.Visible.Value = state;
            _logicAgent.Context.PreparingHeightContext.Visible.Value = state;
            _logicAgent.Context.PreparingAngleContext.Visible.Value = state;
        }

        private void TouchPreparingHitContext(PreparingHitViewPartModelContext context)
        {
            context.ValueSlider.Touch();
            context.InfoSprite.Touch();
            context.Visible.Value = true;
            OnPreparingForceHitSliderValueChanged(context.ValueSlider.Value);
            _showPreparingViewPart.Value = true;
        }

        public void OnReloadButtonClick()
        {
            _showPreparingViewPart.Value = false;
            _reloadManager.ReloadGame();
        }

        public void ProcessFirstPlayerMove()
        {
            if (_logicAgent.Context.Shared.FirstMovePlayer.Type != PlayerType.MyPlayer)
            {
                _logicAgent.Context.IsTimeToHitChips = true;
                _logicAgent.Execute();
            }
        }

        private void OnLogicFinished(GameplayViewModelContext context)
        {
            var nextPlayerType = context.HittingPlayer.Type;
            if (nextPlayerType != PlayerType.MyPlayer)
            {
                _logicAgent.Context.IsTimeToHitChips = true;
                _logicAgent.Execute();
            }
        }

        public override void Dispose()
        {
            PreparingForceHit.OnSliderValueChanged -= OnPreparingForceHitSliderValueChanged;
            PreparingAngleHit.OnSliderValueChanged -= OnPreparingAngleHitSliderValueChanged;
            PreparingTorqueHit.OnSliderValueChanged -= OnPreparingTorqueHitSliderValueChanged;
            PreparingHeightHit.OnSliderValueChanged -= OnPreparingHeightHitSliderValueChanged;

            _logicAgent.OnFinished -= OnLogicFinished;
            _logicAgent.Dispose();

#if WITH_CHEATS
            RemoveDebugCommands();
#endif
        }

#if WITH_CHEATS
        private void AddDebugCommands()
        {
            IngameDebugConsole.DebugLogConsole.AddCommand(nameof(WinGame).ToLower(), string.Empty, WinGame);
            IngameDebugConsole.DebugLogConsole.AddCommand(nameof(LoseGame).ToLower(), string.Empty, LoseGame);
            IngameDebugConsole.DebugLogConsole.AddCommand(nameof(PrepareChipsHit).ToLower(), string.Empty, PrepareChipsHit);
        }

        private void RemoveDebugCommands()
        {
            IngameDebugConsole.DebugLogConsole.RemoveCommand(WinGame);
            IngameDebugConsole.DebugLogConsole.RemoveCommand(LoseGame);
            IngameDebugConsole.DebugLogConsole.RemoveCommand(PrepareChipsHit);
        }

        public async void WinGame()
        {
            await FinishGame(true);
        }

        public async void LoseGame()
        {
            await FinishGame(false);
        }        

        public async void PrepareChipsHit()
        {
            var prepareAction = _diContainer.Instantiate<PrepareChipsStackAction>();
            await prepareAction.ExecuteAsync(_logicAgent.Context);
        }

        private async Task FinishGame(bool isUserNeedBeWinner)
        {
            _logicAgent.Context.HittingPlayer = _logicAgent.Context.Shared.Players.Find(p => isUserNeedBeWinner 
                ? p.Type == PlayerType.MyPlayer
                : p.Type != PlayerType.MyPlayer);
            var prepareAction = _diContainer.Instantiate<PrepareChipsStackAction>();
            var successHitAction = _diContainer.Instantiate<SetSuccessHitStateAction>();
            var collectChipsAction = _diContainer.Instantiate<CollectWinningChips>();
            var finishPlayerMoveAction = _diContainer.Instantiate<FinishPlayerMoveAction>();
            var tryFinishGameAction = _diContainer.Instantiate<TryFinishGameAction>();

            await prepareAction.ExecuteAsync(_logicAgent.Context);
            _logicAgent.Context.HitWinningChipsAndDefs.Clear();
            _logicAgent.Context.HitWinningChipsAndDefs.AddRange(_logicAgent.Context.HittingChipsAndDefs);
            await successHitAction.ExecuteAsync(_logicAgent.Context);
            await collectChipsAction.ExecuteAsync(_logicAgent.Context);
            await finishPlayerMoveAction.ExecuteAsync(_logicAgent.Context);
            await tryFinishGameAction.ExecuteAsync(_logicAgent.Context);
        }
#endif

    }
}