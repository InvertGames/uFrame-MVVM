using System;
using System.Collections;

/// <summary>
/// A ViewModel command that can be executed.
/// IEnumerator is always used so that any command can be a coroutine.
/// </summary>
public class Command : ICommand
{
    public event CommandEvent OnCommandExecuted;

    public event CommandEvent OnCommandExecuting;
    public object Sender { get; set; }

    public object Parameter { get; set; }

    protected Action Delegate { get; set; }

    public Command(Action @delegate)
    {
        Delegate = @delegate;
    }

    public IEnumerator Execute()
    {
        OnOnCommandExecuting();
        if (Delegate != null)
        {
            Delegate();
        }
        OnOnCommandComplete();
        return null;
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
}

public class YieldCommand : ICommand
{
    public event CommandEvent OnCommandExecuted;

    public event CommandEvent OnCommandExecuting;
    public object Sender { get; set; }

    public object Parameter { get; set; }

    protected Func<IEnumerator> EnumeratorDelegate { get; set; }

    public YieldCommand(Func<IEnumerator> enumeratorDelegate)
    {
        EnumeratorDelegate = enumeratorDelegate;
    }

    public IEnumerator Execute()
    {
        OnOnCommandExecuting();
        if (EnumeratorDelegate != null)
        {
            var result = EnumeratorDelegate();
            OnOnCommandComplete();
            return result;
        }
        else
        {
            OnOnCommandComplete();
        }
        return null;
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
}