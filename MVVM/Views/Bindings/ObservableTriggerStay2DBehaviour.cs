using UniRx;
using UnityEngine;
using System;namespace uFrame.MVVM.Bindings {
public class ObservableTriggerStay2DBehaviour : ObservableComponent
{
    private Subject<Collider2D> onTriggerStay2D;

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public void OnTriggerStay2D(Collider2D other)
    {
        if (onTriggerStay2D != null) onTriggerStay2D.OnNext(other);
    }

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public IObservable<Collider2D> OnTriggerStay2DAsObservable()
    {
        return onTriggerStay2D ?? (onTriggerStay2D = new Subject<Collider2D>());
    }
}
}