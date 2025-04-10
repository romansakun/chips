using System;

namespace LogicUtility
{
    public interface ILogicAgent : IDisposable
    {
        IContext LogicContext { get; }
    }
}