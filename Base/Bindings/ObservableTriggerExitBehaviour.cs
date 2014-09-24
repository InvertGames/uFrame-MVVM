using UniRx;
using UnityEngine;

public class ObservableTriggerExitBehaviour : ObservableComponent
{
    private Subject<Collider> onTriggerExit;

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public void OnTriggerExit(Collider other)
    {
        if (onTriggerExit != null) onTriggerExit.OnNext(other);
    }

    /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
    public IObservable<Collider> OnTriggerExitAsObservable()
    {
        return onTriggerExit ?? (onTriggerExit = new Subject<Collider>());
    }


}