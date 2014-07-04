using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UBComponent : MonoBehaviour, IUBContext
{
    //public UBSharedBehaviour _Behaviour;
    [SerializeField,HideInInspector]
    private UBInclude _BehaviourInclude= new UBInclude();

    public UBSharedBehaviour Behaviour
    {
        get
        {
            if (_BehaviourInclude == null)
                return null;

            return _BehaviourInclude.Behaviour;
        }
        set { _BehaviourInclude.Behaviour = value; }
    }

    public virtual void Awake()
    {
        InitializeContext();

        foreach (var trigger in Behaviour.Triggers)
        {
            InitializeTrigger(trigger,Behaviour);
        }
    }

    /// <summary>
    /// Initialize a trigger with TriggerInfo
    /// </summary>
    /// <param name="trigger"></param>
    /// <param name="behaviour"></param>
    private void InitializeTrigger(TriggerInfo trigger, IUBehaviours behaviour)
    {
        trigger.Sheet.Load(behaviour, trigger);
        var triggerComponent = gameObject.AddComponent(trigger.TriggerType) as UBTrigger;
        if (triggerComponent == null)
        {
            Debug.Log(trigger.TriggerType.FullName);
        }
        //triggerComponent.hideFlags = HideFlags.HideInInspector;

        triggerComponent.Instance = this;
        triggerComponent.Sheet = trigger.Sheet;
        triggerComponent.Initialize(trigger, behaviour.SettingsDictionary);
        triggerComponent.Initialized();
    }
    /// <summary>
    /// Initializes this as the context for actions executed on this behaviour.
    /// This initializes all variables from Globals,Incldues,Overrides, and Declares
    /// </summary>
    public virtual void InitializeContext()
    {
        _BehaviourInclude.Initialize(this);
    }

    private Stack<IContextItem> _stackTrace = new Stack<IContextItem>();

    private List<UBVariableBase> _variables;
    public GameObject GameObject { get { return this.gameObject; } }
    public Transform Transform { get { return this.transform; } }

    public IUBContext ParentContext { get; private set; }

    Stack<IContextItem> IUBContext.StackTrace
    {
        get { return _stackTrace; }
        set { _stackTrace = value; }
    }

    public List<UBVariableBase> Variables
    {
        get { return _variables ?? (_variables = new List<UBVariableBase>()); }
        set { _variables = value; }
    }

    public UBInclude BehaviourInclude
    {
        get { return _BehaviourInclude; }
        set { _BehaviourInclude = value; }
    }

    public void ExecuteSheet(UBActionSheet sheet)
    {
        sheet.Execute(this);
    }

    public UBVariableBase GetVariable(string name)
    {
        var v = Variables.FirstOrDefault(p => p.Name == name);
        if (v != null)
            return v;
        return null;
    }

    public UBVariableBase GetVariableById(string id)
    {
        var v = Variables.FirstOrDefault(p => p.Guid == id);
        if (v != null)
            return v;
        return null;
    }

    public void GoTo(string triggerId)
    {

        var trigger = Behaviour.FindTriggerById(triggerId);
        if (trigger != null)
        {
            UBActionSheet.ExecutionHandler.ExecuteSheet(this, trigger.Sheet);
        }
        else
        {
            throw new Exception("Couldn't go to triger " + triggerId);
        }
    }

    public T GetVariableAs<T>(string name)
    {
        return (T)GetVariable(name).LiteralObjectValue;
    }

    public void SetVariable(string variableName, object value)
    {
        var v = GetVariable(variableName);
        if (v != null)
        {
            v.LiteralObjectValue = value;
            return;
        }
    }

    public void SetVariableById(string id, object value)
    {
        var v = GetVariableById(id);
        if (v != null)
        {
            v.LiteralObjectValue = value;

        }
    }

    public void SetVariable(UBVariableBase variableDeclare)
    {
        var v = GetVariableById(variableDeclare.Guid);
        if (v == null)
        {
            Variables.Add(variableDeclare);
        }
        else
        {
            v.LiteralObjectValue = variableDeclare.LiteralObjectValue;
        }
    }
}