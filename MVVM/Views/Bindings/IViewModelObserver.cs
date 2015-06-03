using System.Collections.Generic;
using UnityEngine;

namespace uFrame.MVVM.Bindings
{
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
}