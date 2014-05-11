using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

/// <summary>
/// Base class for a command binding.  Use this class if a different type of command binding is needed.
/// </summary>
public abstract class CommandBinding : Binding
{
    protected readonly List<Action> _UnbindActions = new List<Action>();

    private ICommand _command;

    private Func<ICommand> _commandDelegate;
    private List<Predicate<object>> _conditions;

    public object Argument { get; set; }

    public ICommand Command
    {
        get { return _command ?? (_command = CommandDelegate()); }
        set { _command = value; }
    }

    public Func<ICommand> CommandDelegate
    {
        get
        {
            return _commandDelegate ?? (
                _commandDelegate = () =>
                    {
                        var @base = Source as ViewBase;
                        if (@base != null)
                        {
                            return @base.ViewModelObject.Commands[ModelMemberName];
                        }
                        throw new Exception("Command Binding CommandDelegate couldn't be set automatically.  Try setting it manually.");
                    }
                );
        }
        set
        {
            _commandDelegate = value;
        }
    }

    public bool ExecuteBefore { get; set; }

    protected List<Predicate<object>> Conditions
    {
        get { return _conditions ?? (_conditions = new List<Predicate<object>>()); }
        set { _conditions = value; }
    }

    private Func<object> CommandArgumentSelector { get; set; }

    //private event CommandEvent _subscription;
    public override void Bind()
    {
        base.Bind();
    }

    public bool CanExecute()
    {
        if (Command == null) return false;

        foreach (var condition in Conditions)
        {
            if (condition(Argument)) continue;
            return false;
        }
        return true;
    }

    public void ExecuteCommand()
    {
        if (CanExecute())
        {
            Command.Parameter = GetArgument();
            Source.ExecuteCommand(Command);
        }
    }

    public virtual object GetArgument()
    {
        if (CommandArgumentSelector != null)
        {
            return CommandArgumentSelector();
        }
        return null;
    }

    public CommandBinding SetParameter(object value)
    {
        CommandArgumentSelector = () => value;
        return this;
    }

    public CommandBinding SetParameterSelector(Func<object> commandArgSelector)
    {
        CommandArgumentSelector = commandArgSelector;
        return this;
    }

    public CommandBinding Subscribe(Action execute, bool before = false)
    {
        var commandEvent = new CommandEvent(() => execute());

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

    public CommandBinding Throttle(float seconds)
    {
        var lastThrottleTime = Time.time;
        Conditions.Add(_ =>
        {
            var result = (Time.time - lastThrottleTime) >= seconds;
            if (result)
            {
                lastThrottleTime = Time.time;
            }
            return result;
        });
        return this;
    }

    public override void Unbind()
    {
        base.Unbind();
        Conditions.RemoveAll(p => true);
        foreach (var unbindAction in _UnbindActions)
        {
            unbindAction();
        }
        _UnbindActions.Clear();
        _command = null;
    }

    public CommandBinding When(Func<bool> condition)
    {
        Conditions.Add(_ => condition());
        return this;
    }
}