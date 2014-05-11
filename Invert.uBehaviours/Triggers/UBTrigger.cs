using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;
/// <summary>
/// The component class for triggers.  This should be overriden to create a trigger.
/// Write a normal MonoBehaviour and when you want it to "Trigger" simply invoke ExecuteSheet or ExecuteSheetWithVars.
/// </summary>
public class UBTrigger : MonoBehaviour
{
    /// <summary>
    /// The behaviour instance that this trigger belongs too.
    /// </summary>
    public IUBContext Instance { get; set; }

    /// <summary>
    /// The sheet that belongs to this trigger and will be executed when invoking ExecuteSheet & ExecuteSheetWithVars
    /// </summary>
    public virtual UBActionSheet Sheet { get; set; }

    /// <summary>
    /// This method pulls information on a trigger type.  Basically just invokes the static method that provides variable information.
    /// This is for internal use.
    /// </summary>
    /// <param name="triggerType">The type of trigger by its AssemblyQualifiedName</param>
    /// <returns></returns>
    public static IEnumerable<IUBVariableDeclare> AvailableStaticVariablesByType(string triggerType)
    {
        if (string.IsNullOrEmpty(triggerType)) yield break;
        var type = UBHelper.GetType(triggerType);
        if (type == null) yield break;
        var method = type.GetMethod(type.Name + "Variables", BindingFlags.Static | BindingFlags.Public);
        if (method == null)
            yield break;

        var result = (IEnumerable<IUBVariableDeclare>)method.Invoke(null, null);
        foreach (var item in result)
        {
            yield return item;
        }
    }

    /// <summary>
    /// Executes the sheet that belongs to this trigger.
    /// </summary>
    public virtual void ExecuteSheet()
    {
        if (Sheet == null) return;
        UBActionSheet.ExecutionHandler.TriggerBegin(this);
        Instance.ExecuteSheet(Sheet);
        UBActionSheet.ExecutionHandler.TriggerEnd(this);
    }
    /// <summary>
    /// Executes the sheet that belongs to this trigger with static variables.
    /// Any variables that are used should also be provided by a static method with the
    /// convertion of {TypeName}Variables that returns IEnumerable<IUBVariableDeclare>().
    /// 
    /// For Example: If you had a trigger name MyTrigger it would have static decleration like so
    /// public static IEnumerable<IUBVariableDeclare> MyTriggerVariables() { yield return new UBStaticVariableDeclare() { Initialize } }
    /// </summary>
    public void ExecuteSheetWithVars(params UBVariableBase[] variableDeclares)
    {
        if (Sheet == null) return;
        foreach (var variableDeclare in variableDeclares)
            Instance.SetVariable(variableDeclare);

        
      
        UBActionSheet.ExecutionHandler.TriggerBegin(this);
        Instance.ExecuteSheet(Sheet);
        UBActionSheet.ExecutionHandler.TriggerEnd(this);
    }
    /// <summary>
    /// After the trigger has been initialized
    /// </summary>
    public virtual void Initialized()
    {
    }

    /// <summary>
    /// This is the initialize method that should apply any settings provided by UBSettingAttribute on this class.
    /// For example:
    /// [UBSetting("MyTriggerSetting", typeof(bool), true)]
    /// <code>
    /// public class MyTrigger : UBTrigger { 
    ///     public bool MySetting;
    ///     public virtual void Initialize(TriggerInfo trigger, Dictionary<string, object> settings) {
    ///         MySetting = (bool)settings["MySetting"];
    ///     }
    /// }
    /// </code>
    /// </summary>
    /// <param name="trigger">The info for this trigger that contains the Sheet and name of the trigger.</param>
    /// <param name="settings"></param>
    public virtual void Initialize(TriggerInfo trigger, Dictionary<string, object> settings)
    {
        
    }
}