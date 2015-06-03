using System;
using System.Collections.Generic;
using System.ComponentModel;
using UniRx;

namespace uFrame.MVVM
{
    public class P<T> : ISubject<T>, IObservableProperty, INotifyPropertyChanged
    {
        private object _objectValue;
        private object _lastValue;

        public IObservable<T> ChangedObservable
        {
            get { return this.Where(p => ObjectValue != LastValue); }
        }

        public object ObjectValue
        {
            get { return _objectValue ?? default(T); }
            set
            {
                _lastValue = _objectValue;
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

        public P()
        {
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
            var disposer = Disposable.Create(() => PropertyChanged -= evt);
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

        public IObservable<Unit> AsUnit
        {
            get { return this.Select(p => Unit.Default); }
        }

        public Func<T> Computer { get; set; }

        public IDisposable ToComputed(Func<T> action, params IObservableProperty[] properties)
        {
            Computer = action;
            var disposables = new List<IDisposable>();
            foreach (var property in properties)
            {
                disposables.Add(property.SubscribeInternal(_ =>
                {
                    OnNext(action());
                }));
            }

            //OnNext(action());

            return Disposable.Create(() =>
            {
                foreach (var d in disposables)
                    d.Dispose();
            });
        }

        ///// <summary>
        ///// Makes this property a computed variable.
        ///// </summary>
        ///// <param name="action"></param>
        ///// <param name="properties"></param>
        ///// <returns></returns>
        //public IDisposable ToComputed(Func<ViewModel, T> action, params IObservableProperty[] properties)
        //{
        //    var disposables = new List<IDisposable>();
        //    foreach (var property in properties)
        //    {
        //        disposables.Add(property.SubscribeInternal(_ =>
        //        {
        //            OnNext(action(Owner));
        //        }));
        //    }
        //    action(Owner);

        //    return Disposable.Create(() =>
        //    {
        //        foreach (var d in disposables)
        //            d.Dispose();
        //    });
        //}

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
            get { return typeof (T); }
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {

            PropertyChangedEventHandler evt = delegate { observer.OnNext(Value); };
            PropertyChanged += evt;
            return Disposable.Create(() => PropertyChanged -= evt);
        }

        public T Value
        {
            get { return (T) ObjectValue; }
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
}