using UniRx;
using UnityEngine;
using System;namespace uFrame.MVVM.Bindings {
public class ObservableTriggerEnterBehaviour : ObservableComponent
{
    private Subject<Collider> onTriggerEnter;

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public void OnTriggerEnter(Collider other)
    {
        if (onTriggerEnter != null) onTriggerEnter.OnNext(other);
    }

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public IObservable<Collider> OnTriggerEnterAsObservable()
    {
        return onTriggerEnter ?? (onTriggerEnter = new Subject<Collider>());
    }


}
}