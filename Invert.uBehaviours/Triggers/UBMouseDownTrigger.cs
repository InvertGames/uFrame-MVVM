using System.Collections.Generic;

[UBCategory("Mouse")]
public class UBMouseDownTrigger : UBTrigger
{
    public static void OnMouseDown(IUBCSharpGenerator generator, TriggerInfo data)
    {
        generator.AppendFormat(generator.InvokeTriggerSheet());
    }

    public override void Initialize(TriggerInfo trigger, Dictionary<string, object> settings)
    {
        base.Initialize(trigger, settings);
    }

    public void OnMouseDown()
    {
        if (Sheet != null)
        {
            ExecuteSheet();
        }
    }
}