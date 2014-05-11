using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;

using UnityEditor;
using UnityEngine;

/// <summary>
/// Represents a UBehaviour Data file that can be included into UBInstances.
/// </summary>
public class UBehaviour : ScriptableObject, IBehaviourVisitable, IUBehaviours
{
    [SerializeField]
    [HideInInspector]
    private List<UBVariableDeclare> _declares = new List<UBVariableDeclare>();

    [SerializeField, HideInInspector]
    private UBGlobals _globals;

    [SerializeField, HideInInspector]
    private List<UBInclude> _includes;

    [SerializeField]
    [HideInInspector]
    private List<UBActionSheet> _sheets;

    [SerializeField]
    private List<TriggerInfo> _triggers;

    [SerializeField]
    [HideInInspector]
    private string[] _triggerTemplates;
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

    public Dictionary<string, object> SettingsDictionary
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

    public void Accept(IBehaviourVisitor visitor)
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

    public void FillAvailableActionSheets(SerializedObject obj, Dictionary<string, SerializedProperty> list)
    {
        // Custom Action Sheets
        //var actionSheets = obj.FindProperty("_triggers");
        //for (var i = 0; i < actionSheets.arraySize; i++)
        //{
        //    var sp = actionSheets.GetArrayElementAtIndex(i);
        //    var actionSheet = sp.objectReferenceValue as UBActionSheet;
        //    if (actionSheet != null && !list.ContainsKey(actionSheet.name))
        //    {
        //        list.Add(actionSheet.name, sp);
        //    }
        //}

        //// Custom Action Sheets
        //var customSheets = obj.FindProperty("_customActionSheets");
        //for (var i = 0; i < customSheets.arraySize; i++)
        //{
        //    var sp = customSheets.GetArrayElementAtIndex(i);
        //    var actionSheet = sp.objectReferenceValue as UBActionSheet;
        //    if (actionSheet != null)
        //        list.Add(actionSheet.name, sp);
        //}
    }

    public IUBVariableDeclare FindDeclare(string guid, TriggerInfo trigger = null)
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

        return GetIncludedDeclares().FirstOrDefault(p => p.Guid == guid);
    }

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

    public IEnumerable<TriggerGroup> GetTriggerGroups()
    {
        yield return new TriggerGroup(this.name, Triggers.Where(p=>!p.IsCustom && !p.IsStatic));
        yield return new TriggerGroup("Custom",Triggers.Where(p=>p.IsCustom && !p.IsStatic));
        yield return new TriggerGroup("Included",GetIncludedTriggers());

    }

    public IEnumerable<TriggerInfo> GetInstanceTriggers()
    {
        return Triggers;
    }

    public IEnumerable<TriggerInfo> GetEventTriggers()
    {
        yield break;
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
        foreach (var declare in GetIncludedDeclares())
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

    public virtual IEnumerable<TriggerInfo> GetIncludedTriggers()
    {
        yield break;
    }

    public virtual IEnumerable<IUBVariableDeclare> GetIncludedDeclares()
    {
        if (Globals != null)
        {
            foreach (var item in Globals.Declares)
            {
                yield return item;
            }
        }
    }

    public IEnumerable<IUBVariableDeclare> GetTriggerDeclares()
    {
        foreach (var triggerInfo in Triggers)
        {
            if (triggerInfo == null) continue;
            var result = UBTrigger.AvailableStaticVariablesByType(triggerInfo.TriggerTypeName);
            foreach (var item in result)
                yield return item;
        }
    }

    public virtual void Initialize(UBehavioursInstance instance)
    {
        if (Globals != null)
        {
            Globals.Push(instance);
        }
        foreach (var declare in Declares.Where(p=>!p._Expose))
            declare.Push(instance);
    }

    
}