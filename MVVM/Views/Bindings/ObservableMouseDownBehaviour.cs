using UniRx;
using UnityEngine;
using System;namespace uFrame.MVVM.Bindings {
public class ObservableMouseDownBehaviour : ObservableComponent
{
    private Subject<Unit> onMouseDown;

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public void OnMouseDown()
    {
        if (onMouseDown != null) onMouseDown.OnNext(Unit.Default);
    }

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public IObservable<Unit> OnMouseDownAsObservable()
    {
        return onMouseDown ?? (onMouseDown = new Subject<Unit>());
    }


}

public class ObservableOnDestroyBehaviour : ObservableComponent
{
    private Subject<Unit> onDestroy;

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public void OnDestroy()
    {
        if (onDestroy != null) onDestroy.OnNext(Unit.Default);
    }

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public IObservable<Unit> OnDestroyAsObservable()
    {
        return onDestroy ?? (onDestroy = new Subject<Unit>());
    }

}
}