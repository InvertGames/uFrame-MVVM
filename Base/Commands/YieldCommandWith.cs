using System;
using System.Collections;
using UniRx;

/// <summary>
/// A coroutine command with a parameter.
/// </summary>
/// <typeparam name="T"></typeparam>
[Obsolete]
public class YieldCommandWith<T> : ICommandWith<T>
{
    public event CommandEvent OnCommandExecuted;

    public event CommandEvent OnCommandExecuting;

    public void Execute(object parameter)
    {
        Parameter = parameter;
        Execute();
    }

    public bool CanExecute(object parameter)
    {
        throw new NotImplementedException();
    }

    public object Sender { get; set; }

    public object Parameter { get; set; }

    protected Func<T, IEnumerator> EnumeratorDelegate
    {
        get;
        set;
    }

    public YieldCommandWith(Func<T, IEnumerator> enumeratorDelegate)
    {
        EnumeratorDelegate = enumeratorDelegate;
    }

    public YieldCommandWith(T sender, Func<T, IEnumerator> enumeratorDelegate)
    {
        Parameter = sender;
        EnumeratorDelegate = enumeratorDelegate;
    }

    public void Execute()
    {
        OnOnCommandExecuting();
        if (EnumeratorDelegate != null)
        {
            OnOnCommandComplete();
        }
        else
        {
            OnOnCommandComplete();
        }

    }

    protected virtual void OnOnCommandComplete()
    {
        CommandEvent handler = OnCommandExecuted;
        if (handler != null) handler();
    }

    protected virtual void OnOnCommandExecuting()
    {
        CommandEvent handler = OnCommandExecuting;
        if (handler != null) handler();
    }
    public void OnCompleted()
    {

    }

    public void OnError(Exception error)
    {
        throw error;
    }

    public void OnNext(Unit value)
    {
        Execute();
    }

    public IDisposable Subscribe(IObserver<Unit> observer)
    {
        CommandEvent handler = () => observer.OnNext(Unit.Default);
        this.OnCommandExecuted += handler;

        return Disposable.Create(() => OnCommandExecuted -= handler);
    }

    T ICommandWith<T>.Parameter
    {
        get
        {
            return (T)((IParameterCommand)this).Parameter;
        }
        set
        {
            ((IParameterCommand)this).Parameter = value;
        }
    }

    public void Execute(T parameter)
    {
        ((ICommand) this).Execute(parameter);
    }

    public bool CanExecute(T parameter)
    {
        return ((ICommand) this).CanExecute(parameter);
    }
}
/// <summary>
/// A coroutine command with a parameter.
/// </summary>
/// <typeparam name="T"></typeparam>
[Obsolete]
public class YieldCommandWithSender<T> : ICommandWith<T>
{
    public event CommandEvent OnCommandExecuted;

    public event CommandEvent OnCommandExecuting;

    public void Execute(object parameter)
    {
        Parameter = parameter;
        Execute();
    }

    public bool CanExecute(object parameter)
    {
        throw new NotImplementedException();
    }

    public object Sender { get; set; }

    public object Parameter { get; set; }

    T ICommandWith<T>.Parameter
    {
        get
        {
            return (T)((IParameterCommand)this).Parameter;
        }
        set
        {
            ((IParameterCommand)this).Parameter = value;
        }
    }

    public void Execute(T parameter)
    {
        ((ICommand) this).Execute(parameter);
    }

    public bool CanExecute(T parameter)
    {
        return ((ICommand) this).CanExecute(parameter);
    }

    protected Func<T, IEnumerator> EnumeratorDelegate
    {
        get;
        set;
    }

    public YieldCommandWithSender(Func<T, IEnumerator> enumeratorDelegate)
    {
        EnumeratorDelegate = enumeratorDelegate;
    }

    public YieldCommandWithSender(T sender, Func<T, IEnumerator> enumeratorDelegate)
    {
        Sender = sender;
        EnumeratorDelegate = enumeratorDelegate;
    }

    public void Execute()
    {
        OnOnCommandExecuting();
        if (EnumeratorDelegate != null)
        {
            //var result = EnumeratorDelegate((T)Sender);
            OnOnCommandComplete();
            //return result;
        }
        else
        {
            OnOnCommandComplete();
        }

        //return null;
    }

    protected virtual void OnOnCommandComplete()
    {
        CommandEvent handler = OnCommandExecuted;
        if (handler != null) handler();
    }

    protected virtual void OnOnCommandExecuting()
    {
        CommandEvent handler = OnCommandExecuting;
        if (handler != null) handler();
    }
    public void OnCompleted()
    {

    }

    public void OnError(Exception error)
    {
        throw error;
    }

    public void OnNext(Unit value)
    {
        Execute();
    }

    public IDisposable Subscribe(IObserver<Unit> observer)
    {
        CommandEvent handler = () => observer.OnNext(Unit.Default);
        this.OnCommandExecuted += handler;

        return Disposable.Create(() => OnCommandExecuted -= handler);
    }
}

/// <summary>
/// A coroutine command with a parameter.
/// </summary>
/// <typeparam name="TSender"></typeparam>
/// <typeparam name="TArgument"></typeparam>
[Obsolete]
public class YieldCommandWithSenderAndArgument<TSender, TArgument> : ICommandWith<TArgument>
{
    public event CommandEvent OnCommandExecuted;

    public event CommandEvent OnCommandExecuting;


    public void Execute(object parameter)
    {
        Parameter = parameter;
        Execute();
    }

    public bool CanExecute(object parameter)
    {
        throw new NotImplementedException();
    }

    public object Sender { get; set; }

    public object Parameter { get; set; }

    TArgument ICommandWith<TArgument>.Parameter
    {
        get
        {
            return (TArgument)((IParameterCommand)this).Parameter;
        }
        set
        {
            ((IParameterCommand)this).Parameter = value;
        }
    }

    public void Execute(TArgument parameter)
    {
        ((ICommand) this).Execute(parameter);
    }

    public bool CanExecute(TArgument parameter)
    {
        return ((ICommand) this).CanExecute(parameter);
    }

    protected Func<TSender, TArgument, IEnumerator> EnumeratorDelegate
    {
        get;
        set;
    }

    public YieldCommandWithSenderAndArgument(Func<TSender,TArgument, IEnumerator> enumeratorDelegate)
    {
        EnumeratorDelegate = enumeratorDelegate;
    }

    public YieldCommandWithSenderAndArgument(TSender sender, Func<TSender, TArgument, IEnumerator> enumeratorDelegate)
    {
        Sender = sender;
        EnumeratorDelegate = enumeratorDelegate;
    }

    public void Execute()
    {
        OnOnCommandExecuting();
        if (EnumeratorDelegate != null)
        {
            OnOnCommandComplete(); 
        }
        else
        {
            OnOnCommandComplete();
        }
    }

    protected virtual void OnOnCommandComplete()
    {
        CommandEvent handler = OnCommandExecuted;
        if (handler != null) handler();
    }

    protected virtual void OnOnCommandExecuting()
    {
        CommandEvent handler = OnCommandExecuting;
        if (handler != null) handler();
    }
    public void OnCompleted()
    {

    }

    public void OnError(Exception error)
    {
        throw error;
    }

    public void OnNext(Unit value)
    {
        Execute();
    }

    public IDisposable Subscribe(IObserver<Unit> observer)
    {
        CommandEvent handler = () => observer.OnNext(Unit.Default);
        this.OnCommandExecuted += handler;

        return Disposable.Create(() => OnCommandExecuted -= handler);
    }
}