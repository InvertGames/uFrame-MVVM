using System.Diagnostics;
using UBehaviours.Actions;

public class DefaultExecutionHandler : IUBExecutionHandler
{
    public virtual void ExecuteSheet(IUBContext context, UBActionSheet sheet)
    {
        if (sheet.IsForward)
        {
            if (sheet.ForwardTo.Sheet != null)
            {

                context.ExecuteSheet(sheet.ForwardTo.Sheet);
                return;
                //ExecuteSheet(context, sheet.ForwardTo.Sheet);
            }

        }
        else
        {
            foreach (var action in sheet.Actions)
            {
                if (!action.Enabled) continue;

                ExecuteAction(context, action);
               // UnityEngine.Debug.Log(action.ToString() + " Executed");
            }
        }
    }

    public void TriggerBegin(UBTrigger ubTrigger)
    {
    }

    public void TriggerEnd(UBTrigger ubTrigger)
    {
    }

    protected virtual void ExecuteAction(IUBContext context, UBAction action)
    {
        action.Execute(context);
    }
}