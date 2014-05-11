using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class UBInstanceBehaviour : MonoBehaviour, IBehaviourVisitable, IUBehaviours, IUBContext
{
    [SerializeField]
    private List<UBVariableDeclare> _declares = new List<UBVariableDeclare>();

    [SerializeField, HideInInspector]
    private UBGlobals _globals;

    [SerializeField, HideInInspector]
    private List<UBInclude> _includes = new List<UBInclude>();

    [SerializeField]
    [HideInInspector]
    private List<UBActionSheet> _sheets;

    private Stack<IContextItem> _stackTrace = new Stack<IContextItem>();

    [SerializeField]
    [HideInInspector]
    private List<TriggerInfo> _triggers = new List<TriggerInfo>();

    private List<UBVariableBase> _variables;

    [SerializeField]
    private List<BehaviourSetting> _settings = new List<BehaviourSetting>();

    private Dictionary<string, object> _settingsDictionary;

    /// <summary>
    /// A lazy-loaded dictionary for easy access of settings.  This directly represents the Settings Property
    /// </summary>
    public Dictionary<string, object> SettingsDictionary
    {
        get
        {
            if (_settingsDictionary == null || _settings.Count < 1)
            {
                _settingsDictionary = new Dictionary<string, object>();
                foreach (BehaviourSetting setting in _settings)
                    _settingsDictionary.Add(setting.Name, setting.Value);

            }

            return _settingsDictionary;
        }
    }

    public string Name { get { return name; } }

    /// <summary>
    /// The declared variables that are created in the editor.  
    /// If you want to modify the variables at runtime use "Variables"
    /// </summary>
    public List<UBVariableDeclare> Declares
    {
        get
        {
            return _declares;
        }
        set
        {
            _declares = value;
        }
    }

    /// <summary>
    /// The included behaviours that should also apply to this behaviour.
    /// </summary>
    public List<UBInclude> Includes
    {
        get { return _includes; }
        set { _includes = value; }
    }

    /// <summary>
    /// The Sheets that exist on any action that belongs to this behaviour.
    /// </summary>
    public List<UBActionSheet> Sheets
    {
        get { return _sheets; }
        set { _sheets = value; }
    }

    /// <summary>
    /// The triggers that will execute and belong strictly to this behviour. To get included triggers use GetAllTriggers()
    /// </summary>
    public List<TriggerInfo> Triggers
    {
        get { return _triggers; }
        set { _triggers = value; }
    }

    /// <summary>
    /// The settings that are applied to this behaviour.  Each setting will be passed to every trigger for use.
    /// </summary>
    List<BehaviourSetting> IUBehaviours.Settings
    {
        get { return _settings; }
        set { _settings = value; }
    }

    /// <summary>
    /// The accept method for the visitor pattern.  Create a IBehaviourVisitor if you want to easily iterate over everything in a behaviour
    /// </summary>
    /// <param name="visitor"></param>
    void IBehaviourVisitable.Accept(IBehaviourVisitor visitor)
    {
        //_Behaviour.Accept(visitor);
        var list = new List<IUBVariableDeclare>(); FillIncludedDeclares(list);
        foreach (var instanceVariable in list)
        {
            instanceVariable.Accept(visitor);
        }

        foreach (var trigger in Triggers)
        {
            visitor.Visit(trigger);

            trigger.Sheet.Accept(visitor);
        }
    }

    /// <summary>
    /// Initialize the context with variables from includes and declared variables.
    /// Also Initializes each trigger onto the instance.
    /// </summary>
    public virtual void Awake()
    {
        InitializeContext();

        // Instance triggers will be first
        foreach (var trigger in Triggers)
        {
            InitializeTrigger(trigger, this);
        }
        foreach (var include in Includes)
        {
            if (include == null) continue;
            if (include.Behaviour == null) continue;
            foreach (var trigger in include.Behaviour.Triggers)
            {
                InitializeTrigger(trigger, include.Behaviour);
            }
        }
    }

    /// <summary>
    /// Create an action sheet that belongs to this behaviour.
    /// </summary>
    /// <param name="name">The name of the actionsheet to create.</param>
    /// <param name="parent">The parent sheet if any</param>
    /// <returns>The sheet that has been created.</returns>
    public UBActionSheet CreateSheet(string name, UBActionSheet parent = null)
    {
        var sheet = new UBActionSheet { Name = name, Parent = parent, RootContainer = this, Guid = Guid.NewGuid().ToString() };
        return sheet;
    }

    public Transform Transform { get; private set; }

    /// <summary>
    /// Executes an ActionSheet with this as the context.
    /// </summary>
    /// <param name="sheet"></param>
    public void ExecuteSheet(UBActionSheet sheet)
    {
        sheet.Execute(this); //new UBContext() { ParentContext = this });
    }

    /// <summary>
    /// Searches declares and static variables for the variable.
    /// </summary>
    /// <param name="guid">The Identifier of the variable to search for.  If static use the name.</param>
    /// <param name="trigger">The trigger to search if any.</param>
    /// <returns></returns>
    public IUBVariableDeclare FindDeclare(string guid, TriggerInfo trigger = null)
    {
        if (Globals != null)
            foreach (var variableDeclare in Globals.Declares)
            {
                if (variableDeclare.Guid == guid)
                {
                    return variableDeclare;
                }
            }

        var declare = Declares.FirstOrDefault(p => p.Guid == guid);
        if (declare != null)
            return declare;

        if (trigger != null)
        {
            var result = UBTrigger.AvailableStaticVariablesByType(trigger.TriggerTypeName);
            var triggerItem = result.FirstOrDefault(p => p.Guid == guid);
            if (triggerItem != null)
            {
                return triggerItem;
            }
        }
        var list = new List<IUBVariableDeclare>(); FillIncludedDeclares(list);
        return list.FirstOrDefault(p => p.Guid == guid);
    }

    /// <summary>
    /// Search all sheets and sheets that belong to triggers by the specified id.
    /// </summary>
    /// <param name="guid">The guid of the sheet to find.</param>
    /// <returns>The sheet with 'guid'</returns>
    public UBActionSheet FindSheet(string guid)
    {
        foreach (var triggerInfo in Triggers)
        {
            if (triggerInfo.Sheet.Guid == guid)
                return triggerInfo.Sheet;
        }
        foreach (var ubActionSheet in Sheets)
        {
            if (ubActionSheet.Guid == guid)
                return ubActionSheet;
        }
        return null;
    }

    /// <summary>
    /// Find a trigger by its id.
    /// </summary>
    /// <param name="triggerId">The guid of the trigger to find.</param>
    /// <returns>The trigger with 'triggerId'</returns>
    public TriggerInfo FindTriggerById(string triggerId)
    {
        foreach (TriggerInfo p in GetAllTriggers())
        {
            if (p.Guid == triggerId) return p;
        }
        return null;
    }
    /// <summary>
    /// Find a trigger by its name.
    /// </summary>
    /// <param name="triggerName">The name of the trigger to find.</param>
    /// <returns>The trigger with 'triggerName'</returns>
    public TriggerInfo FindTriggerByName(string triggerName)
    {
        return Triggers.FirstOrDefault(p => p.DisplayName == triggerName);
    }

    /// <summary>
    /// Find a template trigger by its name.
    /// </summary>
    /// <param name="triggerName">The name of the trigger to find.</param>
    /// <returns>The trigger with 'triggerName'</returns>
    public TriggerInfo FindTemplateTriggerByName(string triggerName)
    {
        return Triggers.FirstOrDefault(p => p.Data == triggerName);
    }
    /// <summary>
    /// Gets all the available triggers
    /// </summary>
    /// <returns></returns>
    public IEnumerable<TriggerInfo> GetAllTriggers()
    {
        foreach (var trigger in Triggers)
        {
            yield return trigger;
        }
        foreach (var include in Includes)
        {
            foreach (var trigger in include.Behaviour.Triggers)
                yield return trigger;
        }
    }

    public IEnumerable<IUBVariableDeclare> GetIncludedDeclares()
    {
        var list = new List<IUBVariableDeclare>();
        FillIncludedDeclares(list);
        return list;
    }

    /// <summary>
    /// Get any predefined triggers that belongs to this behaviour
    /// </summary>
    /// <returns>All of the predefined triggers.</returns>
    public virtual IEnumerable<TriggerInfo> GetIncludedTriggers()
    {
        var cleanup = false;
        foreach (var include in Includes)
        {
            if (include == null || include.Behaviour == null)
            {
                cleanup = true;
                continue;
            }

            foreach (var triggerInfo in include.Behaviour.Triggers)
            {
                yield return triggerInfo;
            }

        }
        if (cleanup)
        {
            Includes.RemoveAll(p => p == null || p.Behaviour == null);
        }
    }

    private IEnumerable<TriggerInfo> GetAvailableEvents()
    {
        foreach (var include in Includes)
        {
            if (include == null) continue;
            if (include.Behaviour == null) continue;
            if (include.Behaviour.TriggerTemplates == null) continue;
            foreach (var item in include.Behaviour.TriggerTemplates)
            {
                var foundTrigger = FindTemplateTriggerByName(item);
                if (foundTrigger != null)
                {
                    yield return foundTrigger;
                    continue;
                }
                yield return new TriggerInfo()
                {
                    IsStatic = true,
                    Guid = Guid.NewGuid().ToString(),
                    Data = item,
                    DisplayName = item,
                    TriggerType = typeof(UBCustomTrigger)
                };
            }
        }
    }


    /// <summary>
    /// Get any static variables that belong to this behaviour. These are known variables that are programtically loaded.
    /// </summary>
    /// <param name="list"></param>
    /// <returns>All of the available predefined variables</returns>
    public virtual void FillIncludedDeclares(List<IUBVariableDeclare> list)
    {
        //yield return new VariableGroup("Globals")
        if (Globals != null)
        {
            foreach (var declare in Globals.Declares)
            {
                list.Add(declare);
            }
        }
        foreach (var include in Includes)
        {
            if (include == null || include.Behaviour == null) continue;
            var declares = include.Behaviour.GetIncludedDeclares();
            foreach (var declare in declares)
                list.Add(declare);
        }
        foreach (var t in Triggers)
        {
            var actionsheet = t.Sheet;
            if (actionsheet == null) continue;
            var result = UBTrigger.AvailableStaticVariablesByType(actionsheet.TriggerType);
            foreach (var item in result)
                list.Add(item);
        }
    }

    IEnumerable<TriggerGroup> IUBehaviours.GetTriggerGroups()
    {
        yield return new TriggerGroup(this.name, Triggers.Where(p => !p.IsCustom && !p.IsStatic));
        yield return new TriggerGroup("Custom", Triggers.Where(p => p.IsCustom && !p.IsStatic));
        yield return new TriggerGroup("Events", GetAvailableEvents());
    }

    /// <summary>
    /// Initializes this as the context for actions executed on this behaviour.
    /// This initializes all variables from Globals,Incldues,Overrides, and Declares
    /// </summary>
    public virtual void InitializeContext()
    {
        if (Globals != null)
        {
            Globals.Push(this);
        }
        foreach (var include in Includes)
        {
            if (include.Behaviour == null) continue;
            include.Initialize(this);
        }
        foreach (var declare in Declares)
        {
            Variables.Add(declare);
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
        triggerComponent.hideFlags = HideFlags.HideInInspector;

        triggerComponent.Instance = this;
        triggerComponent.Sheet = trigger.Sheet;
        triggerComponent.Initialize(trigger, behaviour.SettingsDictionary);
        triggerComponent.Initialized();
    }

    #region Context


    //public UBInstanceBehaviour Behaviour { get { return this; } }

    public UBGlobals Globals
    {
        get { return _globals; }
        set { _globals = value; }
    }

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

    public GameObject GameObject
    {
        get
        {
            return this.gameObject;
        }
    }

    //public Transform Transform
    //{
    //    get
    //    {
    //        return this.transform;
    //    }
    //}
    /// <summary>
    /// Finds a variable in the current context. Use this for runtime variable searching
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <returns></returns>
    public UBVariableBase GetVariable(string name)
    {

        var v = this.Variables.FirstOrDefault(p => p.Name == name);
        if (v != null)
            return v;

        return null;
    }

    /// <summary>
    /// Gets a variable at runtime by its id.
    /// </summary>
    /// <param name="id">The id of the variable to get.</param>
    /// <returns></returns>
    public UBVariableBase GetVariableById(string id)
    {

        var v = this.Variables.FirstOrDefault(p => p.Guid == id);
        if (v != null)
            return v;

        return null;
    }

    /// <summary>
    /// Execute a trigger by id
    /// </summary>
    /// <param name="triggerId"></param>
    public void GoTo(string triggerId)
    {
        var trigger = FindTriggerById(triggerId);
        if (trigger != null)
        {
            UBActionSheet.ExecutionHandler.ExecuteSheet(this, trigger.Sheet);
        }
        else
        {
            throw new Exception("Couldn't go to triger " + triggerId);
        }
    }

    /// <summary>
    /// Gets a variables value by its name
    /// </summary>
    /// <param name="name">The name of the variable</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetVariableAs<T>(string name)
    {
        return (T)GetVariable(name).LiteralObjectValue;
    }
    /// <summary>
    /// Sets a variable by its name
    /// </summary>
    /// <param name="variableName"></param>
    /// <param name="value"></param>
    public void SetVariable(string variableName, object value)
    {
        var v = GetVariable(variableName);
        if (v != null)
        {
            v.LiteralObjectValue = value;
            return;
        }
    }

    /// <summary>
    /// Sets a variable by its id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    public void SetVariableById(string id, object value)
    {
        //IUBContext ctx = this;
        //while (ctx != null)
        //{
        var v = GetVariableById(id);
        if (v != null)
        {
            v.LiteralObjectValue = value;

        }
        //    ctx = ctx.ParentContext;
        //}
    }

    #endregion Context

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