using Invert.uFrame.Editor.ElementDesigner;
using UnityEngine;

public class FindInSceneCommand : EditorCommand<ViewData>
{
    public override void Perform(ViewData node)
    {
        var currentViewType = node.CurrentViewType;
        if (currentViewType != null)
        {
            uFrameEditorSceneManager.NavigateToFirstView(currentViewType);
        }
        else
        {
            Debug.Log("The view you are attempting to navigate to (by double clicking) couldn't be found.  Try saving the diagram before attempting to navigate.");
        }
    }

    public override string CanPerform(ViewData node)
    {
        if (node == null) return "Must be a View";
        return null;
    }
}