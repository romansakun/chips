using System.Threading.Tasks;
using LogicUtility;

namespace UI
{
    public abstract class BaseSelectingFromAllowedChipsViewModelQualifier : IQualifier<SelectingFromAllowedChipsViewModelContext> 
    {
        public INode<SelectingFromAllowedChipsViewModelContext> Next { get; set; }
        public string GetLog() => GetType().Name;

        public virtual Task<float> ScoreAsync(SelectingFromAllowedChipsViewModelContext context)
        {
            var score = Score(context);
            return Task.FromResult(score);
        }

        protected virtual float Score(SelectingFromAllowedChipsViewModelContext context)
        {
            return 0;
        }
    }
}