using UniRx;
using UnityEngine;
using System;
namespace uFrame.MVVM.Bindings {
public class ObservableCollisionEnterBehaviour : ObservableComponent
{
    private Subject<Collision> onCollisionEnter;

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public void OnCollisionEnter(Collision collision)
    {
        if (onCollisionEnter != null) onCollisionEnter.OnNext(collision);
    }

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public IObservable<Collision> OnCollisionEnterAsObservable()
    {
        return onCollisionEnter ?? (onCollisionEnter = new Subject<Collision>());
    }


}
}