using UniRx;
using UnityEngine;
using System;
namespace uFrame.MVVM.Bindings {
public class ObservableCollisionExit2DBehaviour : ObservableComponent
{
    private Subject<Collision2D> onCollisionExit2D;

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public void OnCollisionExit2D(Collision2D collision)
    {
        if (onCollisionExit2D != null) onCollisionExit2D.OnNext(collision);
    }

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public IObservable<Collision2D> OnCollisionExit2DAsObservable()
    {
        return onCollisionExit2D ?? (onCollisionExit2D = new Subject<Collision2D>());
    }


}
}