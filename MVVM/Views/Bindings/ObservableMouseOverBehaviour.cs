using UniRx;
using UnityEngine;
using System;namespace uFrame.MVVM.Bindings {
public class ObservableMouseOverBehaviour : ObservableComponent
{
    private Subject<Unit> onMouseOver;

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public void OnMouseOver()
    {
        if (onMouseOver != null) onMouseOver.OnNext(Unit.Default);
    }

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public IObservable<Unit> OnMouseOverAsObservable()
    {
        return onMouseOver ?? (onMouseOver = new Subject<Unit>());
    }


}
}