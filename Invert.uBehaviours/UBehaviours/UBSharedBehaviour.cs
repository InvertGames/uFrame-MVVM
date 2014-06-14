using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents a UBehaviour Data file that can be included into UBInstances.
/// </summary>
public class UBSharedBehaviour : ScriptableObject, IBehaviourVisitable, IUBehaviours
{
    [SerializeField]
    [HideInInspector]
    private List<UBVariableDeclare> _declares = new List<UBVariableDeclare>();

    [SerializeField, HideInInspector]
    private UBGlobals _globals;

    [SerializeField, HideInInspector]
    private List<UBInclude> _includes = new List<UBInclude>();

    [SerializeField]
    [HideInInspector]
    private List<UBActionSheet> _sheets = new List<UBActionSheet>();

    [SerializeField]
    private List<TriggerInfo> _triggers = new List<TriggerInfo>();

    [SerializeField]
    [HideInInspector]
    private string[] _triggerTemplates = new string[]{};
    [SerializeField]
    private List<BehaviourSetting> _settings = new List<BehaviourSetting>();

    public List<UBVariableDeclare> Declares
    {
        get { return _declares; }
        set { _declares = value; }
    }

    public UBGlobals Globals
    {
        get { return _globals; }
        set { _globals = value; }
    }

    public List<UBInclude> Includes
    {
        get { return _includes; }
        set { _includes = value; }
    }

    public List<UBActionSheet> Sheets
    {
        get { return _sheets; }
        set { _sheets = value; }
    }

    public List<TriggerInfo> Triggers
    {
        get { return _triggers; }
        set { _triggers = value; }
    }

    public string[] TriggerTemplates
    {
        get { return _triggerTemplates; }
        set { _triggerTemplates = value; }
    }

    public List<BehaviourSetting> Settings
    {
        get { return _settings; }
        set { _settings = value; }
    }
    private Dictionary<string, object> _settingsDictionary;

    Dictionary<string, object> IUBehaviours.SettingsDictionary
    {
        get
        {
            if (_settingsDictionary == null || _settings.Count < 1)
            {
                _settingsDictionary = new Dictionary<string, object>();
                foreach (BehaviourSetting setting in Settings)
                    _settingsDictionary.Add(setting.Name, setting.Value);
            }

            return _settingsDictionary;
        }
    }

    public string Name
    {
        get { return name; }
    }

    void IBehaviourVisitable.Accept(IBehaviourVisitor visitor)
    {
        //base.Accept(visitor);
        foreach (var v in Declares)
        {
            visitor.Visit(v);
        }
        foreach (var actionSheet in Triggers)
        {
            if (actionSheet == null) continue;

            // actionSheet.Accept(visitor);
        }
    }

    public void Awake()
    {
        if (Triggers != null)
            foreach (var trigger in Triggers)
                trigger.Sheet.Load(this, trigger);
        if (Sheets != null)
            foreach (var sheet in Sheets)
                sheet.Load(this, null);
    }

    public UBActionSheet CreateSheet(string name, UBActionSheet parent = null)
    {
        var sheet = new UBActionSheet { Name = name, Parent = parent, RootContainer = this, Guid = Guid.NewGuid().ToString() };
        return sheet;
    }

    IUBVariableDeclare IUBehaviours.FindDeclare(string guid, TriggerInfo trigger)
    {
        var declare = GetDeclare(guid);
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
        var list = new List<IUBVariableDeclare>();
        FillIncludedDeclares(list);
        return list.FirstOrDefault(p => p.Guid == guid);
    }

    UBActionSheet IUBehaviours.FindSheet(string guid)
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
    /// Finds a trigger by its string guid.
    /// </summary>
    /// <param name="triggerId">The id of the trigger to find.</param>
    /// <returns></returns>
    public TriggerInfo FindTriggerById(string triggerId)
    {
        foreach (TriggerInfo p in Triggers)
        {
            if (p.Guid == triggerId) return p;
        }
        return null;
    }

    /// <summary>
    /// Finds a trigger by name
    /// </summary>
    /// <param name="triggerName"></param>
    /// <returns></returns>
    public TriggerInfo FindTriggerByName(string triggerName)
    {
        foreach (TriggerInfo p in Triggers)
        {
            if (p.DisplayName == triggerName) return p;
        }
        return null;
    }

    /// <summary>
    /// Gets all the available triggers
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerable<TriggerInfo> GetAllTriggers()
    {
        return Triggers;
    }

    public IEnumerable<IUBVariableDeclare> GetIncludedDeclares()
    {
        var list = new List<IUBVariableDeclare>();
        FillIncludedDeclares(list);
        return list;
    }

    IEnumerable<TriggerGroup> IUBehaviours.GetTriggerGroups()
    {
        yield return new TriggerGroup(this.name, Triggers.Where(p=>!p.IsCustom && !p.IsStatic));
        yield return new TriggerGroup("Custom",Triggers.Where(p=>p.IsCustom && !p.IsStatic));
        var list = new List<TriggerInfo>();
        FillIncludedTriggers(list);
        yield return new TriggerGroup("Included",list);

    }

    public IEnumerable<IUBVariableDeclare> GetAllVariableDeclares()
    {
        if (Globals != null)
        foreach (var variableDeclare in Globals.Declares)
        {
            yield return variableDeclare;
        }
        foreach (var declare in Declares)
        {
            yield return declare;
        }
        var list = new List<IUBVariableDeclare>(); FillIncludedDeclares(list);
        foreach (var declare in list)
            yield return declare;
    }

    public IUBVariableDeclare GetDeclare(UBVariableBase variable)
    {
        return GetAllVariableDeclares().FirstOrDefault(p => p.Guid == variable._ValueFromVariableName);
    }

    public IUBVariableDeclare GetDeclare(string guid)
    {
        return GetAllVariableDeclares().FirstOrDefault(p => p.Guid == guid);
    }

    public virtual void FillIncludedTriggers(List<TriggerInfo> list )
    {
        
    }

    public virtual void FillIncludedDeclares(List<IUBVariableDeclare> list)
    {
        if (Globals != null)
        {
            foreach (var item in Globals.Declares)
                list.Add(item);
        }
    }

    //public IEnumerable<IUBVariableDeclare> GetTriggerDeclares()
    //{
    //    foreach (var triggerInfo in Triggers)
    //    {
    //        if (triggerInfo == null) continue;
    //        var result = UBTrigger.AvailableStaticVariablesByType(triggerInfo.TriggerTypeName);
    //        foreach (var item in result)
    //            yield return item;
    //    }
    //}

    public virtual void Initialize(IUBContext instance)
    {
        if (Globals != null)
        {
            Globals.Push(instance);
        }
        foreach (var declare in Declares.Where(p=>!p._Expose))
            declare.Push(instance);
    }

    
}