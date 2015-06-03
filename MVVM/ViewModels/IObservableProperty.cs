using System;
using UniRx;

namespace uFrame.MVVM
{
    public interface IObservableProperty
    {
        object ObjectValue { get; set; }
        string PropertyName { get; }
#if !DLL
        ViewModel Owner { get; set; }
#endif
        Type ValueType { get; }
        IObservable<Unit> AsUnit { get; }
        IDisposable SubscribeInternal(Action<object> propertyChanged);
    }

    public static class ObservablePropertyExtensions
    {
        public static P<T> AsP<T>(this IObservableProperty property)
        {
            return property as P<T>;
        }
    }
}