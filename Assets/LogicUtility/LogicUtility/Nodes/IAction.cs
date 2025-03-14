using System.Threading.Tasks;

namespace LogicUtility
{
    public interface IAction<T> : INode<T> where T : class, IContext
    {
        Task ExecuteAsync(T context);
    }
}