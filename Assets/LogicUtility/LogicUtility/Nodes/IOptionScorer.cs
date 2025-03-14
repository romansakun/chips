using System;
using System.Threading.Tasks;

namespace LogicUtility.Nodes
{
    public interface IOptionScorer<T> where T : class, IContext
    {
        Task<float> ScoreAsync<TO>(T context, TO option) where TO : IEquatable<TO>;
    }
}