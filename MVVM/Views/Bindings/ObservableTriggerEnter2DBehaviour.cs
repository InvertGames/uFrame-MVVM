using UniRx;
using UnityEngine;
using System;namespace uFrame.MVVM.Bindings {
public class ObservableTriggerEnter2DBehaviour : ObservableComponent
{
    private Subject<Collider2D> onTriggerEnter2D;

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (onTriggerEnter2D != null) onTriggerEnter2D.OnNext(other);
    }

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public IObservable<Collider2D> OnTriggerEnter2DAsObservable()
    {
        return onTriggerEnter2D ?? (onTriggerEnter2D = new Subject<Collider2D>());
    }


}
}