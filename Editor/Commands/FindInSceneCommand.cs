using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using UnityEngine;

public class FindInSceneCommand : EditorCommand<ViewNode>
{
    public override void Perform(ViewNode node)
    {
        var currentViewType = node.CurrentType;
        if (currentViewType != null)
        {
            uFrameEditorSceneManager.NavigateToFirstView(currentViewType);
        }
        else
        {
            Debug.Log("The view you are attempting to navigate to (by double clicking) couldn't be found.  Try saving the diagram before attempting to navigate.");
        }
    }

    public override string CanPerform(ViewNode node)
    {
        if (node == null) return "Must be a View";
        return null;
    }
}