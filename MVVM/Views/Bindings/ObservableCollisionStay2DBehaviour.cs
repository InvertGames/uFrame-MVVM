using UniRx;
using UnityEngine;
using System;namespace uFrame.MVVM.Bindings {
public class ObservableCollisionStay2DBehaviour : ObservableComponent
{
    private Subject<Collision2D> onCollisionStay2D;

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public void OnCollisionStay2D(Collision2D collision)
    {
        if (onCollisionStay2D != null) onCollisionStay2D.OnNext(collision);
    }

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public IObservable<Collision2D> OnCollisionStay2DAsObservable()
    {
        return onCollisionStay2D ?? (onCollisionStay2D = new Subject<Collision2D>());
    }


}
}