using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Potential future use.
/// </summary>
public interface IViewModelObserver
{
    //List<IBinding> Bindings { get; set; }

    void AddBinding(IBinding binding);

    void RemoveBinding(IBinding binding);

    void Unbind();
}