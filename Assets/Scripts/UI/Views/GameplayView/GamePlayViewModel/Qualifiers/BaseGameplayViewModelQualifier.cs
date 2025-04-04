using System.Threading.Tasks;
using LogicUtility;

namespace UI.Gameplay
{
    public abstract class BaseGameplayViewModelQualifier : IQualifier<GameplayViewModelContext> 
    {
        public INode<GameplayViewModelContext> Next { get; set; }
        public string GetLog() => GetType().Name;

        public virtual Task<float> ScoreAsync(GameplayViewModelContext context)
        {
            var score = Score(context);
            return Task.FromResult(score);
        }

        protected virtual float Score(GameplayViewModelContext context)
        {
            return 0;
        }
    }
}