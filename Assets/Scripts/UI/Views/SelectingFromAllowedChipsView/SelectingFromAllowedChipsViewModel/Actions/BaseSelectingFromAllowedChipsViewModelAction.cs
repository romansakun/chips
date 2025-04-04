using System.Threading.Tasks;
using LogicUtility;

namespace UI.SelectingFromAllowedChips
{
    public abstract class BaseSelectingFromAllowedChipsViewModelAction : IAction<SelectingFromAllowedChipsViewModelContext> 
    {
        public INode<SelectingFromAllowedChipsViewModelContext> Next { get; set; }
        public string GetLog() =>  GetType().Name;

        public virtual Task ExecuteAsync(SelectingFromAllowedChipsViewModelContext context)
        {
            Execute(context);
            return Task.CompletedTask;
        }

        protected virtual void Execute(SelectingFromAllowedChipsViewModelContext context)
        {

        }
    }
}