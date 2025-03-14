using System;

namespace UI
{
    public interface IReactiveProperty<out T> : IDisposable
    {
        T Value { get; }
        void Subscribe(Action<T> action, bool withForceAction = true);
        void Unsubscribe(Action<T> action);
    }

}