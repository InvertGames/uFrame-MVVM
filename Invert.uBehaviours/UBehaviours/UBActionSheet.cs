using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This is a class that contains actions.  It is the main class for executing a sequence of actions.
/// </summary>
[HideInInspector]
[ExecuteInEditMode]
[Serializable]
public sealed class UBActionSheet : ByteObject<UBAction>, IBehaviourVisitable, IContextItem
{
    [NonSerialized]
    private static IUBExecutionHandler _executionHandler;
    [NonSerialized]
    private TriggerInfo _forwardTo;

    [SerializeField, HideInInspector]
    private string _ForwardToId;

    [SerializeField]
    private string _guid;

    [SerializeField]
    private string _Name;

    [NonSerialized]
    [HideInInspector]
    private IUBehaviours _rootContainer;

    [SerializeField]
    private string _TriggerType;
    [NonSerialized]
    private UBActionSheet _parent;
    [NonSerialized]
    private TriggerInfo _triggerInfo;

    /// <summary>
    /// This is the ExecutionHandler that will execute any actionsheet.
    /// </summary>
    public static IUBExecutionHandler ExecutionHandler
    {
        get { return _executionHandler ?? (new DefaultExecutionHandler()); }
        set { _executionHandler = value; }
    }

    /// <summary>
    /// The actions that belong to this actionsheet
    /// </summary>
    public List<UBAction> Actions
    {
        get { return Items; }
    }

    /// <summary>
    /// If the sheet forwards to another trigger this will contain the info for the trigger. Otherwise null
    /// </summary>
    public TriggerInfo ForwardTo
    {
        get
        {
            if (string.IsNullOrEmpty(ForwardToId))
                return null;
            return _forwardTo ?? (_forwardTo = RootContainer.FindTriggerById(ForwardToId));
        }
        set
        {
            _forwardTo = value;
            if (value != null)
            {
                ForwardToId = value.Guid;
            }
            else
            {
                ForwardToId = null;
            }
        }
    }

    /// <summary>
    /// The full name gets the path to this action sheet.
    /// </summary>
    public string FullName
    {
        get
        {
            var path = Name;
            var actionSheet = this.Parent;
            while (actionSheet != null)
            {
                path = actionSheet.Name + path;
                actionSheet = actionSheet.Parent;
            }
            return path;
        }
    }

    /// <summary>
    /// An identifier for the ActionSheet
    /// </summary>
    public string Guid
    {
        get
        {
            // Backward compatability
            if (_guid == null)
            {
                _guid = System.Guid.NewGuid().ToString();
            }
            return _guid;
        }
        set { _guid = value; }
    }

    /// <summary>
    /// Is this visible when collapsed.
    /// </summary>
    public bool IsActive
    {
        get
        {
            return this.Actions.Count > 0 || IsForward;
        }
    }

    /// <summary>
    /// Are we currently forwarding to another Trigger?
    /// </summary>
    public bool IsForward
    {
        get
        {
            return ForwardTo != null;
        }
        set
        {
            if (!value)
            {
                ForwardToId = null;
                _forwardTo = null;
            }
        }
    }

    /// <summary>
    /// The name of this actionsheet.
    /// </summary>
    public string Name
    {
        get { return _Name; }
        set { _Name = value; }
    }

    /// <summary>
    /// The actionsheet that parents this actionsheet or the action's parent of this actionsheet.
    /// </summary>
    public UBActionSheet Parent
    {
        get { return _parent; }
        set { _parent = value; }
    }

    /// <summary>
    /// The path to this actionsheet
    /// </summary>
    public string Path
    {
        get
        {
            var path = Name;
            var actionSheet = this.Parent;
            while (actionSheet != null)
            {
                path = actionSheet.Name + ":" + path;
                actionSheet = actionSheet.Parent;
            }
            return path;
        }
    }

    /// <summary>
    /// The behaviour that contains this sheet.
    /// </summary>
    public IUBehaviours RootContainer
    {
        get { return _rootContainer; }
        set { _rootContainer = value; }
    }

    /// <summary>
    /// Gets each actionsheet leading up to this sheet in the flow.
    /// </summary>
    public IEnumerable<UBActionSheet> SheetPath
    {
        get
        {
            var list = new List<UBActionSheet>();

            var actionSheet = this;
            while (actionSheet != null)
            {
                list.Add(actionSheet);
                actionSheet = actionSheet.Parent;
            }
            list.Reverse();
            return list;
        }
    }

    /// <summary>
    /// The trigger info that this actionsheet belongs to
    /// </summary>
    public TriggerInfo TriggerInfo
    {
        get { return _triggerInfo; }
        set { _triggerInfo = value; }
    }

