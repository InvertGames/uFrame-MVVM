using System;
using System.Collections;
using UniRx;
[Obsolete]
public abstract class UFCommand<T> : ICommandWith<T>
{
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
    public event CommandEvent OnCommandExecuted;
    public event CommandEvent OnCommandExecuting;

    public object Sender { get; set; }

    public T Parameter { get; set; }

    object IParameterCommand.Parameter
    {
        get
        {
            return Parameter;
        }
        set
        {
            Parameter = (T)value;
        }
    }

    protected abstract void Perform(T arg);

    public void Execute()
    {
        OnOnCommandExecuting();
        Execute(Parameter);
        OnOnCommandComplete();
    }

    public void Execute(T arg)
    {
        Parameter = arg;
        Execute();
    }

    void ICommand.Execute(object arg)
    {
        Execute((T) arg);
    }

    public virtual bool CanExecute(T parameter)
    {
        return true;
    }

    bool ICommand.CanExecute(object parameter)
    {
        return CanExecute((T)parameter);
    }
}