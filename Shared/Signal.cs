using System;
using UniRx;

public class Signal<TClass> : ISubject<TClass>, ISignal where TClass : ViewModelCommand, new()
{
    private readonly IEventAggregator _aggregator;
    private readonly ViewModel _viewModel;

    public Signal(ViewModel viewModel, IEventAggregator aggregator)
    {
        _aggregator = aggregator;
        _viewModel = viewModel;
    }

    public void OnCompleted()
    {

    }

    public void OnError(Exception error)
    {

    }

    public void OnNext(TClass value)
    {
        value.Sender = _viewModel;
        _aggregator.Publish(value);
    }

    public IDisposable Subscribe(IObserver<TClass> observer)
    {
        return _aggregator.GetEvent<TClass>().Subscribe(observer);
    }

    public Type SignalType
    {
        get { return typeof (TClass); }
    }

    public void Publish(object data)
    {
        OnNext(data as TClass);
    }

    public void Publish()
    {
        OnNext(new TClass()
        {
            Sender = _viewModel
        });
    }
}