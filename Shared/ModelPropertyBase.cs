using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using UniRx;
using UnityEngine;

public class P<T> : ISubject<T>, IObservableProperty, INotifyPropertyChanged
{
    private object _objectValue;
    private object _lastValue;

    public object ObjectValue
    {
        get
        {
            return _objectValue ?? default(T);
        }
        set
        {
            _lastValue = value;
            _objectValue = value;
            OnPropertyChanged(PropertyName);
           
        }
    }
    public string PropertyName { get; set; }
#if !DLL
    public ViewModel Owner { get; set; }
#endif
    public IDisposable SubscribeInternal(Action<object> propertyChanged)
    {
        return this.Subscribe((v) => propertyChanged(v));
    }

    //public IDisposable SubscribeInternal(Action<object> obj)
    //{
    //    this.Subscribe(obj);
    //}

    public object LastValue
    {
        get { return _lastValue; }
        set { _lastValue = value; }
    }

    public IDisposable Subscribe(IObserver<object> observer)
    {
        PropertyChangedEventHandler evt = delegate { observer.OnNext(ObjectValue); };
        PropertyChanged += evt;
        var disposer = new SimpleDisposable(() => PropertyChanged -= evt);
        if (Owner != null)
        {
            Owner.AddBinding(disposer);
        }
        return disposer;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        if (Owner != null)
        Owner.OnPropertyChanged(this, PropertyName);
    }


    public P(T value)
    {
        _objectValue = value;
    }
#if !DLL
    public P(ViewModel owner, string propertyName)
    {
        Owner = owner;
        PropertyName = propertyName;

    }
#endif
    public Type ValueType
    {
        get { return typeof(T); }
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
        
        PropertyChangedEventHandler evt = delegate { observer.OnNext(Value); };
        PropertyChanged += evt;
        return new SimpleDisposable(() => PropertyChanged -= evt);
    }


    public T Value
    {
        get { return (T)ObjectValue; }
        set { ObjectValue = value; }
    }

    public void OnCompleted()
    {
        
    }

    public void OnError(Exception error)
    {
        
    }

    public void OnNext(T value)
    {
        ObjectValue = value;
    }
}