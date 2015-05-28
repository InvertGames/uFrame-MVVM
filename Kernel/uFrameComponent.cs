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
    public ViewBase InstantiateView(ViewModel model)
    {
        return InstantiateView(model, Vector3.zero);
    }

    public ViewBase InstantiateView(ViewModel model, Vector3 position)
    {
        return InstantiateView(model, position, Quaternion.identity);
    }

    public ViewBase InstantiateView(ViewModel model, Vector3 position, Quaternion rotation)
    {
        return transform.InstantiateView(model, position, rotation);
    }

    public ViewBase InstantiateView(GameObject prefab, ViewModel model)
    {
        return InstantiateView(prefab, model, Vector3.zero);
    }

    public ViewBase InstantiateView(GameObject prefab, ViewModel model, Vector3 position)
    {
        return InstantiateView(prefab, model, position, Quaternion.identity);
    }

    public ViewBase InstantiateView(string viewName, string identifier = null)
    {
        return InstantiateView(viewName, null,identifier);
    }

    /// <summary>
    /// Instantiates a view.
    /// </summary>
    /// <param name="viewName">The name of the prefab/view to instantiate</param>
    /// <param name="model">The model that will be passed to the view.</param>
    /// <returns>The instantiated view</returns>
    public ViewBase InstantiateView(string viewName, ViewModel model, string identifier = null)
    {
        return InstantiateView(viewName, model, Vector3.zero,identifier);
    }
    /// <summary>
    /// Instantiates a view.
    /// </summary>
    /// <param name="viewName">The name of the prefab/view to instantiate</param>
    
    /// <param name="position">The position to instantiate the view.</param>
    /// <returns>The instantiated view</returns>
    public ViewBase InstantiateView(string viewName, Vector3 position, string identifier = null)
    {
        return InstantiateView(viewName, null, position, Quaternion.identity, identifier);
    }

    /// <summary>
    /// Instantiates a view.
    /// </summary>
    /// <param name="viewName">The name of the prefab/view to instantiate</param>
    /// <param name="model">The model that will be passed to the view.</param>
    /// <param name="position">The position to instantiate the view.</param>
    /// <returns>The instantiated view</returns>
    public ViewBase InstantiateView(string viewName, ViewModel model, Vector3 position, string identifier = null)
    {
        return InstantiateView(viewName, model, position, Quaternion.identity,identifier);
    }

    /// <summary>
    /// Instantiates a view.
    /// </summary>
    /// <param name="viewName">The name of the prefab/view to instantiate</param>
    /// <param name="model">The model that will be passed to the view.</param>
    /// <param name="position">The position to instantiate the view.</param>
    /// <param name="rotation">The rotation to instantiate the view with.</param>
    /// <returns>The instantiated view</returns>
    public ViewBase InstantiateView(string viewName, ViewModel model, Vector3 position,
        Quaternion rotation, string identifier = null)
    {
        return transform.InstantiateView(viewName, model, position, rotation,identifier);
    }

    /// <summary>
    /// Instantiates a view.
    /// </summary>
    /// <param name="prefab">The prefab/view to instantiate</param>
    /// <param name="model">The model that will be passed to the view.</param>
    /// <param name="position">The position to instantiate the view.</param>
    /// <param name="rotation">The rotation to instantiate the view with.</param>
    /// <returns>The instantiated view</returns>
    public ViewBase InstantiateView(GameObject prefab, ViewModel model, Vector3 position,
        Quaternion rotation, string identifier = null)
    {
        return transform.InstantiateView(prefab, model, position, rotation,identifier);
    }


}