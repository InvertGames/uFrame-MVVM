using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A base interface for A Behaviour.  This could be a scriptableobject or a component.
/// </summary>
public interface IUBehaviours
{
    /// <summary>
    /// The declared variables that are created in the editor.  
    /// If you want to modify the variables at runtime use "Variables"
    /// </summary>
    List<UBVariableDeclare> Declares { get; set; }

    /// <summary>
    /// The included behaviours that should also apply to this behaviour.
    /// </summary>
    List<UBInclude> Includes { get; set; }

    /// <summary>
    /// The Sheets that exist on any action that belongs to this behaviour.
    /// </summary>
    List<UBActionSheet> Sheets { get; set; }

    /// <summary>
    /// The triggers that will execute and belong strictly to this behviour. To get included triggers use GetAllTriggers()
    /// </summary>
    List<TriggerInfo> Triggers { get; set; }

    /// <summary>
    /// The settings that are applied to this behaviour.  Each setting will be passed to every trigger for use.
    /// </summary>
    List<BehaviourSetting> Settings { get; set; }

    /// <summary>
    /// A lazy-loaded dictionary for easy access of settings.  This directly represents the Settings Property
    /// </summary>
    Dictionary<string,object> SettingsDictionary { get; }

    string Name { get;  }

    /// <summary>
    /// Create an action sheet that belongs to this behaviour.
    /// </summary>
    /// <param name="name">The name of the actionsheet to create.</param>
    /// <param name="parent">The parent sheet if any</param>
    /// <returns>The sheet that has been created.</returns>
    UBActionSheet CreateSheet(string name, UBActionSheet parent = null);

    /// <summary>
    /// Find a declared variable.  This is for design time use only.  
    /// These variables will be pushed into the Variables collection at runtime.
    /// </summary>
    /// <param name="variableGuid">The declares identifier.</param>
    /// <param name="trigger">If there is any trigger info pass it along too.</param>
    /// <returns>The declare with 'variableGuid'</returns>
    IUBVariableDeclare FindDeclare(string variableGuid, TriggerInfo trigger = null);

    /// <summary>
    /// Finds an actionsheet that belongs to this behaviour
    /// </summary>
    /// <param name="guid">The guid of the sheet to find.</param>
    /// <returns>The sheet with 'guid'</returns>
    UBActionSheet FindSheet(string guid);

    /// <summary>
    /// Finds a trigger by its GUID.
    /// </summary>
    /// <param name="triggerId">The GUID of the trigger to find</param>
    /// <returns>The sheet with 'triggerId'</returns>
    TriggerInfo FindTriggerById(string triggerId);

    /// <summary>
    /// Finds a trigger by its Name.
    /// </summary>
    /// <param name="triggerName">The Name of the trigger to find</param>
    /// <returns>The sheet with 'triggerName'</returns>
    TriggerInfo FindTriggerByName(string triggerName);

    /// <summary>
    /// Gets all the available triggers including "Includes"
    /// </summary>
    /// <returns></returns>
    IEnumerable<TriggerInfo> GetAllTriggers();

    ///// <summary>
    ///// Get any predefined triggers that belongs to this behaviour
    ///// </summary>
    ///// <returns>All of the predefined triggers.</returns>
    //IEnumerable<TriggerInfo> GetInstanceTriggers();

    ///// <summary>
    ///// Get any predefined triggers that belongs to this behaviour
    ///// </summary>
    ///// <returns>All of the predefined triggers.</returns>
    //IEnumerable<TriggerInfo> GetIncludedTriggers();

    /// <summary>
    /// Get any static variables that belong to this behaviour. These are known variables that are programtically loaded.
    /// </summary>
    /// <param name="list"></param>
    /// <returns>All of the available predefined variables</returns>
    IEnumerable<IUBVariableDeclare> GetIncludedDeclares();

    /// <summary>
    /// The instance triggers that are custom
    /// </summary>
    /// <returns></returns>
    //IEnumerable<TriggerInfo> GetCustomTriggers();

    //IEnumerable<TriggerInfo> GetDefinedTriggers();

    IEnumerable<TriggerGroup> GetTriggerGroups();

}

public class TriggerGroup
{
    public TriggerGroup(string name)
    {
        Name = name;
    }

    public TriggerGroup(string name, IEnumerable<TriggerInfo> triggers)
    {
        Name = name;
        Triggers = triggers;
    }

    public IEnumerable<TriggerInfo> Triggers { get; set; }
    public string Name { get; set; }
    public bool Locked { get; set; }
    public UBSharedBehaviour Behaviour { get; set; }
}
public class VariableGroup
{
    public VariableGroup(string name)
    {
        Name = name;
    }

    public VariableGroup(string name, IEnumerable<IUBVariableDeclare> delcares)
    {
        Name = name;
        Declares = delcares;
    }

    public IEnumerable<IUBVariableDeclare> Declares { get; set; }
    public string Name { get; set; }
    public bool Locked { get; set; }
}