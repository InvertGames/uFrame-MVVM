using UniRx;
using UnityEngine;
using System;namespace uFrame.MVVM.Bindings {
public class ObservableTriggerStayBehaviour : ObservableComponent
{
    private Subject<Collider> onTriggerStay;

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public void OnTriggerStay(Collider other)
    {
        if (onTriggerStay != null) onTriggerStay.OnNext(other);
    }

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public IObservable<Collider> OnTriggerStayAsObservable()
    {
        return onTriggerStay ?? (onTriggerStay = new Subject<Collider>());
    }


}
}