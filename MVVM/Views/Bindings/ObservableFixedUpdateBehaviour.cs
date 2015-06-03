using UniRx;
using UnityEngine;
using System;namespace uFrame.MVVM.Bindings {
public class ObservableFixedUpdateBehaviour : ObservableComponent
{
    private Subject<Unit> onFixedUpdate;

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public void FixedUpdate()
    {
        if (onFixedUpdate != null) onFixedUpdate.OnNext(Unit.Default);
    }

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public IObservable<Unit> OnFixedUpdateAsObservable()
    {
        return onFixedUpdate ?? (onFixedUpdate = new Subject<Unit>());
    }
}
}