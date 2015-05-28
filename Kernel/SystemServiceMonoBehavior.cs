using System.Collections;
using UniRx;

/// <summary>
/// The base class for all services on the kernel.  Services provide an easy communication layer with the use
/// of the EventAggregator.  You can use this.Publish(new AnyType()).  Or you can use this.OnEvent&lt;AnyType&gt;().Subscribe(anyTypeInstance=>{ });
/// In services you can also inject any instances that are setup in any of the SystemLoaders.
/// </summary>
public abstract class SystemServiceMonoBehavior : UnityEngine.MonoBehaviour, ISystemService
{
    /// <summary>
    /// The Event Aggregator used for listening and publishing commands.
    /// </summary>
    [Inject]
    public IEventAggregator EventAggregator { get; set; }

    /// <summary>
    /// This method is to setup an listeners on the EventAggregator, or other initialization requirements.
    /// </summary>
    public virtual void Setup()
    {
        
    }
    /// <summary>
    /// This method is called by the kernel to do any setup the make take some time to complete.  It is executed as 
    /// a co-routine by the kernel.
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator SetupAsync()
    {
        yield break;
    }

    public virtual void Dispose()
    {
        
    }

    /// <summary>
    /// A wrapper for GetEvent on the EventAggregator GetEvent method.
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns>An observable capable of subscriptions and filtering.</returns>
    public IObservable<TEvent> OnEvent<TEvent>()
    {
        return EventAggregator.GetEvent<TEvent>();
    }

    /// <summary>
    /// A wrapper for the Event Aggregator.Publish method.
    /// </summary>
    /// <param name="eventMessage"></param>
    public void Publish(object eventMessage)
    {
        EventAggregator.Publish(eventMessage);
    }
}