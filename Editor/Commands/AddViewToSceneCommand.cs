using Invert.Core.GraphDesigner;
using Invert.uFrame.Editor;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ElementDesigner.Commands;
using Invert.uFrame.Editor.ViewModels;
using UnityEditor;
using UnityEngine;

public class AddViewToSceneCommand : EditorCommand<ElementViewNode>, IDiagramNodeCommand
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

    public override void Perform(ElementViewNode view)
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

    public override bool ShowAsDiabled
    {
        get { return true; }
    }

    public override string CanPerform(ElementViewNode node)
    {
        if (node == null) return "Must be a valid view node.";
        if (node.CurrentType == null) return "You must compile this view first";
        return null;
    }
}