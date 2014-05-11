using System;
using System.Reflection;
using UBehaviours.Actions;

public class UBActionSheetInfo
{
    /// <summary>
    /// If the field is an array what index does the Sheet exist
    /// </summary>
    public int ArrayIndex { get; set; }

    /// <summary>
    /// The field info the field that is of type UBActionSheet this can be an array of UBActionSheet as well.
    /// </summary>
    public FieldInfo Field { get; set; }

    /// <summary>
    /// The Name of the field
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The action that owns the action sheet.
    /// </summary>
    public UBAction Owner { get; set; }

    /// <summary>
    /// The sheet value if it exists
    /// </summary>
    public UBActionSheet Sheet { get; set; }

    /// <summary>
    /// Clears the action sheet on the target.
    /// </summary>
    /// <param name="target"></param>
    public void Clear(object target)
    {
        if (Field.FieldType.IsArray)
        {
            var arr = (Array)Field.GetValue(target);
            arr.SetValue(null, ArrayIndex);
        }
        else
        {
            Field.SetValue(target, null);
        }
    }

    public UBActionSheet Initialize(IUBehaviours behaviours)
    {
        var created = behaviours.CreateSheet(Name);
        behaviours.Sheets.Add(created);
        if (Field.FieldType.IsArray)
        {
            var arr = (Array)Field.GetValue(Owner);
            arr.SetValue(created, ArrayIndex);
        }
        else
        {
            Field.SetValue(Owner, created);
        }

        created.Save(behaviours);
        foreach (var trigger in behaviours.Triggers)
        {
            trigger.Sheet.Save(behaviours);
        }
        foreach (var sheet in behaviours.Sheets)
        {
            sheet.Save(behaviours);
        }
        return created;
    }
}