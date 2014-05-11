using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Potential future use.
/// </summary>
public interface IViewModelObserver
{
    List<IBinding> Bindings { get; set; }

    //bool enabled { get; set; }

    //GameObject gameObject { get; }

    //Rigidbody rigidbody { get; }

    //Transform transform { get; }

    void AddBinding(IBinding binding);

    //void ExecuteCommand(ICommand command);

    //void ExecuteCommand<TArgument>(ICommandWith<TArgument> command, TArgument argument);
    //void ExecuteCommand(ICommand command, object argument);

    void RemoveBinding(IBinding binding);

    void Unbind();
}