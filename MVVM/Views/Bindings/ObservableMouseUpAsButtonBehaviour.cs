using UniRx;
using UnityEngine;
using System;namespace uFrame.MVVM.Bindings {
public class ObservableMouseUpAsButtonBehaviour : ObservableComponent
{
    private Subject<Unit> onMouseUpAsButton;

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public void OnMouseUpAsButton()
    {
        if (onMouseUpAsButton != null) onMouseUpAsButton.OnNext(Unit.Default);
    }

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public IObservable<Unit> OnMouseUpAsButtonAsObservable()
    {
        return onMouseUpAsButton ?? (onMouseUpAsButton = new Subject<Unit>());
    }


}
}