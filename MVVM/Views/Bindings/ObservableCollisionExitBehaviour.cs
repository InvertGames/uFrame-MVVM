using UniRx;
using UnityEngine;
using System;namespace uFrame.MVVM.Bindings {
public class ObservableCollisionExitBehaviour : ObservableComponent
{
    private Subject<Collision> onCollisionExit;

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public void OnCollisionExit(Collision collision)
    {
        if (onCollisionExit != null) onCollisionExit.OnNext(collision);
    }

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public IObservable<Collision> OnCollisionExitAsObservable()
    {
        return onCollisionExit ?? (onCollisionExit = new Subject<Collision>());
    }


}
}