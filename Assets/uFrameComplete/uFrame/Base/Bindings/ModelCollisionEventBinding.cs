using System;
using UnityEngine;

/// <summary>
/// A collision binding that will trigger a command when executed.
/// Use chaining when possible to provide additional options for this binding.
/// </summary>
public class ModelCollisionEventBinding : ModelCommandBinding
{
    /// <summary>
    /// The collision/trigger event that will invoke the command this is bound to.
    /// </summary>
    public CollisionEventType CollisionEvent { get; set; }

    private Func<GameObject, object> CommandArgumentSelector { get; set; }

    /// <summary>
    /// Overriden to supply the CommandArgumentSelector result value if its not equal to null
    /// </summary>
    /// <returns>The object that will be passed as the argument to the command.</returns>
    public override object GetArgument()
    {
        if (CommandArgumentSelector != null)
        {
            return CommandArgumentSelector(Argument as GameObject);
        }
        return base.GetArgument();
    }

    /// <summary>
    /// Set the parameter that will be passed to the command.
    /// </summary>
    /// <param name="commandArgSelector">A selector that will select the object to pass to the command with the collider as the first argument</param>
    /// <returns></returns>
    public ModelCollisionEventBinding SetParameterSelector(Func<GameObject, object> commandArgSelector)
    {
        CommandArgumentSelector = commandArgSelector;
        return this;
    }

    /// <summary>
    /// Subscribe to this collision binding with a reference to the collider.
    /// </summary>
    /// <param name="action">The action to perform with the collider as the parameter.</param>
    /// <param name="before">Execute the action before the action executes. Defaults to false.</param>
    /// <returns>This so it can be further chained.</returns>
    public CommandBinding Subscribe(Action<GameObject> action, bool before = false)
    {
        var commandEvent = new CommandEvent(() => action(this.Argument as GameObject));

        if (before)
        {
            Command.OnCommandExecuting += commandEvent;
            _UnbindActions.Add(() => Command.OnCommandExecuting -= commandEvent);
        }
        else
        {
            Command.OnCommandExecuted += commandEvent;
            _UnbindActions.Add(() => Command.OnCommandExecuted -= commandEvent);
        }

        return this;
    }

    /// <summary>
    /// A filter to determine when a collision should invoke the command this is bound to.
    /// </summary>
    /// <param name="predicate">Return true if the command should be invoked.  Use the GameObject parameter to filter colliders.</param>
    /// <returns>This so it can be further chained.</returns>
    public ModelCollisionEventBinding When(Predicate<GameObject> predicate)
    {
        if (predicate == null) throw new ArgumentNullException("predicate");
        Conditions.Add(o => predicate(Argument as GameObject));
        return this;
    }
}