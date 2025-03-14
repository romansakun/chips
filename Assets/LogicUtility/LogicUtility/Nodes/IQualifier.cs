using System.Threading.Tasks;

namespace LogicUtility
{
    public interface IQualifier<T> : INode<T> where T : class, IContext
    {
        Task<float> ScoreAsync(T context);
    }
}