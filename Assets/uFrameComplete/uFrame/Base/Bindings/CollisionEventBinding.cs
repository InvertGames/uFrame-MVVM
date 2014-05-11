using System;
using UnityEngine;

/// <summary>
/// A component for binding to a collision.
/// </summary>
public class CollisionEventBinding : ComponentCommandBinding
{
    public CollisionEventType _CollisionEvent;

    /// <summary>
    /// The binding provider.  Create the binding that the component will add to the source view here.
    /// </summary>
    /// <returns>The binding that will be added to the source view.</returns>
    protected override IBinding GetBinding()
    {
        return new ModelCollisionEventBinding()
        {
            ModelMemberName = _ModelMemberName,
            Source = _SourceView,
            CollisionEvent = _CollisionEvent,
        };
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        HandleCollision(CollisionEventType.OnCollisionEnter, collision.gameObject);
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        HandleCollision(CollisionEventType.OnCollisionExit, collision.gameObject);
    }

    protected virtual void OnCollisionStay(Collision collision)
    {
        HandleCollision(CollisionEventType.OnCollisionStay, collision.gameObject);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        HandleCollision(CollisionEventType.OnTriggerEnter, other.gameObject);
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        HandleCollision(CollisionEventType.OnTriggerExit, other.gameObject);
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        HandleCollision(CollisionEventType.OnTriggerStay, other.gameObject);
    }

    private void HandleCollision(CollisionEventType eventType, GameObject c)
    {
        var binding = CommandBinding as ModelCollisionEventBinding;
        if (binding == null) return;
        if (eventType != binding.CollisionEvent) return;
        if (binding.Source == null) return;
//        if (!binding.Source.enabled) return;

        binding.Argument = c;
        binding.ExecuteCommand();
    }
}