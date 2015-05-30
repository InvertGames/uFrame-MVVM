using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UniRx;
using UnityEngine;

/// <summary>
/// A base class for all view containers.
/// Simply just utility methods for views and events.
/// </summary>
public class uFrameComponent : MonoBehaviour
{

    protected IEventAggregator EventAggregator { get { return uFrameMVVMKernel.EventAggregator; } }

    public IObservable<TEvent> OnEvent<TEvent>()
    {
        return EventAggregator.GetEvent<TEvent>();
    }

    public void Publish(object eventMessage)
    {
        EventAggregator.Publish(eventMessage);
    }
    protected virtual IEnumerator Start()
    {
        KernelLoading();
        while (!uFrameMVVMKernel.IsKernelLoaded) yield return null;
        KernelLoaded();
    }
    /// <summary>
    /// Before we wait for the kernel to load, even if the kernel is already loaded it will still invoke this before it attempts to wait.
    /// </summary>
    public virtual void KernelLoading()
    {
        
    }

    /// <summary>
    /// The first method to execute when we are sure the kernel has completed loading.
    /// </summary>
    public virtual void KernelLoaded()
    {
        
    }
   


}

