using UniRx;
using UnityEngine;
using System;namespace uFrame.MVVM.Bindings {
public class ObservableMouseExitBehaviour : ObservableComponent
{
    private Subject<Unit> onMouseExit;

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public void OnMouseExit()
    {
        if (onMouseExit != null) onMouseExit.OnNext(Unit.Default);
    }

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public IObservable<Unit> OnMouseExitAsObservable()
    {
        return onMouseExit ?? (onMouseExit = new Subject<Unit>());
    }


}
}