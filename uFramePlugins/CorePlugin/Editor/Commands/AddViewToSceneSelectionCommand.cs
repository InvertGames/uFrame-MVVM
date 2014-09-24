using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ElementDesigner.Commands;
using Invert.uFrame.Editor.ViewModels;
using UnityEditor;

public class AddViewToSceneSelectionCommand : EditorCommand<DiagramNodeViewModel>, IDiagramNodeCommand
{
    public override string Name
    {
        get { return "Create Scene"; }
    }

    public override string Path
    {
        get { return "Add To/Selection"; }
    }

    public override void Perform(DiagramNodeViewModel node)
    {
    
        var view = node.GraphItemObject as ViewData;
        if (view == null) return;

        if (view.CurrentViewType == null)
        {
            EditorUtility.DisplayDialog("Can't add to scene", "The diagram must be saved and have no compiler errors.",
                "OK");
            return;
        }
        Selection.activeGameObject.AddComponent(view.CurrentViewType);
    }

    public override string CanPerform(DiagramNodeViewModel node)
    {
        if (node is ViewNodeViewModel) return null;
        if (Selection.activeGameObject == null) return "No selection currently active.";
        return "Must be a scene manager to perform this action.";
    }
}