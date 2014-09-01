using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ElementDesigner.Commands;
using UnityEditor;
using UnityEngine;

public class AddViewToSceneCommand : EditorCommand<IDiagramNode>, IDiagramNodeCommand
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

    public override void Perform(IDiagramNode node)
    {
     
        var view = node as ViewData;
        if (view == null) return;

        if (view.CurrentViewType == null)
        {
            EditorUtility.DisplayDialog("Can't add to scene", "The diagram must be saved and have no compiler errors.",
                "OK");
            return;
        }

        GameObject obj = new GameObject(view.Name);
        obj.AddComponent(view.CurrentViewType);
    }

    public override string CanPerform(IDiagramNode node)
    {
        if (node is ViewData) return null;
        return "Must be a scene manager to perform this action.";
    }
}