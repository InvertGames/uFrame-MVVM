using UniRx;

public interface IEventAggregator
{
    IObservable<TEvent> GetEvent<TEvent>();
    void Publish<TEvent>(TEvent evt);
}