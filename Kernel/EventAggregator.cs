using System;
using UniRx;

namespace uFrame.Kernel
{
    public class EventAggregator : IEventAggregator, ISubject<object>
    {
        bool isDisposed;
        readonly Subject<object> eventsSubject = new Subject<object>();

        public IObservable<TEvent> GetEvent<TEvent>()
        {
            return eventsSubject.Where(p =>
            {
                return p is TEvent;
            }).Select(delegate(object p)
            {
                return (TEvent)p;
            });
        }

        public void Publish<TEvent>(TEvent evt)
        {
            eventsSubject.OnNext(evt);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;
            eventsSubject.Dispose();
            isDisposed = true;
        }

        public void OnCompleted()
        {
            eventsSubject.OnCompleted();
        }

        public void OnError(Exception error)
        {
            eventsSubject.OnError(error);
        }

        public void OnNext(object value)
        {
            eventsSubject.OnNext(value);
        }

        public IDisposable Subscribe(IObserver<object> observer)
        {
            return eventsSubject.Subscribe(observer);
        }
    }
}
