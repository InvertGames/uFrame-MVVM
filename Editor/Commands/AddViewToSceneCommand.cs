using Invert.Core.GraphDesigner;


using Invert.uFrame.MVVM;
using UnityEditor;
using UnityEngine;

public class AddViewToSceneCommand : EditorCommand<ViewNode>, IDiagramNodeCommand
{
    public override string Group
    {
        get { return "View"; }
    }

    public override string Name
    {
        get { return "Create Scene"; }
    }

    public override string Path
    {
        get { return "Add To/Scene"; }
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

        GameObject obj = new GameObject(view.Name);
        obj.AddComponent(view.CurrentType);
    }

    public override bool CanProcessMultiple
    {
        get { return false; }
    }

    public override bool ShowAsDiabled
    {
        get { return false; }
    }

    public override string CanPerform(ViewNode node)
    {
        if (node == null) return "Must be a valid view node.";
        if (node.CurrentType == null) return "You must compile this view first";
        return null;
    }
}