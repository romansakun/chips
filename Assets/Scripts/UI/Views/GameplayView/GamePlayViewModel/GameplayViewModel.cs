using Cysharp.Threading.Tasks;
using Definitions;
using Factories;
using Gameplay.Battle;
using LogicUtility;
using LogicUtility.Nodes;
using Managers;
using UI.PreparingHit;
using UnityEngine;
using Zenject;

namespace UI.Gameplay
{
    public class GameplayViewModel : ViewModel
    {
        [Inject] private AddressableManager _addressableManager;
        [Inject] private ViewModelFactory _viewModelFactory;
        [Inject] private LogicBuilderFactory _logicBuilder;
        [Inject] private ReloadManager _reloadManager;
        [Inject] private DiContainer _diContainer;
        [Inject] private GuiManager _guiManager;
        [Inject] private GameDefs _gameDefs;

        public NpcViewPartModel LeftNpc { get; } = new();
        public NpcViewPartModel RightNpc { get; } = new();
        public TimerViewPartModel HitTimer { get;} = new();
        public IReactiveProperty<bool> IsPlayerCanHitNow => _logicAgent.Context.IsPlayerCanHitNow;

        private LogicAgent<GameplayViewModelContext> _logicAgent;


        public override void Initialize()
        {
            var builder = _logicBuilder.Create<GameplayViewModelContext>();
            var hitChipsAction = builder
                .AddAction<PrepareVisualAction>()
                .JoinAction<PrepareChipsStackAction>()
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
        }

        public async void SetSharedContext(SharedBattleContext sharedBattleContext)
        {
            _logicAgent.Context.Shared = sharedBattleContext;
            _logicAgent.Context.HittingPlayer = sharedBattleContext.FirstMovePlayer;
            _logicAgent.Context.IsPlayerCanHitNow.Value = sharedBattleContext.FirstMovePlayer.Type == PlayerType.MyPlayer;

            var context = _logicAgent.Context;
            var leftPlayer = context.Shared.Players.Find(p => p.Type == PlayerType.LeftPlayer);
            var rightPlayer = context.Shared.Players.Find(p => p.Type == PlayerType.RightPlayer);
            if (leftPlayer != null)
            {
                context.LeftNpcContext.AvatarSprite.Value = await GetAvatarSprite(leftPlayer.Id);
            }
            if (rightPlayer != null)
            {
                context.RightNpcContext.AvatarSprite.Value = await GetAvatarSprite(rightPlayer.Id);
            }
        }

        public async void OnPrepareButtonClick()
        {
            _logicAgent.Context.IsPlayerCanHitNow.Value = false;

            var viewModel = _viewModelFactory.Create<PreparingForceHitViewModel>();
            var view = await _guiManager.ShowAsync<PreparingHitView, PreparingForceHitViewModel>(viewModel);
            view.SelectForceButton();
            view.OnClose += () =>
            {
                _logicAgent.Context.IsPlayerCanHitNow.Value = true;
            };
        }

        public void OnBitButtonClick()
        {
            if (_logicAgent.IsExecuting)
                return;

            _logicAgent.Context.IsTimeToHitChips = true;
            _logicAgent.Execute();
        }

        public void OnReloadButtonClick()
        {
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

        private async UniTask<Sprite> GetAvatarSprite(string npcDefId)
        {
            var avatarSpriteId = _gameDefs.Npc[npcDefId].AvatarSprite;
            var avatarSprite = await _addressableManager.LoadSpriteAsync(avatarSpriteId);
            return avatarSprite;
        }

        public override void Dispose()
        {
            _logicAgent.OnFinished -= OnLogicFinished;
            _logicAgent.Dispose();

#if WITH_CHEATS
            RemoveDebugCommands();
#endif
        }

//todo: replace cheats
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

        private async UniTask FinishGame(bool isUserNeedBeWinner)
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