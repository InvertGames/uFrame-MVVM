using UniRx;
using UnityEngine;
using System;namespace uFrame.MVVM.Bindings {
public class ObservableMouseDragBehaviour : ObservableComponent
{
    private Subject<Unit> onMouseDrag;

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public void OnMouseDrag()
    {
        if (onMouseDrag != null) onMouseDrag.OnNext(Unit.Default);
    }

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public IObservable<Unit> OnMouseDragAsObservable()
    {
        return onMouseDrag ?? (onMouseDrag = new Subject<Unit>());
    }


}
}