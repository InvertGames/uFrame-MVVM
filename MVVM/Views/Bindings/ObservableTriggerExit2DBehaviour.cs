using UniRx;
using UnityEngine;
using System;namespace uFrame.MVVM.Bindings {
public class ObservableTriggerExit2DBehaviour : ObservableComponent
{
    private Subject<Collider2D> onTriggerExit2D;

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public void OnTriggerExit2D(Collider2D other)
    {
        if (onTriggerExit2D != null) onTriggerExit2D.OnNext(other);
    }

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public IObservable<Collider2D> OnTriggerExit2DAsObservable()
    {
        return onTriggerExit2D ?? (onTriggerExit2D = new Subject<Collider2D>());
    }


}
}