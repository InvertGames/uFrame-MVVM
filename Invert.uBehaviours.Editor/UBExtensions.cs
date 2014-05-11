using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class UBExtensions
{
    public static string PrettyLabel(this string label)
    {
        return Regex.Replace(label.Replace("_", ""), @"((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim();
    }
    /// <summary>
    /// Gets all the available settings for a behaviour.  This is a dynamic reflection method and shouldn't be used at runtime.
    /// This is used internally.
    /// </summary>
    /// <param name="behaviour"></param>
    /// <returns></returns>
    public static IEnumerable<BehaviourSetting> GetSettings(this IUBehaviours behaviour)
    {
        var bSettings = behaviour.Settings;
        var foundSettings = new List<BehaviourSetting>();
        foreach (var trigger in behaviour.Triggers)
        {
            var settings = trigger.GetSettings();
            foreach (var setting in settings)
            {
                if (foundSettings.Exists(p => p.Name == setting.Name)) continue;

                var bSetting = bSettings.FirstOrDefault(p => p.Name == setting.Name);
                if (bSetting == null)
                {
                    bSetting = new BehaviourSetting()
                    {
                        Name = setting.Name,
                        FieldType = setting.SettingType,
                        Value = setting.DefaultValue
                    };
                    bSettings.Add(bSetting);
                }
                bSetting.FieldType = setting.SettingType;
                foundSettings.Add(bSetting);
                
            }
        }
        behaviour.Settings.RemoveAll(p => !foundSettings.Contains(p));
        return foundSettings;
    }

    /// <summary>
    /// Get all the sheets in the Triggers and actions of this behaviour.
    /// </summary>
    /// <param name="behaviour">The behaviour to get the sheets from</param>
    /// <returns>The sheets.</returns>
    public static IEnumerable<UBActionSheet> GetAllSheets(this IUBehaviours behaviour)
    {
        foreach (var trigger in behaviour.Triggers)
        {
            yield return trigger.Sheet;
        }
        foreach (var sheet in behaviour.Sheets)
        {
            yield return sheet;
        }
    }

    /// <summary>
    /// Get the notifications from this behaviour.
    /// </summary>
    /// <param name="behaviour"></param>
    /// <returns></returns>
    public static IEnumerable<IBehaviourNotification> GetNotifications(this IUBehaviours behaviour)
    {
        foreach (var trigger in behaviour.Triggers)
        {
            var errors = trigger.Sheet.CheckForNotifications(behaviour, trigger);
            foreach (var error in errors)
            {
                yield return error;
            }
        }
        foreach (var sheet in behaviour.Sheets)
        {
            var errors = sheet.CheckForNotifications(behaviour, null);
            foreach (var error in errors)
            {
                yield return error;
            }
        }
    }

    /// <summary>
    /// Get all of the trigger declares.  This invokes the {TypeName}Variables on the trigger MonoBehaviour to get
    /// what variables will be added to the context when the trigger is triggered.
    /// </summary>
    /// <param name="behaviour">The behaviour to trigger.</param>
    /// <returns>The triggers</returns>
    public static IEnumerable<IUBVariableDeclare> GetTriggerDeclares(this IUBehaviours behaviour)
    {
        foreach (var triggerInfo in behaviour.Triggers)
        {
            if (triggerInfo == null) continue;
            var result = UBTrigger.AvailableStaticVariablesByType(triggerInfo.TriggerTypeName);
            foreach (var item in result)
                yield return item;
        }
    }
}