    public string TriggerType
    {
        get { return _TriggerType; }
        set { _TriggerType = value; }
    }

    private string ForwardToId
    {
        get { return _ForwardToId; }
        set { _ForwardToId = value; }
    }

    public UBActionSheet(UBActionSheet copy)
        : base(copy)
    {
        _forwardTo = copy._forwardTo;
        _TriggerType = copy._TriggerType;
        _guid = System.Guid.NewGuid().ToString();
        if (_rootContainer != null)
            copy.Load(_rootContainer);
    }

    public UBActionSheet()
    {
    }

    /// <summary>
    /// This will traverse the actionsheet invoking corresponding methods on the visitor.
    /// </summary>
    /// <param name="visitor"></param>
    public void Accept(IBehaviourVisitor visitor)
    {
        visitor.Visit(this);
        foreach (var action in Actions)
        {
            if (action == null) continue;
            visitor.Visit(action);
            action.Accept(visitor);
        }
    }

    /// <summary>
    /// Adds a action to this sheet.
    /// </summary>
    /// <param name="item"></param>
    public override void AddItem(UBAction item)
    {
        base.AddItem(item);
        item.ActionSheet = this;
        item.RootContainer = RootContainer;
    }

    /// <summary>
    /// Check all actions for notifications.  This could be an error or notifying that  breakpoint exists.
    /// </summary>
    /// <param name="behaviour">The behaviour that this actionsheet belongs to or at which to search for notifications.</param>
    /// <returns></returns>
    public IEnumerable<IBehaviourNotification> CheckForNotifications(IUBehaviours behaviour)
    {
        return CheckForNotifications(behaviour, TriggerInfo);
    }

    /// <summary>
    /// Check all actions for notifications.  This could be an error or notifying that  breakpoint exists.
    /// </summary>
    /// <param name="behaviour">The behaviour that this actionsheet belongs to or at which to search for notifications.</param>
    /// <param name="trigger">The trigger that this sheet belongs to if any.</param>
    /// <returns></returns>
    public IEnumerable<IBehaviourNotification> CheckForNotifications(IUBehaviours behaviour, TriggerInfo trigger)
    {
        return Actions.SelectMany(p => p.CheckForNotifications(behaviour, trigger));
    }

    public override void DeserializeItem(IUBSerializable item, UBBinarySerializer serializer)
    {
        base.DeserializeItem(item, serializer);
        var ubAction = item as UBAction;
        if (ubAction != null)
        {
            var action = ubAction;
            action.ActionSheet = this;
            action.RootContainer = this.RootContainer;
        }
    }

    /// <summary>
    /// Executes the action sheet with in a given context.
    /// </summary>
    /// <param name="context"></param>
    public void Execute(IUBContext context)
    {
        ExecutionHandler.ExecuteSheet(context, this);
    }

    /// <summary>
    /// Loads all of the byte data stored on this object and correctly assigns runtime variables needed for execution.
    /// This is used internally and shouldn't be invoked or it might undesired results.
    /// </summary>
    /// <param name="behaviour">The behaviour that this action sheet belongs to and the context to load it in.</param>
    /// <param name="trigger">The trigger info to load with this action sheet if any.</param>
    public void Load(IUBehaviours behaviour, TriggerInfo trigger)
    {
        _forwardTo = null;
        RootContainer = behaviour;
        TriggerInfo = trigger;
        base.Load(behaviour);
    }

    /// <summary>
    /// Loads all of the byte data stored on this object and correctly assigns runtime variables needed for execution.
    /// This is used internally and shouldn't be invoked or it might undesired results.
    /// </summary>
    /// <param name="behaviour">The behaviour that this action sheet belongs to and the context to load it in.</param>
    public override void Load(IUBehaviours behaviour)
    {
        _forwardTo = null;
        RootContainer = behaviour;
        base.Load(behaviour);
    }

    protected override object DeserializeFieldValue(Type t, UBBinarySerializer serializer)
    {
        var result = base.DeserializeFieldValue(t, serializer);
        var resultSheet = result as UBActionSheet;
        if (resultSheet != null)
        {
            resultSheet.Parent = this;
            resultSheet.TriggerInfo = TriggerInfo;
        }

        return result;
    }

    protected override void SerializeObjectValue(UBBinarySerializer serializer, Type t, object v)
    {
        var resultSheet = v as UBActionSheet;
        if (resultSheet != null)
        {
            resultSheet.Parent = this;
            resultSheet.TriggerInfo = TriggerInfo;
        }

        base.SerializeObjectValue(serializer, t, v);
    }
}