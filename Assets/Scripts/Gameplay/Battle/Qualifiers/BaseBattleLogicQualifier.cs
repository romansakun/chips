using System.Threading.Tasks;
using LogicUtility;

namespace Gameplay.Battle
{
    public abstract class BaseBattleLogicQualifier : IQualifier<BattleContext> 
    {
        public INode<BattleContext> Next { get; set; }
        public string GetLog() => GetType().Name;

        public virtual Task<float> ScoreAsync(BattleContext context)
        {
            var score = Score(context);
            return Task.FromResult(score);
        }

        protected virtual float Score(BattleContext context)
        {
            return 0;
        }
    }
}