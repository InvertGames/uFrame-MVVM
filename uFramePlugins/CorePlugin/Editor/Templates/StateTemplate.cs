using Invert.StateMachine;

[TemplateClass("Machines","{0}",MemberGeneratorLocation.DesignerFile)]
public class StateTemplate : Invert.StateMachine.State
{

    private StateTransition _BeginFiring;

    private StateTransition _OnReload;

    private StateTransition _OnEmpty;

    public virtual StateTransition BeginFiring
    {
        get
        {
            return this._BeginFiring;
        }
        set
        {
            _BeginFiring = value;
        }
    }

    public virtual StateTransition OnReload
    {
        get
        {
            return this._OnReload;
        }
        set
        {
            _OnReload = value;
        }
    }

    public virtual StateTransition OnEmpty
    {
        get
        {
            return this._OnEmpty;
        }
        set
        {
            _OnEmpty = value;
        }
    }

    public override string Name
    {
        get
        {
            return "Idle";
        }
    }

    private void BeginFiringTransition()
    {
        this.Transition(this.BeginFiring);
    }

    private void OnReloadTransition()
    {
        this.Transition(this.OnReload);
    }

    private void OnEmptyTransition()
    {
        this.Transition(this.OnEmpty);
    }
}