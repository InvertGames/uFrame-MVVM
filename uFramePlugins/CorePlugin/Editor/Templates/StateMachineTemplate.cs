using System.Collections.Generic;
using Invert.StateMachine;

[TemplateClass("Machines","{0}",MemberGeneratorLocation.DesignerFile)]
public class StateMachineTemplate : Invert.StateMachine.StateMachine
{
    [TemplateConstructor(MemberGeneratorLocation.DesignerFile,"vm","propertyName")]
    public void StateMachineConstructor(ViewModel vm, string propertyName)
    {

    }
    [TemplateProperty()]
    public override Invert.StateMachine.State StartState
    {
        get
        {
            //return this.Idle;
            return null;
        }
    }
 
    [TemplateProperty]
    public virtual StateMachineTrigger FinishedReloading
    {
        get
        {
            //if ((this._FinishedReloading == null))
            //{
            //    this._FinishedReloading = new StateMachineTrigger(this, "FinishedReloading");
            //}
            //return this._FinishedReloading;
            return null;
        }
    }


    [TemplateProperty]
    public virtual State StateProperty
    {
        get
        {
            //if ((this._Idle == null))
            //{
            //    this._Idle = new Idle();
            //}
            //return this._Idle;
            return null;
        }
    }


    public override void Compose(List<State> states)
    {
        //base.Compose(states);
        //this.Idle.StateMachine = this;
        //Idle.BeginFiring = new StateTransition("BeginFiring", Idle, Firing);
        //Idle.OnReload = new StateTransition("OnReload", Idle, Reloading);
        //Idle.OnEmpty = new StateTransition("OnEmpty", Idle, Empty);
        //Idle.AddTrigger(BeginFiring, Idle.BeginFiring);
        //Idle.AddTrigger(OnReload, Idle.OnReload);
        //Idle.AddTrigger(OnEmpty, Idle.OnEmpty);
        //states.Add(Idle);
        //this.Firing.StateMachine = this;
        //Firing.EndFiring = new StateTransition("EndFiring", Firing, Idle);
        //Firing.AddTrigger(EndFiring, Firing.EndFiring);
        //states.Add(Firing);
        //this.Reloading.StateMachine = this;
        //Reloading.FinishedReloading = new StateTransition("FinishedReloading", Reloading, Idle);
        //Reloading.AddTrigger(FinishedReloading, Reloading.FinishedReloading);
        //states.Add(Reloading);
        //this.Empty.StateMachine = this;
        //Empty.OnReload = new StateTransition("OnReload", Empty, Reloading);
        //Empty.AddTrigger(OnReload, Empty.OnReload);
        //states.Add(Empty);
    }
}