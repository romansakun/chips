using System.Threading.Tasks;
using LogicUtility;

namespace Gameplay.Battle
{
    public abstract class BaseBattleLogicAction : IAction<BattleContext> 
    {
        public INode<BattleContext> Next { get; set; }
        public string GetLog() =>  GetType().Name;

        public virtual Task ExecuteAsync(BattleContext context)
        {
            Execute(context);
            return Task.CompletedTask;
        }
        
        protected virtual void Execute(BattleContext context)
        {
            
        }
    }
}