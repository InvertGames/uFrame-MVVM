using System;
using System.Collections;
#if DLL
namespace Invert.MVVM
{
#endif
using UniRx;

public delegate void CommandEvent();

/// <summary>
/// The base command interface for implementing a command in a ViewModel
/// </summary>
public interface ICommand : ISubject<Unit>
{
   
   
    //event EventHandler CanExecuteChanged;
    event CommandEvent OnCommandExecuted;
    event CommandEvent OnCommandExecuting; 

    //object Sender { get; set; }
    //object Parameter { get; set; }
    
    void Execute();
    void Execute(object parameter);
    bool CanExecute(object parameter);
}

/// <summary>
/// A base command interface for implementing a command with a parameter in a ViewModel
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICommandWith<T> : ICommand
{
    //IEnumerator Execute(T parameter);
}
#if DLL
}
#endif
