using System;
using UniRx;
using UnityEngine;

namespace uFrame.MVVM.Bindings
{
    public static class ExtendedCollisionBinding
    {

        //Observables + Bindings for Collision with any unity monobeh

        public static IDisposable BindComponentCollisionWith<T>(this ViewBase t, CollisionEventType eventType,
            Action<T> collision) where T : MonoBehaviour
        {
            return t.AddBinding(OnComponentCollisionWith<T>(t.gameObject, eventType).Subscribe(collision));
        }

        public static IDisposable BindComponentCollision2DWith<T>(this ViewBase t, CollisionEventType eventType,
            Action<T> collision) where T : MonoBehaviour
        {
            return t.AddBinding(OnComponentCollision2DWith<T>(t.gameObject, eventType).Subscribe(collision));
        }

        public static IObservable<T> OnComponentCollisionWith<T>(this GameObject t, CollisionEventType eventType)
            where T : MonoBehaviour
        {
            return
                t.OnCollisionObservable(eventType)
                    .Where(p => p.gameObject.GetComponent<T>() != null)
                    .Select(p => p.gameObject.GetComponent<T>());
        }

        public static IObservable<T> OnComponentCollision2DWith<T>(this GameObject t, CollisionEventType eventType)
            where T : MonoBehaviour
        {
            return
                t.OnCollision2DObservable(eventType)
                    .Where(p => p.gameObject.GetComponent<T>() != null)
                    .Select(p => p.gameObject.GetComponent<T>());
        }

        //Observables and bindings for Trigger with any View
        public static IDisposable BindViewTriggerWith<T>(this ViewBase t, CollisionEventType eventType,
            Action<T> collision) where T : ViewBase
        {
            return t.AddBinding(OnViewTriggerWith<T>(t.gameObject, eventType).Subscribe(collision));
        }

        public static IDisposable BindViewTrigger2DWith<T>(this ViewBase t, CollisionEventType eventType,
            Action<T> collision) where T : ViewBase
        {
            return t.AddBinding(OnViewTrigger2DWith<T>(t.gameObject, eventType).Subscribe(collision));
        }

        public static IObservable<T> OnViewTriggerWith<T>(this GameObject t, CollisionEventType eventType)
            where T : ViewBase
        {
            return t.OnTriggerObservable(eventType).Where(p => p.GetView<T>() != null).Select(p => p.GetView<T>());
        }

        public static IObservable<T> OnViewTrigger2DWith<T>(this GameObject t, CollisionEventType eventType)
            where T : ViewBase
        {
            return
                t.OnTrigger2DObservable(eventType)
                    .Where(p => p.gameObject.GetView<T>() != null)
                    .Select(p => p.gameObject.GetView<T>());
        }

        //Observables and bindings for Trigger with any unity monobeh

        public static IDisposable BindComponentTriggerWith<T>(this ViewBase t, CollisionEventType eventType,
            Action<T> collision) where T : MonoBehaviour
        {
            return t.AddBinding(OnComponentTriggerWith<T>(t.gameObject, eventType).Subscribe(collision));
        }

        public static IDisposable BindComponentTrigger2DWith<T>(this ViewBase t, CollisionEventType eventType,
            Action<T> collision) where T : MonoBehaviour
        {
            return t.AddBinding(OnComponentTriggerWith2D<T>(t.gameObject, eventType).Subscribe(collision));
        }

        public static IObservable<T> OnComponentTriggerWith<T>(this GameObject t, CollisionEventType eventType)
            where T : MonoBehaviour
        {
            return
                t.OnTriggerObservable(eventType)
                    .Where(p => p.gameObject.GetComponent<T>() != null)
                    .Select(p => p.gameObject.GetComponent<T>());
        }

        public static IObservable<T> OnComponentTriggerWith2D<T>(this GameObject t, CollisionEventType eventType)
            where T : MonoBehaviour
        {
            return
                t.OnTrigger2DObservable(eventType)
                    .Where(p => p.gameObject.GetComponent<T>() != null)
                    .Select(p => p.gameObject.GetComponent<T>());
        }







    }
}