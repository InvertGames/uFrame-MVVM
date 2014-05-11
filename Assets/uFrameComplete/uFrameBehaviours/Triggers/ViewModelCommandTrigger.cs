using System;
using System.Collections.Generic;

public class ViewModelCommandTrigger : UBCustomTrigger, IBindingProvider
{
    private Action _unbindAction;

    public string CommandName { get; set; }

    public ViewBase View { get; set; }

    public void Bind(ViewBase view)
    {
        var command = view.ViewModelObject.Commands[CommandName];

        var commandEvent = new CommandEvent(ExecuteSheet);
        command.OnCommandExecuted += commandEvent;

        _unbindAction = () => command.OnCommandExecuted -= commandEvent;
    }

    public override void Initialize(TriggerInfo triggerInfo, Dictionary<string, object> settings)
    {
        CommandName = triggerInfo.Data;
    }

    public override void Initialized()
    {
        base.Initialized();
        var view = GetComponent<ViewBase>();
        if (view == null)
            throw new Exception("View Not Found");

        view.BindingProviders.Add(this);
    }

    public void Unbind(ViewBase viewBase)
    {
        _unbindAction();
    }
}