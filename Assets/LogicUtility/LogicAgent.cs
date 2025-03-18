using System;
using System.Text;
using System.Threading.Tasks;

namespace LogicUtility
{
    public class LogicAgent<TContext> : IDisposable where TContext : class, IContext
    {
        private readonly LogicUtilityClient<TContext> _client;
        private readonly INode<TContext> _root;

        private bool _isExecuting;
        private bool _isNeedNextExecutionAfterCurrent;
        private bool _isDisposed;

        public event Action<TContext> OnFinished = delegate { };

        public TContext Context { get; set; }
        public bool IsExecuting => _isExecuting;

        private StringBuilder _log = new StringBuilder();

        public LogicAgent(TContext context, INode<TContext> rootNode, bool safeMode)
        {
            Context = context;
            _root = rootNode;
            _client = new LogicUtilityClient<TContext>(this, safeMode);
        }

        public async Task ExecuteAsync(bool force = false)
        {
            await ExecuteInternal(force);
        }

        public async void Execute(bool force = false)
        {
            await ExecuteInternal(force);
        }

        private async Task ExecuteInternal(bool forceNext = false, bool needInvokeFinishEvent = true)
        {
            _log.Clear();
            if (_isExecuting)
            {
                _log.AppendLine("Executing is in progress");
                _isNeedNextExecutionAfterCurrent = _isNeedNextExecutionAfterCurrent || forceNext;
                if (_isNeedNextExecutionAfterCurrent == false)
                    _log.AppendLine("Executing is canceled by previous execution");

                return;
            }

            while (true)
            {
                _isExecuting = true;
                await _client.ExecuteAsync(_root);
                _isExecuting = false;

                if (_isNeedNextExecutionAfterCurrent == false || _isDisposed)
                    break;
                
                _log.AppendLine("Executing will be next");
                _isNeedNextExecutionAfterCurrent = false;
            }
            if (needInvokeFinishEvent && _isDisposed == false)
                OnFinished.Invoke(Context);
        }

        public string GetLog()
        {
            return $"{_log}\n{_client.GetNodeLogs()}";
        }

        public void Dispose()
        {
            OnFinished = null;
            _isDisposed = true;
            _client.Dispose();
            Context.Dispose();
        }
    }
}