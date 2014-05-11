using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ViewModelObserver : MonoBehaviour, IViewModelObserver
{
    private List<IBinding> _bindings;

    /// <summary>
    /// The bindings that are attached to this ViewModel
    /// </summary>
    public List<IBinding> Bindings
    {
        get { return _bindings ?? (_bindings = new List<IBinding>()); }
        set { _bindings = value; }
    }

    public virtual void AddBinding(IBinding binding)
    {
        Bindings.Add(binding);
    }

    public virtual void RemoveBinding(IBinding binding)
    {
        Bindings.Remove(binding);
    }

    public virtual void Unbind()
    {
        foreach (var binding in Bindings)
        {
            binding.Unbind();
        }

        // Remove all the bindings that are not from a component
        Bindings.RemoveAll(p => !p.IsComponent);
    }
}