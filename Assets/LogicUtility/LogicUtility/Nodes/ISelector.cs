using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogicUtility
{
    public interface ISelector<T> : INode<T> where T : class, IContext
    {
        List<IQualifier<T>> Qualifiers { get; }

        Task<INode<T>> SelectAsync(T context);
    }
}