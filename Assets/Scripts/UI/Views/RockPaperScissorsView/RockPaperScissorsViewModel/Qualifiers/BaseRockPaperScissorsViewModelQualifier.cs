using System.Threading.Tasks;
using LogicUtility;

namespace UI.RockPaperScissors
{
    public abstract class BaseRockPaperScissorsViewModelQualifier : IQualifier<RockPaperScissorsViewModelContext> 
    {
        public INode<RockPaperScissorsViewModelContext> Next { get; set; }
        public string GetLog() => GetType().Name;

        public virtual Task<float> ScoreAsync(RockPaperScissorsViewModelContext context)
        {
            var score = Score(context);
            return Task.FromResult(score);
        }

        protected virtual float Score(RockPaperScissorsViewModelContext context)
        {
            return 0;
        }
    }
}