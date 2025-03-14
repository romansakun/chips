using System.Threading.Tasks;
using LogicUtility;

namespace UI
{
    public abstract class BaseGameplayViewModelAction : IAction<GameplayViewModelContext> 
    {
        public INode<GameplayViewModelContext> Next { get; set; }
        public string GetLog() =>  GetType().Name;

        public virtual Task ExecuteAsync(GameplayViewModelContext context)
        {
            Execute(context);
            return Task.CompletedTask;
        }
        
        protected virtual void Execute(GameplayViewModelContext context)
        {
            
        }

    }
}