using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

[Serializable]
public abstract class UBAction : IBehaviourVisitable, IContextItem, IUBSerializable
{
    private UBActionSheet _actionSheet;

    private bool _Breakpoint = false;

    private bool _enabled = true;

    private bool _Expanded = true;

    private bool _isCurrentlyActive;

    [SerializeField]
    [HideInInspector]
    private UBActionSheet _parentActionSheet;

    [SerializeField]
    [HideInInspector]
    private IUBehaviours _rootContainer;

    /// <summary>
    /// The actionsheet this action belongs to
    /// </summary>
    public UBActionSheet ActionSheet
    {
        get { return _actionSheet; }
        set { _actionSheet = value; }
    }

    /// <summary>
    /// Is there currently a breakpoint on this action
    /// </summary>
    public bool Breakpoint
    {
        get { return _Breakpoint; }
        set { _Breakpoint = value; }
    }

    /// <summary>
    /// Is this action enabled or disabled.
    /// </summary>
    public bool Enabled
    {
        get { return _enabled; }
        set { _enabled = value; }
    }

    /// <summary>
    /// Is this expanded in the view
    /// </summary>
    public bool Expanded
    {
        get { return _Expanded; }
        set { _Expanded = value; }
    }

    /// <summary>
    /// Is this action the currently executing action. For Debug internal use.
    /// </summary>
    public bool IsCurrentlyActive
    {
        get { return _isCurrentlyActive; }
        set { _isCurrentlyActive = value; }
    }

    /// <summary>
    /// The name of this action. Defaults to "ToString()"
    /// </summary>
    public string Name
    {
        get { return this.ToString(); }
    }

    /// <summary>
    /// The Behaviour Or Behaviour Instance that contains this.
    /// </summary>
    public IUBehaviours RootContainer
    {
        get { return _rootContainer; }
        set { _rootContainer = value; }
    }

    /// <summary>
    /// The visitor pattern accept method which will make the visitor visit variables
    /// </summary>
    /// <param name="visitor">The visitor of this action</param>
    public void Accept(IBehaviourVisitor visitor)
    {
        foreach (var actionSheet in GetActionSheetsFromFields())
        {
            if (actionSheet != null)
                actionSheet.Accept(visitor);
        }
        var variableFields = GetVariableFields();
        foreach (var vf in variableFields)
        {
            visitor.Visit(vf, this);
        }
    }

    /// <summary>
    /// Override this method if you have some custom logic to check for notifications particullarly errors.
    /// There is a helper method to make this easier.  Examine the following:
    /// <code>
    /// public override IEnumerable<IBehaviourNotification> CheckForNotifications(IUBehaviours behaviours, TriggerInfo trigger) {
    ///     yield return Error("There is something wrong with the action {0}", this.GetType().Name);
    /// }
    /// </code>
    /// </summary>
    /// <param name="behaviours">The behaviour that contains this action.</param>
    /// <returns>Instances of IBehaviourError use "Error" helper method.</returns>
    public virtual IEnumerable<IBehaviourNotification> CheckForNotifications(IUBehaviours behaviours, TriggerInfo trigger)
    {
        if (Breakpoint)
        {
            yield return BreakPointNotification();
        }
        var variableFields = GetVisibleFields();
        foreach (var variableField in variableFields)
        {
            if (!typeof(UBVariableBase).IsAssignableFrom(variableField.FieldType)) continue;

            var value = variableField.GetValue(this) as UBVariableBase;
            if (value == null)
            {
                yield return Error("Variable '{0}' must be initialized.", variableField.Name);
            }
            else
            {
                var isRequired = variableField.GetCustomAttributes(typeof(UBRequiredAttribute), false).FirstOrDefault() as UBRequiredAttribute != null;

                var errorMessage = value.CheckForErrors(behaviours, this, trigger, isRequired);
                if (!string.IsNullOrEmpty(errorMessage))
                    yield return Error("{0}: {1}", variableField.Name, errorMessage);
            }
        }
    }

    public virtual void Deserialize(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
        _enabled = serializer.DeserializeBool();
        _Breakpoint = serializer.DeserializeBool();
        Expanded = serializer.DeserializeBool();
    }

    /// <summary>
    /// An error helper method to easily create a IBehaviourError instance.
    /// </summary>
    /// <param name="message">The error message. Similar to string.Format</param>
    /// <param name="args">The format arguments</param>
    /// <returns>An instance of BehaviourError</returns>
    public BehaviourError Error(string message, params object[] args)
    {
        return new BehaviourError()
        {
            Message = string.Format(message, args),
            Source = this
        };
    }
    /// <summary>
    /// An error helper method to easily create a IBehaviourError instance.
    /// </summary>
    /// <param name="message">The error message. Similar to string.Format</param>
    /// <param name="args">The format arguments</param>
    /// <returns>An instance of BehaviourError</returns>
    public BehaviourBreakpoint BreakPointNotification()
    {
        return new BehaviourBreakpoint()
        {
            Message = this.Name,
            Source = this,
            Remove = ()=> { this.Breakpoint = false; this.ActionSheet.Save(); }
        };
    }
    /// <summary>
    /// Execute this action in the specified context.
    /// </summary>
    /// <param name="context">The context at which this action will execute.</param>
    public void Execute(IUBContext context)
    {
        PerformExecute(context);
    }

