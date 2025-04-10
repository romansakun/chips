#if WITH_CHEATS
using System.Threading.Tasks;
using Definitions;
using LogicUtility;
using UI.Gameplay;
using Zenject;

namespace Cheats
{
    public class GameplayViewCheatCommands : ICommandCheat
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private DiContainer _diContainer;

        private LogicAgent<GameplayViewModelContext> _logicAgent;

        public void Initialize()
        {
            _signalBus.Subscribe<LogicAgentCreatedSignal>(OnLogicAgentCreated);
            _signalBus.Subscribe<LogicAgentDisposedSignal>(OnLogicAgentDisposed);
        }

        private void OnLogicAgentCreated(LogicAgentCreatedSignal signal)
        {
            if (signal.LogicAgent.LogicContext is not GameplayViewModelContext)
                return;

            _logicAgent = signal.LogicAgent as LogicAgent<GameplayViewModelContext>;
            AddDebugCommands();
        }

        private void OnLogicAgentDisposed(LogicAgentDisposedSignal signal)
        {
            if (signal.LogicAgent.LogicContext is not GameplayViewModelContext)
                return;

            RemoveDebugCommands();
        }

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

        public void Dispose()
        {
            _signalBus.Unsubscribe<LogicAgentCreatedSignal>(OnLogicAgentCreated);
            _signalBus.Unsubscribe<LogicAgentDisposedSignal>(OnLogicAgentDisposed);
        }
    }
}
#endif