using System;
using System.Collections;
#if DLL
namespace Invert.MVVM
{
#endif
using UniRx;
[Obsolete("Due to AOT issues this has been deprecated.")]
public delegate void CommandEvent();

/// <summary>
/// The base command interface for implementing a command in a ViewModel
/// </summary>
[Obsolete("Due to AOT issues this has been deprecated.")]
public interface ICommand : ISubject<Unit>
{
   
   
    //event EventHandler CanExecuteChanged;
    event CommandEvent OnCommandExecuted;
    event CommandEvent OnCommandExecuting; 

    //object Sender { get; set; }
    //object Parameter { get; set; }
    
    //void Execute();
    void Execute(object parameter);
    bool CanExecute(object parameter);
}

[Obsolete("Due to AOT issues this has been deprecated.")]
public interface IParameterCommand : ICommand
{
    object Parameter { get; set; }
}
/// <summary>
/// A base command interface for implementing a command with a parameter in a ViewModel
/// </summary>
/// <typeparam name="T"></typeparam>
[Obsolete("Due to AOT issues this has been deprecated.")]
public interface ICommandWith<T> : IParameterCommand
{
    //IEnumerator Execute(T parameter);

    new T Parameter { get; set; }

    void Execute(T parameter);
    bool CanExecute(T parameter);
}
#if DLL
}
#endif