    /// <summary>
    /// Find all the actionsheets available by this action. (Uses reflection)
    /// </summary>
    /// <returns></returns>
    public IEnumerable<UBActionSheet> GetActionSheetsFromFields()
    {
        var properties =
            GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.FieldType == typeof(UBActionSheet))
                .ToArray();

        foreach (var property in properties)
        {
            if (property.Name == "_ForwardTo") continue;
            yield return property.GetValue(this) as UBActionSheet;
        }
    }

    /// <summary>
    /// Get available action sheets exposed by this with additional info.
    /// </summary>
    /// <param name="behaviours">The behaviour this exists on.</param>
    /// <returns>IEnumerable of UBActionSheetInfo's</returns>
    public virtual IEnumerable<UBActionSheetInfo> GetAvailableActionSheets(IUBehaviours behaviours)
    {
        var properties =
            GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.FieldType == typeof(UBActionSheet))
                .ToArray();

        foreach (var property in properties)
        {
            if (property.Name == "_ForwardTo") continue;
            yield return new UBActionSheetInfo()
            {
                Field = property,
                Name = property.Name,
                Sheet = property.GetValue(this) as UBActionSheet,
                Owner = this
            };
        }
    }

    /// <summary>
    /// Get the UBActionSheet's that lead to this action.
    /// </summary>
    /// <returns></returns>
    public List<UBActionSheet> GetPath()
    {
        var list = new List<UBActionSheet>();

        var actionSheet = this.ActionSheet;
        while (actionSheet != null)
        {
            list.Add(actionSheet);
            actionSheet = actionSheet.Parent;
        }
        list.Reverse();
        return list;
    }

    /// <summary>
    /// Gets all variable fields on this action. (Uses reflection)
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IUBFieldInfo> GetVariableFields()
    {
        var properties =
            GetType().GetFields(BindingFlags.Instance | BindingFlags.Public).ToArray();

        foreach (var property in properties)
        {
            //if (property.FieldType.IsArray &&
            //-=    typeof(UBVariableBase).IsAssignableFrom(property.FieldType.GetElementType())) continue;

            yield return new ReflectionFieldInfo(property);
        }
    }

    /// <summary>
    /// Gets all visible inspector fields and returns IUBFieldInfo.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IUBFieldInfo> GetVisibleFields()
    {
        var vFields = GetVariableFields();
        foreach (var vField in vFields)
        {
            if (vField.FieldType.IsArray &&
                typeof(UBVariableBase).IsAssignableFrom(vField.FieldType.GetElementType())) continue;

            yield return vField;
        }
        var fields = GetFields().ToArray();
        foreach (var field in fields)
        {
            yield return field;
        }
    }

    /// <summary>
    /// Serializes this action to a byte array. For internal use.
    /// </summary>
    /// <param name="referenceHolder"></param>
    /// <param name="serializer"></param>
    public virtual void Serialize(IReferenceHolder referenceHolder, UBBinarySerializer serializer)
    {
        serializer.Serialize(_enabled);
        serializer.Serialize(_Breakpoint);
        serializer.Serialize(Expanded);
    }

    public override string ToString()
    {
        return this.GetType().Name;
    }
    /// <summary>
    /// <code>
    /// [UBCategory("Network")]
    ///public class ConnectByGUID : UBAction
    ///{
    ///
    ///    [UBRequired]
    ///    public UBString _GUID = new UBString();
    /// 
    ///    [UBRequireVariable]
    ///    [UBRequired]
    ///    public UBEnum _Result = new UBEnum(typeof(NetworkConnectionError));
    ///    protected override void PerformExecute(IUBContext context)
    ///    {
    ///        if (_Result != null)
    ///        {
    ///            _Result.SetValue(context, Network.Connect(_GUID.GetValue(context)));
    ///        }
    ///
    ///    }
    ///
    ///    public override void WriteCode(IUBCSharpGenerator sb)
    ///    {
    ///        // Append expression will automatically resolve variable names with the convention
    ///        // #FIELD_NAME#
    ///        sb.AppendExpression("Network.Connect(#_GUID#)");
    ///    }
    ///
    ///}
    /// </code>
    /// </summary>
    /// <param name="sb"></param>
    public virtual void WriteCode(IUBCSharpGenerator sb)
    {
    }

    protected virtual IEnumerable<IUBFieldInfo> GetFields()
    {
        yield break;
    }

    /// <summary>
    /// This method should be implemented on any Derived class to execute the action in the context.
    /// </summary>
    /// <param name="context">The context in which to perform this action.</param>
    protected abstract void PerformExecute(IUBContext context);

    protected IUBFieldInfo[] UBArrayFieldInfo<T>(string fieldName, ref T[] array, int length, Func<object> itemInitializer) where T : UBVariableBase
    {
        var list = new List<IUBFieldInfo>();
        if (array.Length != length)
        {
            Array.Resize<T>(ref array, length);
        }
        for (int index = 0; index < array.Length; index++)
        {
            var arg = array[index];
            if (arg == null)
            {
                array.SetValue(itemInitializer(), index);
            }
        }
        var fieldInfo = GetType().GetField(fieldName);
        for (int index = 0; index < array.Length; index++)
        {
            //var ubSystemObject = _Args[index];
            list.Add(new ArrayItemFieldInfo(fieldInfo, index));
        }
        return list.ToArray();
    }
}