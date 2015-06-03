using UniRx;
using UnityEngine;
using System;namespace uFrame.MVVM.Bindings {
public class ObservableMouseEnterBehaviour : ObservableComponent
{
    Subject<Unit> onMouseEnter;

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public void OnMouseEnter()
    {
        if (onMouseEnter != null) onMouseEnter.OnNext(Unit.Default);
    }

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public IObservable<Unit> OnMouseEnterAsObservable()
    {
        return onMouseEnter ?? (onMouseEnter = new Subject<Unit>());
    }
}
}