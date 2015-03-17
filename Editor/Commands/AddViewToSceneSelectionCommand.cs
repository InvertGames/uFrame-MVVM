using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using UnityEditor;

public class AddViewToSceneSelectionCommand : EditorCommand<ViewNode>, IDiagramNodeCommand
{
    public override string Name
    {
        get { return "Create Scene"; }
    }

    public override string Path
    {
        get { return "Add To/Selection"; }
    }

    public override bool CanProcessMultiple
    {
        get { return false; }
    }

    public override void Perform(ViewNode view)
    {
    
        if (view == null) return;

        if (view.CurrentType == null)
        {
            EditorUtility.DisplayDialog("Can't add to scene", "The diagram must be saved and have no compiler errors.",
                "OK");
            return;
        }
        Selection.activeGameObject.AddComponent(view.CurrentType);
    }

    public override string CanPerform(ViewNode node)
    {
        
        if (Selection.activeGameObject == null) return "No selection currently active.";
        if (node != null) return null;
        return "Must be a scene manager to perform this action.";
    }
}