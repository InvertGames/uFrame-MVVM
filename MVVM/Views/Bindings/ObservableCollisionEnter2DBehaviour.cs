using System;
using UniRx;
using UnityEngine;

namespace uFrame.MVVM.Bindings
{
    public class ObservableCollisionEnter2DBehaviour : ObservableComponent
    {
        private Subject<Collision2D> onCollisionEnter2D;

        /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (onCollisionEnter2D != null) onCollisionEnter2D.OnNext(collision);
        }

        /// <summary>OnMouseEnter is called when the mouse entered the GUIElement or Collider.</summary>
        public IObservable<Collision2D> OnCollisionEnter2DAsObservable()
        {
            return onCollisionEnter2D ?? (onCollisionEnter2D = new Subject<Collision2D>());
        }


    }

    public class ObservableComponent : MonoBehaviour, IDisposable
    {
        public void Dispose()
        {
            Destroy(this);
        }
    }
}