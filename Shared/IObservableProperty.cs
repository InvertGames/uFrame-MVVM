using System;

public interface IObservableProperty
{
    object ObjectValue { get; set; }
    string PropertyName { get; }
#if !DLL
    ViewModel Owner { get; set; }
#endif
    Type ValueType { get; }
    IDisposable SubscribeInternal(Action<object> propertyChanged);
}