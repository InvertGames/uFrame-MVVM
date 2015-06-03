using UniRx;
using UnityEngine;
using System;namespace uFrame.MVVM.Bindings {
public class ObservableCollisionStayBehaviour : ObservableComponent
{
    private Subject<Collision> onCollisionStay;

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public void OnCollisionStay(Collision collision)
    {
        if (onCollisionStay != null) onCollisionStay.OnNext(collision);
    }

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public IObservable<Collision> OnCollisionStayAsObservable()
    {
        return onCollisionStay ?? (onCollisionStay = new Subject<Collision>());
    }


}
}